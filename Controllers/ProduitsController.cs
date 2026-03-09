using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Models;

namespace StockManagement.Controllers;

public class ProduitsController : Controller
{
    private readonly AppDbContext _context;

    public ProduitsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Produits  (+ recherche + filtre catégorie)
    public async Task<IActionResult> Index(string? recherche, string? categorie)
    {
        var query = _context.Produits.AsQueryable();

        if (!string.IsNullOrWhiteSpace(recherche))
            query = query.Where(p => p.Nom.Contains(recherche) || (p.Reference != null && p.Reference.Contains(recherche)));

        if (!string.IsNullOrWhiteSpace(categorie))
            query = query.Where(p => p.Categorie == categorie);

        ViewBag.Recherche = recherche;
        ViewBag.Categorie = categorie;
        ViewBag.Categories = await _context.Produits
            .Select(p => p.Categorie)
            .Distinct()
            .Where(c => c != null)
            .OrderBy(c => c)
            .ToListAsync();

        ViewBag.TotalProduits = await _context.Produits.CountAsync();
        ViewBag.StockBas = await _context.Produits.CountAsync(p => p.Quantite <= p.StockMinimum);
        ViewBag.ValeurTotale = await _context.Produits.SumAsync(p => (decimal?)p.Quantite * p.Prix) ?? 0;

        return View(await query.OrderBy(p => p.Nom).ToListAsync());
    }

    // GET: Produits/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var produit = await _context.Produits.FindAsync(id);
        if (produit == null) return NotFound();
        return View(produit);
    }

    // GET: Produits/Create
    public IActionResult Create() => View();

    // POST: Produits/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nom,Reference,Categorie,Quantite,Prix,StockMinimum,Description")] Produit produit)
    {
        if (ModelState.IsValid)
        {
            produit.DateAjout = DateTime.UtcNow;
            _context.Add(produit);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Produit \"{produit.Nom}\" ajouté avec succès.";
            return RedirectToAction(nameof(Index));
        }
        return View(produit);
    }

    // GET: Produits/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var produit = await _context.Produits.FindAsync(id);
        if (produit == null) return NotFound();
        return View(produit);
    }

    // POST: Produits/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Reference,Categorie,Quantite,Prix,StockMinimum,Description,DateAjout")] Produit produit)
    {
        if (id != produit.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(produit);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Produit \"{produit.Nom}\" modifié avec succès.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Produits.AnyAsync(p => p.Id == produit.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(produit);
    }

    // GET: Produits/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var produit = await _context.Produits.FindAsync(id);
        if (produit == null) return NotFound();
        return View(produit);
    }

    // POST: Produits/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var produit = await _context.Produits.FindAsync(id);
        if (produit != null)
        {
            string nom = produit.Nom;
            _context.Produits.Remove(produit);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Produit \"{nom}\" supprimé.";
        }
        return RedirectToAction(nameof(Index));
    }

    // POST: Ajustement rapide de quantité
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AjusterQuantite(int id, int delta)
    {
        var produit = await _context.Produits.FindAsync(id);
        if (produit == null) return NotFound();

        produit.Quantite = Math.Max(0, produit.Quantite + delta);
        await _context.SaveChangesAsync();
        TempData["Success"] = $"Stock mis à jour : {produit.Nom} → {produit.Quantite} unités.";
        return RedirectToAction(nameof(Index));
    }
}
