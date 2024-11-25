using AlphaKilo.DataAccess.Data;
using AlphaKilo.DataAccess.Repository.IRepository;
using AlphaKilo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaKilo.DataAccess.Repository {
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository {
        private readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db) {
            _db = db;
        }

        public void Update(ShoppingCart shoppingCart) {
            _db.ShoppingCarts.Update(shoppingCart);
        }
    }
}
