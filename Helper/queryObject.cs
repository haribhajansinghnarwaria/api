namespace api.Helper
{
    public class queryObject
    {
        public string? Symbol { get; set; } // Nulaable as we do not need to always pass these query paramas alwayss
        public string? CompanyName { get; set; }

        public string SortBy { get; set; } = null;//y which column we want to be sort

        public bool IsDescending { get; set; } = false; // By which order asc or desc
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}
