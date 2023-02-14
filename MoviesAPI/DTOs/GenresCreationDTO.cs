using MoviesAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.DTOs
{
    public class GenresCreationDTO
    {

        [Required(ErrorMessage = "The Field with name {0} is required")]
        [StringLength(50, ErrorMessage = "10 Charcter only")]
        [FirstLetterUppercaseAttribute]
        public string Name { get; set; }
    }
}
