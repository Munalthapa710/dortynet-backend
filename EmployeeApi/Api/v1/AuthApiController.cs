using EmployeeApi.Data;
using EmployeeApi.Model.Employee;
using EmployeeApi.ViewModel.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeApi.Api.v1;

[ApiController]
[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public AuthApiController(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestViewModel request)
    {
        var configuredUsername = _configuration["ApiUser:Username"];
        var configuredPassword = _configuration["ApiUser:Password"];

        if (request.Username == configuredUsername && request.Password == configuredPassword)
        {
            var adminToken = CreateToken(request.Username, "Manager");
            return Ok(adminToken);
        }

        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Email == request.Username);

        if (employee is null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var hasher = new PasswordHasher<Employee>();
        var passwordResult = hasher.VerifyHashedPassword(
            employee,
            employee.PasswordHash,
            request.Password);

        if (passwordResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized("Invalid username or password.");
        }

        var token = CreateToken(employee.Email, employee.Role, employee.Id);
        return Ok(token);
    }

    private LoginResponseViewModel CreateToken(string username, string role, int? employeeId = null)
    {
        var secret = _configuration["Jwt:Secret"]
            ?? throw new InvalidOperationException("Jwt:Secret is not configured.");
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expirationMinutes = _configuration.GetValue("Jwt:ExpirationMinutes", 30);
        var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var claims = new List<Claim>
        {
             new Claim(JwtRegisteredClaimNames.Sub, username),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             new Claim(ClaimTypes.Name, username),
             new Claim(ClaimTypes.Role, role)
        };

        if (employeeId.HasValue)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, employeeId.Value.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new LoginResponseViewModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwt),
            ExpiresAt = expiresAt
        };
    }
}
