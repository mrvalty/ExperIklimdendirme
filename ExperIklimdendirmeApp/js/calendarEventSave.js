
$(document).ready(function () {
    CalendarEvents();
    getCustomersList();
    //    getCustomerSelectItem();
});

function CalendarEvents() {
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
            //alert('clicked ' + selectedDate.format());
            $('#baslangicTarihi').val(selectedDate.format());
            $('#bitisTarihi').val(selectedDate.format());
            $('#event_entry_modal').modal("show");

        },
        editable: true,

        events: function (start, end, timezone, callback) {
            $.ajax({
                url: '/Calendar/GetCalendarEvents/',
                type: "POST",
                dataType: "JSON",

                success: function (msg) {
                    var events = [];

                    $.each(msg, function (i, data) {
                        events.push(
                            {
                                id : data.id,
                                title: data.title,
                                start: moment(data.start),
                                end: moment(data.end)
                            });
                    });

                    callback(events);
                }
            });
        },


        eventClick: function (event, delta, revertFunc) {
            getEvents(event);
        }

    });
}

function meetingSave() {

    var _title = $("#title").val();
    var _baslangicTarihi = $("#baslangicTarihi").val();
    var _bitisTarihi = $("#bitisTarihi").val();
    var _musteriid = $("#customerList").val();

    if (_title == "" || _baslangicTarihi == "" || _bitisTarihi == "" || _musteriid == "") {

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
        url: "/Calendar/AddOrEditItem/",
        data: JSON.stringify
            ({

                title: _title,
                start: _baslangicTarihi,
                end: _bitisTarihi,
                Customerid: _musteriid

            }),

        contentType: "application/json; charset=utf-8",

        dataType: "json",

        beforeSend: function () {

        },
        error: function (request, status, error) {
            $(function () {
                toastr.error("Sistem bir hata oluştu.");
                toastr.options = {
                    "timeOut": "10000",
                    "showDuration": "10000",
                    "progressBar": true
                }
            });
        },
        success: function () {

        },
        complete: function () {
            //$('#event_entry_modal').modal('hide');
            window.location.href = "/Calendar/GetCalendarEvents";
        }
    });

}

function getEvents(selectedItem) {
    getCustomerSelectItem();
    var id = selectedItem.id;

    //$("#elementId :selected").text(); // The text content of the selected option
    //$("#elementId").val(); // The value of the selected option

    $.ajax({
        type: "POST",
        url: "/Calendar/GetCalendarItemEvent/",
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
            $('#customerid').val(msg.customerid);

        },
        complete: function () {
            $('#event_entry_modal2').modal({
                show: true,

            });
        }
    });
}

function meetingUpdate(calendarid) {

    var _title = $("#title1").val();
    var _baslangicTarihi = $("#baslangicTarihi1").val();
    var _bitisTarihi = $("#bitisTarihi1").val();
    var _customerid = $('#customerid').val();

    $.ajax({
        type: "POST",
        url: "/Calendar/UpdateItemDate/",
        data: JSON.stringify
            ({
                id: calendarid,
                title: _title,
                start: _baslangicTarihi,
                end: _bitisTarihi,
                Customerid: _customerid

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
            window.location.href = "/Calendar/GetCalendarEvents";
        }
    });

}

function meetingDelete(calendarid) {
    var _id = calendarid;
    swal({
        buttons: {
            cancel: "İPTAL",
            confirm: "TAMAM"
        },
        title: "UYARI",
        html: true,
        text: "Bu randevuyu silmek istiyor musunuz?",
        icon: "info",
        dangerMode: true
    })
        .then((willDelete) => {
            if (willDelete) {

                $.ajax({
                    type: "POST",
                    url: "/Calendar/DeleteItemDate",
                    data: JSON.stringify
                        ({
                            id: _id

                        }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function () {

                    },
                    error: function (request, status, error) {

                    },
                    success: function (msg) {

                    },
                    complete: function () {

                        window.location.href = "/Calendar/GetCalendarEvents";
                    }
                });

            }

        });
}

function getCustomersList() {
    $.ajax({
        type: "POST",
        url: "/Calendar/GetCustomersList/",
        data: JSON.stringify
            ({

            }),

        contentType: "application/json; charset=utf-8",

        dataType: "json",

        beforeSend: function () {

        },
        error: function (request, status, error) {


        },
        success: function (msg) {
            var _dizi = msg;

            var content = '';
            content += "<option value='-1'>Seçiniz...</option>";
            for (i = 0; i < _dizi.length; i++) {
                content += "<option value='" + _dizi[i].customerid + "'>" + _dizi[i].Name + "</option>";
                //alert(_dizi[i].FirstName);
            }
            $("#customerList").html(content);

            //$('#event_entry_modal2').modal({
            //    show: true,

            //});
        },
        complete: function () {

        }
    });
}

function getCustomerSelectItem() {
    $.ajax({
        type: "POST",
        url: "/Calendar/GetCustomersList/",
        data: JSON.stringify
            ({

            }),

        contentType: "application/json; charset=utf-8",

        dataType: "json",

        beforeSend: function () {

        },
        error: function (request, status, error) {


        },
        success: function (msg) {
            var _dizi = msg;

            var content = '';
            content += "<option value='-1'>Seçiniz...</option>";
            for (i = 0; i < _dizi.length; i++) {
                content += "<option value='" + _dizi[i].customerid + "'>" + _dizi[i].Name + "</option>";
                //alert(_dizi[i].FirstName);
            }
            $("#customerid").html(content);

            //$('#event_entry_modal2').modal({
            //    show: true,

            //});
        },
        complete: function () {

        }
    });
}

