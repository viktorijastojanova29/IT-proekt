using DrinkAndGo1.Data;
using DrinkAndGo1.Models;
using DrinkAndGo1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DrinkAndGo1.Controllers
{
    public class ShoppingCartController : Controller
    {

        private readonly DrinkAndGoContext _db = new DrinkAndGoContext();
        private readonly ShoppingCart _shoppingCart = new ShoppingCart();
        // GET: ShoppingCart
        public ShoppingCartController() { }
        public ShoppingCartController(DrinkAndGoContext db, ShoppingCart shoppingCart)
        {
            _db = db;
            _shoppingCart = shoppingCart;
        }

        public ViewResult Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(shoppingCartViewModel);
        }

        public ActionResult AddToShoppingCart(int id)
        {
            var selectedDrink = _db.Drinks.AsNoTracking().FirstOrDefault(p => p.DrinkId == id);
            if (selectedDrink != null)
            {
                var idd = selectedDrink.DrinkId;
                _shoppingCart.AddToCart(selectedDrink, 1);
            }
            return RedirectToAction("Index");
        }


        public ActionResult RemoveFromShoppingCart(int id)
        {
            var selectedDrink = _db.Drinks.FirstOrDefault(p => p.DrinkId == id);
            if (selectedDrink != null)
            {
                _shoppingCart.RemoveFromCart(selectedDrink);
            }
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}