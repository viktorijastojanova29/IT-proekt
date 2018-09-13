using DrinkAndGo1.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;

namespace DrinkAndGo1.Models
{
    public class ShoppingCart
    {
        private readonly DrinkAndGoContext _db = new DrinkAndGoContext();
        public ShoppingCart() { }
        private ShoppingCart(DrinkAndGoContext db)
        {
            _db = db;
        }

        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public void AddToCart(Drink drink, int amount)
        {
            var shoppingCartItem =
                   _db.ShoppingCartItems.SingleOrDefault(
                        s => s.DrinkId == drink.DrinkId);

            var id1 = drink.DrinkId;
            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem();
                shoppingCartItem.Amount = 1;
                shoppingCartItem.DrinkId = drink.DrinkId;
               
                _db.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
               // _db.ShoppingCartItems.Find(shoppingCartItem.ShoppingCartId).Amount++;
               shoppingCartItem.Amount++;
            }

            _db.SaveChanges();
        }



        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??
                   (ShoppingCartItems =
                       _db.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                           .Include(s => s.Drink)
                           .ToList());
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _db.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Drink.Price * c.Amount).Sum();
            return total;
        }

        public int RemoveFromCart(Drink drink)
        {
            var shoppingCartItem =
                    _db.ShoppingCartItems.SingleOrDefault(
                        s => s.Drink.DrinkId == drink.DrinkId && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _db.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            _db.SaveChanges();

            return localAmount;

        }

        public void ClearCart()
        {
            var cartItems = _db
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            _db.ShoppingCartItems.RemoveRange(cartItems);

            _db.SaveChanges();
        }


    }
}
    
    
