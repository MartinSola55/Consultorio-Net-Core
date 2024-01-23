$(document).ready(function () {
    moment.locale('es');
    $('#DatePicker').daterangepicker({
        ranges: {
            'Hoy': [moment(), moment()],
            'Este mes': [moment().startOf('month'), moment().endOf('month')],
            'Siguiente mes': [moment().add(1, 'month').startOf('month'), moment().add(1, 'month').endOf('month')],
            'Siguientes 2 meses': [moment().add(1, 'month').startOf('month'), moment().add(2, 'month').endOf('month')],
            'Siguientes 3 meses': [moment().add(1, 'month').startOf('month'), moment().add(3, 'month').endOf('month')],
        },
        locale: {
            applyLabel: "Aceptar",
            cancelLabel: 'Cancelar',
            customRangeLabel: 'Seleccionar rango',
        },
        applyClass: "btn-info",
    });
});

function checkAll(e) {
    let checkboxes = $('input[type="checkbox"]');
    if (e.checked) {
        for (let i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked = true;
        }
    } else {
        for (let i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked = false;
        }
    }
}

function save() {
    let checkboxes = $('input[name="horarioCheckBox"]:checked');
    let ids = [];
    for (let i = 0; i < checkboxes.length; i++) {
        ids.push(checkboxes[i].id.split('_')[1]);
    }
    let date = $('#DatePicker').val().split(' - ');
    let start = date[0];
    let end = date[1];
    $('#horarios').val(ids);
    $('#dateFrom').val(start);
    $('#dateTo').val(end);

    let form = $("#form-save");
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            Swal.fire({
                title: response.message,
                icon: 'success',
                showCancelButton: false,
                confirmButtonColor: '#1e88e5',
                confirmButtonText: 'OK',
                allowOutsideClick: false,
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.reload();
                }
            });
        },
        error: function (errorThrown) {
            Swal.fire({
                icon: 'error',
                title: errorThrown.responseJSON.title,
                html: errorThrown.responseJSON.message + "<br/>" + errorThrown.responseJSON.error,
                confirmButtonColor: '#1e88e5',
            });
        }
    });
}