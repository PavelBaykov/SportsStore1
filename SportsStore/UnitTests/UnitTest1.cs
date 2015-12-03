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
            IEnumerable<Product> result = ((ProductsListViewModel)controller.List(null,2).Model).Products;
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
            ProductsListViewModel model = (ProductsListViewModel)controller.List(null,2).Model;
            //Assert
            Assert.AreEqual(2,model.PagingInfo.CurrentPage);
            Assert.AreEqual(5, model.PagingInfo.TotalItems);
            Assert.AreEqual(3, model.PagingInfo.ItemsPerPage);
            Assert.AreEqual(2, model.PagingInfo.TotalPages);
        }

        [TestMethod]
        public void Can_filter()
        {
            //Assert
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1",Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2",Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3",Category = "Cat2"},
                new Product {ProductID = 4, Name = "P4",Category = "Cat1"},
                new Product {ProductID = 5, Name = "P5",Category = "Cat1"}
            }.AsQueryable());
            ;
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Act
            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();


            //Arrange
            Assert.AreEqual(result.Length,2);
            Assert.AreEqual(result[0].Name,"P2");
            Assert.AreEqual(result[1].Name,"P3");

        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 4, Name = "P2", Category = "Oranges"}
            }.AsQueryable());

            NavController target = new NavController(mock.Object);

            string categorySelected = "Apples";

            string result = target.Menu(categorySelected).ViewBag.SelectedCategory;

            Assert.AreEqual(categorySelected,result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
            }.AsQueryable());

            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;

            int res1 = ((ProductsListViewModel) target.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductsListViewModel)target.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductsListViewModel)target.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 5);
        }
    }
}
