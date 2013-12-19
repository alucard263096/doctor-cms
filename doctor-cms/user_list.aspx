<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="user_list.aspx.cs"  Inherits="SunStar_CMS.admin.user_list"  Title="SUN Star CMS"  %>
<%@ MasterType virtualpath="~/Base.Master" %>
<%@ Register TagPrefix="uc" TagName="DataListing" Src=".\UserControls\cDataListing.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phMainContent" runat="server">
        <table style="width:100%" cellpadding="1" cellspacing="2" border="0">
			<tr>
				<td style="width:150"><img src="image/spacer.gif" style="width:150;border-width:0" height="10" alt="spacer" /></td>
				<td style="width:40%">&nbsp;</td>
				<td style="width:150"><img src="image/spacer.gif" style="width:150;border-width:0" height="10" alt="spacer" /></td>
				<td style="width:40%">&nbsp;</td>
			</tr>
			<tr>
				<td class="bigtitle" colspan="2">用户管理</td>
				<td align="right" colspan="2">
                    <asp:Button ID="btnSearch" runat="server" Text="搜索" CssClass="button1" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnNew" runat="server" Text="新建" CssClass="button1" OnClick="btnNew_Click" />
                    <asp:Button ID="btnDelete" runat="server" Text="删除" CssClass="button1" OnClick="btnDelete_Click" />
				</td>
			</tr>
			<tr><td colspan="4"><hr /></td></tr>
			<tr>
				<td class="text">登录名: </td>
				<td><asp:TextBox ID="txtLoginID" runat="server" CssClass="forminput" ></asp:TextBox></td>
				<td class="text">用户名: </td>
				<td class="text"><asp:TextBox ID="txtUserName" runat="server" CssClass="forminput" ></asp:TextBox></td>
			</tr>
			<tr>
				<td class="text">状态: </td>
				<td class="text"><asp:DropDownList ID="ddlStatus" runat="server" CssClass="forminput">
                    </asp:DropDownList></td>
				<td class="text">&nbsp;</td>
				<td class="text">&nbsp;</td>
			</tr>
			<tr><td colspan="4">&nbsp;</td></tr>
			<uc:DataListing ID="ucResult" runat="server" />
		</table>
</asp:Content>