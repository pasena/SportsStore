using SportsStore.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsStore.UnitTests
{
    /// <summary>
    ///This is a test class for CartTest and is intended
    ///to contain all CartTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CartTest
    {
        [TestMethod]
        public void Can_Add_New_Line()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            CartLine[] results = target.Lines.ToArray();

            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);

            CartLine[] results = target.Lines.OrderBy(o => o.Product.ProductID).ToArray();

            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            target.RemoveLine(p2);

            Assert.AreEqual(target.Lines.Where(w=> w.Product == p2).Count() , 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Car_Total()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1", Price=10 };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price=5};

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 3);

            Assert.AreEqual(target.ComputeTotalValue(), 25);
        }

        [TestMethod]
        public void Can_Clear_Content()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 10 };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 5 };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 3);

            target.Clear();

            Assert.AreEqual(target.Lines.Count(), 0);
        }
    }
}
