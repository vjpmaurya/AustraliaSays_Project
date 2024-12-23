using System.ComponentModel.DataAnnotations;

namespace AustraliyaDemo.Entity
{
    public class Site
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string Url { get; set; }
        public string LogoPath { get; set; }
        public int TypeId { get; set; }
        public bool IsActive { get; set; } =true;
        public DateTime CreatedOn { get; set; }= DateTime.Now;
        public DateTime LastModified { get; set; }=DateTime.Now;
    }
}
