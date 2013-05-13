using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.NuoDB.Tests.Entities;
using NHibernate.SqlCommand;
using NuoDb.Data.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.NuoDB.Tests
{
    [TestClass]
    public class NuoDbDialectFixture
    {
        private NuoDbDialect dialect;

        [TestMethod]
        public void Fixture()
        {
            var mapper = new ModelMapper();
            mapper.AddMapping<ProductMap>();
            mapper.AddMapping<OrderMap>();
            mapper.AddMapping<OrderDetailMap>();
            
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            
            var config = new Configuration();
            config.AddDeserializedMapping(mapping, "ProductMap");
            config.Configure();
            
            //create the database
            new NHibernate.Tool.hbm2ddl.SchemaExport(config).Execute(true, true, false);

            var factory = config.BuildSessionFactory();
            var session = factory.OpenSession();

            var product = new Product();
            product.ProductName = "Test";
            product.Description = string.Join("",Enumerable.Repeat("1", 8000).ToArray());
            product.Image = new byte[8000];
            product.Price = 10.9999m;

            var order = new Order();
            order.OrderDate = DateTime.UtcNow;
            order.Freight = 1.22m;
            order.Details = new List<OrderDetail>() { new OrderDetail() { Product = product, Quantity = 2 } };

            session.Save(product);
            session.Save(order);
            session.Flush();

            var loaded = session.Get<Order>(order.Id);
            Assert.AreEqual(order, loaded);
            Assert.AreEqual(1.22m, order.Freight);
            session.Close();
        }
    }
}
