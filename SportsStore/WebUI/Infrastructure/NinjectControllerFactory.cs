using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Ninject;
using Domain.Abstract;
using Domain.Entities;
using Domain.Concrete;

namespace WebUI.Models
{
    // реализация пользовательской фабрики контроллеров,
    // наследуясь от фабрики используемой по умолчанию
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            // создание контейнера
            ninjectKernel = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance(RequestContext requestContext,
        Type controllerType)
        {
            // получение объекта контроллера из контейнера
            // используя его тип
            return controllerType == null
            ? null
            : (IController)ninjectKernel.Get(controllerType);
        }
        private void AddBindings()
        {
            //Mock<IProductRepository> mock = new Mock<IProductRepository>();
            //mock.Setup(m => m.Products).Returns(new List<Product>
            //{
            //    new Product {Name = "Football", Price = 25},
            //    new Product {Name = "Surf board", Price = 179},
            //    new Product {Name = "Running shoes", Price = 95}
            //}.AsQueryable());
            //ninjectKernel.Bind<IProductRepository>().ToConstant(mock.Object);
            ninjectKernel.Bind<IProductRepository>().To<EFProductRepository>();
        }
    }
}
