$(document).ready(function () {
    $('#cbx-estudio').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-estudio',
        toolbar: {
            reset: function () {
                limpiarComponentesEstudioUnir();
            }
        },
        fnSelect: function () {
            var itemData = $('#cbx-estudio').combobox('getData');
            $("#lb-id-estudio").text(itemData.Id);
            $("#lb-plan").text(itemData.Plan);
            $("#lb-activo").text(itemData.Activo);
            $("#lb-tipo-estudio").text(itemData.TipoEstudio.Nombre);
            $("#lb-rama").text(itemData.Rama);
            $("#lb-titulo-ruct").text(itemData.Titulo ? itemData.Titulo.DisplayName : '-');
        },
        fnTextChange: function () {
            limpiarComponentesEstudioUnir();
        }
    }));
    $('#cbx-plan-integracion').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-plan',
        toolbar: {
            reset: function () {
                limpiarComponentesPlanEstudio();
                $('#cbx-especializacion').combobox('reset');
                $('#cbx-especializacion').combobox('disable');
            }
        },
        fnSelect: function () {
            var itemData = $('#cbx-plan-integracion').combobox('getData');
            $('#lb-id-plan-de-estudio').text(itemData.Id);
            $('#lb-anyo').text(itemData.Anyo);
            $('#lb-plan-oficial').text(itemData.EsOficial ? "Si" : "No");
            $('#lb-estudio').text(itemData.Estudio);
            $('#lb-tipo-de-estudio-plan').text(itemData.TipoEstudio);
            $('#lb-rama-de-conocimiento-plan').text(itemData.Rama);
            $('#lb-titulo').text(itemData.Titulo);

            $('#cbx-especializacion').combobox('reset');
            $('#cbx-especializacion').combobox('enable');

        },
        fnTextChange: function () {
            limpiarComponentesPlanEstudio();
            $('#cbx-especializacion').combobox('reset');
            $('#cbx-especializacion').combobox('disable');
        }
    }));
    $('#cbx-especializacion').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-especializacion',
        fnParams: function () {
            return {
                FilterPlanEstudio: $('#cbx-plan-integracion').combobox('getId')
            };
        },
        toolbar: {
            reset: true
        }
    }));
    $('#cbx-especializacion').combobox('disable');
    $('#btn-guardar').click(function () {
        var params = {};
        var errors = [];
        params.IdEstudioGestor = $('#cbx-estudio').combobox('getId');
        params.IdRefPlanErp = $('#cbx-plan-integracion').combobox('getId');
        params.IdRefEspecializacion = $('#cbx-especializacion').combobox('getId');
        validateEstudioIntegracion(params, errors);
        if (errors.length === 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-estudio',
                type: 'PUT',
                data: $.toJSON(params),
                success: function (data, status, xhr) {
                    showMessage($.Globalization.localize('MessageOperacionExitosamente'), true);
                    gotoController('EstudioIntegracion/Index');
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
        var errors = [];
        params.IdEstudioGestor = $('#cbx-estudio').combobox('getId');
        params.IdRefPlanErp = $('#cbx-plan-integracion').combobox('getId');
        params.IdRefEspecializacion = $('#cbx-especializacion').combobox('getId');
        validateEstudioIntegracion(params, errors);
        if (errors.length === 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-estudio',
                type: 'PUT',
                data: $.toJSON(params),
                success: function (data, status, xhr) {
                    showMessage($.Globalization.localize('MessageOperacionExitosamente'), true);
                    $('#cbx-estudio').combobox('reset');
                    limpiarComponentesEstudioUnir();
                    $('#cbx-plan-integracion').combobox('reset');
                    $('#cbx-especializacion').combobox('reset').combobox('disable');
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
        gotoController('EstudioIntegracion/Index');
    });

    limpiarComponentesEstudioUnir();
    limpiarComponentesPlanEstudio();
});

function validateEstudioIntegracion(params, errors) {
    if (isNull(params.IdEstudioGestor)) {
        errors.push($.Globalization.localize('ErrorNoEstudioGestor'));
    }
    if (isNull(params.IdRefPlanErp)) {
        errors.push($.Globalization.localize('ErrorNoPlanEstudioErp'));
    }
}

function limpiarComponentesEstudioUnir() {
    $("#lb-id-estudio").text("-");
    $("#lb-plan").text("-");
    $("#lb-activo").text("-");
    $("#lb-tipo-estudio").text("-");
    $("#lb-rama").text("-");
    $("#lb-titulo-ruct").text("-");
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