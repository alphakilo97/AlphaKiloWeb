using AlphaKilo.DataAccess.Data;
using AlphaKilo.DataAccess.Repository.IRepository;
using AlphaKilo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaKilo.DataAccess.Repository {
    public class CategoryRepository : Repository<Category>, ICategoryRepository {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db) {
            _db = db;
        }

        public void SaveChanges() {
            _db.SaveChanges();
        }

        public void Update(Category category) {
            _db.Categories.Update(category);
        }
    }
}
