<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PolicyDisplay.aspx.cs" Inherits="kalimataUI.webPages.PolicyDisplay" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form2" runat="server">
        <div>
            <h3>Policy Display</h3>
            <h3>Dea Tacita</h3>
            <asp:Table ID="Table1" runat="server" CellPadding="10"  GridLines="Both" HorizontalAlign="Center">
               <asp:TableRow>
                   <asp:TableCell ID="kpLiability">Liability</asp:TableCell>
                   <asp:TableCell ID="kpCollision">Collision</asp:TableCell>
                   <asp:TableCell ID="kpComprehensive">Comprehensive</asp:TableCell>
                   <asp:TableCell ID="kpProtected">Protected</asp:TableCell>
                   <asp:TableCell ID="kpUninsured">Uninsured</asp:TableCell>
                   <asp:TableCell ID="kpPremium">Premium</asp:TableCell>
                   <asp:TableCell ID="kpPolicyNumber">Policy Number</asp:TableCell>
                   <asp:TableCell ID="kpPolicy">Policy</asp:TableCell>
                   <asp:TableCell ID="kpPolicyHolder">Policy Holder</asp:TableCell>
                   <asp:TableCell ID="kpCurrency">Currency</asp:TableCell>
               </asp:TableRow>
            </asp:Table>

        </div>
        <div>
            <asp:Button ID="Button1" runat="server" Text="Create Policy" OnClick="PolicyCreatePage_OnClick"/>

        </div>
        <div>
            <asp:Button ID="Button3" runat="server" Text="Claim Creation" OnClick="CreateClaim_OnClick" />
        </div>

          <div>
          <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
            <asp:Button ID="Button2" runat="server" Text="View Claims" OnClick="ViewSpecificClaim"/>
        </div>
    </form>

      
    
</body>
</html>
