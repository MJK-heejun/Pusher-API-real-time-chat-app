using System;
using System.Data;
using System.Data.SqlClient;

namespace MyChat.Helper
{
    public class DBHelper
    {
        //Connection setting
        private static readonly string CONNECTION_STRING = System.Configuration.ConfigurationManager.ConnectionStrings["ChatDbConnection"].ConnectionString;

        public static DataSet Query(string sqlString, SqlParameter[] parameters = null)
        {

            SqlConnection conn = new SqlConnection(CONNECTION_STRING);
            try
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sqlString, conn))
                {
                    if (parameters != null)
                    {
                        foreach (SqlParameter param in parameters)
                        {
                            if ((param.Direction == ParameterDirection.InputOutput ||
                                  param.Direction == ParameterDirection.Input) &&
                                 (param.Value == null))
                            {
                                param.Value = DBNull.Value;
                            }
                            command.Parameters.Add(param);
                        }
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds, "ds");
                        command.Parameters.Clear();
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
    #region -- DB Exception --
    public class DatabaseException : Exception
    {
        public DatabaseException()
            : base() { }
        public DatabaseException(string message)
            : base(message) { }
        public DatabaseException(string message, Exception innerException)
            : base(message, innerException) { }
        public DatabaseException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
    #endregion


}