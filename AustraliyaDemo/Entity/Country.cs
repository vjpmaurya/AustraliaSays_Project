using System.ComponentModel.DataAnnotations;

namespace AustraliyaDemo.Entity
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string CountryName { get; set; }
    }
}
