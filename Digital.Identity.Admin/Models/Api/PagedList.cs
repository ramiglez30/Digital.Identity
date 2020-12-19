namespace Digital.Identity.Admin.Models.Api
{
    public class PagedList
    {
        public int PageNumber { get; set; }
        public int PageTotal { get; set; }

        public int Skipped()
        {
            return PageNumber * PageTotal;
        }
    }
}
