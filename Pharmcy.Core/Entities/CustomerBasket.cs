﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmcy.Core.Entities
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public CustomerBasket(string id)
        {
            Id = id;
            
        }
    }
}
