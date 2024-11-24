using AlphaKilo.DataAccess.Data;
using AlphaKilo.DataAccess.Repository.IRepository;
using AlphaKilo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaKilo.DataAccess.Repository {
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserResopsitory {
        private readonly ApplicationDbContext _db; 
        public ApplicationUserRepository(ApplicationDbContext db) : base(db) {
            _db = db;
        }
    }
}
