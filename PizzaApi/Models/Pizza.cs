using System.ComponentModel.DataAnnotations;

namespace PizzaApi.Models;

public class Pizza
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public bool IsGlutenFree { get; set; }
}
