using System.ComponentModel.DataAnnotations;

namespace AustraliyaDemo.Entity
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailId { get; set; }
        public DateTime LastLogingDate { get; set; }= DateTime.Now; 
        public bool IsActive { get; set; }=true;    
        public DateTime CreatedOn { get; set; }=DateTime.Now;
        public DateTime LastModified { get; set; }= DateTime.Now;
    }
}
