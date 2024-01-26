$(document).ready(function () {
    moment.locale('es');
    $('#TurnosDatePicker').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        isInvalidDate: function (date) {
            if (date.day() == 1 || date.day() == 2 || date.day() == 3 || date.day() == 5)
                return false;
            return true;
        },
    }); 
    $('#TurnosDatePicker').on('change', function () {
        searchTurnos($(this).val());
    });
});

function searchTurnos(date) {
    $('#turnosTable tbody').empty();
    let loadingRow = `
    <tr>
        <td>
            <div class="spinner-border text-primary" role="status">
                <span class="sr-only">Cargando...</span>
            </div>
        </td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>`;
    $('#turnosTable tbody').append($(loadingRow));
    $('#form-searchTurnosByDate input[name="dateString"]').val(date);
    let form = $('#form-searchTurnosByDate');
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            $('#turnosTable tbody').empty();
            response.data.forEach(turno => {
                let btns = '-';
                if (turno.disponible) {
                    btns = `
                    <button type='button' class='btn btn-info btn-rounded btn-sm ml-2' onclick='addTurno(${turno.diaHorarioID})'><i class='bi bi-plus-lg'></i></button>
                    <button type='button' class='btn btn-outline-danger btn-rounded btn-sm ml-2' onclick='deleteHorario(${turno.diaHorarioID})'><i class='bi bi-trash3'></i></button>
                    `;
                } else if (turno.nombre !== '-') {
                    btns = `
                    <button type='button' class='btn btn-outline-info btn-rounded btn-sm ml-2' onclick='editTurno(${JSON.stringify(turno)})'><i class='bi bi-pencil'></i></button>
                    <button type='button' class='btn btn-outline-danger btn-rounded btn-sm ml-2' onclick='deleteTurno(${turno.diaHorarioID})'><i class='bi bi-trash3'></i></button>
                    `;
                }
                let row = `<tr data-id='${turno.diaHorarioID}'>
                    <td>${turno.nombre}</td>
                    <td>${turno.apellido}</td>
                    <td>${turno.obraSocial}</td>
                    <td>${turno.telefono}</td>
                    <td>${turno.hora}</td>
                    <td>${turno.disponible === true ? '<i class="bi bi-check2" style="font-size: 1.5rem"></i>' : '<i class="bi bi-x-lg" style="font-size: 1.3rem"></i>'}</td>
                    <td class="d-flex flex-row">${btns}</td>
                </tr>`;
                $('#turnosTable tbody').append($(row));
            });
        },
        error: function (error) {
            $('#turnosTable tbody').empty();
            let row = `
            <tr>
                <td>
                    <h6 class="text-danger">No se pudo cargar la información</h6>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>`;
            $('#turnosTable tbody').append($(row));
        }
    });
}

function addTurno(id) {
    $('#modalTitle').text('Agregar turno');
    let form = $('#form-turno');
    $(form).attr("action", "/Turnos/Create");
    $("#formContainer form input[name='Turno.DiaHorarioID']").val(id);
    $("#formContainer form input[name='Turno.ID']").prop("disabled", true);
    $("#formContainer form input:not([type='hidden']").val("");
    $("#formContainer form select").val("");
    $('.modal').modal('show');
}

function fillTableTurno(turno) {
    let row = `
        <td>${turno.nombre}</td>
        <td>${turno.apellido}</td>
        <td>${turno.obraSocial}</td>
        <td>${turno.telefono}</td>
        <td>${turno.hora}</td>
        <td><i class="bi bi-x-lg" style="font-size: 1.3rem"></i></td>
        <td class="d-flex flex-row">
            <button type='button' class='btn btn-outline-info btn-rounded btn-sm ml-2' onclick='editTurno(${JSON.stringify(turno)})'><i class='bi bi-pencil'></i></button>
            <button type='button' class='btn btn-outline-danger btn-rounded btn-sm ml-2' onclick='deleteTurno(${turno.diaHorarioID})'><i class='bi bi-trash3'></i></button>
        </td>`;
    $(`#turnosTable tbody tr[data-id='${turno.diaHorarioID}']`).html(row);
}

function fillTableHorarioLibre(horario) {
    let row = `
        <td>-</td>
        <td>-</td>
        <td>-</td>
        <td>-</td>
        <td>${horario.hora}</td>
        <td><i class="bi bi-check2" style="font-size: 1.5rem"></i></td>
        <td class="d-flex flex-row">
            <button type='button' class='btn btn-info btn-rounded btn-sm ml-2' onclick='addTurno(${horario.id})'><i class='bi bi-plus-lg'></i></button>
            <button type='button' class='btn btn-outline-danger btn-rounded btn-sm ml-2' onclick='deleteHorario(${horario.id})'><i class='bi bi-trash3'></i></button>
        </td>
    `;
    $(`#turnosTable tbody tr[data-id='${horario.id}']`).html(row);
}

function fillTableHorarioNoDisponible(horario) {
    let row = `
        <td>-</td>
        <td>-</td>
        <td>-</td>
        <td>-</td>
        <td>${horario.hora}</td>
        <td><i class="bi bi-x-lg" style="font-size: 1.3rem"></i></td>
        <td>-</td>
    `;
    $(`#turnosTable tbody tr[data-id='${horario.id}']`).html(row);
}

function deleteTurno(id) {
    Swal.fire({
        title: '¿Seguro deseas eliminar este turno?',
        text: "No podrás revertir esta acción",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Confirmar',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-danger waves-effect waves-light px-3 py-2',
            cancelButton: 'btn btn-default waves-effect waves-light px-3 py-2'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $("#form-deleteTurno input[name='diaHorarioID']").val(id);
            sendForm("deleteTurno");
        }
    });
}

function deleteHorario(id) {
    Swal.fire({
        title: '¿Seguro deseas eliminar este horario?',
        text: "No podrás revertir esta acción",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Confirmar',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-danger waves-effect waves-light px-3 py-2',
            cancelButton: 'btn btn-default waves-effect waves-light px-3 py-2'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $("#form-deleteHorario input[name='diaHorarioID']").val(id);
            sendForm("deleteHorario");
        }
    });
}

function sendForm(action) {
    let form = document.getElementById(`form-${action}`);

    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            toastrSuccess(response.message);
            if (action === 'turno') {
                $('.modal').modal('hide');
                fillTableTurno(response.data);
            } else if (action === 'deleteTurno') {
                fillTableHorarioLibre(response.data);
            } else if (action === 'deleteHorario') {
                fillTableHorarioNoDisponible(response.data);
            }
        },
        error: function (errorThrown) {
            toastrWarning(errorThrown.responseJSON.message, errorThrown.responseJSON.title);
        }
    });
}