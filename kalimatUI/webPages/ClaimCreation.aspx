<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClaimCreation.aspx.cs" Inherits="kalimataUI.webPages.ClaimCreation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form2" runat="server">
        <div>
            <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="Submit Claim" OnClick="Button1_Click"/>
        </div>
    </form>
</body>
</html>
