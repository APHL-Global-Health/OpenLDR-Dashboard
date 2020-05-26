//TODO: This should be dynamic later on
var apikey = "e98389ca62d99875ba7a4e0f2929960b";
var dash_swiper = null;
var dash_swiper_other = null;

document.addEventListener('click', function(e) {
	var el = e.target;
	// Go up in the nodelist until we find a node with .href (HTMLAnchorElement)
	while (el && !el.href) {
		el = el.parentNode;
	}

	if (el) {
		e.preventDefault();
		app.NavigateTo(new URL(el.href));
		return;
	}
});

$(document).on("click", ".select--generic li", function() {

	if($(this).parent().hasClass('Disabled')) { /* Do Nothing*/ }
	else if($(this).parent().hasClass('Manual')) {					
		//app.signals.emit('onSelection', {"Item":$(this), "Selection":"Manual"}, app);
	}
	else
	{
		if($(this).is("[data-selected]")) {
			if($(this).parent().find('li').length > 1){
				$(this).parent().toggleClass('is-open');
				$(this).parent().siblings().removeClass('is-open');
			}
		} 
		else {
			$(this).attr('data-selected','true').siblings().removeAttr('data-selected');
			$(this).parent().removeClass('is-open'); 
			//app.signals.emit('onSelection', {"Item":$(this), "Selection":"Auto"}, app);
		}
	}
});

$(document).on("click", ".select--editable li", function() {
	if($(this).parent().hasClass('Disabled')) { /* Do Nothing*/ }
	else if($(this).parent().hasClass('Manual')) { 
		app.signals.emit('onSelection', {"Item":$(this), "Selection":"Manual"}, app);
	}
	else
	{
	  if($(this).is("[data-selected]")) {
		if($(this).parent().find('li').length > 1)
		{
			$(this).parent().toggleClass('is-open');
			$(this).parent().siblings().removeClass('is-open');
		}
	  } else {
		$(this).parent().find('input').val($(this).html());
		$(this).parent().removeClass('is-open'); 
		app.signals.emit('onSelection', {"Item":$(this), "Selection":"Auto"}, app);				
	  }
	}
});
	
$('body').click(function(e){
	var $select = $(".Selection");
	if( $select.hasClass("is-open") /*&& !$select.hasClass("Manual")*/ ) {
		$select.removeClass("is-open")
	}
	
	var $calendar = $(".CalendarParent");		
	if (!$calendar.is(e.target) && $calendar.has(e.target).length === 0)
	{
		$calendar.find(".CalendarDateRange").addClass('FlexDisplay');
		$calendar.removeClass("Opened");
	}
});

// Open/close
$(document).on('click', '.select-dropdown', function(event) {
	var options = $(this).find('.option');
	if(options.length > 1){
	
		$('.select-dropdown').not($(this)).removeClass('open');
		$(this).toggleClass('open');
		if ($(this).hasClass('open')) {
			$(this).find('.option').attr('tabindex', 0);
			$(this).find('.selected').focus();
		} else {
			$(this).find('.option').removeAttr('tabindex');
			$(this).focus();
		}
	}
});
// Close when clicking outside
$(document).on('click', function(event) {
	if ($(event.target).closest('.select-dropdown').length === 0) {
		$('.select-dropdown').removeClass('open');
		$('.select-dropdown .option').removeAttr('tabindex');
	}
	event.stopPropagation();
});
// Option click
$(document).on('click', '.select-dropdown .option', function(event) {
  $(this).closest('.list').find('.selected').removeClass('selected');
  $(this).addClass('selected');
  var text = $(this).data('display-text') || $(this).text();
  $(this).closest('.select-dropdown').find('.current').text(text);
  $(this).closest('.select-dropdown').prev('select').val($(this).data('value')).trigger('change');
});

// Keyboard events
$(document).on('keydown', '.select-dropdown', function(event) {
  var focused_option = $($(this).find('.list .option:focus')[0] || $(this).find('.list .option.selected')[0]);
  // Space or Enter
  if (event.keyCode == 32 || event.keyCode == 13) {
	if ($(this).hasClass('open')) {
	  focused_option.trigger('click');
	} else {
	  $(this).trigger('click');
	}
	return false;
	// Down
  } else if (event.keyCode == 40) {
	if (!$(this).hasClass('open')) {
	  $(this).trigger('click');
	} else {
	  focused_option.next().focus();
	}
	return false;
	// Up
  } else if (event.keyCode == 38) {
	if (!$(this).hasClass('open')) {
	  $(this).trigger('click');
	} else {
	  var focused_option = $($(this).find('.list .option:focus')[0] || $(this).find('.list .option.selected')[0]);
	  focused_option.prev().focus();
	}
	return false;
  // Esc
  } else if (event.keyCode == 27) {
	if ($(this).hasClass('open')) {
	  $(this).trigger('click');
	}
	return false;
  }
});

$('#searchBtn-Transmission').unbind( "click" );
$('#searchBtn-Transmission').click(function() { 
	if(!$('#searchBtn-Transmission').hasClass('Disabled')){
		var year = $(".Selection.year_transmission").find("li[data-selected=true]").attr('data-value');
		var month = $(".Selection.month_transmission").find("li[data-selected=true]").attr('data-value');
		
		var yearRex = /^(19[0-9]{2}|[2-9][0-9]{3})$/gi;
		var monthRex = /^([1-9]|0[1-9]|1[0-2])$/gi;

		if(yearRex.test(year)){
			if(monthRex.test(month)){
				fetchTransmissionData(year, month);
			}					
		}
	}
});

function create_custom_dropdowns() {
  $('select').each(function(i, select) {
	if (!$(this).next().hasClass('select-dropdown')) {
	  $(this).after('<div class="select-dropdown ' + ($(this).attr('class') || '') + '" tabindex="0"><span class="current"></span><div class="list"><ul></ul></div></div>');
	  var dropdown = $(this).next();
	  var options = $(select).find('option');
	  var selected = $(this).find('option:selected');
	  dropdown.find('.current').html(selected.data('display-text') || selected.text());
	  options.each(function(j, o) {
		var display = $(o).data('display-text') || '';
		dropdown.find('ul').append('<li class="option ' + ($(o).is(':selected') ? 'selected' : '') + '" data-value="' + $(o).val() + '" data-display-text="' + display + '">' + $(o).text() + '</li>');
	  });
	}
  });
}

function Selection(args){
	if(args.Selection == "Auto"){ 
		if(args.Item.parent().hasClass('filter_section')){
			//loadChart();
		}
	}
}

function SelectedItem(item){
	$(item).parent().children().each(function(){
		$(this).removeClass('Selected');
	});
	$(item).addClass('Selected');
}

function InitDatePicker(dt, options){
	if(dt.datepicker != null && typeof(dt.datepicker) === "function"){
		dt.datepicker(options);
		return dt.datepicker().data('datepicker');
	}
	return null;
}

function daysInMonth(year, month) {
	return new Date(year, month, 0).getDate();
}

function isDateBeforeToday(date) {
	return new Date(date.toDateString()) <= new Date(new Date().toDateString());
}

function businessDaysInMonth(year, month){
	var days = [];
	var ar = new Array(32 - new Date(year, month-1, 32).getDate()).fill(1);
	ar.forEach(function(id, index) {
		var dt = new Date(year, month-1, index + 1);
		if([0, 6].indexOf(dt.getDay()) === -1)days.push(dt.getDate());
	});			
	return days;
}

function isNumberKey(evt){
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function isDecimalKey(evt){
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode > 31 && (charCode != 46 &&(charCode < 48 || charCode > 57)))
        return false;
    return true;
}

function getDateTime(id){
	var str = $(id).val();
	if(str.trim().length > 0){

		try{
			var parts = str.split(" ");
			if(parts.length == 2){
				var _dt = parts[0];
				var _tm = parts[1];
				
				if(_dt != undefined && _dt != null && _tm != undefined && _tm != null){
					var _dt_parts = _dt.split("/");
					var _tm_parts = _tm.split(":");
					
					if(_dt_parts.length == 3 && _tm_parts.length > 1){
						return new Date(parseInt(_dt_parts[2], 10),
							  parseInt(_dt_parts[1], 10) - 1,
							  parseInt(_dt_parts[0], 10),
							  parseInt(_tm_parts[0], 10),
							  parseInt(_tm_parts[1], 10));
					}
				}				
			}
		}
		catch(e){}
	}
	return null;
}

function InitDatePicker(dt, options){
	if(dt.datepicker != null && typeof(dt.datepicker) === "function"){
		dt.datepicker(options);
		return dt.datepicker().data('datepicker');
	}
	return null;
}

function focusEvents(item,focusIn,focusOut){
	item.focus(focusIn);
	item.focusout(focusOut);
}

function isValidDate(dateStr) {
 
    
    var msg = "";
    // Checks for the following valid date formats:
    // MM/DD/YY   MM/DD/YYYY   MM-DD-YY   MM-DD-YYYY
    // Also separates date into month, day, and year variables
 
    // To require a 2 & 4 digit year entry, use this line instead:
    //var datePat = /^(\d{1,2})(\/|-)(\d{1,2})\2(\d{2}|\d{4})$/;
    // To require a 4 digit year entry, use this line instead:
    var datePat = /^(\d{1,2})(\/|-)(\d{1,2})\2(\d{4})$/;
 
    var matchArray = dateStr.match(datePat); // is the format ok?
    if (matchArray == null) {
        msg = "Date is not in a valid format.";
        return msg;
    }
 
    month = matchArray[1]; // parse date into variables
    day = matchArray[3];
    year = matchArray[4];
 
    
    if (month < 1 || month > 12) { // check month range
        msg = "Month must be between 1 and 12.";
        return msg;
    }
 
    if (day < 1 || day > 31) {
        msg = "Day must be between 1 and 31.";
        return msg;
    }
 
    if ((month==4 || month==6 || month==9 || month==11) && day==31) {
        msg = "Month "+month+" doesn't have 31 days!";
        return msg;
    }
 
    if (month == 2) { // check for february 29th
    var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
    if (day>29 || (day==29 && !isleap)) {
        msg = "February " + year + " doesn't have " + day + " days!";
        return msg;
    }
    }
 
    if (day.charAt(0) == '0') day= day.charAt(1);
    
    //Incase you need the value in CCYYMMDD format in your server program
    //msg = (parseInt(year,10) * 10000) + (parseInt(month,10) * 100) + parseInt(day,10);
    
    return msg;  // date is valid
}

function numberWithCommas(x) {
	return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
		
function transaction_transition(slide){
	if(slide == 0 && dash_swiper != null)dash_swiper.slidePrev();
	else if(slide == 1 && dash_swiper != null)dash_swiper.slideNext();
}

function transaction_transition_other(slide){
	if(slide == 0 && dash_swiper_other != null)dash_swiper_other.slidePrev();
	else if(slide == 1 && dash_swiper_other != null)dash_swiper_other.slideNext();
}

function groupByTestName(data, testName){
	return Object.values(data.reduce(function(acc, { System, TestingLab, Test, Registered, Tested, Authorised, Tested_Workload, Authorised_Workload }, index, arr){
		if(Test == testName){
			const key = System + '_' + TestingLab+ '_' + Test;
			acc[key] = acc[key] || { System, TestingLab, Test, Registered, Tested, Authorised, Tested_Workload, Authorised_Workload };
		}		
		return acc;
	},[]));
}

function groupByTestingArea(data, element){
	var g = Object.values(data.reduce(function(acc, { System, TestingLab, Test, Registered, Tested, Authorised, Tested_Workload, Authorised_Workload }, index, arr){
		if(System == element.System && TestingLab == element.TestingLab){
			const key = System + '_' + TestingLab+ '_' + Test;			
			acc[key] = acc[key] || { Registered, Tested, Authorised, Tested_Workload, Authorised_Workload };
		}
		return acc;
	},[]));

	return (g.length == 0 ? [{ Registered:0, Tested:0, Authorised:0, Tested_Workload:0, Authorised_Workload:0 }] : g);
}

function injectHtmlData(hasInner, outerValue,innerValue){
	var val = outerValue == undefined && outerValue == null ? "0" : numberWithCommas(outerValue.toString());
	if(hasInner)
		val += " ("+(innerValue == undefined && innerValue == null ? "0" : numberWithCommas(innerValue.toString()))+")";
	return val;
}

function createDataTable(id, labs, headers, populatingFunction){
	var doc = $(id);
	var html = '<div class="FlexDisplay FlexColumn" style="min-width:288px; max-width:288px; height:100%;">';
	html += '		<div class="FlexDisplay FlexRow Divider-Bottom" style="width:100%; height:64px;">';
	html += '			<span style="font-weight:bold; font-size:16px; margin:auto auto;">Laboratory Name</span>';
	html += '		</div>';
	labs.forEach(function(element, index) {
		html += '		<div class="FlexDisplay FlexRow '+(index+1 < labs.length ? 'Divider-Bottom' : 'Divider-Bottom')+'" style="width:100%; height:32px;">';
		html += '			<div class="FlexDisplay FlexRow Divider-Right" style="width:24px; height:100%;">';
		html += '				<span style="font-weight:bold; font-size:10px; margin:auto auto;">'+(index+1 < 10 ? '0'+(index+1) : index+1)+'</span>';
		html += '			</div>';
		html += '			<div class="FlexDisplay Flex" style="height:100%;">';
		html += '				<span style="font-weight:bold; font-size:10px; margin:auto 8px;">'+element.TestingLab+' - '+(element.System == 'eSRS' ? 'TilleLab' : element.System)+'</span>';
		html += '			</div>';
		html += '		</div>';
	});
	html += '</div>';	
		
	headers.forEach(function(element, colIndex) {
		var text = element.toString();
		
		html  += '<div class="FlexDisplay FlexColumn FlexGrow Divider-Left Divider-Bottom" style="height:100%;">';
		html += '	<div class="FlexDisplay FlexGrow Divider-Left Divider-Bottom" style="height:64px;">';
		html += '		<span style="font-weight:bold; text-align:center; font-size:12px; margin:auto auto;">'+text+'</span>';
		html += '	</div>';
		
		labs.forEach(function(l_element, rowIndex) {
			html += populatingFunction(l_element, colIndex, rowIndex);				
		});		
		html += '</div>';
	});
	
	doc.append(html);
}

function createTickedTable(id, labs, headers, month, populatingFunction){
	var doc = $(id);
	
	var html = '<div class="FlexDisplay FlexColumn" style="min-width:288px; max-width:288px; height:100%;">';
	html += '		<div class="FlexDisplay FlexRow Divider-Bottom" style="width:100%; height:64px;">';
	html += '			<span style="font-weight:bold; font-size:16px; margin:auto auto;">Laboratory Name</span>';
	html += '		</div>';
	labs.forEach(function(element, index) {
		html += '		<div class="FlexDisplay FlexRow '+(index+1 < labs.length ? 'Divider-Bottom' : 'Divider-Bottom')+'" style="width:100%; height:32px;">';
		html += '			<div class="FlexDisplay FlexRow Divider-Right" style="width:24px; height:100%;">';
		html += '				<span style="font-weight:bold; font-size:10px; margin:auto auto;">'+(index+1 < 10 ? '0'+(index+1) : index+1)+'</span>';
		html += '			</div>';
		html += '			<div class="FlexDisplay Flex" style="height:100%;">';
		html += '				<span style="font-weight:bold; font-size:10px; margin:auto 8px;">'+element.TestingLab+' - '+(element.System == 'eSRS' ? 'TilleLab' : element.System)+'</span>';
		html += '			</div>';
		html += '		</div>';
	});
	html += '</div>';
			
	//Populate the ticks based on day
	headers.forEach(function(element, colIndex) {
		var text = element.toString();

		html += '<div class="FlexDisplay FlexColumn FlexGrow Divider-Left Divider-Bottom" style="height:100%;">';
		html += '	<div class="FlexDisplay FlexGrow Divider-Left Divider-Bottom" style="height:64px;">';
		html += '		<span style="font-weight:bold; text-align:center; font-size:12px; margin:auto auto;">'+text+'<br/>'+month+'</span>';
		html += '	</div>';
		
			labs.forEach(function(el, rowIndex) {
				html += populatingFunction(element, el, colIndex, rowIndex);
			});
		
		html += '</div>';			
	});
	
	doc.append(html);
}

function createTransmisionReport(data, year, month, output, systemsWithOtherTests){
	//Clear Everything
	$('.Transmission').html("");
	$('.OtherTransmission').html("");
	$('.Transmission-Details').html("");
	$('.OtherTransmission-Details').html("");
	$(".Transmission-Container").removeClass("FlexDisplay");
	
	//Create logic
	var days 			= daysInMonth(year, month);
	var businessDays 	= businessDaysInMonth(year, month);
	
	var monthNames 		= ["January", "February", "March", "April", "May", "June","July", "August", "September", "October", "November", "December"];
	var monthShortNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun","Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
	var aphlLogo 		= "data:image/jpeg;base64,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QklNBCYAAAAAAA4AAAAAAAAAAAAAP4AAADhCSU0EDQAAAAAABAAAAHg4QklNBBkAAAAAAAQAAAAeOEJJTQPzAAAAAAAJAAAAAAAAAAABADhCSU0nEAAAAAAACgABAAAAAAAAAAE4QklNA/UAAAAAAEgAL2ZmAAEAbGZmAAYAAAAAAAEAL2ZmAAEAoZmaAAYAAAAAAAEAMgAAAAEAWgAAAAYAAAAAAAEANQAAAAEALQAAAAYAAAAAAAE4QklNA/gAAAAAAHAAAP////////////////////////////8D6AAAAAD/////////////////////////////A+gAAAAA/////////////////////////////wPoAAAAAP////////////////////////////8D6AAAOEJJTQQAAAAAAAACAAA4QklNBAIAAAAAAAQAAAAAOEJJTQQwAAAAAAACAQE4QklNBC0AAAAAAAYAAQAAAAM4QklNBAgAAAAAABAAAAABAAACQAAAAkAAAAAAOEJJTQQeAAAAAAAEAAAAADhCSU0EGgAAAAADRwAAAAYAAAAAAAAAAAAAAGQAAADIAAAACQBBAFAASABMACAATABvAGcAbwAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAyAAAAGQAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAQAAAAAAAG51bGwAAAACAAAABmJvdW5kc09iamMAAAABAAAAAAAAUmN0MQAAAAQAAAAAVG9wIGxvbmcAAAAAAAAAAExlZnRsb25nAAAAAAAAAABCdG9tbG9uZwAAAGQAAAAAUmdodGxvbmcAAADIAAAABnNsaWNlc1ZsTHMAAAABT2JqYwAAAAEAAAAAAAVzbGljZQAAABIAAAAHc2xpY2VJRGxvbmcAAAAAAAAAB2dyb3VwSURsb25nAAAAAAAAAAZvcmlnaW5lbnVtAAAADEVTbGljZU9yaWdpbgAAAA1hdXRvR2VuZXJhdGVkAAAAAFR5cGVlbnVtAAAACkVTbGljZVR5cGUAAAAASW1nIAAAAAZib3VuZHNPYmpjAAAAAQAAAAAAAFJjdDEAAAAEAAAAAFRvcCBsb25nAAAAAAAAAABMZWZ0bG9uZwAAAAAAAAAAQnRvbWxvbmcAAABk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bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIiB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIiB4bWxuczpzdFJlZj0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlUmVmIyIgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDUzYgKFdpbmRvd3MpIiB4bXA6Q3JlYXRlRGF0ZT0iMjAxOS0wMi0xOFQwNzoxMDoyMyswMzowMCIgeG1wOk1ldGFkYXRhRGF0ZT0iMjAxOS0wMi0xOFQwNzoxMDozNiswMzowMCIgeG1wOk1vZGlmeURhdGU9IjIwMTktMDItMThUMDc6MTA6MzYrMDM6MDAiIGRjOmZvcm1hdD0iaW1hZ2UvanBlZyIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDoxRDY3MEQxRDMzMzNFOTExOTVGNUI3NDM2OUFDQjQwMiIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDoxQjY3MEQxRDMzMzNFOTExOTVGNUI3NDM2OUFDQjQwMiIgeG1wTU06T3JpZ2luYWxEb2N1bWVudElEPSJ4bXAuZGlkOjFCNjcwRDFEMzMzM0U5MTE5NUY1Qjc0MzY5QUNCNDAyIiBwaG90b3Nob3A6Q29sb3JNb2RlPSIzIiBwaG90b3Nob3A6SUNDUHJvZmlsZT0ic1JHQiBJRUM2MTk2Ni0yLjEiPiA8eG1wTU06SGlzdG9yeT4gPHJkZjpTZXE+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJjcmVhdGVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjFCNjcwRDFEMzMzM0U5MTE5NUY1Qjc0MzY5QUNCNDAyIiBzdEV2dDp3aGVuPSIyMDE5LTAyLTE4VDA3OjEwOjIzKzAzOjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgQ1M2IChXaW5kb3dzKSIvPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6MUM2NzBEMUQzMzMzRTkxMTk1RjVCNzQzNjlBQ0I0MDIiIHN0RXZ0OndoZW49IjIwMTktMDItMThUMDc6MTA6MzYrMDM6MDAiIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIFBob3Rvc2hvcCBDUzYgKFdpbmRvd3MpIiBzdEV2dDpjaGFuZ2VkPSIvIi8+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJjb252ZXJ0ZWQiIHN0RXZ0OnBhcmFtZXRlcnM9ImZyb20gYXBwbGljYXRpb24vdm5kLmFkb2JlLnBob3Rvc2hvcCB0byBpbWFnZS9qcGVnIi8+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJkZXJpdmVkIiBzdEV2dDpwYXJhbWV0ZXJzPSJjb252ZXJ0ZWQgZnJvbSBhcHBsaWNhdGlvbi92bmQuYWRvYmUucGhvdG9zaG9wIHRvIGltYWdlL2pwZWciLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249InNhdmVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjFENjcwRDFEMzMzM0U5MTE5NUY1Qjc0MzY5QUNCNDAyIiBzdEV2dDp3aGVuPSIyMDE5LTAyLTE4VDA3OjEwOjM2KzAzOjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgQ1M2IChXaW5kb3dzKSIgc3RFdnQ6Y2hhbmdlZD0iLyIvPiA8L3JkZjpTZXE+IDwveG1wTU06SGlzdG9yeT4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MUM2NzBEMUQzMzMzRTkxMTk1RjVCNzQzNjlBQ0I0MDIiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MUI2NzBEMUQzMzMzRTkxMTk1RjVCNzQzNjlBQ0I0MDIiIHN0UmVmOm9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDoxQjY3MEQxRDMzMzNFOTExOTVGNUI3NDM2OUFDQjQwMiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8P3hwYWNrZXQgZW5kPSJ3Ij8+/+IMWElDQ19QUk9GSUxFAAEBAAAMSExpbm8CEAAAbW50clJHQiBYWVogB84AAgAJAAYAMQAAYWNzcE1TRlQAAAAASUVDIHNSR0IAAAAAAAAAAAAAAAEAAPbWAAEAAAAA0y1IUCAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAARY3BydAAAAVAAAAAzZGVzYwAAAYQAAABsd3RwdAAAAfAAAAAUYmtwdAAAAgQAAAAUclhZWgAAAhgAAAAUZ1hZWgAAAiwAAAAUYlhZWgAAAkAAAAAUZG1uZAAAAlQAAABwZG1kZAAAAsQAAACIdnVlZAAAA0wAAACGdmlldwAAA9QAAAAkbHVtaQAAA/gAAAAUbWVhcwAABAwAAAAkdGVjaAAABDAAAAAMclRSQwAABDwAAAgMZ1RSQwAABDwAAAgMYlRSQwAABDwAAAgMdGV4dAAAAABDb3B5cmlnaHQgKGMpIDE5OTggSGV3bGV0dC1QYWNrYXJkIENvbXBhbnkAAGRlc2MAAAAAAAAAEnNSR0IgSUVDNjE5NjYtMi4xAAAAAAAAAAAAAAASc1JHQiBJRUM2MTk2Ni0yLjEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFhZWiAAAAAAAADzUQABAAAAARbMWFlaIAAAAAAAAAAAAAAAAAAAAABYWVogAAAAAAAAb6IAADj1AAADkFhZWiAAAAAAAABimQAAt4UAABjaWFlaIAAAAAAA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wB8RSSdQB48aNxNeOCOuU/3vOQufeSvcu79ytvv7w7DuLI0VzE7hraVUAMDOprHQqWh+EFDRSWjegKdIfKvpj5Abm271v/Mf2jS9h4ivak2/gflJjp59r92bB8hhpManYG68AaSbsTZVIQqtPlIqusxqM0rGojBVDXd+Xd02SCa/5KuWhdas1qe6F/M+GrV0OfRKBuAp0C+Rfdrkz3C3Gw5Z+8PtEd/byaY4t4UmK+t/wp9TNHQ3UC+s4keOrNVxRRsUdm9A74+FfwL37tP+XlPl4t3beybdlUc+bNB2NufcGOra7G1O95NvwZXHz4WtzM22KINRQRUZSRICsMbVEqs0K2G72vM/NtpPziFNu48M0rGqkA6KkEMBqOSTitSdII66Fcy8jbz7P+x++bX7CPIN0gf6pDJouZZFJTxzGGRo2fwlBRVjOoKQimRgetZzO/LPrr5ZZijj+b20I6Xds0UGNpflN0ht/Hbf7KxMESiCkHZfXcQj2d2xtvHgKrLBFi81RwKVpppLCBp1i5cv+XY2k5TutVtkm1mYtGf+ach74mPzLKSakD4uubV57tcs+615Dbe92yiPdyAi7xYRpFdJwA+rthSC7iXHwLDOiCkcjYQ7Ln8q74p5r4w9YbxkxfyL29350z2tkMDvjqmXa2FrMZhcXDLR1tPms7A1VmsukNbueP7NKuki0rBPQEufM0irBfuDzBFv1/b+JsrWm5wBkl1GpJqKDgML3EHz1elOukP3Yfa689tOWd0+l9wId85R3Jop7IxRlEQaXEkgq70Mo8NWjHwGKhOqoFqHuPusneve/de697917r3v3Xuve/de697917r3v3Xuve/de697917r3v3Xuve/de697917r3v3Xuve/de697917r3v3Xus/tT011//1d/c/Q/6x/3r3pvhP2dbHEdYfafpzr3v3Xuk7uzaW19+bbzOzt67ew269qbioZcbndubgx1LlsNlqGaxelr8fWxTU1RFqUMNSkqyhhZgD7ftrq4s54rm2maOdDVWUkEH5EZ6L912nbN82672jebCG62u4QpJFKivG6niGVgQR55GCARkdUl9wfyEvjVvfcNRmuruxOwOmcdXTSS1m0oKfH7823SiVyzw4E7gnpc/jYNDWVJ66tRONICjT7lPbfd/fLS3EN9aRXLgUD5Rv9tSqn8lHWGPNv3Fvbbe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jZni5pb+qEm0SX1xdVgIj0zPbpBDI0kcUkkkqqoEnhUq3HT0ptpfzHO9e2K34Xf6MururcZT/J7Nbz6239hew67sjH7r6T7h6dxGVzvbe28/Q47AmKTF/aY1YsI0wgqZpKiJ6hY42uHYt+urltp+ntogLlmRgxfVHIgJcEAZFPh4HhXpHf8AtFy5scfuMN43u9eTZIoriB4FgaK8tbplS1kRmeoarVmpqUBWCEkdJbYP81Lt3dvZddtSs6W2TFFURfLL77rrHZjer9z9Cw/HLD53KbQ3H8hMVWYGLD4nbfckmHihx70wprNWw/bSVp1AN2/MdxNcmFrRB/bVUFvEj8IEhpBQAK9MEU4ilelm7eyWx7fsibhFzHcEr+7CtwyRfSXpv2RZY7Fg5dpLTUTIG1fA2sR46QOzf5vPfuU6w3TvPdXxy2rg8/j9v/DHee28JNXb5xhzW1/lP2D/AHMy9dBS1mLrJsji9m0w81Nkopo1rJSFlpadbsE8fNN2beSWSwQOFgYCrCqzNpJ88DyNc+g6N9w9guWoN7sttseb5pbVpd3ikcLC2iTbIPGVSQw0tLwaMg6BlXfoWYP5nPatRnvm3i4uuOqYqn4vT970Wzdu5nd+TwOS3snU+8MJtrEbh3BlJKjIZHCYHKUuVeoqpIcFLRxtEFWtClmRUvMNwz7sv08X+L+IFBYgtoYAEmpoDWp7afPoPy+zWzRW/tvK273xj3tbJpZEhV1h+qid2jjWiq7qV0qDMGNalPIif8XPm18h/lT0r2Nurr3q7riu7R6+7U2/s2fDzZDPYrZFdt3IYXHZrL11Dudsll9qbgyGOFU0Plwmfy1KqFZJAk4+0Kjb93v9ysp5ba1iN0koWlSFIIqTWpBI/osfXjjok539uOWOR+Y9osN13q7XZbuyeYPpRpg4dkUGLSkqK1K0mhiatQKr39Ww+xZ1B3X/0t/j37r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3X//T3+Pfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvde9+691737r3Xvfuvdf/9k=";
	var tickSent 		= "data:image/png;base64,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";
	var tickFailed 		= "data:image/png;base64,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";
	var tickNA 			= "data:image/png;base64,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";
	
	//Get System/TestingLab from data
	var labs = Object.values(data.reduce(function(acc, { System, TestingLab }, index, arr){
		if(!TestingLab.startsWith("Unknown")){
			const key = System + '_' + TestingLab;
			acc[key] = acc[key] || { System, TestingLab };
		}
		return acc;
	},[]));
	
	//Filter out other labs that perform other tests too
	var labsThatSubmitOtherTests = labs.filter(function(v) { return systemsWithOtherTests.includes(v.System); });
	
	var data_grouped = Object.values(data.reduce((acc, { System, TestingLab, Test, Registered, Tested, Authorised, Tested_Workload, Authorised_Workload }) => {
		if(!TestingLab.startsWith("Unknown")){
			const key = System + '_' + TestingLab+ '_' + Test;
			acc[key] = acc[key] || { System, TestingLab, Test, Registered:0, Tested:0, Authorised:0, Tested_Workload:0, Authorised_Workload:0 };

			acc[key].Registered 			+= Registered;
			acc[key].Tested 				+= Tested;
			acc[key].Authorised 			+= Authorised;
			acc[key].Tested_Workload 		+= Tested_Workload;
			acc[key].Authorised_Workload 	+= Authorised_Workload;
		}
		return acc;
	}, {}))
	//Not necessary to sort but helps with debugging
	.sort(function(a, b) { return a.System.localeCompare(b.System) || a.TestingLab.localeCompare(b.TestingLab) || b.Test.localeCompare(b.Test); });
	
	var data_vl  	= groupByTestName(data_grouped, "HIVVL");
	var data_eid 	= groupByTestName(data_grouped, "HIVPC");
	var data_other 	= groupByTestName(data_grouped, "Other");
	
	//Create Ticked Table : Viral Load and EID
	createTickedTable('.Transmission', labs, businessDays, monthShortNames[month-1],
		function(date, element, colIndex, rowIndex){
			
			var lab_data = data.filter(function(v) { return (v.System === element.System && v.TestingLab === element.TestingLab && (v.Test === 'HIVVL' || v.Test === 'HIVPC' || v.Test === 'EID')); });
			var found = lab_data.filter(function(v) { return (v.Date === date); });
			
			var _html = "";	
			_html += '	<div class="FlexDisplay FlexGrow Divider-Left '+(rowIndex+1 < labs.length ? 'Divider-Bottom' : 'Divider-Bottom')+'" style="height:32px;">';
			if(isDateBeforeToday(new Date(year, month-1, date)))
				_html += '		<img src="'+(found.length == 0 ? tickFailed : tickSent)+'" style="margin:auto auto; width:12px; height:12px;" />';
			else
				_html += '		<img src="'+(found.length == 0 ? tickNA : tickSent)+'" style="margin:auto auto; width:12px; height:12px;" />';
			_html += '	</div>';
			
			return _html;
		}
	);
	
	//Create Ticked Table : Other
	createTickedTable('.OtherTransmission', labsThatSubmitOtherTests, businessDays, monthShortNames[month-1],
		function(date, element, colIndex, rowIndex){
			
			var lab_data = data.filter(function(v) { return (v.System === element.System && v.TestingLab === element.TestingLab && v.Test === 'Other'); });
			var found = lab_data.filter(function(v) { return (v.Date === date); });
			
			var _html = "";	
			_html += '	<div class="FlexDisplay FlexGrow Divider-Left '+(rowIndex+1 < labs.length ? 'Divider-Bottom' : 'Divider-Bottom')+'" style="height:32px;">';
			if(isDateBeforeToday(new Date(year, month-1, date)))
				_html += '		<img src="'+(found.length == 0 ? tickFailed : tickSent)+'" style="margin:auto auto; width:12px; height:12px;" />';
			else
				_html += '		<img src="'+(found.length == 0 ? tickNA : tickSent)+'" style="margin:auto auto; width:12px; height:12px;" />';
			_html += '	</div>';
			
			return _html;
		}
	);
	
	//Create Data Table : Viral Load and EID
	createDataTable('.Transmission-Details', labs, 
		[
			"Viral Load<br/>Specimens<br/>Registered",
			"Viral Load<br/>Specimens<br/>Tested (a)",
			"Viral Load<br/>Specimens<br/>Authorized (b)",
			"EID<br/>Specimens<br/>Registered",
			"EID<br/>Specimens<br/>Tested (a)",
			"EID<br/>Specimens<br/>Authorized (b)"
		],
		function(element, colIndex, rowIndex){
			var vl 	= groupByTestingArea(data_vl , element).shift();		
			var eid = groupByTestingArea(data_eid, element).shift();
			
			var _html = "";			
				_html += '	<div class="FlexDisplay FlexGrow Divider-Left '+(rowIndex+1 < labs.length ? 'Divider-Bottom' : 'Divider-Bottom')+'" style="height:32px;">';
				_html += '		<span style="font-weight:bold; text-align:center; font-size:12px; margin:auto auto;">';				
					 if(colIndex == 0)_html += injectHtmlData(false, vl.Registered);
				else if(colIndex == 3)_html += injectHtmlData(false, eid.Registered);
				else if(colIndex == 1)_html += injectHtmlData(true , vl.Tested	  , vl.Tested_Workload);
				else if(colIndex == 4)_html += injectHtmlData(true , eid.Tested	  , eid.Tested_Workload);
				else if(colIndex == 2)_html += injectHtmlData(true , vl.Authorised , vl.Authorised_Workload);
				else if(colIndex == 5)_html += injectHtmlData(true , eid.Authorised, eid.Authorised_Workload);
				_html += '		</span>';
				_html += '	</div>';
				
			return _html;
		}
	);

	//Create Data Table : Other
	createDataTable('.OtherTransmission-Details', labsThatSubmitOtherTests, [ "Specimens Registered", "Specimens Tested (a)", "Specimens Tested (a)" ],
		function(element, colIndex, rowIndex){
			var other = groupByTestingArea(data_other, element).shift();
			
			var _html = "";			
				_html += '	<div class="FlexDisplay FlexGrow Divider-Left '+(rowIndex+1 < labs.length ? 'Divider-Bottom' : 'Divider-Bottom')+'" style="height:32px;">';
				_html += '		<span style="font-weight:bold; text-align:center; font-size:12px; margin:auto auto;">';
					 if(colIndex == 0)_html += injectHtmlData(false, other.Registered);
				else if(colIndex == 1)_html += injectHtmlData(true , other.Tested	 	, other.Tested_Workload);
				else if(colIndex == 2)_html += injectHtmlData(true , other.Authorised , other.Authorised_Workload);
				_html += '		</span>';
				_html += '	</div>';
				
			return _html;
		}
	);

	//Clean up
	$(".Transmission-Container").addClass("FlexDisplay");
	$('#searchBtn-Transmission').removeClass('Disabled');
	$(".Transmission-Spinner").removeClass("FlexDisplay");
	
	//Allow swiping gestures
	dash_swiper = new Swiper('.swiper-container-Transmission');
	dash_swiper_other = new Swiper('.swiper-container-OtherTransmission');
}

function fetchTransmissionData(year, month){
	if(!$('#searchBtn-Transmission').hasClass('Disabled')){
		$('.Transmission').html("");
		$('.OtherTransmission').html("");
		$('.Transmission-Details').html("");
		$('.OtherTransmission-Details').html("");
		$('.Transmission-Container').removeClass("FlexDisplay");
		$('.Transmission-Spinner').addClass("FlexDisplay");		
		$('#searchBtn-Transmission').addClass('Disabled');
		
		var timeout = 1000*60*5; //Time out after 5 minutes cause way too long
		var port = 44325;
		var protocol = 'https';
		var domain = 'localhost';
		var url = protocol+'://'+domain+':'+port+'/api/openldr/general/'+apikey+'/v1/json/transmission/'+year+'/'+month;

		fetch(url, {},timeout)
		.then((response) => response.json())
		.then((data) => {
			if(data.length > 0){
				createTransmisionReport(data, year, month, null, ["Disa*Lab"]);
			}
			else{
				//TODO: Show dialog
				console.log("No data");
				
				$('.Transmission').html("");
				$('.OtherTransmission').html("");
				$('.Transmission-Details').html("");
				$('.OtherTransmission-Details').html("");
				$(".Transmission-Container").removeClass("FlexDisplay");
				$(".Transmission-Spinner").removeClass("FlexDisplay");
				$('#searchBtn-Transmission').removeClass('Disabled');
			}
		})
		.catch((error) => {
			console.log(error);
			$('.Transmission').html("");
			$('.OtherTransmission').html("");
			$('.Transmission-Details').html("");
			$('.OtherTransmission-Details').html("");
			$(".Transmission-Container").removeClass("FlexDisplay");
			$(".Transmission-Spinner").removeClass("FlexDisplay");
			$(".Transmission-Spinner").removeClass("FlexDisplay");
			$('#searchBtn-Transmission').removeClass('Disabled');
		});	
		
	}
}

function fetchData(){
	var year = $(".Selection.year_transmission").find("li[data-selected=true]").attr('data-value');
	var month = $(".Selection.month_transmission").find("li[data-selected=true]").attr('data-value');
		
	var yearRex = /^(19[0-9]{2}|[2-9][0-9]{3})$/gi;
	var monthRex = /^([1-9]|0[1-9]|1[0-2])$/gi;

	if(yearRex.test(year)){
		if(monthRex.test(month)){			
			fetchTransmissionData(year, month);
		}
		else console.error("Failed to parse month");
	}
	else console.error("Failed to parse year");		
}

function init(){
	var dt_trasmission = new Date();
	for(var i=dt_trasmission.getFullYear(); i>=2013; i--){
		$(".year_transmission").append("<li "+(i==dt_trasmission.getFullYear() ? "data-selected='true'" : "")+" data-value='"+i+"'>"+i+"</li>");
	}

	for(var i=1; i<=12; i++){
		$(".month_transmission").append("<li "+(i==dt_trasmission.getMonth()+1 ? "data-selected='true'" : "")+" data-value='"+i+"'>"+i+"</li>");
	}
	
	fetchData();
}

init();
