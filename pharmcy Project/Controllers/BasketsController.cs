using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharmcy.Core.Entities;
using Pharmcy.Core.Repositories;
using pharmcy_Project.Errors;

namespace pharmcy_Project.Controllers
{
    
    public class BasketsController : APIBaseController
    {
        private readonly IBasketRepository _repo;

        public BasketsController(IBasketRepository repo )
        {
           _repo = repo;
        }
        //Get //Recreate
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>>GetCustomerBasket(string basketId)
        {
            var Basket=await _repo.GetBaketAsync(basketId);
            //if (Basket is null) 
            //{ 
            //return new CustomerBasket(basketId);
            //}
           return Basket is null ? new CustomerBasket(basketId):Ok(Basket);
        }
        //Update oR Create New Basket
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>>UpdateBasket(CustomerBasket Basket)
        {

            var CreatedOrUpdatedBasket = await _repo.UpdateBasket(Basket);
            if (CreatedOrUpdatedBasket is null) return BadRequest(new ApiResponse(400) );
            return Ok(CreatedOrUpdatedBasket);

        }
        //Delete
        [HttpDelete]
        public async Task<ActionResult<bool>>DeleteBasket(string BasketId)
        {
        return   await _repo.DeleteBasketAsync(BasketId);

        }
    }
}
