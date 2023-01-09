﻿using learnmvc.DataAccess.Repository.IRepository;
using learnmvc.Models;
using learnmvc.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace learnmvc.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
		public int OrderTotal { get; set; }
		public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,includeProperties:"Product")
            };
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = getPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
				ShoppingCartVM.CartTotal+= (cart.Count*cart.Price);

			}
            return View(ShoppingCartVM);
        }
		public IActionResult Summary()
		{
			return View();
		}
		public IActionResult plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(cart,1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
		public IActionResult Minus(int cartId)
		{
			var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            if (cart.Count <=1)
            {
				_unitOfWork.ShoppingCart.Remove(cart);
            }
            else
            {
			    _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
            }
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}
		public IActionResult Remove(int cartId)
		{
			var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
			_unitOfWork.ShoppingCart.Remove(cart);
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}

		private double getPriceBasedOnQuantity(double quantity ,double price,double price50,double price100)
        {
            if (quantity<=50)
            {
                return price;
            }
            else if (quantity <= 100)
            {
                return price50;
            }
            else
            {
                return price100;
            }
        }
    }
}
