using System.ComponentModel.DataAnnotations;

namespace ElektroCity.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Полето е задължително")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
    }
}