using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElektroCity.Models
{
    public class Screening
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Моля, изберете филм.")]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie? Movie { get; set; } // Оправено: Сложено е '?', за да не блокира валидацията

        [Required(ErrorMessage = "Моля, въведете дата и час.")]
        [Display(Name = "Дата и час на прожекцията")]
        [DataType(DataType.DateTime)]
        public DateTime Showtime { get; set; }

        [Required(ErrorMessage = "Моля, въведете име на зала.")]
        [Display(Name = "Зала")]
        public string HallName { get; set; }

        [Required(ErrorMessage = "Моля, въведете цена на билета.")]
        [Display(Name = "Цена на билет")]
        [Range(0.01, 1000.00, ErrorMessage = "Цената трябва да е по-голяма от 0.")]
        public decimal TicketPrice { get; set; }
    }
}