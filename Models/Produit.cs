using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockManagement.Models;

public class Produit
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(100)]
    [Display(Name = "Nom du produit")]
    public string Nom { get; set; } = string.Empty;

    [StringLength(50)]
    [Display(Name = "Référence")]
    public string? Reference { get; set; }

    [StringLength(100)]
    [Display(Name = "Catégorie")]
    public string? Categorie { get; set; }

    [Required(ErrorMessage = "La quantité est obligatoire")]
    [Range(0, int.MaxValue, ErrorMessage = "La quantité doit être positive")]
    [Display(Name = "Quantité en stock")]
    public int Quantite { get; set; }

    [Required(ErrorMessage = "Le prix est obligatoire")]
    [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Prix unitaire (DH)")]
    public decimal Prix { get; set; }

    [Display(Name = "Stock minimum")]
    public int StockMinimum { get; set; } = 5;

    [StringLength(500)]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Date d'ajout")]
    public DateTime DateAjout { get; set; } = DateTime.UtcNow;

    // Propriété calculée : alerte si stock bas
    public bool StockBas => Quantite <= StockMinimum;
}
