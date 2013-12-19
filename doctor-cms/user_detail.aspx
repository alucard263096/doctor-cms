<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="user_detail.aspx.cs"  Inherits="SunStar_CMS.admin.user_detail"  Title="SUN Star CMS"  %>
<%@ MasterType virtualpath="~/Base.Master" %>
<%@ Register TagPrefix="uc" TagName="CheckBoxList" Src="~/UserControls/CheckBoxList.ascx" %>
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
                    <asp:Label ID="lblTitle" runat="server" Text="�û�����"></asp:Label></td>
				<td align="right" colspan="2">
                    <asp:Button ID="btnNew" runat="server" CssClass="button1" OnClick="btnNew_Click"
                        Text="�½�" />
                    <asp:Button ID="btnSaveNext" runat="server" Text="���沢�������" CssClass="button1" OnClick="btnSaveNext_Click" />
                    <asp:Button ID="btnSave" runat="server" Text="����" CssClass="button1" OnClick="btnSave_Click" />
                    <asp:Button ID="btnQuit" runat="server" Text="����" CssClass="button1" OnClick="btnQuit_Click" />
				</td>
			</tr>
			<tr><td colspan="4" style="height: 33px"><hr /></td></tr>
			<tr>
				<td class="text" style="width: 136px">��¼��: </td>
				<td><asp:TextBox ID="txtLoginID" runat="server" CssClass="forminput" Width="200px"></asp:TextBox></td>
				<td class="text">�û�����: </td>
				<td><asp:TextBox ID="txtUserName" runat="server" CssClass="forminput" Width="200px"></asp:TextBox></td>
			</tr>
            <asp:PlaceHolder ID="phResetPassword" runat="server"></asp:PlaceHolder>
			<tr>
				<td class="text" style="width: 136px">�����ʼ�: </td>
				<td><asp:TextBox ID="txtEmail" runat="server" CssClass="forminput" Width="200px"></asp:TextBox></td>
				<td class="text">״̬: </td>
				<td><asp:DropDownList ID="ddlStatus" runat="server">
                    </asp:DropDownList></td>
			</tr>
			<tr>
				<td class="text" style="width: 136px">��ע: </td>
				<td class="text" colspan="3">
                    <asp:TextBox ID="txtRemarks" CssClass="forminput" runat="server" TextMode="MultiLine" Columns="100" Rows="3"></asp:TextBox></td>
			</tr>
			<tr>
				<td class="textbold" style="width: 136px">Ȩ��</td>
				<td class="text" colspan="3" align="right">
					������:&nbsp;&nbsp;
					<asp:DropDownList ID="ddlAccessRight" runat="server">
                    </asp:DropDownList>&nbsp;&nbsp;
					<asp:Button ID="btnCopy" runat="server" Text="Copy" CssClass="button1" OnClick="btnCopy_Click" />&nbsp;
				</td>
			</tr>
			<tr>
				<td colspan="4">
					<uc:CheckBoxList ID="ucblAccessRight" Column="4" runat="server" />
				</td>
			</tr>
		</table>
</asp:Content>
