<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cDataListing.ascx.cs"  Inherits="SunStar_CMS.admin.UserControls.cDataListing" %>
					<table style="width:100%"><tr>
					    <td class="textbold" colspan="3"><asp:Label ID="lblListTitle" CssClass="utextbold" runat="server" Text="搜索结果:"></asp:Label>&nbsp;&nbsp;&nbsp;(<asp:TextBox ID="txtSelectedCount" runat="server" ReadOnly="true" CssClass="centerforminput" Width="40px" Text="0" IsCountField="false"></asp:TextBox><asp:Label ID="lblTotalCount" runat="server" Text="Label"></asp:Label>)</td>
					    <td class="textbold" align="right">
                            <asp:Button ID="btnPrevious" runat="server" CssClass="button1" Text=" < " OnClick="btnPrevious_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblPage" runat="server" Text="Label"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnNext" runat="server" CssClass="button1" Text=" > " OnClick="btnNext_Click" />
					    </td>
                    </tr>
					<tr>
						<td colspan="4">
                            <table border="0" cellpadding="0" cellspacing="0" style="width:100%;text-align:left;" >
                                <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
                                <asp:PlaceHolder ID="phListing" runat="server"></asp:PlaceHolder>
                                <asp:HiddenField ID="txtPreviousSortField" runat="server" />
                                <asp:HiddenField ID="txtPreviousSortOrder" runat="server" />
                                <asp:HiddenField ID="txtCurrentPage" runat="server" />
                            </table>
						</td>
					</tr></table>
<script type="text/javascript" language="javascript"> 
var intSelected = 0;
var intTotal = 0;
function cOn(tr,bcolor){ 
    if(document.getElementById||(document.all && !(document.getElementById))){ 
        tr.oldBackgroundColor = tr.style.backgroundColor;
        tr.style.backgroundColor=bcolor; 
    } 
} 
function cOff(tr,bcolor){ 
    if(document.getElementById||(document.all && !(document.getElementById))){ 
        tr.style.backgroundColor=tr.oldBackgroundColor; 
    }
}
function highlightRow (checkbox, color, bcolor, ocolor) { 
  var tr; 
  if (checkbox.parentNode) { 
    tr = checkbox.parentNode; 
    while (tr.nodeName.toLowerCase() != 'tr') 
      tr = tr.parentNode; 
  } 
  else if (checkbox.parentElement) { 
    tr = checkbox.parentElement; 
    while (tr.tagName.toLowerCase() != 'tr') 
      tr = tr.parentElement; 
  } 
  if (tr) { 
    if (checkbox.checked) { 
      intSelected++;
      tr.style.backgroundColor = color; 
      tr.oldBackgroundColor = tr.style.backgroundColor; 
    } 
    else { 
      intSelected--;
      tr.style.backgroundColor = bcolor; 
      tr.oldBackgroundColor = ocolor; 
    } 
  } 
} 

function highlightCheckRow (checkbox, color, ocolor) { 
  var tr; 
  if (checkbox.parentNode) { 
    tr = checkbox.parentNode; 
    while (tr.nodeName.toLowerCase() != 'tr') 
      tr = tr.parentNode; 
  } 
  else if (checkbox.parentElement) { 
    tr = checkbox.parentElement; 
    while (tr.tagName.toLowerCase() != 'tr') 
      tr = tr.parentElement; 
  } 
  if (tr) { 
    if (checkbox.checked) { 
      tr.style.backgroundColor = color; 
    } 
    else { 
      tr.style.backgroundColor = ocolor; 
    } 
  } 
} 

function DisplayCount(checkbox,att){
    var intSel = 0;
    var e = checkbox.form;
    var d;
	for(i=0;i<e.elements.length;i++) {
		if(e.elements[i].type=="checkbox" && e.elements[i].id.indexOf('*'+att) >= 0)
	    {
	        if (e.elements[i].checked){
	            intSel++;
	        }
	    }
		if(e.elements[i].getAttribute("IsCountField")!=null && e.elements[i].getAttribute(att)!=null) {
		    d = e.elements[i];
		}
	}
	d.value = intSel;
}

function onCheckAllWithCount(e, att) {
    intTotal = 0;
	var d = e.form;
	for(i=0;i<d.elements.length;i++) {
		if(d.elements[i].type=="checkbox" && d.elements[i]!=e && d.elements[i].id.indexOf('*'+att) >= 0)
		{
		    intTotal++;
			d.elements[i].checked = e.checked;
		}
	}
    if (e.checked){
        intSelected = intTotal;
    }else{
        intSelected = 0;
    }
    DisplayCount(e,att);
}


function onCheckAllWithHightLight(e, color, bcolor, ocolor, att) {
    intTotal = 0;
	var d = e.form;
	for(i=0;i<d.elements.length;i++) {
		if(d.elements[i].type=="checkbox" && d.elements[i]!=e && d.elements[i].id.indexOf('*'+att) >= 0)
		{
		    intTotal++;
		    var c = d.elements[i];
			d.elements[i].checked = e.checked;
			highlightCheckRow(c, color, ocolor);
		}
	}
    if (e.checked){
        intSelected = intTotal;
    }else{
        intSelected = 0;
    }
    DisplayCount(e,att);
	
}


//--> 
</script> 