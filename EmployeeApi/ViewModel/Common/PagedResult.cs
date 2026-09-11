namespace EmployeeApi.ViewModel.Common
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();

        public int RowTotal { get; set; }

        public int Page { get; set; }

        public int Limit { get; set; }
    }
}
