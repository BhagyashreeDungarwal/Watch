using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.Models;

namespace Watch.DataAccess.Repository.IRepository
{
    public interface IShopingCartRepository : IRepository<ShopingCart>
    {
        int IncrementCount(ShopingCart shopingCart, int count);
        int DecrementCount(ShopingCart shopingCart, int count);
    }
}
