using Pharmcy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmcy.Core.Repositories
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?>GetBaketAsync(string Baskeid);
        Task<CustomerBasket?>UpdateBasket(CustomerBasket Basket);
        Task<bool>DeleteBasketAsync(string Baskeid);
    }
}
