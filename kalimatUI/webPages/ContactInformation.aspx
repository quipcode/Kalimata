<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactInformation.aspx.cs" Inherits="kalimataUI.webPages.ContactInformation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form2" runat="server">
        <div>
            <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList><br />
            <label>Full Name: </label><asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><b />
            <label>Country: </label><asp:TextBox ID="TextBox2" runat="server"></asp:TextBox><b />
            <label>Preferred Currency: </label><asp:TextBox ID="TextBox3" runat="server"></asp:TextBox><b />
            <asp:Button ID="Button1" runat="server" Text="Display Information" OnClick="Update_OnClick"/><br />
            <asp:Button ID="Button2" runat="server" Text="View Policies" OnClick="NewPage_OnClick" />
            
        </div>
    </form>
</body>
</html>
