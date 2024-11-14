using AlphaKiloWebRazor_Temp.Data;
using AlphaKiloWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlphaKiloWebRazor_Temp.Pages.Categories {
    public class DeleteModel : PageModel {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category? Category { get; set; }
        public DeleteModel(ApplicationDbContext applicationDbContext) {
            _db = applicationDbContext;
        }

        public void OnGet(int? id) {
            if(id != null && id > 0 )
                Category = _db.Categories.FirstOrDefault(c => c.Id == id);
        }
        public IActionResult OnPost() {
            if (Category == null || Category.Id == 0) {
                TempData["error"] = "Category was not found.";
            }
            else {
                var name = Category.Name;
                _db.Remove(Category);
                _db.SaveChanges();
                TempData["success"] = $"Category '{name}' was deleted successfully!";
            }
            return RedirectToPage("Index");
        }
    }
}