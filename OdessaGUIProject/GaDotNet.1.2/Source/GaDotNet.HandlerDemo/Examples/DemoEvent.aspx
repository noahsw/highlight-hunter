<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DemoEvent.aspx.cs" Inherits="GaDotNet.HandlerDemo.Examples.DemoEvent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div><h1>Event tracking demo</h1>
	
    
    	<asp:Literal ID="litResult" runat="server"></asp:Literal>
		<br />
    
    </div>
	Domain name: <asp:TextBox id="txtDomain" runat="server">diaryofaninja.com</asp:TextBox><br />
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
		Text="Log event" />
    </form>
</body>
</html>
