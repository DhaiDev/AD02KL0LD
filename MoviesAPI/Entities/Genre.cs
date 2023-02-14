using MoviesAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Field with name {0} is required")]
        [StringLength(10, ErrorMessage = "10 Charcter only")]
        [FirstLetterUppercaseAttribute]
        public string Name { get; set; }   

 

 
    }
}
