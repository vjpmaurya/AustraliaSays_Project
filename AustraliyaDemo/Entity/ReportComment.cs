using System.ComponentModel.DataAnnotations;

namespace AustraliyaDemo.Entity
{
    public class ReportComment
    {
        [Key]
        public int Id { get; set; }
        public int CommentId { get; set; }
        public string ReportComments { get; set; }
        public bool IsActive { get; set; }=true;
        public DateTime CreatedOn { get; set; }=DateTime.Now;
        public DateTime LastModified { get; set; }= DateTime.Now;
    }
}
