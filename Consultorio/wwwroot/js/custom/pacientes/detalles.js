function toastrSuccess(message, title) {
    return toastr.success(
        message,
        title,
        {
            "onclick": null,
            "showDuration": 300,
            "hideDuration": 1000,
        }
    );
};

function toastrWarning(message, title) {
    return toastr.warning(
        message,
        title,
        {
            "onclick": null,
            "showDuration": 300,
            "hideDuration": 1000,
        }
    );
};

function editFechaNacimiento(btn) {
    let nacimiento = $('#pacienteFechaNacimiento').text();
    $(btn).hide();
    $(btn).next().show();
    $('#pacienteFechaNacimiento').hide();
    $('#pacienteFechaNacimiento').after(`<input type="text" id="newFechaNacimiento" class="form-control" value="${nacimiento}" />`);
    moment.locale('es');
    $('#newFechaNacimiento').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
    });
    $(`#newFechaNacimiento`).focus();
}

function editDatos(dato, btn) {
    let datoValue = $(`#paciente${dato}`).text();
    $(btn).hide();
    $(btn).next().show();
    $(`#paciente${dato}`).hide();
    $(`#paciente${dato}`).after(`<input type="text" id="new${dato}" class="form-control" value="${datoValue}" />`);
    $(`#new${dato}`).focus();
}

function saveDatos(dato, btn) {
    $(btn).hide();
    const spinner = $(btn).next();
    spinner.show();

    let datoValue = $(`#new${dato}`).val();
    $('#datoToUpdate').val(dato);
    $('#datoValue').val(datoValue);
    if (dato === 'ObraSocial') datoValue = $(`#new${dato} option:selected`).text();
    sendForm(spinner, btn, dato, datoValue);
}

function sendForm(spinner, btn, dato, datoValue) {
    const form = $('#form-datos');
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            spinner.hide();
            $(btn).prev().show();
            $(`#paciente${dato}`).show();
            $(`#paciente${dato}`).text(datoValue);
            if (dato === 'ObraSocial') $(`#paciente${dato}`).data('id', $('#newObraSocial').val());
            $(`#new${dato}`).remove();
            toastrSuccess(response.message, response.title);
        },
        error: function (response) {
            spinner.hide();
            $(btn).show();
            toastrWarning(response.responseJSON.message, response.responseJSON.title);
        }
    });
}

function saveHC() {
    let date = $('#DatePicker').val();
    let description = $('#newDescription').val();
    if (description == '') {
        Swal.fire({
            icon: 'warning',
            title: "Error",
            text: "Debes ingresar una descripción",
            confirmButtonColor: '#2f3d4a',
        });
        return;
    }
    let form = $('#form-saveHC');
    $.ajax({
        url: $(form).attr('action'),
        method: $(form).attr('method'),
        data: $(form).serialize(),
        success: function (response) {
            toastrSuccess(response.message, response.title);
            let newCard = `
                        <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                            <div class="card card-outline-inverse">
                                <div class="card-header">
                                    <h4 class="m-b-0 text-white">
                                        ${date}
                                        <button class="btn btn-sm waves-effect waves-light" type="button" onclick="editHC(${response.data})"><i class="bi bi-pencil-square text-white"></i></button>
                                    </h4>
                                </div>
                                <div class="card-body">
                                    <p class="card-text">${description}</p>
                                </div>
                            </div>
                        </div>
                `;
            $('#divNewHC').hide();
            $('#divNewHC').after(newCard);
        },
        error: function (response) {
            toastrWarning(response.responseJSON.message, response.responseJSON.title);
        }
    });
}