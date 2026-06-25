namespace EmployeeApi.ViewModel.Auth;

public class LoginResponseViewModel
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
}
