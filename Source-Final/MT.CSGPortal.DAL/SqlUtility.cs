using System.Configuration;
using System.Data.SqlClient;

namespace MT.CSGPortal.DAL
{
    public static class SqlUtility
    {
        #region Private variable
        private const string CONNECTIONKEYNAME = "CSGPortalConnection";
        #endregion
        
        #region Property
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[CONNECTIONKEYNAME].ConnectionString;
            }
        }

        public static SqlConnection GetConnection
        {
            get
            {
                return new SqlConnection(ConnectionString);
            }
        }
        #endregion
             
        #region ExecuteDataReader
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteDataReader(SqlCommand command)
        {
            return ExecuteDataReader(command, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteDataReader(SqlCommand command, SqlParameter parameters)
        {
            if (parameters != null)
                command.Parameters.Add(parameters);
            return command.ExecuteReader();
        }    
        #endregion     
    }
}