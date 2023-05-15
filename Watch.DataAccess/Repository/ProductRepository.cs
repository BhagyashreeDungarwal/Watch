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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
       {
        //   _db.Products.Update(obj);
        //To Retrive 1 by 1 Data From Data FirstOrDefauolt is used..
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);

            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Price50 = obj.Price50;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Descrptioin = obj.Descrptioin;
                objFromDb.Brand = obj.Brand;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.CoverTypeId = obj.CoverTypeId;

                //if we updated title or other roperty at that time image is not be updated.
                if (obj.ImageURL != null)
                {
                    objFromDb.ImageURL = obj.ImageURL;
                }
            }
        }
    }
}
