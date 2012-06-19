using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository = null;
        private IOrderProcessor orderProcessor = null;

        public CartController(IProductRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
        }

        public ViewResult Index(Cart cart, string returnURL)
        {
            return View(new CartIndexViewModel { Cart = cart, ReturnURL = returnURL });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int productID, string returnURL)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productID);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new { returnURL });

        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productID, string returnURL)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productID);

            if (product != null)
                cart.RemoveLine(product);

            return RedirectToAction("Index", new { returnURL });
        }

        public ViewResult Summary(Cart cart)
        {
            return View(cart);
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
                ModelState.AddModelError("", "Sorry, your cart is empty!");

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(new ShippingDetails());
            }
        }
        
        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }
    }
}
