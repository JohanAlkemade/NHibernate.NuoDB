using NHibernate.Dialect.Function;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Dialect
{
    public class NuoDbDialect : Dialect
    {
        public NuoDbDialect()
        {
            RegisterCharacterTypeMappings();
            RegisterNumericTypeMappings();
            RegisterDateTimeTypeMappings();
            RegisterLargeObjectTypeMappings();
            RegisterFunctions();
        }

        private void RegisterFunctions()
        {
            RegisterFunction("concat", new VarArgsSQLFunction(NHibernateUtil.String, "", " || ", ""));
            RegisterFunction("now", new NoArgSQLFunction("now", NHibernateUtil.DateTime));
            RegisterFunction("length", new StandardSQLFunction("CHAR_LENGTH", NHibernateUtil.Int32));
            RegisterFunction("substring", new EmulatedLengthSubstringFunction());
        }

        private void RegisterLargeObjectTypeMappings()
        {
            RegisterColumnType(DbType.Binary, "BINARY VARYING(8000)");
        }

        private void RegisterDateTimeTypeMappings()
        {
            RegisterColumnType(DbType.Time, "TIME");
            RegisterColumnType(DbType.Date, "DATE");
            RegisterColumnType(DbType.DateTime, "DATE");
        }

        private void RegisterNumericTypeMappings()
        {
            RegisterColumnType(DbType.Boolean, "BOOLEAN");
            RegisterColumnType(DbType.Byte, "SMALLINT");
            RegisterColumnType(DbType.Decimal, "DECIMAL(19,5)");
            RegisterColumnType(DbType.Decimal, 19, "DECIMAL($p, $s)");
            RegisterColumnType(DbType.Double, "DOUBLE PRECISION"); //synonym for FLOAT(53)
            RegisterColumnType(DbType.Int16, "SMALLINT");
            RegisterColumnType(DbType.Int32, "INTEGER");
            RegisterColumnType(DbType.Int64, "BIGINT");
            RegisterColumnType(DbType.Single, "REAL"); //synonym for FLOAT(24)
        }

        private void RegisterCharacterTypeMappings()
        {
            RegisterColumnType(DbType.AnsiStringFixedLength, "CHARACTER(255)");
            RegisterColumnType(DbType.AnsiStringFixedLength, 8000, "CHARACTER($l)");
            RegisterColumnType(DbType.AnsiString, "CHARACTER VARYING(255)");
            RegisterColumnType(DbType.AnsiString, 8000, "CHARACTER VARYING($1)");
            RegisterColumnType(DbType.String, "STRING");
        }

        public override bool SupportsIdentityColumns
        {
            get { return true; }
        }

        public override bool SupportsSequences
        {
            get
            {
                return false;
            }
        }

        public override InsertGeneratedIdentifierRetrievalMethod InsertGeneratedIdentifierRetrievalMethod
        {
            get
            {
                return InsertGeneratedIdentifierRetrievalMethod.ReturnValueParameter;
            }
        }

        public override string IdentityColumnString
        {
            get
            {
                return "GENERATED ALWAYS AS IDENTITY";
            }
        }

        public override bool SupportsInsertSelectIdentity
        {
            get
            {
                return true;
            }
        }
        public override string GetIdentitySelectString(string identityColumn, string tableName, DbType type)
        {
            return "";
        }

        public override string CurrentTimestampSQLFunctionName
        {
            get
            {
                return "NOW";
            }
        }

        public override string CurrentTimestampSelectString
        {
            get
            {
                return "SELECT NOW()";
            }
        }

        public override bool SupportsCurrentTimestampSelection
        {
            get
            {
                return true;
            }
        }
                
        public override string GetDropTableString(string tableName)
        {
            return base.GetDropTableString(tableName) + " IF EXISTS";
        }

        public override bool SupportsLimit
        {
            get
            {
                return true;
            }
        }

        public override bool SupportsLimitOffset
        {
            get
            {
                return true;
            }
        }

        public override SqlCommand.SqlString GetLimitString(SqlCommand.SqlString queryString, SqlCommand.SqlString offset, SqlCommand.SqlString limit)
        {
            var pagingBuilder = new SqlStringBuilder(queryString);

            pagingBuilder.Add(" limit ");
                     
            if (limit != null)
                pagingBuilder.Add(limit);
            else
                pagingBuilder.Add(int.MaxValue.ToString());

            if (offset != null)
            {
                pagingBuilder.Add(" offset ").Add(offset);
            }

            return pagingBuilder.ToSqlString();
        }

    }

}
