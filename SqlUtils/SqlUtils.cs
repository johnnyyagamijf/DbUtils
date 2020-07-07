using SQLite;
using System;

namespace DbUtils
{
    public class SqlUtils
    {
        public SqlUtils()
        {
            this._PathDb = "Data Source=" + System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }

        private string _PathDb { get; set; }

        public void ExecuteDataReader(string sql, string bdName, Action<System.Data.SQLite.SQLiteDataReader> callback)
        {
            try
            {
                using (var connection = new System.Data.SQLite.SQLiteConnection(System.IO.Path.Combine(_PathDb, $"{bdName}.db")))
                {
                    connection.Open();
                    using (var comm = new System.Data.SQLite.SQLiteCommand(connection))
                    {
                        comm.CommandText = sql;

                        using (var reader = comm.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                    callback.Invoke(reader);
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                // Log.Info("SQLiteEx", ex.Message);
            }
        }

        public bool ExecuteNonQuery(int productId, double value, string bdName)
        {
            try
            {
                using (var connection = new System.Data.SQLite.SQLiteConnection(System.IO.Path.Combine(_PathDb, $"{bdName}.db")))
                {
                    connection.Open();

                    using (var comm = new System.Data.SQLite.SQLiteCommand(connection))
                    {
                        comm.CommandText = "INSERT INTO RequestProd VALUES (@Id, @ProductId, @Value)";
                        comm.Parameters.AddWithValue("@Id", new Random().Next(new Random().Next(1, 999), 999));
                        comm.Parameters.AddWithValue("@ProductId", productId);
                        comm.Parameters.AddWithValue("@Value", value);

                        comm.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                //Log.Info("SQLiteEx", ex.Message);
                return false;
            }

            return true;
        }
    }
}