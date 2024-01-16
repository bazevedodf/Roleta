namespace Roleta.Persistencia.Models
{
    public class PageParams
    {
        public const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        public int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public string Term { get; set; } = string.Empty;
        public string ParentEmail { get; set; } = string.Empty;
        public DateTime DataIni { get; set; } = DateTime.Now.Date;
        public DateTime DataFim { get; set; } = DateTime.Now.AddDays(1).Date;
    }
}
