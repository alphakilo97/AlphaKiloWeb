using AlphaKiloWebRazor_Temp.Data;
using AlphaKiloWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlphaKiloWebRazor_Temp.Pages.Categories
{
    public class CreateModel : PageModel {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category Category { get; set; }

        public CreateModel(ApplicationDbContext applicationDbContext) {
            _db = applicationDbContext;
        }

        public IActionResult OnPost() {
            _db.Add(Category);
            int result = _db.SaveChanges();
            if (result == 0) {
                TempData["error"] = "There was a problem writing to the database.";
            }
            else
                TempData["success"] = $"Category '{Category.Name}' was created successfully!";
            return RedirectToPage("Index");
        }
    }
}
