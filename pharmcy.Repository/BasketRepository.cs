using Microsoft.EntityFrameworkCore.Storage;
using Pharmcy.Core.Entities;
using Pharmcy.Core.Repositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IDatabase = StackExchange.Redis.IDatabase;

namespace pharmcy.Repository
{


  
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        //ask clr to inject object from class that implement IConnectionMultiplexer
        public BasketRepository(IConnectionMultiplexer redis)
        {

           _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string Baskeid)
        {
            return await _database.KeyDeleteAsync(Baskeid);
        }

        public async Task<CustomerBasket?> GetBaketAsync(string Baskeid)
        {
            var Basket = await _database.StringGetAsync(Baskeid);
            if (Basket.IsNull) return null;
            else
               return JsonSerializer.Deserialize<CustomerBasket>(Basket);


        }

        public async Task<CustomerBasket?> UpdateBasket(CustomerBasket Basket)
        {
            var Json=JsonSerializer.Serialize(Basket);
         var createdorUpdated=  await _database.StringSetAsync(Basket.Id, Json, TimeSpan.FromDays(1));
            if (!createdorUpdated)
                return null;
            return await GetBaketAsync(Basket.Id);

        }
    }
}
