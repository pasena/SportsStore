using SportsStore.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using SportsStore.Domain.Abstract;
using System.Web.Mvc;
using Moq;
using SportsStore.Domain.Entities;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SportsStore.UnitTests
{


    /// <summary>
    ///This is a test class for AdminControllerTest and is intended
    ///to contain all AdminControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdminControllerTest
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { 
                new Product{ProductID=1, Name="P1", Category="Cat1"},
                new Product{ProductID=2, Name="P2", Category="Cat2"},
                new Product{ProductID=3, Name="P3", Category="Cat3"}
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            Product[] result =
                ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { 
                new Product{ProductID=1, Name="P1", Category="Cat1"},
                new Product{ProductID=2, Name="P2", Category="Cat2"},
                new Product{ProductID=3, Name="P3", Category="Cat3"}
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            Product p1 = target.Edit(1).ViewData.Model as Product;
            Product p2 = target.Edit(2).ViewData.Model as Product;
            Product p3 = target.Edit(3).ViewData.Model as Product;

            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { 
                new Product{ProductID=1, Name="P1", Category="Cat1"},
                new Product{ProductID=2, Name="P2", Category="Cat2"},
                new Product{ProductID=3, Name="P3", Category="Cat3"}
            }.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            Product result = target.Edit(4).ViewData.Model as Product;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valida_Changes()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            AdminController target = new AdminController(mock.Object);

            Product product = new Product { Name = "Test" };

            ActionResult result = target.Edit(product);

            mock.Verify(m => m.SaveProduct(product));

            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalida_Changes()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            AdminController target = new AdminController(mock.Object);

            Product product = new Product { Name = "Teste" };

            target.ModelState.AddModelError("erro", "erro");

            ActionResult result = target.Edit(product);

            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
