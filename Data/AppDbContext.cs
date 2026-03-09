using Microsoft.EntityFrameworkCore;
using StockManagement.Models;

namespace StockManagement.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Produit> Produits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Données de test initiales (dates statiques obligatoires pour HasData)
        var dateRef = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<Produit>().HasData(
            new Produit { Id = 1, Nom = "Stylo Bic", Reference = "STY-001", Categorie = "Fournitures", Quantite = 150, Prix = 2.50m, StockMinimum = 20, DateAjout = dateRef },
            new Produit { Id = 2, Nom = "Ramette de papier A4", Reference = "PAP-001", Categorie = "Fournitures", Quantite = 30, Prix = 45.00m, StockMinimum = 10, DateAjout = dateRef },
            new Produit { Id = 3, Nom = "Clavier USB", Reference = "INF-001", Categorie = "Informatique", Quantite = 3, Prix = 180.00m, StockMinimum = 5, DateAjout = dateRef }
        );
    }
}
