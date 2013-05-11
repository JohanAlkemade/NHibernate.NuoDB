using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.NuoDB.Tests.Entities
{
    public class Product
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }

    public class ProductMap : ClassMapping<Product>
    {
        public ProductMap()
        {
            Schema("test");
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Name, m => m.Column("name"));
        }
    }

}
