using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models;

public class Widget
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Widget name")]
    public string Name { get; set; }

    [Required]
    [Range(0, 1_000_000, ErrorMessage = "Price must be between 0 and 1,000,000")]
    [Display(Name = "Widget price")]
    public double Price { get; set; }

    public bool AdminOnly { get; set; }

    public Widget() { }

    public Widget(int id, string name, double price, bool adminOnly)
    {
        Id = id;
        Name = name;
        Price = price;
        AdminOnly = adminOnly;
    }
}
