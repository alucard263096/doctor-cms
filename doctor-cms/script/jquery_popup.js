
//SETTING UP OUR POPUP
//0 means disabled; 1 means enabled;
var popupStatus=0;
//loading popup with jQuery magic
function loadPopup(url)
{
    //loads popup only if it is disabled
    if(popupStatus==0)
    {
        $("#backgroundPopup").css({
            "opacity":"0.7"
        });
        $("#backgroundPopup").fadeIn("slow");
        $("#popupContact").fadeIn("slow");
        //$("#contactArea").load('upload_flash.aspx');
        popupStatus=1;
    }
}
//disabling popup withjQuery magic!
function disablePopup()
{
    //disables popup only if it is enabled
    if(popupStatus==1)
    {
        $("#backgroundPopup").fadeOut("slow");
        $("#popupContact").fadeOut("slow");
        popupStatus=0;
    }
}

//centering popup
function centerPopup()
{
    var windowWidth=document.documentElement.clientWidth;
    var windowHeight=document.documentElement.clientHeight;
    var popupHeight=$("#popupContact").height();
    var popupWidth=$("#popupContact").width();
    //alert(windowHeight);
    //alert(popupHeight);
    //alert(windowWidth);
    //alert(popupWidth);
    //centering
    var scroll_top=document.documentElement.scrollTop;
    var scroll_left=document.documentElement.scrollLeft;
    $("#popupContact").css({
        "position":"absolute",
        "top":windowHeight/2-popupHeight/2+scroll_top,
        "left":windowWidth/2-popupWidth/2+scroll_left
    });
    
    //only need force for IE6
    $("#backgroundPopup").css({
        "height":windowHeight
    });
    
}

$(document).ready(function(){
    $("#popupContactClose").click(function(){
        disablePopup();
    });
    $("#backgroundPopup").click(function(){
        disablePopup();
    });
    $(document).keypress(function(e){
        if(e.keyCode==27 && popupStatus==1)
        {
            disablePopup();
        }
    });
});