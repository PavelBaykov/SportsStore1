using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;

        public int PageSize = 4;

        public ProductController(IProductRepository paramRepo)
        {
            repository = paramRepo;
        }

        public ViewResult List(string currentCategoryParam, int page = 1)
        {
            //return View(repository.Products.OrderBy(p=>p.ProductID).Skip(((page-1)*PageSize)).Take(PageSize));
                
                
            ProductsListViewModel model = new ProductsListViewModel()
            {
                Products= repository.Products.Where(p => p.Category == currentCategoryParam || currentCategoryParam == null)
                    .OrderBy(p => p.ProductID)
                    .Skip(((page - 1) * PageSize))
                    .Take(PageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Products.Where(p => p.Category == currentCategoryParam || currentCategoryParam == null).Count()
                },
                CurrentCategory = currentCategoryParam
            };
            return View(model);
        }
    }
}