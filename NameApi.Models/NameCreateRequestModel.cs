using System.ComponentModel.DataAnnotations;

namespace NameApi.Models
{
    public class NameCreateRequestModel
    {
        [Required, MaxLength(200)]
        public string Name { get; set; }
    }
}
