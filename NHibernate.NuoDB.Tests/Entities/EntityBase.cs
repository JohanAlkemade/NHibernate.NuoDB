using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.NuoDB.Tests.Entities
{
    public class EntityBase
    {

        private int? _oldHashCode;

        public virtual int Id { get; set; }

        public override int GetHashCode()
        {
            // Once we have a hash code we'll never change it
            if (_oldHashCode.HasValue)
                return _oldHashCode.Value;

            bool thisIsTransient = Id == 0;

            // When this instance is transient, we use the base GetHashCode()
            // and remember it, so an instance can NEVER change its hash code.
            if (thisIsTransient)
            {
                _oldHashCode = base.GetHashCode();
                return _oldHashCode.Value;
            }
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as EntityBase;
            if (other == null)
                return false;

            // handle the case of comparing two NEW objects
            bool otherIsTransient = other.Id == 0;
            bool thisIsTransient = Id == 0;
            if (otherIsTransient && thisIsTransient)
                return ReferenceEquals(other, this);


            return other.Id.Equals(Id);

        }


    }
}
