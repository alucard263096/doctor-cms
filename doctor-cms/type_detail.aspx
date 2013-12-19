<%@ Page Language="C#" MasterPageFile="~/admin/Base.Master" AutoEventWireup="true" CodeBehind="type_detail.aspx.cs" Inherits="SunStar_CMS.admin.type_detail" Title="无标题页" %>
<%@ MasterType virtualpath="~/admin/Base.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phMainContent" runat="server">
    <table style="width:100%" cellpadding="1" cellspacing="2" border="0">
			<tr>
				<td style="width:136px"><img src="image/spacer.gif" alt="spacer" style="width:150; border-width:0" height="10" /></td>
				<td style="width:40%">&nbsp;</td>
				<td style="width:150"><img src="image/spacer.gif" alt="spacer" style="width:150; border-width:0" height="10" /></td>
				<td style="width:40%">&nbsp;</td>
			</tr>
			<tr>
				<td class="bigtitle" colspan="2">
                    <asp:Label ID="lblTitle" runat="server" Text="类型管理"></asp:Label></td>
				<td align="right" colspan="2">
                    <asp:Button ID="btnNew" runat="server" CssClass="button1" OnClick="btnNew_Click"
                        Text="新建" />
                    <asp:Button ID="btnSaveNext" runat="server" Text="保存并继续添加" CssClass="button1" OnClick="btnSaveNext_Click" />
                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="button1" OnClick="btnSave_Click" />
                    <asp:Button ID="btnQuit" runat="server" Text="后退" CssClass="button1" OnClick="btnQuit_Click" />
				</td>
			</tr>
			<tr><td colspan="4" style="height: 33px"><hr /></td></tr>
			<tr>
				<td class="text" style="width: 136px">中文名: </td>
				<td><asp:TextBox ID="txtChnName" runat="server" CssClass="forminput" Width="200px"></asp:TextBox></td>
				<td class="text">英文名: </td>
				<td><asp:TextBox ID="txtEngName" runat="server" CssClass="forminput" Width="200px"></asp:TextBox></td>
			</tr>
			<tr>
				<td class="text">状态: </td>
				<td><asp:DropDownList ID="ddlStatus" runat="server">
                    </asp:DropDownList></td>
			</tr>
			<tr>
				<td class="text" style="width: 136px">备注: </td>
				<td class="text" colspan="3">
                    <asp:TextBox ID="txtRemarks" CssClass="forminput" runat="server" TextMode="MultiLine" Columns="100" Rows="3"></asp:TextBox></td>
			</tr>
	</table>
</asp:Content>
