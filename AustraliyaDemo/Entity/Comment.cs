using System.ComponentModel.DataAnnotations;

namespace AustraliyaDemo.Entity
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public string Comments { get; set; }
        public bool IsActive { get; set; }= true;
        public DateTime CreatedOn { get; set; }=DateTime.Now;
        public DateTime LastModified { get; set; }= DateTime.Now;
    
    }
}
