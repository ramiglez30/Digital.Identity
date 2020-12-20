namespace Digital.Identity.Admin.Models.Api
{
    public class PagedList
    {
        public int PageNumber { get; set; } = 0;
        public int PageTotal { get; set; } = 10;

        public int Skipped()
        {
            return PageNumber * PageTotal;
        }
    }
}
