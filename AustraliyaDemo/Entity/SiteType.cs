using System.ComponentModel.DataAnnotations;

namespace AustraliyaDemo.Entity
{
    public class SiteType
    {
        [Key]
        public int Id { get; set; }
        public string SiteName { get; set; }
    }
}
