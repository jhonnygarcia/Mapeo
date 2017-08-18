$(document).ready(function () {
    $('#cbx-plantilla-estudio').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-plantilla-estudio',
        toolbar: {
            reset: function () {
                limpiarComponentesPlantillaEstudio();
            }
        },
        fnSelect: function () {
            var itemData = $('#cbx-plantilla-estudio').combobox('getData');
            $('#lb-id-plantilla-estudio').text(itemData.Id);
            $('#lb-tipo-de-estudio').text(itemData.TipoEstudio);
            $('#lb-rama-de-conocimiento').text(itemData.Rama);
        },
        fnTextChange: function () {
            limpiarComponentesPlantillaEstudio();
        }
    }));
    $('#btn-guardar').click(function () {
        var params = {};
        var errors = new Array();
        params.IdPlantillaEstudio = $('#cbx-plantilla-estudio').combobox('getId');
        params.IdPlan = $('#hd-id-plan').val();

        if (isNull(params.IdPlantillaEstudio)) {
            errors.push($.Globalization.localize('ErrorNoPlantillaEstudio'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-plantilla-estudio/' + params.IdPlan,
                data: $.toJSON(params),
                success: function (data, status, xhr) {
                    showMessage($.Globalization.localize('MessageOperacionExitosamente'), true);
                    gotoController('PlantillaEstudioIntegracion/Index');
                },
                error: function (xhr) {
                    if (xhr.status !== 500) {
                        var result = $.parseJSON(xhr.responseText);
                        showErrors(result.Errors);
                    } else {
                        showApplicationFatalErrorMessage();
                    }
                },
                complete: function (xhr, status) {
                    $.unblockUI();
                }
            });
        } else {
            showErrors(errors);
        }
    });
    $('#btn-cancelar').click(function () {
        gotoController('PlantillaEstudioIntegracion/Index');
    });
    LoadData();
});

function LoadData() {
    var id = $('#hd-id-plan').val();
    $.ajax({
        url: SiteUrl + 'api/v1/mapeo-plantilla-estudio/' + id,
        type: 'GET',
        success: function (data, status, xhr) {
            $('#cbx-plantilla-estudio')
                .combobox('setValue', data.PlantillaEstudio.DisplayName)
                .combobox('setId', data.PlantillaEstudio.Id);

            $('#lb-id-plantilla-estudio').text(data.PlantillaEstudio.Id);
            $('#lb-tipo-de-estudio').text(data.PlantillaEstudio.TipoEstudio.Nombre);
            $('#lb-rama-de-conocimiento').text(data.PlantillaEstudio.Rama.Nombre);

            $('#lb-plan-de-estudio').text(data.Plan.DisplayName);
            $('#lb-id-plan-de-estudio').text(data.Plan.Id);
            $('#lb-anyo').text(data.Plan.Anyo);
            $('#lb-plan-oficial').text(data.Plan.EsOficial
                    ? $.Globalization.localize('TextSi')
                    : $.Globalization.localize('TextNo'));
            $('#lb-estudio').text(data.Plan.Estudio.DisplayName);
            $('#lb-tipo-de-estudio-plan').text(data.Plan.Estudio.TipoEstudio.Nombre);
            $('#lb-rama-de-conocimiento-plan').text(data.Plan.Estudio.RamaConocimiento.Nombre);
            $('#lb-titulo').text(data.Plan.Titulo.DisplayName);
        },
        error: function (xhr) {
            if (xhr.status !== 500) {
                var result = $.parseJSON(xhr.responseText);
                showErrors(result.Errors);
            } else {
                showApplicationFatalErrorMessage();
            }
        }
    });
}

function limpiarComponentesPlantillaEstudio() {
    $('#lb-id-plantilla-estudio').text('-');
    $('#lb-tipo-de-estudio').text('-');
    $('#lb-rama-de-conocimiento').text('-');
}