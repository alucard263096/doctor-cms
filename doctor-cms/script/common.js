	
	function isFormValidDate(formElem) {
		if(!isValidDDMMYYYY(formElem.value)) {
			alert("Invalid date input, please enter again!");
			formElem.focus();
		}
	}

    function isFormValidYear(formElem){
	    if(trim(formElem.value).length==4){
		    if(isNumeric(formElem.value,true,false,false,0)){
			    if(Number(formElem.value)<=1900){
			        alert("年份必须大于1900!");
			        formElem.select();
			        formElem.focus();
			        return false;
			    }else{
			        formElem.value = trim(formElem.value);
			        return true;
			    }
			}else{
			    alert("必须输入数字!");
		        formElem.select();
		        formElem.focus();
			    return false;
		    }
	    }else{
		    alert("必须输入数字!");
	        formElem.select();
	        formElem.focus();
	        return false;
	    }
    }

    function isValidDDMMYYYY(ddmmyyyy) {
	    ddmmyyyy += '';
        ddmmyyyy = ddmmyyyy.replace(/[^0-9]+/g,'');
        if (ddmmyyyy.length == 0) return true;
        if (ddmmyyyy.length != 8) return false;

        day 	= ddmmyyyy.substring(0,2);
        month	= parseInt(ddmmyyyy.substring(2,4),10) - 1;
        year	= ddmmyyyy.substring(4,8);

        var test = new Date(year,month,day);

        if ( year == y2k(test.getYear()) &&
             month == test.getMonth() &&
             day == test.now())
            return true;
        else
            return false;
    }

	function isFormNumeric(formElem,isInteger,canNegative,canZero,defaultValue){
		if (!isNumeric(formElem.value,isInteger,canNegative,canZero)) {
			alert("Please enter" + (canNegative ? "" : " positive") + (isInteger ? " integer." : " number."));
			formElem.value = defaultValue;
			formElem.select();
			formElem.focus();
			return false;
		} else if (trim(formElem.value) == "") {
			formElem.value = defaultValue;
		} else {
			formElem.value = trim(formElem.value);
		}
		return true;
	}

	function isFormDecimal(formElem,isInteger,canNegative,canZero,places,defaultValue){
		if (!isNumeric(formElem.value,isInteger,canNegative,canZero)) {
			alert("Please enter" + (canNegative ? "" : " positive") + (isInteger ? " integer." : " number."));
			formElem.value = defaultValue;
			formElem.select();
			formElem.focus();
			return false;
		} else if(!chkDecimal(formElem.value,places)) {
			alert("The amount only allows "+places+" decimal places.");
			formElem.value = defaultValue;
			formElem.select();
			formElem.focus();
			return false;
		} else if (trim(formElem.value) == "") {
			formElem.value = defaultValue;
		} else {
			formElem.value = trim(formElem.value);
		}
		return true;
	}

	function isFormNullNumeric(formElem,isInteger,canNegative,canZero,canNull,defaultValue){
		if (!isNumeric(formElem.value,isInteger,canNegative,canZero)) {
			alert("Please enter" + (canNegative ? "" : " positive") + (isInteger ? " integer." : " number."));
			formElem.value = defaultValue;
			formElem.select();
			formElem.focus();
			return false;
		} else if (trim(formElem.value) == "") {
		    if (canNull) {
			    formElem.value = "";
			}else{
			    formElem.value = "0";
		    }
		} else {
			formElem.value = trim(formElem.value);
		}
		return true;
	}

	function isFormNullDecimal(formElem,isInteger,canNegative,canZero,canNull,places,defaultValue){
		if (!isNumeric(formElem.value,isInteger,canNegative,canZero)) {
			alert("Please enter" + (canNegative ? "" : " positive") + (isInteger ? " integer." : " number."));
			formElem.value = defaultValue;
			formElem.select();
			formElem.focus();
			return false;
		} else if(!chkDecimal(formElem.value,places)) {
			alert("The value only allows "+places+" decimal places.");
			formElem.value = defaultValue;
			formElem.select();
			formElem.focus();
			return false;
		} else if (trim(formElem.value) == "") {
		    if (canNull) {
			    formElem.value = "";
			}else{
			    formElem.value = "0";
		    }
		} else {
			formElem.value = trim(formElem.value);
		}
		return true;
	}

	function isNumeric(num,isInteger,canNegative,canZero) {
		if (isNaN(num)) {
			return false;
		} else if (!canZero && (Number(num) == 0)) {
		    return false;
		} else {
			if (isInteger) {
				if(!canNegative)
					return (num.toString().indexOf(".")==-1 && num.toString().indexOf("-")==-1);
				else
					return (num.toString().indexOf(".")==-1);
			} else {
				if(!canNegative)
					return (num.toString().indexOf("-")==-1);
				else
					return true;
			}
		}
	}

	function isDecimal(num,isInteger,canNegative,canZero,places){
		if (!isNumeric(num,isInteger,canNegative,canZero,places)) {
			return false;
		} else if(!chkDecimal(num,places)) {
			return false;
		} else if (trim(num) == "") {
			return false;
		} else {
    		return true;
		}
		return true;
	}

	function chkDecimal(num,places){
		if(isNaN(num)||isNaN(places)){
			return false;
		}else{
			var elem = num.split(".");
			if(elem.length>1){
				if(elem[1].length>places){
					return false;
				}else{
					return true;
				}
			}else{
				return true;
			}
		}
	}

	function roundUp(num,places){
		if(isNumeric(num,false,true,true)){
            var iMultiplier = Math.pow(10, places); 
            var sNumber = (Math.round(num*iMultiplier))/iMultiplier+""; 
            if ( sNumber.indexOf(".")==-1 ) 
                sNumber += "."; 
            for ( var i=sNumber.length-1-sNumber.indexOf("."); i<places; i++ ) 
                sNumber +="0"; 
            if ( sNumber.charAt(sNumber.length-1)=="." ) 
                sNumber = sNumber.substring(0, sNumber.length-1) 
            return sNumber; 
		}else{
			return 0.00;
		}
	}

	function trim(field) {
		var retval = "";
		retval = field.replace(/^\s+/g, "");	// (\s+) means all white space, (^) means after the Star of the line
		retval = retval.replace(/\s+$/g, "");	// ($) means before the end of line
		return retval;
	}

	function isEmpty(s)	{
		return ((s == null) || (s.length == 0));
	}

	function isPositiveInteger(s){
		return (/^\d*$/.test(s));
	}

	function isPositiveNumber(s){
		return (/^(\d*\.?\d*|\.\d+)$/.test(s));
	}

	function isNumber(s){
		return (/^-?(\d*\.?\d*|\.\d+)$/.test(s));
	}

	function isAlphanumeric(s){
		return (/^\w*$/.test(s));
	}

	function isAlphabet(s){
		return (/^[a-zA-Z]*$/.test(s));
	}

	function isAlphanumericSpc(s){
		return (/^[\w\s]*$/.test(s));
	}

    function isFormValidEmail(formElem,canEmpty){
	    if(!canEmpty && trim(formElem.value).length==0){
            alert("Please enter email address.");
            formElem.select();
            formElem.focus();
            return false;
	    }else if (!isEmailAddress(formElem.value)){
		    alert("Please input a valid email address.");
	        formElem.select();
	        formElem.focus();
	        return false;
	    }else{
	        return true;
	    }
    }

	function isEmailAddress(s){
		return (/^[^\s\,\;\'\"]+@[^\s\,\;\'\"]+\.[^\s\,\;\'\"]+$|^$/.test(s));
	}

	function isPersent(s){
		return (/^-?(\d*\.?\d*|\.\d+)%?$/.test(s));
	}

	// the parameter is the date string in dd/mm/yyyy format
	function validDateRange(date_from, date_to) {
		var int_from_year = parseInt(date_from.substring(6,10),10);
		var int_from_month = parseInt(date_from.substring(3,5),10);
		var int_from_day = parseInt(date_from.substring(0,2),10);
		var int_to_year = parseInt(date_to.substring(6,10),10);
		var int_to_month = parseInt(date_to.substring(3,5),10);
		var int_to_day = parseInt(date_to.substring(0,2),10);
		return !(int_from_year>int_to_year || (int_from_year==int_to_year && int_from_month>int_to_month) ||
				(int_from_year==int_to_year && int_from_month==int_to_month && int_from_day>int_to_day));
	}

    function isValidYear(num){
	    if(trim(num).length==4){
		    if(isNumeric(num,true,false,false,0)){
			    if(Number(num)<=1900){
			        return false;
			    }else{
			        return true;
			    }
			}else{
			    return false;
		    }
	    }else{
	        return false;
	    }
    }
    
    //Added by Dennis Lau on 2008/01/28
    function capLock(e){
        kc = e.keyCode?e.keyCode:e.which;
        sk = e.shiftKey?e.shiftKey:((kc == 16)?true:false);
        if(((kc >= 65 && kc <= 90) && !sk)||((kc >= 97 && kc <= 122) && sk))
             return true;
        else
             return false;
    }

