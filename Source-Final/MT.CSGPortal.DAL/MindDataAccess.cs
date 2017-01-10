
using MT.CSGPortal.Portable.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;



namespace MT.CSGPortal.DAL
{
    public class MindDataAccess :  IMindDataAccess
    {
        #region <<data access methods>>
        /// <summary>
        /// Add, edit (includes setting status to innactive) mind information in Portal DB
        /// </summary>
        /// <param name="mindProfile">Mind Details including contacts</param>
        /// <returns></returns>
        public int ManageMindProfile(MindFullProfile mindProfile)
        {
            using (DataTable contactTable = new DataTable())
            {
                if (mindProfile.MindContacts!=null)
                {
                    List<MindContact> contactList = mindProfile.MindContacts.ToList<MindContact>();
                    contactTable.Columns.Add("MID");
                    contactTable.Columns.Add("ContactTypeId");
                    contactTable.Columns.Add("ContactText");

                    foreach (var item in contactList)
                    {
                        var nextRow = contactTable.NewRow();
                        nextRow["MID"] = mindProfile.MindDetails.MID;
                        nextRow["ContactTypeId"] = item.MindContactType.ContactTypeId;
                        nextRow["ContactText"] = item.ContactText;
                        contactTable.Rows.Add(nextRow);
                    }
                }

                Type mindType = mindProfile.MindDetails.GetType();
                IList<PropertyInfo> properties = mindType.GetProperties();

                using (var conn = SqlUtility.GetConnection)
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("USP_SaveProfile", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (var item in properties)
                        {
                            string property = item.Name;
                            object value = item.GetValue(mindProfile.MindDetails);
                            cmd.Parameters.Add(new SqlParameter(property, value));
                        }
                        cmd.Parameters.Add(new SqlParameter("@contacts", contactTable));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            return 1;
        }

        /// <summary>
        /// Fetch details of all minds in portal DB
        /// </summary>
        /// <returns>Details of minds excluding contact</returns>
        public List<Mind> GetAllMinds
        {
            get
            {
                using (SqlConnection sqlConn = SqlUtility.GetConnection)
                {
                    sqlConn.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("USP_GetAllMinds", sqlConn))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader result = SqlUtility.ExecuteDataReader(sqlCommand))
                        {
                            return ConvertReaderToMind(result);
                        }
                    }
                }               
            }
        }

        /// <summary>
        /// Fetch details of all minds in portal DB
        /// </summary>
        /// <returns>Details of minds excluding contact</returns>
        public List<string> GetAllMIds
        {
            get
            {
                List<string> lstMindIds = new List<string>();

                using (SqlConnection sqlConn = SqlUtility.GetConnection)
                {
                    sqlConn.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("USP_FetchMIds", sqlConn))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader result = SqlUtility.ExecuteDataReader(sqlCommand))
                        {
                            while (result.Read())
                            {
                                lstMindIds.Add(result["MID"].ToString());
                            }
                        }
                    }
                }
                return lstMindIds;
            }
        }

        /// <summary>
        /// Get contact details of a mind by MID
        /// </summary>
        /// <param name="mid">MID</param>
        /// <returns>List of mind contacts</returns>
        public List<MindContact> GetMindContactsByMid(string mid)
        {
            List<MindContact> contactList = new List<MindContact>();
            using (SqlConnection sqlConn = SqlUtility.GetConnection)
            {
                sqlConn.Open();
                using(SqlCommand sqlCommand = new SqlCommand("USP_FetchMindContacts", sqlConn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader result = SqlUtility.ExecuteDataReader(sqlCommand, new SqlParameter("@mid", mid)))
                    {
                        while (result.Read())
                        {
                            MindContact mindContact = new MindContact()
                            {
                                MID = result["MID"].ToString(),
                                MindContactId = Convert.ToInt32(result["ContactTypeId"]),
                                ContactText = result["ContactText"].ToString(),
                                MindContactType = new ContactType()
                                {
                                    ContactTypeId = Convert.ToInt32(result["ContactTypeId"]),
                                    Name = result["Name"].ToString()
                                }
                            };
                            contactList.Add(mindContact);
                        }
                    }
                }
            }
            return contactList;
        }

        /// <summary>
        /// Get all details of a mind from portal DB by MID
        /// </summary>
        /// <param name="id">MID</param>
        /// <returns>Mind's full profile</returns>
        public MindFullProfile GetMindFullProfileById(string id)
        {
            MindFullProfile profile = new MindFullProfile();
            profile.MindDetails = GetMindByID(id);
            if (profile.MindDetails != null)
            {
                profile.MindContacts = GetMindContactsByMid(id);
                return profile;
            }
            else
                return null;
        }

        /// <summary>
        /// Get mind details from Portal DB by MID
        /// </summary>
        /// <param name="id">MID</param>
        /// <returns>Mind details excluding contact info</returns>
        public Mind GetMindByID(string id)
        {
            Mind mind = null;
            List<Mind> mindData = null;
            using (SqlConnection sqlConn = SqlUtility.GetConnection)
            {
                sqlConn.Open();
                using(SqlCommand sqlCommand = new SqlCommand("USP_GetMindById", sqlConn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader result = SqlUtility.ExecuteDataReader(sqlCommand, new SqlParameter("@mid", id)))
                    {
                        mindData = ConvertReaderToMind(result);
                    }
                }
            }

            if (mindData.Count > 0)
            {
                mind = mindData.First();
            }
            return mind;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mindData"></param>
        /// <returns></returns>
        private static List<Mind> ConvertReaderToMind(DbDataReader mindData)
        {
            List<Mind> lstMind = new List<Mind>();
            while (mindData.Read())
            {
                Mind mind = new Mind()
                {
                    MID = mindData["MID"].ToString(),
                    Name = mindData["Name"].ToString(),
                    ProfessionalSummary = mindData["ProfessionalSummary"].ToString(),
                    ExperienceInMonths = Convert.ToInt32(mindData["ExperienceInMonths"]),
                    Designation = mindData["Designation"].ToString(),
                    BaseLocation = mindData["BaseLocation"].ToString(),
                    Qualification = mindData["Qualification"].ToString(),
                    JoinedDateDD = Convert.ToInt32(mindData["JoinedDateDD"]),
                    JoinedDateMM = Convert.ToInt32(mindData["JoinedDateMM"]),
                    JoinedDateYYYY = Convert.ToInt32(mindData["JoinedDateYYYY"]),
                    InactiveDateDD = Convert.ToString(mindData["InactiveDateDD"]).Length == 0 ? 0 : Convert.ToInt32(mindData["InactiveDateDD"]),
                    InactiveDateMM = Convert.ToString(mindData["InactiveDateMM"]).Length == 0 ? 0 : Convert.ToInt32(mindData["InactiveDateMM"]),
                    InactiveDateYYYY = Convert.ToString(mindData["InactiveDateYYYY"]).Length == 0 ? 0 : Convert.ToInt32(mindData["InactiveDateYYYY"])
                };
                lstMind.Add(mind);
            }          
            return lstMind;
        }

        /// <summary>
        /// Get the number of active architects in the portal
        /// </summary>
        /// <returns>int</returns>
        public int GetArchitectCount()
        {
            using (SqlConnection sqlConn = SqlUtility.GetConnection)
            {
                sqlConn.Open();
                using (SqlCommand sqlCommand = new SqlCommand("USP_GetArchitectCount", sqlConn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    SqlParameter outputParam = new SqlParameter("@ArchitectCount", SqlDbType.Int);
                    outputParam.Direction = ParameterDirection.Output;
                    sqlCommand.Parameters.Add(outputParam);
                    sqlCommand.ExecuteNonQuery();
                    return Convert.ToInt32(sqlCommand.Parameters["@ArchitectCount"].Value);
                }
            }
        }

        /// <summary>
        /// Checks if mind exists in portal
        /// </summary>
        /// <param name="MID"></param>
        /// <returns>True if mind exists</returns>
        public bool IsMindInPortal(string MID)
        {
            using (SqlConnection sqlConn = SqlUtility.GetConnection)
            {
                sqlConn.Open();
                using (SqlCommand cmd = new SqlCommand("USP_IsMindInPortal", sqlConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MID", MID));
                    SqlParameter outputParam = new SqlParameter("@result", SqlDbType.Int);
                    outputParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParam);
                    cmd.ExecuteNonQuery();
                    return Convert.ToInt32(cmd.Parameters["@result"].Value) > 0;
                }
            }
        }
        #endregion
    }
}

