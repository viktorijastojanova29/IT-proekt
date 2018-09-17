using DrinkAndGo1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinkAndGo1.Models
{
    public class OrderRepository
    {
        private readonly DrinkAndGoContext _db = new DrinkAndGoContext();
        private readonly ShoppingCart _shoppingCart;


        public OrderRepository(DrinkAndGoContext dbContext, ShoppingCart shoppingCart)
        {
            _db = dbContext;
            _shoppingCart = shoppingCart;
        }


        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;

            _db.Orders.Add(order);

            var shoppingCartItems = _shoppingCart.ShoppingCartItems;

            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Amount = shoppingCartItem.Amount,
                    DrinkId = shoppingCartItem.Drink.DrinkId,
                    OrderId = order.OrderId,
                    Price = shoppingCartItem.Drink.Price
                };

                _db.OrderDetails.Add(orderDetail);
            }

            _db.SaveChanges();
        }
    }
}