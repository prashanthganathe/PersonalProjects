using System.Data.Entity;

namespace AngularJSAuthentication.API
{
 
        public class MySqlConfiguration : DbConfiguration
        {
            public MySqlConfiguration()
            {
                SetHistoryContext("MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, schema));
            }
        }
    
}