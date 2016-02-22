<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MEFDemoSimple.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="float:left;">
            Input text<br />
            <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Width="350px" Height="175px"></asp:TextBox>
        </div>
        <div style="float:left;padding: 0 20px 0 20px;">
            <div>
                Operations<br />
                <asp:ListBox ID="ListBox1" runat="server" Height="175px" Width="150px" OnDataBinding="ListBox1_DataBinding"></asp:ListBox>
            </div>
            <div style="clear:both"></div>
            <div>
                <asp:Button ID="Button1" runat="server" Text="Execute" OnClick="Button1_Click" />
            </div>
        </div>
        <div style="float:left;">
            Results<br />
            <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" Width="350px" Height="175px"></asp:TextBox>
        </div>
    </div>
    </form>
</body>
</html>
