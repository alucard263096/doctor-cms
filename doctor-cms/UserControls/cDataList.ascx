<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cDataList.ascx.cs"  Inherits="SunStar_CMS.admin.UserControls.cDataList" %>
	
					<table><tr><td style="width: 185px">
                        <asp:Label ID="title" runat="server" Text="Label"></asp:Label></td> </tr>
					<tr  ><td style="width: 185px">  
					<table   border="0" cellpadding="0" cellspacing="0" style="width:100%">
					<asp:PlaceHolder ID="phHeader" runat="server"  ></asp:PlaceHolder>
					<asp:PlaceHolder ID="phBody" runat="server" ></asp:PlaceHolder>
					</table>
					</td></tr>
			
					</table>
<script type="text/javascript" language="javascript"> 
var intSelected = 0;
var intTotal = 0;
var bg;
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
function mouseOver(tr)
{
  bg=tr.style.backgroundColor;
  tr.style.backgroundColor="#80AB73";
}
function mouserOut(tr)
{
  tr.style.backgroundColor=bg;
}
function onChange(textBox)
{
   alert(textBox.value);
}
function show(object)
{
   object.parentNode.nextSibling.innerHTML='new!'
}




//--> 
</script> 