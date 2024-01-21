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
    let loadingRow = `<tr>
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
                let row = `<tr>
                    <td>${turno.nombre}</td>
                    <td>${turno.apellido}</td>
                    <td>${turno.obraSocial}</td>
                    <td>${turno.telefono}</td>
                    <td>${turno.hora}</td>
                    <td>${turno.disponible === true ? '<i class="bi bi-check2" style="font-size: 1.5rem"></i>' : '<i class="bi bi-x-lg" style="font-size: 1.3rem"></i>'}</td>
                    <td>ALGO</td>
                </tr>`;
                $('#turnosTable tbody').append($(row));
            });
        },
        error: function (error) {
            $('#turnosTable tbody').empty();
            let row = `<tr>
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