using Mazen.DataAccess.Repository.IRepository;
using Mazen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace MazenWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger , IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index() //Iaciton resutl is return type of all the results of an action type
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties:"Category");
            return View(productList);
        }
        public IActionResult Details(int productId) //Iaciton resutl is return type of all the results of an action type
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = productId
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart) //Iaciton resutl is return type of all the results of an action type
        {
            // after using Authorize , these 2 lines to get the id of the current user 
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;
            
            // now if we added item to cart and then try to added another 
            // this will make to differnt entry in the db so to make it update the db instead of adding another field
            // 1- check in the db if there's obj shoppingCart with the same productId && appUserId ??
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u=>u.ApplicationUserId == userId &&
            u.ProductId ==  shoppingCart.ProductId);

            // ** when u retrive something with .net it always keeps tracking it ,so no need for update as it updates automatic.
            if(cartFromDb != null)
            {
                // shopping cart exists 
                cartFromDb.Count += shoppingCart.Count;
               _unitOfWork.ShoppingCart.Update(cartFromDb); // no need as automaitc update

            }
            else
            {
                // add cart record to db cause it's new 
                _unitOfWork.ShoppingCart.Update(shoppingCart);
            }
            TempData["success"] = "Cart Updated Successfully";

            _unitOfWork.Save();
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}