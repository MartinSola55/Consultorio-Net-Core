function fillTable(item) {
    let active = "";
    let btn = "";
    if (item.habilitada) {
        active = `<i class="bi bi-check2" style="font-size: 1.5rem"></i>`;
        btn = `<button type='button' class='btn btn-danger btn-rounded btn-sm mx-1' onclick='changeState(${item.id}, "${item.habilitada}")'><i class='bi bi-toggle-on'></i></button>`;
    } else {
        active = `<i class="bi bi-x-lg" style="font-size: 1.3rem"></i>`;
        btn = `<button type='button' class='btn btn-info btn-rounded btn-sm mx-1' onclick='changeState(${item.id}, "${item.habilitada}")'><i class='bi bi-toggle-off'></i></button>`;
    }
    let content = `
    <tr data-id='${item.id}'>
        <td>${item.nombre}</td>
        <td class='text-center'>${active}</td>
        <td class='d-flex flex-row justify-content-center'>
            <button type='button' class='btn btn-outline-info btn-rounded btn-sm mr-2' onclick='edit(${JSON.stringify(item)})' data-toggle="modal" data-target="#modalCreate"><i class="bi bi-pencil"></i></button>
            ${btn}
            <button type='button' class='btn btn-outline-danger btn-rounded btn-sm ml-2' onclick='softDelete(${item.id})'><i class='bi bi-trash'></i></button>
        </td>
    </tr>`;
    $('#ObrasSocialesTable').DataTable().row.add($(content)).draw();
}

function removeFromTable(id) {
    $('#ObrasSocialesTable').DataTable().row(`[data-id="${id}"]`).remove().draw();
}

function softDelete(id) {
    Swal.fire({
        title: '¿Seguro deseas eliminar esta obra social?',
        html: "No podrás revertir esta acción<br/>La obra social no se eliminará si existen pacientes asociados a ella.",
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
            $("#form-delete input[name='id']").val(id);
            sendForm("delete");
        }
    });
}

function changeState(id, isActive) {
    let title = "";
    if (isActive === "True" || isActive === "true") {
        title = "¿Seguro deseas dar de baja esta obra social?";
    } else {
        title = "¿Seguro deseas dar de alta esta obra social?";
    }
    Swal.fire({
        title: title,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Confirmar',
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-warning waves-effect waves-light px-3 py-2',
            cancelButton: 'btn btn-default waves-effect waves-light px-3 py-2'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $("#form-state input[name='id']").val(id);
            sendForm("state");
        }
    });
}

function sendForm(action) {
    let form = document.getElementById(`form-${action}`);

    let errors = $(".input-validation-error");

    if (errors.length === 0) {
        $.ajax({
            url: $(form).attr('action'),
            method: $(form).attr('method'),
            data: $(form).serialize(),
            success: function(response) {
                $("#btnCloseModalCreate").click();
                toastrSuccess(response.message);
                if (action === 'create') {
                    fillTable(response.data);
                } else if (action === 'delete') {
                    removeFromTable(response.data);
                } else {
                    removeFromTable(response.data.id);
                    fillTable(response.data);
                }
            },
            error: function (errorThrown) {
                toastrWarning(errorThrown.responseJSON.message, errorThrown.responseJSON.title);
            }
        });   
    }
}

function add() {
    $("#modalTitle").text("Agregar obra social");
    $("#formContainer form").attr("action", "/ObrasSociales/Create");
    $("#formContainer form").attr("id", "form-create");
    $("#formContainer form input:not([type='hidden']").val("");
    $("input[name='NewObraSocial.ID']").prop("disabled", true);
    $("#btnSendModal").text("Agregar");
};
    
function edit(entity) {
    $("#formContainer form input:not([type='hidden']").val("");
    $("input[name='NewObraSocial.ID']").val(entity.id);
    $("input[name='NewObraSocial.ID']").prop("disabled", false);

    $("#modalTitle").text("Editar obra social");
    $("#formContainer form").attr("action", "/ObrasSociales/Edit");
    $("#formContainer form").attr("id", "form-edit");
    $("#btnSendModal").text("Confirmar");

    $("input[name='NewObraSocial.Nombre']").val(entity.nombre);
}

$(document).ready(function () {
    $("#btnSendModal").on("click", function () {
        if ($("#formContainer form").attr('id') === 'form-create') {
            sendForm("create");
        } else if ($("#formContainer form").attr('id') === 'form-edit') {
            sendForm("edit");
        }
    });
    $('#ObrasSocialesTable').DataTable({
        "language": {
            "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ obras sociales",
            "sInfoEmpty": "Mostrando 0 a 0 de 0 obras sociales",
            "sInfoFiltered": "(filtrado de _MAX_ obras sociales en total)",
            "emptyTable": 'No hay obras sociales que coincidan con la búsqueda',
            "sLengthMenu": "Mostrar _MENU_ obras sociales",
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