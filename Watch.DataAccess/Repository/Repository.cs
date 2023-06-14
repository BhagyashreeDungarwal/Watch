using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Watch.DataAccess;
using Watch.DataAccess.Repository.IRepository;

namespace Watch.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        //Retrive the data from database
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        //Access The data
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            //_db.Products.Include(u => u.Category).Include(u => u.CoverType);
            //binding the _db  To DBset..
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            //It is as same as    _db.WatchCategories.Add(obj);
            dbSet.Add(entity);
        }
        //Include Prop -"Category,CoverType"
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter=null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter!=null)
            {
                query = query.Where(filter);
            }
           
            //For Product and we can access all foreign key data of Category,CoverType
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {//includeProp is used to Retrive 1 Property at a Time
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

   
        public T GetFirstorDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();

        }

        public void RemoneRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}
