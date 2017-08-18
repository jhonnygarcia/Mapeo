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
    $('#cbx-plan').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-plan',
        toolbar: {
            reset: function () {
                limpiarComponentesPlanEstudio();
            }
        },
        fnSelect: function () {
            var itemData = $('#cbx-plan').combobox('getData');
            $('#lb-id-plan-de-estudio').text(itemData.Id);
            $('#lb-anyo').text(itemData.Anyo);
            $('#lb-plan-oficial').text(itemData.EsOficial
                    ? $.Globalization.localize('TextSi')
                    : $.Globalization.localize('TextNo'));
            $('#lb-estudio').text(itemData.Estudio);
            $('#lb-tipo-de-estudio-plan').text(itemData.TipoEstudio);
            $('#lb-rama-de-conocimiento-plan').text(itemData.Rama);
            $('#lb-titulo').text(itemData.Titulo);
        },
        fnTextChange: function () {
            limpiarComponentesPlanEstudio();
        }
    }));
    $('#btn-guardar').click(function () {
        var params = {};
        var errors = new Array();
        params.IdPlantillaEstudio = $('#cbx-plantilla-estudio').combobox('getId');
        params.IdPlan = $('#cbx-plan').combobox('getId');

        if (isNull(params.IdPlantillaEstudio)) {
            errors.push($.Globalization.localize('ErrorNoPlantillaEstudio'));
        }
        if (isNull(params.IdPlan)) {
            errors.push($.Globalization.localize('ErrorNoPlan'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-plantilla-estudio',
                type: 'PUT',
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
    $('#btn-guardar-continuar').click(function () {
        var params = {};
        var errors = new Array();
        params.IdPlantillaEstudio = $('#cbx-plantilla-estudio').combobox('getId');
        params.IdPlan = $('#cbx-plan').combobox('getId');

        if (isNull(params.IdPlantillaEstudio)) {
            errors.push($.Globalization.localize('ErrorNoPlantillaEstudio'));
        }
        if (isNull(params.IdPlan)) {
            errors.push($.Globalization.localize('ErrorNoPlan'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-plantilla-estudio',
                type: 'PUT',
                data: $.toJSON(params),
                success: function (data, status, xhr) {
                    showMessage($.Globalization.localize('MessageOperacionExitosamente'), true);
                    $('#cbx-plantilla-estudio').combobox('reset');
                    limpiarComponentesPlantillaEstudio();
                    $('#cbx-plan').combobox('reset');
                    limpiarComponentesPlanEstudio();
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

    limpiarComponentesPlantillaEstudio();
    limpiarComponentesPlanEstudio();
});

function limpiarComponentesPlantillaEstudio() {
    $('#lb-id-plantilla-estudio').text('-');
    $('#lb-tipo-de-estudio').text('-');
    $('#lb-rama-de-conocimiento').text('-');
}

function limpiarComponentesPlanEstudio() {
    $('#lb-id-plan-de-estudio').text('-');
    $('#lb-anyo').text('-');
    $('#lb-plan-oficial').text('-');
    $('#lb-estudio').text('-');
    $('#lb-tipo-de-estudio-plan').text('-');
    $('#lb-rama-de-conocimiento-plan').text('-');
    $('#lb-titulo').text('-');
}