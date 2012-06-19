using SportsStore.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using SportsStore.Domain.Abstract;
using System.Web.Mvc;
using Moq;
using SportsStore.Domain.Entities;
using System.Linq;
using System.Collections.Generic;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTests
{
    [TestClass()]
    public class ProductControllerTest
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { 
                                                new Product {ProductID = 1, Name= "P1"},
                                                new Product {ProductID = 2, Name= "P2"},
                                                new Product {ProductID = 3, Name= "P3"},
                                                new Product {ProductID = 4, Name= "P4"},
                                                new Product {ProductID = 5, Name= "P5"}
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;


            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(result.ToString(), @"<a href=""Page1"">1</a><a class=""selected"" href=""Page2"">2</a><a href=""Page3"">3</a>");
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] { 
                                                new Product {ProductID = 1, Name= "P1"},
                                                new Product {ProductID = 2, Name= "P2"},
                                                new Product {ProductID = 3, Name= "P3"},
                                                new Product {ProductID = 4, Name= "P4"},
                                                new Product {ProductID = 5, Name= "P5"}
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);

            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            PagingInfo pageInfo = result.PagingInfo;

            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] { 
                new Product{ProductID=1, Name="P1", Category="Cat1"},
                new Product{ProductID=2, Name="P2", Category="Cat2"},
                new Product{ProductID=3, Name="P3", Category="Cat1"},
                new Product{ProductID=4, Name="P4", Category="Cat2"},
                new Product{ProductID=5, Name="P5", Category="Cat1"}
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 2;

            Product[] result = ((ProductsListViewModel)controller.List("Cat1", 1).Model).Products.ToArray();

            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P1" && result[0].Category == "Cat1");
            Assert.IsTrue(result[1].Name == "P3" && result[1].Category == "Cat1");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] 
            { 
                new Product{ProductID = 1, Name="P1", Category="Cat1"},
                new Product{ProductID = 2, Name="P2", Category="Cat1"},
                new Product{ProductID = 3, Name="P3", Category="Cat3"},
                new Product{ProductID = 4, Name="P4", Category="Cat2"}
            }.AsQueryable());

            NavController controller = new NavController(mock.Object);

            string[] result = ((IEnumerable<string>)controller.Menu().Model).ToArray();

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0], "Cat1");
            Assert.AreEqual(result[1], "Cat2");
            Assert.AreEqual(result[2], "Cat3");

        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] 
            { 
                new Product{ProductID = 1, Name="P1", Category="Cat1"},
                new Product{ProductID = 2, Name="P2", Category="Cat2"},
            }.AsQueryable());

            NavController controller = new NavController(mock.Object);

            string result = controller.Menu("Cat2").ViewBag.SelectedCategory;

            Assert.AreEqual(result, "Cat2");
        }
    }
}
