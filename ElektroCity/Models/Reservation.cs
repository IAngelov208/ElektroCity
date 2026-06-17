using System.ComponentModel.DataAnnotations;

namespace ElektroCity.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public int ScreeningId { get; set; }
        public Screening? Screening { get; set; }

        [Required]
        public string UserEmail { get; set; } = string.Empty;

        [Required]
        public string ChosenSeats { get; set; } = string.Empty; // Пази текст от типа: "Р1-М4, Р1-М5"

        public decimal TotalPrice { get; set; }

        public DateTime ReservedAt { get; set; } = DateTime.Now;
    }
}