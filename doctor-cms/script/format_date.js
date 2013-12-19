
for(i=0;i<document.forms.length;i++) {
	var d = document.forms[i];
	for(j=0;j<d.elements.length;j++) {
		var formElem = d.elements[j];
		if(formElem.getAttribute("IsDateField")!=null) {
			formElem.onkeyup = function() {
				validateiDate(this);
			}
			formElem.onblur = function() {
				isiValidDate(this);
			}
		}else if(formElem.getAttribute("IsRefreshDateField")!=null) {
			formElem.onkeyup = function() {
				validateiDate(this);
			}
			formElem.onblur = function() {
				isrValidDate(this);
			}
		}else if(formElem.getAttribute("IsMYDateField")!=null) {
			formElem.onkeyup = function() {
				validateiMYDate(this);
			}
			formElem.onblur = function() {
				isiValidMYDate(this);
			}
		}
		
	}
}

function validateiDate(formElem) {
	var intKeyCode = window.event.keyCode;
	if(intKeyCode!=37 && intKeyCode!=39 &&
		intKeyCode!=8 && intKeyCode!=46 &&
		intKeyCode!=13 && intKeyCode!=9
	) {
		var strValue = formElem.value;
		strValue = strValue.replace(/[^0-9]+/g,'');
		if(strValue.length>8) strValue = strValue.substring(0,8);
		var intDay = null;
		var intMonth = null;
		var intYear = null;
		if(strValue.length>0) {
			if(strValue.length>1)
				intDay = strValue.substring(0,2);
			else
				intDay = strValue.substring(0,1);
		}
		if(strValue.length>2) {
			if(strValue.length>3)
				intMonth = strValue.substring(2,4);
			else
				intMonth = strValue.substring(2,3);
		}
		if(strValue.length>4)
			intYear = strValue.substring(4,strValue.length);

		strValue = "";
		if(intYear!=null && intYear.length>3) {
			var test = new Date(intYear,parseInt(intMonth,10)-1,intDay);
			intDay = test.now();
			intMonth = test.getMonth() + 1;
			intYear = iy2k(test.getYear());
			strValue =	((intDay > 9) ? intDay : "0" + intDay) + "/" +
						((intMonth  > 9) ? intMonth  : "0" + intMonth ) + "/" +
						intYear;
		} else {
			if(intDay!=null) {
				if(intDay.length==2) {
					if(parseInt(intDay)>31)
						strValue += "31/";
					else
						strValue += intDay + "/";
				} else if(parseInt(intDay)>3)
					strValue += "0" + intDay + "/";
				else
					strValue += intDay;
			}
			if(intMonth!=null) {
				if(intMonth.length==2) {
					if(parseInt(intMonth)>12)
						strValue += "12/";
					else
						strValue += intMonth + "/";
				} else if(parseInt(intMonth)>1)
					strValue += "0" + intMonth + "/";
				else
					strValue += intMonth
			}
			if(intYear!=null)
				strValue += intYear;
		}
		formElem.value = strValue;
	} else if(intKeyCode==13) { // enter key is pressed
		var d = formElem.form;
		for(i=0;i<d.elements.length;i++) {
			if(d.elements[i].name == formElem.name) {
				if (d.elements[i+1]!=null) {
					d.elements[i+1].focus();
					return;
				}
			}
		}
	}
}

function validateiMYDate(formElem) {
	var intKeyCode = window.event.keyCode;
	if(intKeyCode!=37 && intKeyCode!=39 &&
		intKeyCode!=8 && intKeyCode!=46 &&
		intKeyCode!=13 && intKeyCode!=9
	) {
		var strValue = formElem.value;
		strValue = strValue.replace(/[^0-9]+/g,'');
		if(strValue.length>6) strValue = strValue.substring(0,6);
		var intDay = null;
		var intMonth = null;
		var intYear = null;
		if(strValue.length>0) {
			if(strValue.length>1)
				intMonth = strValue.substring(0,2);
			else
				intMonth = strValue.substring(0,1);
		}
		if(strValue.length>2)
			intYear = strValue.substring(2,strValue.length);

		strValue = "";
		
		if(intYear!=null && intYear.length>3) {
			var test = new Date(intYear,parseInt(intMonth,10)-1,1);
			intDay = test.now();
			intMonth = test.getMonth() + 1;
			intYear = iy2k(test.getYear());
			strValue =	((intMonth  > 9) ? intMonth  : "0" + intMonth ) + "/" + intYear;
		} else {
			if(intMonth!=null) {
				if(intMonth.length==2) {
					if(parseInt(intMonth)>12)
						strValue += "12/";
					else
						strValue += intMonth + "/";
				} else if(parseInt(intMonth)>1)
					strValue += "0" + intMonth + "/";
				else
					strValue += intMonth
			}
			if(intYear!=null)
				strValue += intYear;
		}
		formElem.value = strValue;
	} else if(intKeyCode==13) { // enter key is pressed
		var d = formElem.form;
		for(i=0;i<d.elements.length;i++) {
			if(d.elements[i].name == formElem.name) {
				if (d.elements[i+1]!=null) {
					d.elements[i+1].focus();
					return;
				}
			}
		}
	}
}


function iy2k(number) {
	var y2kNumber = (number < 1000) ? number + 1900 : number;
	return (y2kNumber < 1900) ? 1900 : y2kNumber;
}

function isiValidDDMMYYYY(ddmmyyyy) {
	ddmmyyyy += '';
    ddmmyyyy = ddmmyyyy.replace(/[^0-9]+/g,'');
    if (ddmmyyyy.length == 0) return true;
    if (ddmmyyyy.length != 8) return false;

    day 	= ddmmyyyy.substring(0,2);
    month	= parseInt(ddmmyyyy.substring(2,4),10) - 1;
    year	= ddmmyyyy.substring(4,8);

    var test = new Date(year,month,day);

    if ( year == iy2k(test.getYear()) &&
         month == test.getMonth() &&
         day == test.now())
        return true;
    else
        return false;
}

function isiValidMMYYYY(mmyyyy) {
	mmyyyy += '';
    mmyyyy = mmyyyy.replace(/[^0-9]+/g,'');
    if (mmyyyy.length == 0) return true;
    if (mmyyyy.length != 6) return false;

    month 	= mmyyyy.substring(0,2);
    year	= mmyyyy.substring(2,6);

    return true;
}


function isiValidDate(formElem) {
	if(!isiValidDDMMYYYY(formElem.value)) {
		alert("日期无效。请重新输入!");
		formElem.select();
		formElem.focus();
	}
}

function isrValidDate(formElem) {
	if(!isiValidDDMMYYYY(formElem.value)) {
		alert("日期无效。请重新输入!");
		formElem.select();
		formElem.focus();
	}else{
	    refreshForm();
	}
}
function isiValidMYDate(formElem) {
	if(!isiValidMMYYYY(formElem.value)) {
		alert("日期无效。请重新输入!");
		formElem.select();
		formElem.focus();
	}
}
