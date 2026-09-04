namespace EmployeeApi.Model.Client
{
    public class Client
    {
        public int Id { get; set; }

        public string ClientName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string ProjectName { get; set; } = string.Empty;
    }
}
