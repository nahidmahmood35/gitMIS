//// ---- Validation

function validation() {

    var isValid = true;
    
    $('input.required, select.required').each(function () {
        if ($.trim($(this).val()) == 0) {
            $(this).css({ "border": "1px solid #ff0000" });
            $(this).closest("div").find(".select2-selection").css({ "border": "1px solid #ff0000" });
        isValid = false; }
        else { $(this).css({ "border": "" }); isValid = true; }
    });
    $('textarea.defaultNA, input.defaultNA').each(function () {
        if ($.trim($(this).val()) == '') {$(this).val('N/A');}
    });
    $('textarea.defaultZero, input.defaultZero').each(function () {
        if ($.trim($(this).val()) == '') {$(this).val(0);}
    });
    return isValid;
}



//// ----- Additional Function
(function ($) {
    $(document).ready(function () {
        document.onkeydown = function (e) {
                if (e.ctrlKey && e.keyCode === 13) {
                    $('#btnSave').focus().click();
                    return false;
                }
            };

        var $focus = $('input.focus');
        $focus.on('keyup', function (e) {
            if (e.which === 13) {
                var ind = $focus.index(this);
                $focus.eq(ind + 1).focus().select();
            }
        });
        var url = window.location.href;
        $("#cssmenu a").each(function () {
            if (url == (this.href)) {
                $(this).closest("#cssmenu .has-sub").addClass("active");
                $(this).closest("#cssmenu .has-sub ul").css("display", "block");
                $(this).closest("#cssmenu .has-sub ul li").addClass("active-menu");
            }
        });

        $('#cssmenu > ul > li > a').click(function () {
            $('#cssmenu li').removeClass('active');
            $(this).closest('li').addClass('active');
            var checkElement = $(this).next();
            if ((checkElement.is('ul')) && (checkElement.is(':visible'))) {
                $(this).closest('li').removeClass('active');
                checkElement.slideUp('normal');
            }
            if ((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
                $('#cssmenu ul ul:visible').slideUp('normal');
                checkElement.slideDown('normal');
            }
            if ($(this).closest('li').find('ul').children().length == 0) {
                return true;
            } else {
                return false;
            }
        });
    });
})(jQuery);

$(function () {
    $(".datepicker").datepicker({
        changeYear: true,
        changeMonth: true,
        yearRange: '1960:2060',
        dateFormat: 'yy-mm-dd'
    }).datepicker("setDate", new Date());
});

function dayAdd(param) {
    var d = new Date();
    d.setDate(d.getDate() + param);
    var month = '' + (d.getMonth() + 1);
    var day = '' + d.getDate();
    var year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    var someFormattedDate = [year, month, day].join('-');
    return someFormattedDate;
}

function ToJavaScriptDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [year, month, day].join('-');
}

function ToJsonDate(jsonDate) {
    var shortDate = null;    
    if (jsonDate) {  
        var regex = /-?\d+/;  
        var matches = regex.exec(jsonDate);  
        var dt = new Date(parseInt(matches[0]));  
        var month = dt.getMonth() + 1;  
        var monthString = month > 9 ? month : '0' + month;  
        var day = dt.getDate();  
        var dayString = day > 9 ? day : '0' + day;  
        var year = dt.getFullYear();  
        shortDate = year + '-' + monthString + '-' + dayString;
    }  
    return shortDate;  
};

function pad(str, max) {
    str = str.toString();
    return str.length < max ? pad("0" + str, max) : str;
}
$('.isnumber').keypress(function (e) {
    var character = String.fromCharCode(e.keyCode);
    var newValue = this.value + character;
    if (isNaN(newValue) || parseFloat(newValue) * 100 % 1 > 0) {
        e.preventDefault();
        return false;
    }
});
$(document).ready(function () {
    $("#content").hide().fadeIn(1000);
    $('.summernote').summernote({
        height: 100,
        toolbar: [
            ['style', ['bold', 'italic', 'underline', 'clear']],
            ['fontsize', ['fontsize', 'fontname']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            //['Insert', ['picture', 'link', 'table']],
        ]
    });
    $("#Image").change(function (e) {
        var img = e.target.files[0];
        if (!iEdit.open(img, true, function (res) {
            $("#result").attr("src", res);
        })) {
            alert("Whoops! That is not an image!");
        }
    });
});
$(".select2").select2({
    placeholder: '---- Select Option ----',
    selectOnBlur: true,
    tags: false,
    
});
$('#invList').on('click', function () {
    $(".list-area").fadeToggle(1000);
});
$(window).on('beforeunload', function () {
            
});
//$('#btnSave').on('click', function () {
//    var res = validation();
//    if (res == true) { $("#btnSave").prop('disabled', true); }
//});



function sumColumn(index) {
    var total = 0;
    $("table #tbody td:nth-child(" + index + ")").each(function () {
        total += parseFloat($(this).text(), 10) || 0;
    });
    return total;
}

function lessTkOrPc(totalAmount, lessField, typeField) {
    var lessAmt = 0;
    var afterlessTotal = 0;
    if ($(typeField).val() == 'Tk.') {
        lessAmt = $(lessField).val();
        afterlessTotal = $(totalAmount).val() - $(lessField).val();
        if (afterlessTotal < 0) {
            afterlessTotal = $(totalAmount).val();
            lessAmt = 0;
            $(lessField).val(0);
        }
    }
    if ($(typeField).val() == '%') {
        lessAmt = ($(totalAmount).val() * $(lessField).val())/ 100;
        afterlessTotal = $(totalAmount).val() - lessAmt
        if (afterlessTotal < 0) {
            afterlessTotal = $(totalAmount).val();
            lessAmt = 0;
            $(lessField).val(0);
        }
    }
    return [lessAmt, afterlessTotal];
}

function UpdateTotalAmount() {
    var tamount = parseFloat(sumColumn(5));
    $("#txtTotalAmount").val(tamount);
    $("#txtNetPayable").val(tamount);
    $("#txtDueAmount").val(tamount);
    $('#txtInvestigationName').focus();
    $('.grid-input input').not("input[id=txtunit]").val("");

}

// API ajax function 


$('#txtBedNo').autocomplete({
    source: function (request, response) {
        $.ajax({
            type: "GET",
            url: apiUrl + "BedApi/GetAvailabeBedList",
            dataType: "Json",
            data: { 'searchString': $("#txtBedNo").val() },
            success: function (data) {
                response(data.slice(0, 10));
            }
        });
    },
    select: function (event, ui) {
        $('#txtBedId').val(ui.item.bedId);
        $('#txtBedNo').val(ui.item.description);
        $('#txtFloorNo').val(ui.item.floorNo);
        $('#txtIndoorDepartment').val(ui.item.deptId);
        $('#txtBedCharge').val(ui.item.charge);
        $('#txtAdmissionCharge').val(ui.item.admissionCharge);
        return false;
    },
    minLength: 3
}).data("ui-autocomplete")._renderItem = function (ul, item) {
    if (ul.children().length === 0) {
        $("<thead><tr><th>Bed Info</th><th>Room No</th><th>Charge</th><th>Status</th></tr></thead>").appendTo(ul)
    }
    var html = "<td>" + item.description + "</td>";
    html += "<td>" + item.roomNo + "</td>";
    html += "<td>" + item.charge + "</td>";
    html += "<td>" + item.bedStatus + "</td>";
    return $("<tr></tr>").append(html).appendTo(ul);
};


function GetGenderList() {
    $.ajax({
        type: "GET",
        url: apiUrl + "DoctorApi/GetGenderList",
        dataType: "Json",
        success: function (data) {
            $('#txtGender').html($("<option></option>").attr("value", 0).text("-- Select --"));
            $.each(data, function (key, item) {
                var rows = "<option value='" + item.id + "'>" + item.name + "</option>";
                $('#txtGender').append(rows);
            });
        },
        error: function (errormessage) { alert(errormessage.responseText); }
    });
}

function GetReligionList() {
    $.ajax({
        type: "GET",
        url: apiUrl + "PatientRegistrationApi/GetReligionList",
        dataType: "Json",
        success: function (data) {
            $('#txtReligion').html($("<option></option>").attr("value", 0).text("-- Select --"));
            $.each(data, function (key, item) {
                var rows = "<option value='" + item.id + "'>" + item.name + "</option>";
                $('#txtReligion').append(rows);
            });
        },
        error: function (errormessage) { alert(errormessage.responseText); }
    });
}

function GetPackageList() {
    $.ajax({
        type: "GET",
        url: apiUrl + "AdmissionApi/GetIndoorPackageList",
        dataType: "Json",
        success: function (data) {
            $('#txtPackage').html($("<option></option>").attr("value", 0).text("-- Select --"));
            $.each(data, function (key, item) {
                var rows = "<option value='" + item.id + "'>" + item.name + "</option>";
                $('#txtPackage').append(rows);
            });
        },
        error: function (errormessage) { alert(errormessage.responseText); }
    });
}

function GetMainDepartmentList() {
    $.ajax({
        type: "GET",
        url: apiUrl + "AdmissionApi/GetIndoorDepartmentListByPno?deptId=0",
        dataType: "Json",
        success: function (data) {
            $('#txtIndoorDepartment').html($("<option></option>").attr("value", 0).text("-- Select --"));
            $.each(data, function (key, item) {
                var rows = "<option value='" + item.projectId + "'>" + item.title + "</option>";
                $('#txtIndoorDepartment').append(rows);
            });
        },
        error: function (errormessage) { alert(errormessage.responseText); }
    });
}

function GeDepartmentList() {
    $.ajax({
        type: "GET",
        url: apiUrl + "Doctorapi/GetOutdoorDepartmentList?UserName=1",
        dataType: "Json",
        success: function (data) {
            $('#txtOutDoorDepartment').html($("<option></option>").attr("value", 0).text("-- Select --"));
            $.each(data, function (key, item) {
                var rows = "<option value='" + item.id + "'>" + item.name + "</option>";
                $('#txtOutDoorDepartment').append(rows);
            });
        },
        error: function (errormessage) { alert(errormessage.responseText); }
    });
}

function GetMPOList() {
    $.ajax({
        type: "GET",
        url: apiUrl + "Doctorapi/GetMioList",
        dataType: "Json",
        success: function (data) {
            $('#txtmpo').html($("<option></option>").attr("value", 0).text("-- Select --"));
            $.each(data, function (key, item) {
                var rows = "<option value='" + item.id + "'>" + item.name + "</option>";
                $('#txtmpo').append(rows);
            });
        },
        error: function (errormessage) { alert(errormessage.responseText); }
    });
}


function GetBloodGroupList() {
    $.ajax({
        type: "GET",
        url: apiUrl + "PatientRegistrationApi/GetBloodGroupList",
        dataType: "Json",
        success: function (data) {
            $('#txtBloodGroup').html($("<option></option>").attr("value", 0).text("-- Select --"));
            $.each(data, function (key, item) {
                var rows = "<option value='" + item.id + "'>" + item.name + "</option>";
                $('#txtBloodGroup').append(rows);
            });
        },
        error: function (errormessage) { alert(errormessage.responseText); }
    });
}
