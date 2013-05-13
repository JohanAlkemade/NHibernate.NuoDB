using NuoDb.Data.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Driver
{
    public class NuoDbClientDriver : DriverBase
    {
        public override System.Data.IDbCommand CreateCommand()
        {
            return new NuoDbCommand();
        }

        public override System.Data.IDbConnection CreateConnection()
        {
            return new NuoDbConnection();
        }

        public override string NamedPrefix
        {
            get { return "@"; }
        }

        public override bool UseNamedPrefixInParameter
        {
            get { return true; }
        }

        public override bool UseNamedPrefixInSql
        {
            get { return false; }
        }
        
        protected override void InitializeParameter(System.Data.IDbDataParameter dbParam, string name, SqlTypes.SqlType sqlType)
        {
            base.InitializeParameter(dbParam, name, sqlType);
        }
    }
}
