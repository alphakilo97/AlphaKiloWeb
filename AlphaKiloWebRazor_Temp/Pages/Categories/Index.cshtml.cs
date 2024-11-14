using AlphaKiloWebRazor_Temp.Data;
using AlphaKiloWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlphaKiloWebRazor_Temp.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public List<Category> CategoryList { get; set; }

        public IndexModel(ApplicationDbContext applicationDbContext) {
            _db = applicationDbContext;
        }
        public void OnGet() {
            CategoryList = _db.Categories.ToList();
        }
    }
}
