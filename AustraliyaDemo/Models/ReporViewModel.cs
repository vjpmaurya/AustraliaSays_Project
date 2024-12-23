namespace AustraliyaDemo.Models
{
    public class ReporViewModel
    {
        public int ReportId { get; set; }
        public int CommentId { get; set; }
        public string? CommentName { get; set; }
        public string? ReportName { get; set; }
        public DateTime CreatedOn { get; set; }=DateTime.Now;
      

    }
}
