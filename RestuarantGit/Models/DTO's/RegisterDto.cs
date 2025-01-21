using Delivery.Resutruant.API.Models.Enums;
using System.ComponentModel.DataAnnotations;
public class RegisterDto
{
    [Required(ErrorMessage = "Full Name is required.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Birthdate is required.")]
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone]
    public string PhoneNumber { get; set; }
}
