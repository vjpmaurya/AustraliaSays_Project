using System.ComponentModel.DataAnnotations;

namespace AustraliyaDemo.Entity
{
    public class ArticleType
    {
        [Key]
        public int Id { get; set; }
        public string ArticleName { get; set; }
        
    }
}
