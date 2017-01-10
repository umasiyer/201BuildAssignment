<%@ Page Title="Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" ValidateRequest="false" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" CodeBehind="Search.aspx.cs" Inherits="MT.CSGPortal.UserInterface.Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="Scripts/jquery-1.9.1.js"></script>
    <script src="Scripts/jquery-ui.js"></script>
    <script type="text/javascript">
        function pageLoad() {
            $(function () {

                var icons = {

                    header: "ui-icon-circle-arrow-e",

                    activeHeader: "ui-icon-circle-arrow-s"

                };

                $("#accordion").accordion({
                    collapsible: true,
                    icons: icons,
                    active: false

                });

            });
        }

    </script>

    <!-- Center -->
    <div class="container-center">
        <h2>Manage Minds</h2>
        <div class="clearfix"></div>
        <div class="well col-sm-12" style="width: 700px;">
            <asp:Label ID="lblCountTag" runat="server" Text="Active Minds in the portal: "></asp:Label>
            <asp:Label ID="lblCount" runat="server" Text=""></asp:Label>
        </div>
        <div class="clearfix"></div>
        <!-- Search input container -->
        <asp:UpdatePanel ID="upSearchContainer" runat="server">
            <ContentTemplate>
                <div class="well col-sm-12" style="width: 700px;">
                    <asp:Label ID="lblSeacrhOption" runat="server" Text="Search in:" ForeColor="Maroon"></asp:Label>
                    &nbsp;
                        <asp:DropDownList ID="ddlSearchOption" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSearchOption_SelectedIndexChanged">
                            <asp:ListItem Value="0">AD</asp:ListItem>
                            <asp:ListItem Value="1">Portal</asp:ListItem>
                        </asp:DropDownList>
                    <div class="input-group">
                        <table>
                            <tr>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Enter MID or Name !" ControlToValidate="txtSearchString" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="searchStringRegularExpressionValidator" runat="server" ErrorMessage="Must be alphanumeric in 3-25 characters! " ForeColor="Red" ControlToValidate="txtSearchString" ValidationExpression="[0-9 a-zA-Z]{3,25}$"></asp:RegularExpressionValidator>
                                    <asp:TextBox ID="txtSearchString" class="form-control" ClientIDMode="Static" runat="server" Columns="80"></asp:TextBox>
                                </td>
                                <td>
                                    <br />
                                    <span>
                                        <asp:Button ID="btnSearch" class="btn btn-primary" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:UpdateProgress ID="UpdateProgress1" DynamicLayout="False" runat="server" AssociatedUpdatePanelID="upSearchContainer">
                                        <ProgressTemplate>
                                            Please Wait ...
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>

                <asp:Panel ID="pnlParent" runat="server" Visible="false">

                    <div class="panel-group col-sm-12" id="accordion">

                        <asp:Repeater ID="repAccordian" runat="server">

                            <ItemTemplate>

                                <h3 style="background-color: #f5f5f5; font-weight: normal; font-size: large; width: 680px; text-decoration: underline; border: 1px solid #e3e3e3">

                                    <%#Eval("Name") %></h3>

                                <div>

                                    <table style="background-color: #E8E8E8; width: 680px;">
                                        <tr>

                                            <td rowspan="1" style="position: relative; left: 10px">
                                                <asp:Image ID="Image2" ImageUrl='<%#Eval("Image") %>' Height="150" Width="160" runat="server" />
                                            </td>


                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <b>Basic Profile</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>

                                                            <b>Name</b>

                                                        </td>

                                                        <td>

                                                            <%#Eval("Name") %>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>

                                                            <b>Designation</b>

                                                        </td>

                                                        <td>

                                                            <%#Eval("Designation") %>

                                                        </td>
                                                    </tr>


                                                    <tr>

                                                        <td>
                                                            <b>Mid</b>

                                                        </td>

                                                        <td>

                                                            <%#Eval("MID") %>

                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>
                                                            <b>Joined Date</b>

                                                        </td>

                                                        <td>

                                                            <%#Eval("JoinedDate") %>

                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td>

                                                            <asp:Button ID="btnAddUpdate" runat="server" OnCommand="btnAddUpdate_Command" CommandArgument='<%#Eval("MID") %>' Text='<%#Eval("ButtonText") %>' class="btn btn-primary" />

                                                        </td>

                                                    </tr>

                                                </table>
                                            </td>

                                        </tr>

                                    </table>

                                </div>

                            </ItemTemplate>

                        </asp:Repeater>

                    </div>
                    <div style="position: relative; left: 300px;">
                        <asp:Panel ID="pnlMoreResult" runat="server">
                            <asp:LinkButton ID="btnMoreResult2" runat="server" OnClick="btnMoreResult_Click">More Result</asp:LinkButton>
                        </asp:Panel>
                    </div>
                </asp:Panel>
                <div style="position: relative; left: 325px;">
                    <asp:Label ID="lblMoreResult" runat="server" Text="" ForeColor="#666666"></asp:Label>
                    <asp:UpdateProgress ID="UpdateProgress2" DynamicLayout="true" runat="server" AssociatedUpdatePanelID="upSearchContainer" v>
                        <ProgressTemplate>
                            <div>
                                <asp:Image ID="Image1" src="Images/mainloader.gif" runat="server" Width="30" Height="30" />
                                <%--   Please wait....--%>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    <div class="clearfix"></div>
</asp:Content>
