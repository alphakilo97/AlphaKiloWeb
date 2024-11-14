using AlphaKiloWebRazor_Temp.Data;
using AlphaKiloWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AlphaKiloWebRazor_Temp.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category Category { get; set; }

        public EditModel(ApplicationDbContext applicationDbContext) {
            _db = applicationDbContext;
        }

        public void OnGet(int? id){
            if(id != null &&  id > 0)
                Category = _db.Categories.FirstOrDefault(c => c.Id == id);
        }
        public IActionResult OnPost(){
            if (Category == null || Category.Id == 0) {
                TempData["error"] = "Category with was not found";
            }
            else {
                _db.Update(Category);
                _db.SaveChanges();
                TempData["success"] = $"Category '{Category.Name}' was updated successfully!";
            }
            return RedirectToPage("Index");
        }
    }
}
