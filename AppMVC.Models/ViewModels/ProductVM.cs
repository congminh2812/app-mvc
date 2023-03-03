﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppMVC.Models.ViewModels
{
    public class ProductVM
    {
        [ValidateNever]
        public Product Product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? TypeList { get; set; }
    }
}
