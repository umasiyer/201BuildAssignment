<%@ Page Title="Add Architect" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddArchitect.aspx.cs" Inherits="MT.CSGPortal.UserInterface.AddArchitect" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function backToResults() {

            window.history.go(<%=pageNumber*(-1) %>)
            return false;
        }
    </script>
    <%--  <!-- Left -->

    <div class="container-left">
        <div class="col-sm-2">
            <div class="bs-example">
                <div class="list-group">
                    <a href="Search.aspx" class="list-group-item">Search
                    </a>
                    <a href="SyncDetails.aspx" class="list-group-item">Synchronise
                    </a>
                    <a href="AddArchitect.aspx" class="list-group-item active">AddArchitect
                    </a>
                </div>
            </div>

        </div>
    </div>--%>

    <!-- Center -->

    <div class="container-center">
        <div class="col-sm-7">

            <h2>Manage Mind</h2>
            <!-- Architect details container -->
            <asp:Label ID="lblFormMessage" runat="server" Text=""></asp:Label>&nbsp;&nbsp; &nbsp; &nbsp;
            <br />
            <asp:LinkButton ID="btnLnkGoBack" runat="server" OnClick="btnBack_Click" CausesValidation="false">Go Back</asp:LinkButton>
            <div id="AddArchitectForm" class="well">

                <div id="responseMsgs">

                    <fieldset aria-orientation="vertical">
                        <legend>Architect details</legend>
                        <!-- MID -->
                        <div class="form-group">
                            <asp:Label ID="lblMidErr" runat="server" Text="" class="col-lg-4 control-label"></asp:Label>
                            <div class="col-lg-8">
                                <asp:RequiredFieldValidator ID="mIdRequiredFieldValidator" runat="server" ErrorMessage="Enter MID !" ControlToValidate="txtMID" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                            <asp:Label ID="lblMID" runat="server" Text="MID" class="col-lg-4 control-label" Font-Bold="true"></asp:Label>
                            <div class="col-lg-8">
                                <asp:TextBox ID="txtMID" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="clearfix"></div>

                        <!-- Name -->
                        <div class="form-group">
                            <asp:Label ID="lblNameErr" runat="server" Text="" class="col-lg-4 control-label"></asp:Label>
                            <div class="col-lg-8">
                                <asp:RequiredFieldValidator ID="nameRequiredFieldValidator" runat="server" ErrorMessage="Enter Name !" ControlToValidate="txtName" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                            <asp:Label ID="lblName" runat="server" Text="Name" class="col-lg-4 control-label" Font-Bold="true"></asp:Label>

                            <div class="col-lg-8">
                                <asp:TextBox ID="txtName" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="clearfix"></div>

                        <!-- Professional summary -->
                        <div class="form-group">
                            <asp:Label ID="lblProfessionalSummary" runat="server" Text="Professional Summary" class="col-lg-4 control-label" Font-Bold="true"></asp:Label>
                            <div class="col-lg-8">
                                <asp:TextBox ID="txtProfessionalSummary" runat="server" class="form-control" Rows="4" TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                                <span class="help-block">Max. of 200 Characters</span>
                            </div>
                        </div>



                        <!-- Exp in months -->
                        <div class="form-group">
                            <asp:Label ID="lblExpErr" runat="server" Text="" class="col-lg-4 control-label"></asp:Label>
                            <div class="col-lg-8">
                                <asp:RequiredFieldValidator ID="expRequiredFieldValidator" runat="server" ErrorMessage="Enter Experience In Months !" ForeColor="Red" Display="Dynamic" ControlToValidate="txtExperienceInMonths"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="expRegularExpressionValidator" runat="server" ErrorMessage="Enter Valid Number of Months !" ControlToValidate="txtExperienceInMonths" ForeColor="Red" ValidationExpression="[0-9]*" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>
                            <asp:Label ID="lblExperienceInMonths" runat="server" Text="Experience In Months" class="col-lg-4 control-label" Font-Bold="true"></asp:Label>
                            <div class="col-lg-8">
                                <asp:TextBox ID="txtExperienceInMonths" runat="server" class="form-control" MaxLength="8"></asp:TextBox>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="clearfix"></div>


                        <!-- Designation -->
                        <div class="form-group">
                            <asp:Label ID="lblDesignation" runat="server" Text="Designation" class="col-lg-4 control-label" Font-Bold="true"></asp:Label>
                            <div class="col-lg-8">
                                <asp:TextBox ID="txtDesignation" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="clearfix"></div>


                        <!-- Base Location -->
                        <div class="form-group">
                            <asp:Label ID="lblBaseLocation" runat="server" Text="Base Location" class="col-lg-4 control-label" Font-Bold="true"></asp:Label>
                            <div class="col-lg-8">
                                <asp:TextBox ID="txtBaseLocation" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="clearfix"></div>


                        <!-- Qualification -->
                        <div class="form-group">
                            <asp:Label ID="lblQualification" runat="server" Text="Qualification" class="col-lg-4 control-label" Font-Bold="true"></asp:Label>
                            <div class="col-lg-8">
                                <asp:TextBox ID="txtQualification" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="clearfix"></div>


                        <!-- Joined Date -->
                        <div class="form-group">
                            <asp:Label ID="lblJoinedDate" runat="server" Text="Joined Date" class="col-lg-4 control-label" Font-Bold="true"></asp:Label>
                            <div class="col-lg-8">
                                <asp:TextBox ID="txtJoinedDate" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="clearfix"></div>


                        <!-- Extension num -->
                        <div class="form-group">
                            <%--                            <asp:Label ID="lblExtensionNumberErr" runat="server" Text="" class="col-lg-4 control-label"></asp:Label>
                            <div class="col-lg-8">
                                <asp:RegularExpressionValidator ID="extensionNumberRegularExpressionValidator" runat="server" ErrorMessage="Phone Number seems to be invalid !" ControlToValidate="txtExtensionNumber" ForeColor="Red" ValidationExpression="^([0-9\(\)\/\+ \-A-Za-z]*)$" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>--%>
                            <asp:Label ID="lblExtensionNumber" runat="server" Text="Extension Number" class="col-lg-4 control-label" Font-Bold="true"></asp:Label>
                            <div class="col-lg-8">
                                <asp:TextBox ID="txtExtensionNumber" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="clearfix"></div>


                        <!-- Cell phone num -->
                        <div class="form-group">
                            <asp:Label ID="lblCellPhoneNumberErr" runat="server" Text="" class="col-lg-4 control-label"></asp:Label>
                            <div class="col-lg-8">
                                <asp:RegularExpressionValidator ID="cellPhoneNumberRegularExpressionValidator" runat="server" ErrorMessage="Phone Number seems to be invalid !" ControlToValidate="txtCellPhoneNumber" ForeColor="Red" ValidationExpression="^([0-9\(\)\/\+ \-]*)$" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>
                            <asp:Label ID="lblCellPhoneNumber" runat="server" Text="Cell Phone Number" class="col-lg-4 control-label" Font-Bold="true"></asp:Label>
                            <div class="col-lg-8">
                                <asp:TextBox ID="txtCellPhoneNumber" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="clearfix"></div>


                        <!-- Residence phone num -->
                        <div class="form-group">
                            <asp:Label ID="lblResidencePhoneNumberErr" runat="server" Text="" class="col-lg-4 control-label"></asp:Label>
                            <div class="col-lg-8">
                                <asp:RegularExpressionValidator ID="residencePhoneNumberRegularExpressionValidator" runat="server" ErrorMessage="Phone Number seems to be invalid !" ControlToValidate="txtResidencePhoneNumber" ForeColor="Red" ValidationExpression="^([0-9\(\)\/\+ \-]*)$" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>
                            <asp:Label ID="lblResidencePhoneNumber" runat="server" Text="Residence Phone Number" class="col-lg-4 control-label" Font-Bold="true"></asp:Label>
                            <div class="col-lg-8">
                                <asp:TextBox ID="txtResidencePhoneNumber" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="clearfix"></div>


                        <!-- Work email -->
                        <div class="form-group">
                            <asp:Label ID="lblReqWorkEmail" runat="server" Text="" class="col-lg-4 control-label"></asp:Label>
                            <div class="col-lg-8">
                                <asp:RequiredFieldValidator ID="workEmailRequiredFieldValidator" runat="server" ErrorMessage="Work Email is Required !" ControlToValidate="txtWorkEmail" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>

                                <asp:RegularExpressionValidator ID="workEmailRegularExpressionValidator" runat="server" ErrorMessage="Enter Valid Email !" ControlToValidate="txtWorkEmail" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>

                            </div>


                            <asp:Label ID="lblWorkEmail" runat="server" Text="Work Email" class="col-lg-4 control-label" Font-Bold="true"></asp:Label>
                            <div class="col-lg-8">
                                <asp:TextBox ID="txtWorkEmail" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="clearfix"></div>
                        </div>

                        <div class="clearfix"></div>


                        <!-- personal email -->
                        <div class="form-group">
                            <asp:Label ID="lblPersonalEMailErr" runat="server" Text="" class="col-lg-4 control-label"></asp:Label>
                            <div class="col-lg-8">
                                <asp:RegularExpressionValidator ID="personalEMailRegularExpressionValidator" runat="server" ErrorMessage="Enter Valid Email !" ControlToValidate="txtPersonalEMail" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>
                            <asp:Label ID="lblPersonalEMail" runat="server" Text="Personal EMail" class="col-lg-4 control-label" Font-Bold="true"></asp:Label>
                            <div class="col-lg-8">
                                <asp:TextBox ID="txtPersonalEMail" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <div class="clearfix"></div>

                        <!-- buttons -->
                        <div class="clearfix"></div>

                        <div class="form-group">
                            <div class="col-lg-10 col-lg-offset-2">
                                <asp:Button ID="btnBack" runat="server" class="btn btn-default" ClientIDMode="Static" Text="Back" OnClick="btnBack_Click" CausesValidation="false" />
                                <asp:Button ID="btnSaveChanges" runat="server" Text="Save Changes" class="btn btn-primary" OnClick="btnSaveChanges_Click" />

                            </div>
                        </div>
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                        <div class="clearfix"></div>
                    </fieldset>
                    <%-- #endregion--%>
                </div>
            </div>
        </div>
    </div>

    <%--  <!-- Right -->
    <div class="container-right">
        <div class="col-sm-3">
            <div class="well">
                Sample div content!
            </div>

            <div class="bs-example" style="margin-bottom: 40px;">
                <span class="label label-default">Tag1</span>
                <span class="label label-primary">Tag2</span>
                <span class="label label-success">Tag3</span>
                <span class="label label-warning">Tag4</span>
                <span class="label label-danger">Tag5</span>
                <span class="label label-info">Tag6</span>
            </div>
            <div class="well">
                Some more sample div content!
            </div>
            <div class="list-group">
                <a href="#" class="list-group-item">
                    <h4 class="list-group-item-heading">List item 1</h4>
                    <p class="list-group-item-text">Donec id elit non mi porta gravida at eget metus.</p>
                </a>
                <a href="#" class="list-group-item">
                    <h4 class="list-group-item-heading">List item 2</h4>
                    <p class="list-group-item-text">Maecenas sed diam eget risus varius blandit.</p>
                </a>
            </div>

            <div class="well">
                More and more div content!
            </div>
        </div>
    </div>--%>

    <div class="clearfix"></div>
</asp:Content>
