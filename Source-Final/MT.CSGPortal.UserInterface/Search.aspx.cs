using MT.CSGPortal.BL;
using MT.CSGPortal.DAL;
using MT.CSGPortal.Entities;
using MT.CSGPortal.Portable.Entities;
using MT.CSGPortal.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;


namespace MT.CSGPortal.UserInterface
{
    public partial class Search : System.Web.UI.Page
    {
        #region Properties
        List<MindBasicProfile> mindBasicProfileList = null;
        static List<ProfileSummary> profiles = new List<ProfileSummary>();
        private IActiveDirectoryManager manager = null;
        private byte pageSize = (byte)ApplicationSettingsReader.PageSizeForADSearchResult;
        static byte pageNumber = 1;
        #endregion


        /// <summary>
        /// On Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected void Page_Load(object sender, EventArgs e)
        {
            // To clear the Session on Refresh and on first time PageLoad
            if (!IsPostBack && Session["prevPage"] == null)
            {
                pageNumber = 1;
                profiles = new List<ProfileSummary>();
                Session.Remove("searchString");
                ClearSession();
            }
            Session["prevPage"] = null;

            //Disable the button for double click
            btnSearch.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSearch, null) + ";");
            pnlMoreResult.Visible = false;
            lblMoreResult.Text = string.Empty;
            lblMsg.Text = string.Empty;

            //To get the total number of Active minds in Portal
            IMindDataAccess mindDataProvider = new MindDataAccess();
            try
            {
                lblCount.Text = mindDataProvider.GetArchitectCount().ToString();
            }
            catch (Exception ex)
            {

                lblMsg.Text = ex.Message;
            }

            //Fetch Data from the session state if it is available
            if (Session["profiles"] != null && Session["ddlIndex"] != null && Session["pageNumber"] != null && Session["searchString"] != null)
            {
                ddlSearchOption.SelectedIndex = (int)Session["ddlIndex"];
                pageNumber = (byte)Session["pageNumber"];
                List<ProfileSummary> prevProfiles = Session["profiles"] as List<ProfileSummary>;
                repAccordian.DataSource = prevProfiles;
                repAccordian.DataBind();
                txtSearchString.Text = Session["searchString"] as string;
                if (Session["endOfRecords"] != null && (bool)Session["endOfRecords"] == false)
                {
                    pnlMoreResult.Visible = true;
                }
                else
                {
                    pnlMoreResult.Visible = false;
                    lblMoreResult.Text = "End of Result !";
                }
                pnlParent.Visible = true;
                ClearSession();

            }
        }

        /// <summary>
        /// method to check search string 
        /// </summary>
        /// <returns></returns>
        protected bool CheckSearchString()
        {
            //To check searchstring's  length
            if (String.IsNullOrEmpty(txtSearchString.Text.Trim()) || txtSearchString.Text.Trim().Length < 3 || txtSearchString.Text.Trim().Length > 25)
            {
                return false;
            }

            //if searchString is different then clear the list and session
            if (Session["searchString"] != null)
            {
                if (!((string)Session["searchString"]).Equals(txtSearchString.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    pageNumber = 1;
                    profiles = new List<ProfileSummary>();
                    ClearSession();
                }
            }
            //To save current searchstring in Session for future purpose
            Session["searchString"] = txtSearchString.Text.Trim();
            return true;
        }

        /// <summary>
        /// To  check Session content
        /// </summary>
        /// <returns></returns>
        protected bool CheckSession()
        {
            //if endOfRecords is reached, don't perform search and return false
            if (Session["endOfRecords"] != null && (bool)Session["endOfRecords"] == true)
            {
                lblMoreResult.Text = "End of Result !";
                return false;
            }

            //if pageNumber is same, don't perform search and return false
            if (Session["pageNumber"] != null && (byte)Session["pageNumber"] == pageNumber)
            {
                pnlMoreResult.Visible = true;
                return false;
            }

            //To save current pageNumber in Session for future purpose
            Session["pageNumber"] = pageNumber;
            return true;
        }
        /// <summary>
        /// On Search button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // if searchString is equal to textbox string then return
            if (Session["searchString"] != null)
            {
                if (((string)Session["searchString"]).Equals(txtSearchString.Text.Trim(), StringComparison.OrdinalIgnoreCase) && profiles != null && profiles.Count > 0)
                {
                    pnlMoreResult.Visible = true;
                    return;
                }
            }
            SearchMinds();
        }

        /// <summary>
        /// Method to Search mind in AD or portal
        /// </summary>
        private void SearchMinds()
        {
            if (!CheckSearchString())
            {
                return;
            }

            if (!CheckSession())
            {
                return;
            }
            SearchResult<MindBasicProfile> result = null;

            try
            {
                switch (ddlSearchOption.SelectedItem.Value)
                {
                    case "0":
                        if (ApplicationSettingsReader.IsADMocked > 0)
                        {
                            manager = new ActiveDirectoryManagerMocked();
                        }
                        else
                        {
                            manager = new ActiveDirectoryManager();
                        }

                        result = manager.Search(txtSearchString.Text.Trim(), pageNumber);
                        break;
                    case "1": IProfileManager profileManger = new ProfileManager();
                        result = profileManger.SearchMinds(txtSearchString.Text.Trim(), pageNumber);
                        break;

                }
            }
            catch (Exception ex)
            {

                lblMsg.Text = ex.Message;
            }
            if (result != null)
            {
                BindRepeater(result);
            }
        }

        /// <summary>
        /// Method to create list of Profiles
        /// </summary>
        /// <returns>a list of profilesummary</returns>
        public List<ProfileSummary> CreateProfilesList()
        {
            foreach (MindBasicProfile item in mindBasicProfileList)
            {

                if (ApplicationSettingsReader.IsADMocked > 0)
                {
                    manager = new ActiveDirectoryManagerMocked();
                }
                else
                {
                    manager = new ActiveDirectoryManager();
                }

                byte[] imgsrc = null;
                try
                {
                    imgsrc = manager.GetImage(item.Mid);
                }
                catch (Exception ex)
                {

                    lblMsg.Text = ex.Message;
                }
                string url = null;
                if (imgsrc != null)
                {
                    url = "data:image/jpg;base64," + Convert.ToBase64String((byte[])imgsrc);
                }

                string btnText = "Add/Update";
                IProfileManager profile = new ProfileManager();
                if (profile.IsMindInPortal(item.Mid) == true)
                {
                    btnText = "Update";
                }
                else
                {
                    btnText = "Add";
                }

                profiles.Add(new ProfileSummary { Name = item.Name, Designation = item.Designation, MID = item.Mid, Image = url, JoinedDate = item.JoinedDate, ButtonText = btnText });
            }

            return profiles;
        }


        /// <summary>
        /// On Add/Update button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddUpdate_Command(object sender, CommandEventArgs e)
        {
            Session["profiles"] = profiles;
            Session["ddlIndex"] = ddlSearchOption.SelectedIndex;
            Session["pageNumber"] = pageNumber;
            Session["searchString"] = txtSearchString.Text.Trim();
            //Navigate to AddArchitect page
            Response.Redirect("AddArchitect?mid=" + e.CommandArgument);
        }


        /// <summary>
        /// On More Result link button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMoreResult_Click(object sender, EventArgs e)
        {
            pageNumber++;
            SearchMinds();
        }


        /// <summary>
        /// On Selecting AD or Portal in the dropdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSearchOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pnlParent.Visible == true)
            {
                pnlParent.Visible = false;
            }
            pageNumber = 1;
            profiles = new List<ProfileSummary>();
            Session.Remove("searchString");
            ClearSession();
        }


        #region Helper Methods
        /// <summary>
        /// Method to bind the repeater
        /// </summary>
        /// <param name="result"></param>
        private void BindRepeater(SearchResult<MindBasicProfile> result)
        {
            if (result.ResultData != null)
            {
                if (result.ResultData.Count > 0)
                {
                    pnlParent.Visible = true;
                    mindBasicProfileList = result.ResultData;
                    repAccordian.DataSource = CreateProfilesList();
                    repAccordian.DataBind();

                    if (result.EndOfRecords == true)
                    {
                        pnlMoreResult.Visible = false;
                        Session["endOfRecords"] = true;
                        lblMoreResult.Text = "End of Result !";
                    }
                    else
                    {
                        Session["endOfRecords"] = false;
                        pnlMoreResult.Visible = true;
                        lblMoreResult.Text = "";
                    }
                }
                else
                {
                    if (profiles.Count > 0)
                    {
                        lblMoreResult.Text = "End of Result !";
                    }
                    else
                    {
                        repAccordian.DataSource = null;
                        repAccordian.DataBind();
                        lblMoreResult.Text = "Result Not Found !";
                    }
                }
            }
        }


        /// <summary>
        /// Method to clear the sessions
        /// </summary>
        private void ClearSession()
        {
            Session.Remove("ddlIndex");
            Session.Remove("profiles");
            Session.Remove("endOfRecords");
            //Session.Remove("searchString");
            Session.Remove("pageNumber");
        }
        #endregion
    }
}