using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMRApp.Models
{
    class Database
    {
        private SqlConnection conn;

        public Database()
        {
            /*
            conn = new SqlConnection(@"Server = den1.mysql4.gear.host;
            Database = jac18; User Id = jac18; password = tp%ipd12");
            */
            conn = new SqlConnection(@"Server = localhost;
            Database = trip; User Id = root; password = root");
        }
    }
}
