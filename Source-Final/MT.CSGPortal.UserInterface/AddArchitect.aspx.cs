using MT.CSGPortal.Portable.Entities;
using System;
using System.Collections.Generic;
using MT.CSGPortal.BL;
using MT.CSGPortal.Utility;
namespace MT.CSGPortal.UserInterface
{
    public partial class AddArchitect : System.Web.UI.Page
    {
        string mid = null;
        IProfileManager profileService = null;
        public static int pageNumber = 1;
        private static MindFullProfile fullProfile = null;
        private IActiveDirectoryManager manager = null;

        /// <summary>
        /// On Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            btnLnkGoBack.Visible = false;
            if (!IsPostBack)
            {
                pageNumber = 1;

                //Get the mid which is passed from the previous page
                mid = Request.QueryString["mid"];
                profileService = new ProfileManager();
                if (mid != null)
                {
                    try
                    {
                        //Fetch the full profile details of the given mid from portal
                        fullProfile = profileService.GetMindFullProfileByMid(mid);
                    }
                    catch (Exception ex)
                    {

                        lblFormMessage.Text = ex.Message;
                        lblFormMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    //If the mind is already in portal load the details according to the information fetched.
                    if (fullProfile != null)
                    {
                        lblFormMessage.Text = "You are about to update the mind details in Portal !";
                        lblFormMessage.ForeColor = System.Drawing.Color.Blue;
                        LoadDetails();
                        btnSaveChanges.Text = "Update";
                    }
                    else
                    {
                        //If mind is not added in portal, fetch the full profile details from AD

                        if (ApplicationSettingsReader.IsADMocked > 0)
                        {
                            manager = new ActiveDirectoryManagerMocked();
                        }
                        else
                        {
                            manager = new ActiveDirectoryManager();
                        }

                        try
                        {
                            fullProfile = manager.GetMindFullProfileById(mid);
                        }
                        catch (Exception ex)
                        {

                            lblFormMessage.Text = ex.Message;
                            lblFormMessage.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        //Load the details with the information fetched
                        if (fullProfile != null)
                        {
                            lblFormMessage.Text = "You are about to add a mind to Portal !";
                            lblFormMessage.ForeColor = System.Drawing.Color.Blue;
                            LoadDetails();
                            btnSaveChanges.Text = "Add";
                        }

                    }
                }
            }

        }


        /// <summary>
        /// Method to load details from the mindfullprofile entity
        /// </summary>
        /// <param name="fullProfile"></param>
        private void LoadDetails()
        {

            txtMID.Text = fullProfile.MindDetails.MID;
            txtName.Text = fullProfile.MindDetails.Name;
            txtExperienceInMonths.Text = fullProfile.MindDetails.ExperienceInMonths.ToString();
            txtBaseLocation.Text = fullProfile.MindDetails.BaseLocation;
            txtDesignation.Text = fullProfile.MindDetails.Designation;
            txtJoinedDate.Text = string.Format("{0} {1} {2}", fullProfile.MindDetails.JoinedDateDD == 0 ? string.Empty : fullProfile.MindDetails.JoinedDateDD.ToString(), (fullProfile.MindDetails.JoinedDateMM > 0 && fullProfile.MindDetails.JoinedDateMM < 13) ? System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(fullProfile.MindDetails.JoinedDateMM) : string.Empty, fullProfile.MindDetails.JoinedDateYYYY == 0 ? string.Empty : fullProfile.MindDetails.JoinedDateYYYY.ToString());
            txtProfessionalSummary.Text = fullProfile.MindDetails.ProfessionalSummary;
            txtQualification.Text = fullProfile.MindDetails.ProfessionalSummary;
            IEnumerable<MindContact> mindContact = fullProfile.MindContacts;
            if (mindContact != null)
            {
                foreach (var item in mindContact)
                {
                    if (item.MindContactType.ContactTypeId == 1)
                    {
                        txtExtensionNumber.Text = item.ContactText;

                    }
                    else if (item.MindContactType.ContactTypeId == 2)
                    {
                        txtCellPhoneNumber.Text = item.ContactText;

                    }
                    else if (item.MindContactType.ContactTypeId == 3)
                    {
                        txtResidencePhoneNumber.Text = item.ContactText;
                    }
                    else if (item.MindContactType.ContactTypeId == 4)
                    {
                        txtWorkEmail.Text = item.ContactText;
                    }
                    else if (item.MindContactType.ContactTypeId == 5)
                    {
                        txtPersonalEMail.Text = item.ContactText;
                    }
                }
            }

        }


        /// <summary>
        /// On Add/Update button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            profileService = new ProfileManager();

            int expInMonths = 0;
            //Returning if the experience entered is not an integer
            if (!(Int32.TryParse(txtExperienceInMonths.Text.Trim(), out expInMonths)))
            {
                lblFormMessage.Text = "Enter Valid Month !";
                lblFormMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
            //Returning if workemail is empty or null
            if (String.IsNullOrEmpty(txtWorkEmail.Text.Trim()))
            {

                lblFormMessage.Text = "Work Email is Required !";
                lblFormMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
            //Saving the AddArchitect object with the details entered
            MT.CSGPortal.Entities.AddArchitect profile = new Entities.AddArchitect()
    {
        MID = txtMID.Text.Trim(),
        Name = txtName.Text.Trim(),
        PersonalEMail = txtPersonalEMail.Text.Trim(),
        ProfessionalSummary = txtProfessionalSummary.Text.Trim(),
        Qualification = txtQualification.Text.Trim(),
        ResidencePhoneNumber = txtResidencePhoneNumber.Text.Trim(),
        WorkEmail = txtWorkEmail.Text.Trim(),
        ExtensionNumber = txtExtensionNumber.Text.Trim(),
        ExperienceInMonths = expInMonths,
        Designation = txtDesignation.Text.Trim(),
        CellPhoneNumber = txtCellPhoneNumber.Text.Trim(),
        BaseLocation = txtBaseLocation.Text.Trim(),
    };

            profile.MindDetails.JoinedDateDD = fullProfile.MindDetails.JoinedDateDD;
            profile.MindDetails.JoinedDateMM = fullProfile.MindDetails.JoinedDateMM;
            profile.MindDetails.JoinedDateYYYY = fullProfile.MindDetails.JoinedDateYYYY;

            MindFullProfile newFullProfile = new MindFullProfile()
            {
                MindDetails = profile.MindDetails,
                MindContacts = profile.MindContacts
            };
            IProfileManager existingProfile = new ProfileManager();
            bool isPresent = false;
            //Check if the mind is in portal
            try
            {

                isPresent = existingProfile.IsMindInPortal(profile.MID);
                profileService.ManageMindProfile(newFullProfile);
            }
            catch (Exception ex)
            {

                lblFormMessage.Text = ex.Message;
                lblFormMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }
            //Change the lblFormMessage accoding to whether the mind is in portal or not
            if (isPresent == true)
                lblFormMessage.Text = "Succesfully Updated !";
            if (isPresent == false)
                lblFormMessage.Text = "Succesfully Added !";
            lblFormMessage.ForeColor = System.Drawing.Color.Green;
            btnSaveChanges.Text = "Update";
            if (IsPostBack)
                pageNumber++;

            btnLnkGoBack.Visible = true;

        }


        /// <summary>
        /// Method to get the pageNumber
        /// </summary>
        /// <returns></returns>
        public int GetPageNumber()
        {
            return pageNumber;
        }


        /// <summary>
        /// On back button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Session["prevPage"] = "AddArchitect";
            Response.Redirect("Search.aspx");
        }

    }
}