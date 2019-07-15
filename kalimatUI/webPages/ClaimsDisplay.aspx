<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClaimsDisplay.aspx.cs" Inherits="kalimataUI.webPages.ClaimsDisplay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h3>Claims Display</h3>
            <%--<h3>Dea Tacita</h3>--%>
            <asp:Table ID="ClaimsTable" runat="server" CellPadding="10"  GridLines="Both" HorizontalAlign="Center">
               <asp:TableRow>
                   <asp:TableCell ID="kp_claimid">Claim ID</asp:TableCell>
                   <asp:TableCell ID="kp_claim">Claim Title</asp:TableCell>
                   <asp:TableCell ID="kp_claimcontact">Claim Owner ID</asp:TableCell>
                   <asp:TableCell ID="kp_claimpolicy">Policy ID</asp:TableCell>
               </asp:TableRow>
            </asp:Table>
        </div>
    </form>
</body>
</html>
