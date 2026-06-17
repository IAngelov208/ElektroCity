using System.ComponentModel.DataAnnotations;

namespace ElektroCity.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Жанр")]
        public string Genre { get; set; }

        [Required]
        [Display(Name = "Времетраене")]
        public int DurationMinutes { get; set; }

        [Required]
        [Display(Name = "Линк към постер")]
        public string ImageUrl { get; set; }

        // ВАЖНО: Това прави връзката 1 филм към много прожекции
        public virtual ICollection<Screening> Screenings { get; set; } = new List<Screening>();
    }
}