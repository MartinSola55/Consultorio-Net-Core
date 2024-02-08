function fillTable(item) {
    let content = `
    <tr data-id='${item.id}'>
        <td>${item.mensaje}</td>
        <td>${item.desde}</td>
        <td>${item.hasta}</td>
        <td class='d-flex flex-row justify-content-center'>
            <button type='button' class='btn btn-outline-danger btn-rounded btn-sm ml-2' onclick='softDelete(${item.id})'><i class='bi bi-trash'></i></button>
        </td>
    </tr>`;
    $('#DataTable').DataTable().row.add($(content)).draw();
}

function removeFromTable(id) {
    $('#DataTable').DataTable().row(`[data-id="${id}"]`).remove().draw();
}

function softDelete(id) {
    Swal.fire({
        title: '¿Seguro deseas eliminar esta alerta?',
        text: "No podrás revertir esta acción",
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Confirmar',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-danger waves-effect waves-light px-3 py-2',
            cancelButton: 'btn btn-default waves-effect waves-light px-3 py-2'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $("#form-delete input[name='id']").val(id);
            sendForm("delete");
        }
    });
}

function sendForm(action) {
    let form = document.getElementById(`form-${action}`);

    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function(response) {
            $("#btnCloseModalCreate").click();
            toastrSuccess(response.message);
            if (action === 'create') {
                fillTable(response.data);
            } else {
                removeFromTable(response.data);
            }
        },
        error: function (errorThrown) {
            toastrWarning(errorThrown.responseJSON.message, errorThrown.responseJSON.title);
        }
    });   
}

function add() {
    $("#formContainer form input:not([type='hidden']").val("");
};

$(document).ready(function () {
    $('#DataTable').DataTable({
        "language": {
            "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ alertas",
            "sInfoEmpty": "Mostrando 0 a 0 de 0 alertas",
            "sInfoFiltered": "(filtrado de _MAX_ alertas en total)",
            "emptyTable": 'No hay alertas que coincidan con la búsqueda',
            "sLengthMenu": "Mostrar _MENU_ alertas",
            "sSearch": "Buscar:",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior",
            },
        },
    });
});