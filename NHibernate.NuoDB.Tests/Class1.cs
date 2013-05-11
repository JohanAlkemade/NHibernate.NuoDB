using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.NuoDB.Tests.Entities;
using NuoDb.Data.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.NuoDB.Tests
{
    [TestClass]
    public class Class1
    {
        [TestMethod]
        public void MyTestMethod()
        {
            var mapper = new ModelMapper();
            mapper.AddMapping<ProductMap>();
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            
            var config = new Configuration();
            config.AddDeserializedMapping(mapping, "ProductMap");
            config.Configure();
             
            new NHibernate.Tool.hbm2ddl.SchemaExport(config).Execute(true, true, false);

            var factory = config.BuildSessionFactory();
            var session = factory.OpenSession();

            var product = new Product();
            session.Save(product);
            session.Close();

            Assert.IsFalse(product.Id == 0);
        }
    }
}
