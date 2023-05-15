using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.DataAccess;
using Watch.DataAccess.Repository.IRepository;
using Watch.Models;

namespace Watch.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

       

        public void Update(Category obj)
        {
            _db.WatchCategories.Update(obj);
        }
    }
}
