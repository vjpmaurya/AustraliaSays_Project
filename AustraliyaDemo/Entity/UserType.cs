using System.ComponentModel.DataAnnotations;

namespace AustraliyaDemo.Entity
{
    public class UserType
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
    }
}
