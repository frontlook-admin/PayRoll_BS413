<%@ Page Title="Salary Info" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Salinfo.aspx.cs" Inherits="Salinfo" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="Server">
    <div style="padding-left: 100px">
        <h2><%: Title %></h2>
    </div>


    <div class=" row">
        <div class="col-sm-4"></div>
        <div>

            <div class=" form-horizontal">
                <div class=" row">
                    <br />
                    <div>
                        
                        <asp:Panel id="add_sec_salinfo_panel" runat="server" visible="True" class="form-horizontal" wrap="True">
                            <div class="row">
                                <div class="col-sm-2" style="padding-left:265px "></div>
                                <div class="form-group">
                                    <asp:Label ForeColor="Black" Height="21px" runat="server" AssociatedControlID="emp" CssClass="col-md-4 control-label" Text="Select Code" Width="155px" />

                                    <asp:DropDownList runat="server" ID="emp" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="emp_SelectedIndexChanged" />

                                </div>
                            </div>
                            <div id="add_sec_salinfo" runat="server" visible="True" class="form-horizontal" wrap="True">
                            </div>
                            <br />
                            <div class="form-horizontal" wrap="True">
                                <div class="row">
                                    <div class="col-sm-4"></div>
                                    <div style="padding-left: 60px; text-anchor: middle; horiz-align: center">
                                        <asp:Button BackColor="#0066FF" BorderStyle="None" Font-Bold="True" CssClass="form-control" ForeColor="White" ID="update_salinfo" ClientIDMode="Inherit" runat="server" value Text="Submit" OnClick="update_salinfo_Click"/>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            BindControls();
        });

        // CALL A WEB METHOD TO SAVE DATA USING AJAX.
        function BindControls() {
            $('#update_salinfo').click(function () {
                saveTextValue();
            });

            var values = new Array();

            function saveTextValue() {

                // WHILE CREATING THE TEXTBOXES THROUGH A CODE PROCEDURE, 
                // WE HAVE ASSIGNED A CLASS NAME CALLED "fld" TO EACH TEXTBOX.
                // USING THE CLASS NAME, WE CAN EASILY EXTRACT VALUES FROM THE INPUT BOXES.
                $('.textfield').each(function () {
                    if (this.value != '') {
                        values.push("'" + this.value + "'");
                    }
                });

                if (values != '') {
                    // ONCE WE HAVE ALL THE VALUES, MAKE THE CALL TO OUR WEB METHOD.
                    $.ajax({
                        type: 'POST',
                        url: 'http://localhost:58202/Salinfo',
                        data: "{'val':'" + escape(values) + "'}",
                        dataType: 'json',
                        headers: { "Content-Type": "application/json" },
                        success: function (response) {
                            alert(response.d);      // DONE.
                            values = '';
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(errorThrown);
                        }
                    });
                }
                else { alert("Fields cannot be empty.") }
            }
        }
    </script>
</asp:Content>

