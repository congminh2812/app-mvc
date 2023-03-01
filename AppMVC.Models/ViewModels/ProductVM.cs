using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppMVC.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        public IEnumerable<SelectListItem>? TypeList { get; set; }
    }
}
