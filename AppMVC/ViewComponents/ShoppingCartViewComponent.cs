using AppMVC.DataAccess.Repository.IRepository;
using AppMVC.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppMVC.Web.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claims != null)
            {
                if (HttpContext.Session.GetInt32(SD.SessionShoppingCart) != null)
                {
                    return View(HttpContext.Session.GetInt32(SD.SessionShoppingCart));
                }
                else
                {
                    HttpContext.Session.SetInt32(SD.SessionShoppingCart, _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == claims.Value).Count());
                    return View(HttpContext.Session.GetInt32(SD.SessionShoppingCart));
                }
            }

            HttpContext.Session.Clear();
            return View(0);
        }
    }
}
