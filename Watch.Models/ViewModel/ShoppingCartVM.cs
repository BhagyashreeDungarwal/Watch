using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.Models.ViewModel
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShopingCart> ListCart { get; set; }
        public double CartTotal { get; set; }
    }
}
