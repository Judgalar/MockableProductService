using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public class Product
{
    [Key]
    public long Id { get; private set; }
    
    [Required]
    [StringLength(50)]
    public required string Name { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "The price must be greater than zero")]

    public required decimal Price { get; set; }
}
