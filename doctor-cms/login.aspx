<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs"  Inherits="SunStar_CMS.admin.login"  Title="SUN Star CMS" %>


<html>
<head runat="server">
    <title>内容管理系统</title>
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="style/stylesheet.css" type="text/css" />
    <script language="javascript" src="script/common.js"></script>
</head>
<body style="background-color:#FFFFFF; margin-left:0; margin-top:0">
    <form id="form1" runat="server">
    <table style="width:100%;height:100%" border="0" cellpadding="1" cellspacing="0">
		<tr>
			<td style="height:300" align="center" valign="middle">
				<table width=500px  cellpadding="2" cellspacing="2"  class="tableline1">
					<tr>
						<td >&nbsp;</td>
					</tr>
					<tr>
						<td  align="center" class="caption">
                            <asp:Label ID="lblSiteName" runat="server" Text=""></asp:Label> - 文本管理系统</td>
					</tr>
					<tr>
						<td  align="center" class="bigtitle">网站/管理中心</td>
					</tr>
					<tr>
					    <td align="center" >
					        <table>
					            <tr>
						            <td style="width:100" class="style7" align="right">登录名: </td>
						            <td style="width:200">
                                        <asp:TextBox ID="txtUsername" runat="server" CssClass="style7"></asp:TextBox></td>
					            </tr>
					            <tr>
						            <td style="width:100" class="style7" align="right">密码: </td>
						            <td style="width:200">
                                        <asp:TextBox ID="mskPassword" runat="server" CssClass="style7" MaxLength="12" TextMode="Password"></asp:TextBox></td>
					            </tr>
					            <tr>
						            <td>&nbsp;</td>
						            <td align="left">
                                        <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="登录" CssClass="button1" />
                                        <asp:Label ID="lblError" runat="server" ForeColor="#C00000"></asp:Label></td>
					            </tr>
					            <tr>
						            <td colspan="2">&nbsp;<asp:Label ID="lblCapsLock" runat="server" ForeColor="Red" Text="你开启了大小写转换按钮"></asp:Label></td>
					            </tr>
					        </table>
					    </td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
    </form>
</body>
</html>
