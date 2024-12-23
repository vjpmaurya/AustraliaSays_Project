using System.ComponentModel.DataAnnotations;

namespace AustraliyaDemo.Entity
{
    public class Article
    {
        [Key]

        public int Id { get; set; }
        public int UserId { get; set; }
        public int SiteId { get; set; }
        public string ArticleName { get; set; }
        public string URL { get; set; }
        public string Decription { get; set; }
        public int TypeId { get; set; }
        public string Tags { get; set; }
        public bool IsActive { get; set; }=true;
        public DateTime CreatedOn { get; set; }=DateTime.Now;
        public DateTime LastModified { get; set; }= DateTime.Now;

     
       
    }
}
