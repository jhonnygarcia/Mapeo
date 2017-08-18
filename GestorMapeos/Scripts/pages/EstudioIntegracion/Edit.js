$(document).ready(function () {
    $('#cbx-plan-integracion').combobox(CustomDefaultCombobox({
        url: SiteUrl +  'api/v1/erp-commons/search-plan',
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
    $('#btn-guardar').click(function () {
        var params = {};
        var errors = [];
        params.IdEstudioGestor = $('#hd-id-estudio').val();
        params.IdRefPlanErp = $('#cbx-plan-integracion').combobox('getId');
        params.IdRefEspecializacion = $('#cbx-especializacion').combobox('getId');
        validateEstudioIntegracion(params, errors);
        if (errors.length === 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-estudio/' + params.IdEstudioGestor,
                data: $.toJSON(params),
                success: function (data) {
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
    $('#btn-cancelar').click(function () {
        gotoController('EstudioIntegracion/Index');
    });
    LoadData();
});

function LoadData() {
    var id = $('#hd-id-estudio').val();
    $.ajax({
        url: SiteUrl + 'api/v1/mapeo-estudio/' + id,
        type: 'GET',
        data: $.toJSON({ id: id }),
        success: function (data, status, xhr) {
            $("#lb-estudio-gestor").text(data.EstudioUnir.Nombre);
            $("#lb-id-estudio").text(data.EstudioUnir.Id);
            $("#lb-plan").text(data.EstudioUnir.PlanEstudio);
            $("#lb-activo").text(data.EstudioUnir.Activo);
            $("#lb-tipo-estudio").text(data.EstudioUnir.TipoEstudio.Nombre);
            $("#lb-rama").text(data.EstudioUnir.RamaEstudio);
            $("#lb-titulo-ruct").text(data.EstudioUnir.Titulo ? data.EstudioUnir.Titulo.DisplayName : "-");

            $("#cbx-plan-integracion")
                        .combobox('setValue', data.PlantillaEstudioIntegracion.Plan.DisplayName)
                        .combobox('setId', data.PlantillaEstudioIntegracion.Plan.Id);
            $('#cbx-especializacion').combobox('enable');
            if (data.Especializacion) {
                $('#cbx-especializacion')
                    .combobox('setId', data.Especializacion.Id)
                    .combobox('setValue', data.Especializacion.Nombre);
            }

            $('#lb-id-plan-de-estudio').text(data.PlantillaEstudioIntegracion.Plan.Id);
            $('#lb-anyo').text(data.PlantillaEstudioIntegracion.Plan.Anyo);
            $('#lb-plan-oficial').text(data.PlantillaEstudioIntegracion.Plan.EsOficial ? "Si" : "No");
            $('#lb-estudio').text(data.PlantillaEstudioIntegracion.Plan.Estudio.DisplayName);
            $('#lb-tipo-de-estudio-plan').text(data.PlantillaEstudioIntegracion.Plan.Estudio.TipoEstudio.Nombre);
            $('#lb-rama-de-conocimiento-plan').text(data.PlantillaEstudioIntegracion.Plan.Estudio.RamaConocimiento.Nombre);
            $('#lb-titulo').text(data.PlantillaEstudioIntegracion.Plan.Titulo.DisplayName);
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

function validateEstudioIntegracion(params, errors) {
    if (isNull(params.IdRefPlanErp)) {
        errors.push($.Globalization.localize('ErrorNoPlanEstudioErp'));
    }
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