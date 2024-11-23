using AlphaKilo.DataAccess.Data;
using AlphaKilo.DataAccess.Repository.IRepository;
using AlphaKilo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlphaKilo.DataAccess.Repository {
    public class CompanyRepository : Repository<Company>, ICompanyRepository {
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) : base(db) {
            _db = db;
        }

        public void Update(Company company) {
            _db.Companies.Update(company);
        }
    }
}
