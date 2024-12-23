using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace AustraliyaDemo.Models
{
    public class Usermodel
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int UserId { get; set; }
        public string? EmailId { get; set; }
        public string? UserPassword { get; set; }
       
    }
}
