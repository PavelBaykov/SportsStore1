using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;
using WebUI.HtmlHelpers;
using WebUI.Models;


namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            //Act
            IEnumerable<Product> result = ((ProductsListViewModel)controller.List(2).Model).Products;
            //Assert

            Product[] prodArray = result.ToArray();
            Assert.AreEqual(prodArray.Length,2);
            Assert.IsTrue(prodArray[0].Name == "P4");
            Assert.IsTrue(prodArray[1].Name == "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Arrange
            HtmlHelper helper = null;

            PagingInfo pInfo = new PagingInfo()
            {
                CurrentPage = 2,
                ItemsPerPage = 10,
                TotalItems = 28
            };

            Func<int, string> del = x => "Page" + x.ToString();

            //Act
            MvcHtmlString mvcString = helper.PageLinks(pInfo, del);

            //Assert
            Assert.AreEqual(@"<a href=""Page1"">1</a>"+ @"<a class=""selected"" href=""Page2"">2</a>" + @"<a href=""Page3"">3</a>",mvcString.ToString());


        }

        [TestMethod]
        public void PagingInfo_Correct_page()
        {
            //Assert
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }.AsQueryable());

           ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Act
            ProductsListViewModel model = (ProductsListViewModel)controller.List(2).Model;
            //Arrange
            Assert.AreEqual(2,model.PagingInfo.CurrentPage);
            Assert.AreEqual(5, model.PagingInfo.TotalItems);
            Assert.AreEqual(3, model.PagingInfo.ItemsPerPage);
            Assert.AreEqual(2, model.PagingInfo.TotalPages);
        }
    }
}
