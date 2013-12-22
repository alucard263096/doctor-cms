<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="event_detail.aspx.cs" Inherits="doctor_cms.event_detail" %>
<%@ MasterType virtualpath="~/Base.Master" %>
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
                    <asp:Label ID="lblTitle" runat="server" Text="活动管理"></asp:Label></td>
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
				<td class="text" style="width: 136px">标题: </td>
				<td><asp:TextBox ID="txtTitle" runat="server" CssClass="forminput" Width="200px"></asp:TextBox></td>
				<td class="text">状态: </td>
				<td><asp:DropDownList ID="ddlStatus" runat="server">
                    </asp:DropDownList></td>
			</tr>
			<tr>
				<td class="text" style="width: 136px">简介: </td>
				<td colspan="3"><asp:TextBox ID="txtSummary" runat="server" TextMode="MultiLine" Columns="100" Rows="3" MaxLength="80" CssClass="forminput" Width="200px"></asp:TextBox></td>
			</tr>
			<tr>
				<td class="text" style="width: 136px">图片: </td>
				<td ><asp:FileUpload ID="imageupload" runat="server" /></td>
				<td colspan="2"><asp:Image ID="imgurl" runat="server" Width="100px" /></td>
			</tr>
			<tr>
				<td class="text" style="width: 136px">正文: </td>
				<td class="text" colspan="3">
                    <asp:TextBox ID="txtContent" CssClass="forminput" runat="server" TextMode="MultiLine" Columns="100" Rows="10"></asp:TextBox></td>
			</tr>
			<tr>
				<td class="text" style="width: 136px">发布时间: </td>
				<td><asp:TextBox ID="txtPublishedDate" runat="server" CssClass="forminput" Width="200px"></asp:TextBox>(yyyy-mm-dd hh:mm:ss)</td>
			</tr>
	</table>
</asp:Content>
