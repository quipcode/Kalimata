<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PolicyCreation.aspx.cs" Inherits="kalimataUI.webPages.PolicyCreation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <h3 id="Header1"></h3>
    <h2> Create a Policy </h2>
    <form id="form2" runat="server">
        <div>
            <asp:Label ID="PolicyNameLabel" runat="server" Text="Policy Name: "></asp:Label>
            <asp:TextBox ID="PolicyNameTextBox" runat="server"></asp:TextBox><br />

            <asp:Label ID="PolicyType" runat="server" Text="Choose Your Policy Types"></asp:Label>
            <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                <asp:ListItem Value="kp_collison">Collision</asp:ListItem>
                <asp:ListItem Value="kp_comprehensive">Comprehensive</asp:ListItem>
                <asp:ListItem Value="kp_liability">Liability</asp:ListItem>
                <asp:ListItem Value="kp_protected">Protected</asp:ListItem>
                <asp:ListItem Value="kp_uninsured">Uninsured</asp:ListItem>
            </asp:CheckBoxList>
            <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="CreatePolicy_OnClick"/>
        </div>
    </form>
</body>
</html>
