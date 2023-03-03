
$(document).ready(function () {
    GetCalendarEvents();
    $('.input-group.date').datepicker({ format: "dd.mm.yyyy" });
    $('#baslangicTarihi1').datepicker({ format: "dd.mm.yyyy" });
});

//$(function () {
//    $('#calendar').fullCalendar({
//        selectable: true,
//        header: {
//            left: 'prev,next today',
//            center: 'title',
//            right: 'month,agendaWeek,agendaDay'
//        },
//        dayClick: function (date) {
//            alert('clicked ' + date.format());
//        },
//        select: function (startDate, endDate) {
//            alert('selected ' + startDate.format() + ' to ' + endDate.format());
//        }
//    });

//});

function GetCalendarEvents() {
    $('#calendar').fullCalendar({
        selectable: true,
        lang: 'tr',
        locale: 'tr',
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        dayClick: function (selectedDate) {
            /*alert('clicked ' + date.format());*/
            $('#event_entry_modal').modal("show");
        },
        editable: true,
        events: '/Home/GetCalendarEvents/',
        eventClick: function (event, delta, revertFunc) {
            getEvents(event);
        }

    });
}

function meetingSave() {

    var _title = $("#title").val();
    var _baslangicTarihi = $("#baslangicTarihi").val();
    var _bitisTarihi = $("#bitisTarihi").val();

    if (title == "" || baslangicTarihi == "" || bitisTarihi == "") {

        $(function () {
            toastr.warning("Lütfen Tüm Alanları Giriniz.");
            toastr.options = {
                "timeOut": "10000",
                "showDuration": "10000",
                "progressBar": true
            }
        });
        return false;
    }

    $.ajax({
        type: "POST",
        url: "/Home/AddOrEditItem/",
        data: JSON.stringify
            ({

                title: _title,
                start: _baslangicTarihi,
                end: _bitisTarihi

            }),

        contentType: "application/json; charset=utf-8",

        dataType: "json",

        beforeSend: function () {

        },
        error: function (request, status, error) {

            UyariMesajiVer('Sistemsel bir hata oluştu');
        },
        success: function () {
        },
        complete: function () {
        }
    });

}

function getEvents(selectedItem) {

    var id = selectedItem.id;

    $.ajax({
        type: "POST",
        url: "/Home/GetCalendarItemEvent/",
        data: JSON.stringify
            ({
                id: id
            }),

        contentType: "application/json; charset=utf-8",

        dataType: "json",

        beforeSend: function () {

        },
        error: function (request, status, error) {


        },
        success: function (msg) {
            
            $('#title1').val(msg.title);
            $('#baslangicTarihi1').val(msg.start);
            $('#bitisTarihi1').val(msg.end);
            $('#calendarid').val(msg.calendarid);

            $('#event_entry_modal2').modal({
                show: true,

            });
        },
        complete: function () {

        }
    });
}

function meetingUpdate() {

    var _id = $("#calendarid").val();
    var _title = $("#title1").val();
    var _baslangicTarihi = $("#baslangicTarihi1").val();
    var _bitisTarihi = $("#bitisTarihi1").val();

    //if (title == "" || baslangicTarihi == "" || bitisTarihi == "") {

    //    $(function () {
    //        toastr.warning("Lütfen Tüm Alanları Giriniz.");
    //        toastr.options = {
    //            "timeOut": "10000",
    //            "showDuration": "10000",
    //            "progressBar": true
    //        }
    //    });
    //    return false;
    //}

    $.ajax({
        type: "POST",
        url: "/Home/UpdateItemDate/",
        data: JSON.stringify
            ({
                id : _id,
                title: _title,
                start: _baslangicTarihi,
                end: _bitisTarihi

            }),

        contentType: "application/json; charset=utf-8",

        dataType: "json",

        beforeSend: function () {

        },
        error: function (request, status, error) {

        },
        success: function () {
        },
        complete: function () {
        }
    });

}
