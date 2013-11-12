<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DemoPageView.aspx.cs" Inherits="GaDotNet.HandlerDemo.Examples.DemoPageView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div><h1>Page view demo</h1>
	
    
    	<asp:Literal ID="litResult" runat="server"></asp:Literal>
		<br />
    
    </div>
	Domain Name:<asp:TextBox ID="txtDomainName" runat="server">www.diaryofaninja.com</asp:TextBox><br />

    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
		Text="Log page view" />
    </form>
</body>
</html>
