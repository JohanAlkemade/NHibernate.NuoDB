using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.NuoDB.Tests.Entities
{

    public class Product : EntityBase
    {
        public virtual string ProductName { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal Price { get; set; }
        public virtual byte[] Image { get; set; }
    }

    public class Order : EntityBase
    {
        public virtual DateTime OrderDate { get; set; }
        public virtual decimal Freight { get; set; }
        public virtual IList<OrderDetail> Details { get; set; }
    }

    public class OrderDetail : EntityBase
    {
        public virtual Product Product { get; set; }
        public virtual uint Quantity { get; set; }
    }

    public class OrderMap : ClassMapping<Order>
    {
        public OrderMap()
        {
            Schema("northwind");

            Table("Orders");

            Id(x => x.Id, m =>
            {
                m.Generator(Generators.Sequence);
                m.Column("OrderId");
            });
            Property(x => x.OrderDate);
            Property(x => x.Freight);
            List(x => x.Details, m =>
            {
                m.Key(x => x.Column("OrderId"));
                m.Inverse(true);
                m.Cascade(Cascade.All | Cascade.DeleteOrphans | Cascade.Persist);
            }, a => a.OneToMany());
        }
    }

    public class OrderDetailMap : ClassMapping<OrderDetail>
    {
        public OrderDetailMap()
        {
            Schema("northwind");

            Table("OrderDetails");

            Id(x => x.Id, m => m.Generator(Generators.Native));

            ManyToOne(x => x.Product, m =>
            {
                m.Cascade(Cascade.All);
                m.Column("OrderId");
            });

            Property(x => x.Quantity);
        }
    }

    public class ProductMap : ClassMapping<Product>
    {
        public ProductMap()
        {
            Schema("northwind");

            Table("Products");

            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.ProductName);
            Property(x => x.Description, m =>
            {
                m.Length(8000);
            });
            Property(x => x.Price, m =>
            {
                m.Column("price");
                m.Type(NHibernateUtil.Currency);
            });
            Property(x => x.Image, m =>
            {
                m.Length(200);
            });
        }
    }

}
