
	function onDelete() {
		var d = document.form1;
		for(i=0;i<d.elements.length;i++) {
			if(d.elements[i].type=="checkbox" && d.elements[i].checked) {
				if(confirm("��ȷ��Ҫ�޸�ѡ������?"))
					d.submit();
				return;
			}
		}
		alert("������ѡ��һ��");
	}
	
	
	function onCheckAll(e) {
		var d = e.form;
		for(i=0;i<d.elements.length;i++) {
			if(d.elements[i].type=="checkbox" && d.elements[i]!=e)
				d.elements[i].checked = e.checked;
		}
	}
	
	function onCancelCheckAll(e) {
		var d = e.form;
		for(i=0;i<d.elements.length;i++) {
			if(d.elements[i].type=="checkbox" && d.elements[i].name!="all_id") {
				if(!d.elements[i].checked) {
					d.all_id.checked = false;
					return;
				}
			}
			d.all_id.checked = true;
		}
	}

	function newWindow(mypage,w,h,scroll){
		LeftPosition = (screen.width) ? (screen.width-w)/2 : 0;
		TopPosition = (screen.height) ? (screen.height-h)/2 : 0;
		settings = 'height='+h+',width='+w+',top='+TopPosition+',left='+LeftPosition+',scrollbars='+scroll+',resizable=yes';
		win = window.open(mypage,'popup',settings)
	}