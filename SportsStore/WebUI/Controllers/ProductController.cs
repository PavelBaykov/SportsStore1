using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;

namespace WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;

        public ProductController(IProductRepository paramRepo)
        {
            repository = paramRepo;
        }

        public ViewResult List()
        {
            return View(repository.Products);
        }
    }
}