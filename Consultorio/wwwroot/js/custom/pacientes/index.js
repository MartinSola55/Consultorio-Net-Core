function createDate(dateString) {
    let date = new Date(dateString);
    let day = ("0" + date.getDate()).slice(-2);
    let month = ("0" + (date.getMonth() + 1)).slice(-2);
    let year = date.getFullYear();
    return day + "/" + month + "/" + year;
}

function loadingRow() {
    $('#PacientesTable tbody').empty();
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
        <td></td>
        <td></td>
    </tr>`;
    $('#PacientesTable tbody').append($(loadingRow));
}

function errorRow() {
    $('#PacientesTable tbody').empty();
    let row = `
    <tr>
        <td><h6 class="text-danger">No se pudo cargar la información</h6></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
    </tr>`;
    $('#PacientesTable tbody').append($(row));
}

function fillTable(pacientes) {
    $('#PacientesTable tbody').empty();
    pacientes.forEach(item => {
        let row = `
                <tr data-id="${item.id}">
                    <td><a href="/Pacientes/Detalles/${item.id}" target="_blank">${item.nombre}</a></td>
                    <td>${item.apellido}</td>
                    <td>${item.direccion}</td>
                    <td>${item.localidad}</td>
                    <td>${item.telefono}</td>
                    <td>${item.obraSocial}</td>
                    <td>${item.fechaNacimiento}</td>
                    <td>${item.updatedAt}</td>
                    <td class='d-flex flex-row justify-content-center'>
                        <button type='button' class='btn btn-outline-danger btn-rounded btn-sm mr-2' onclick='softDelete(${JSON.stringify(item)})'><i class="bi bi-trash3"></i></button>
                    </td>
                </tr>`;
        $('#PacientesTable tbody').append($(row));
    });
}

$(document).ready(function () {
    let timeoutId;
    $('#NombreApellido').on('input', function () {
        clearTimeout(timeoutId);
        timeoutId = setTimeout(searchByName, 1000);
    });

    moment.locale('es');
    $('#DatePicker').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true
    });

    $('#DatePicker').val('');

    $('#DatePicker').on('change', function () {
        loadingRow();
        searchByDate();
    });
});

function searchByName() {
    const name = $('#NombreApellido').val();
    if (name.length < 4) return toastrWarning("Debes ingresar al menos 4 caracteres para buscar por nombre o apellido");
    loadingRow();
    const form = $('#form-searchNombreApellido');
    $(form).find('input[name="words"]').val(name);
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            fillTable(response.data)
        },
        error: function () {
            errorRow();
        }
    });
}

function searchByDate() {
    const date = $('#DatePicker').val();
    const form = $('#form-searchByDate');
    $(form).find('input[name="date"]').val(date);
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            fillTable(response.data)
        },
        error: function () {
            errorRow();
        }
    });
}

function softDelete(paciente) {
    Swal.fire({
        title: `¿Seguro deseas eliminar al paciente ${paciente.apellido}, ${paciente.nombre}?`,
        html: "No podrás revertir esta acción<br/>Se eliminarán también todas sus historias clínicas",
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
            sendForm(paciente.id);
        }
    });
}

function sendForm(id) {
    let form = document.getElementById('form-delete');
    $(form).find('input[name="id"]').val(id)

    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            toastrSuccess(response.message);
            removeFromTable(response.data);
        },
        error: function (errorThrown) {
            toastrWarning(errorThrown.responseJSON.message, errorThrown.responseJSON.title);
        }
    });
}

function removeFromTable(id) {
    $(`#PacientesTable tbody tr[data-id="${id}"]`).remove();
}