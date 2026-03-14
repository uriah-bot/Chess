using System.Data;
using System.Data.OleDb;

namespace Chess.Data
{
    // windows only (bc of OleDB)
    public static class DbConnectionProvider
    {
        private static string ConnectionString;

        public static OleDbConnection GetConnection()
        {
            if (ConnectionString == null)
            {
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "");
                ConnectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=True;";
            }
            return new OleDbConnection(ConnectionString);
        }

        public static async Task<DataTable> ExecuteQueryAsync(string sql, params OleDbParameter[] parameters)
        {
            using (var con = GetConnection())
            using (var cmd = new OleDbCommand(sql, con))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                await con.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return dt;
                }
            }
        }

        public static async Task<int> ExecuteCommandAsync(string sql, params OleDbParameter[] parameters)
        {
            using (var con = GetConnection())
            using (var cmd = new OleDbCommand(sql, con))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                await con.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
