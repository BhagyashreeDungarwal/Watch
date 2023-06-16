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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }
        //we make the payment status is a nullable property bcoz it is possible we do not want to update paymentstatus everytime.
        public void UpdateStatus(int id, string orderStatus, string? paymentstatus = null)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id); 
            if(orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if(paymentstatus!= null)
                {
                    orderFromDb.PaymentStatus = paymentstatus;
                }
            }
        }
    }
}
