

function customerDelete(customerid) {
    swal({
        buttons: {
            cancel: "İPTAL",
            confirm: "TAMAM"
        },
        title: "UYARI",
        html: true,
        text: "Müşteri kaydını silmek istiyor musunuz?",
        icon: "info",
        dangerMode: true
    })
        .then((willDelete) => {
            if (willDelete) {

                $.ajax({
                    type: "POST",
                    url: "/Home/CustomerDelete",
                    data: JSON.stringify
                        ({
                            id: customerid,

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

                        window.location.href = "/Home/GetCustomerList";
                    }
                });

            }

        });
}
