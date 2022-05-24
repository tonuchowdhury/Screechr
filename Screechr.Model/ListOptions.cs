namespace Screechr.Model
{
    public class ListOptions
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 50;
        public int SortBy { get; set; } = 1; // 0=Asc, 1=Desc
        public string? userName { get; set; } // search screech by username
    }
}
