using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilesDepositoryApplication.Models.SqlRepository
{
    public partial class SqlRepository : IRepository
    {
        public SqlRepositoryDataContext Db { get; set; }

        public SqlRepository(SqlRepositoryDataContext context)
        {
            Db = context;
        }

        public SqlRepository() : this(new SqlRepositoryDataContext())
        {
            
        }
    }
}