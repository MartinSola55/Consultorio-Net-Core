$(document).ready(function () {
    moment.locale('es');
    $('#DatePicker').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
    });
    $('#DatePicker').on('change', function () {

    });
});