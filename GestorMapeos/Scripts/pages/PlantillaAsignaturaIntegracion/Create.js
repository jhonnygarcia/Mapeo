$(document).ready(function () {
    $('#cbx-plantilla-estudio').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-plantilla-estudio',
        toolbar: {
            reset: function () { }
        },
        fnSelect: function () {
            $('#cbx-plantilla-asignatura').combobox('reset');
            limpiarComponentesPlantillaAsignatura();
        },
        fnTextChange: function () { }
    }));
    $('#cbx-plantilla-asignatura').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-plantiila-asignatura',
        toolbar: {
            reset: function () {
                limpiarComponentesPlantillaAsignatura();
            }
        },
        fnSelect: function () {
            var itemData = $('#cbx-plantilla-asignatura').combobox('getData');
            $('#lb-codigo-plantilla-asignatura').text($.Globalization.localize('EtiquetaCodigo') + itemData.Codigo);
            $('#lb-id-plantilla-asignatura').text(itemData.Id);
            $('#lb-tipo-asignatura').text(itemData.TipoAsignatura);
            $('#lb-creditos').text(parseFloat(itemData.Creditos).toFixed(2));
            $('#lb-plantilla-estudio').text(itemData.PlantillaEstudio);
        },
        fnTextChange: function () {
            limpiarComponentesPlantillaAsignatura();
        },
        fnParams: function () {
            return {
                IdPlantillaEstudio: $('#cbx-plantilla-estudio').combobox('getId')
            };
        },
        fnPreloadLabel: function (itemData) {
            return itemData.Codigo + " - " + itemData.Descripcion;
        },
        fnPreloadValue: function (itemData) {
            return itemData.Descripcion;
        }
    }));
    $('#cbx-plan-estudio-erp').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-plan',
        toolbar: {
            reset: function () { }
        },
        fnSelect: function () {
            $('#cbx-asignatura-plan-erp').combobox('reset');
            limpiarComponentesAsignaturaPlan();
        },
        fnTextChange: function () { }
    }));
    $('#cbx-asignatura-plan-erp').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-asignatura-plan',
        toolbar: {
            reset: function () {
                limpiarComponentesAsignaturaPlan();
            }
        },
        fnSelect: function () {
            var itemData = $('#cbx-asignatura-plan-erp').combobox('getData');
            $('#lb-id-asignatura-plan-erp').text(itemData.Id);
            $('#lb-tipo-asignatura-erp').text(itemData.TipoAsignatura);
            $('#lb-creditos-erp').text(parseFloat(itemData.Creditos).toFixed(2));
            $('#lb-plan-estudio-erp').text(itemData.PlanEstudio);
        },
        fnTextChange: function () {
            limpiarComponentesAsignaturaPlan();
        },
        fnParams: function () {
            return {
                IdPlanEstudio: $('#cbx-plan-estudio-erp').combobox('getId')
            };
        }
    }));
    $('#btn-guardar').click(function () {
        var params = {};
        var errors = new Array();
        params.IdAsignaturaPlan = $('#cbx-asignatura-plan-erp').combobox('getId');
        params.IdPlantillaAsignatura = $('#cbx-plantilla-asignatura').combobox('getId');
        
        if (isNull(params.IdPlantillaAsignatura)) {
            errors.push($.Globalization.localize('ErrorNoPlantillaAsignatura'));
        }
        if (isNull(params.IdAsignaturaPlan)) {
            errors.push($.Globalization.localize('ErrorNoAsignaturaPlan'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-plantilla-asignatura',
                type : 'PUT',
                data: $.toJSON(params),
                success: function (data, status, xhr) {
                    showMessage($.Globalization.localize('MessageOperacionExitosamente'), true);
                    gotoController('PlantillaAsignaturaIntegracion/Index');
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
        params.IdAsignaturaPlan = $('#cbx-asignatura-plan-erp').combobox('getId');
        params.IdPlantillaAsignatura = $('#cbx-plantilla-asignatura').combobox('getId');

        if (isNull(params.IdPlantillaAsignatura)) {
            errors.push($.Globalization.localize('ErrorNoPlantillaAsignatura'));
        }
        if (isNull(params.IdAsignaturaPlan)) {
            errors.push($.Globalization.localize('ErrorNoAsignaturaPlan'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-plantilla-asignatura',
                type: 'PUT',
                data: $.toJSON(params),
                success: function (data, status, xhr) {
                    showMessage($.Globalization.localize('MessageOperacionExitosamente'), true);
                    $('#cbx-plantilla-asignatura').combobox('reset');
                    limpiarComponentesPlantillaAsignatura();
                    $('#cbx-asignatura-plan-erp').combobox('reset');
                    limpiarComponentesAsignaturaPlan();
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
        gotoController('PlantillaAsignaturaIntegracion/Index');
    });

    limpiarComponentesPlantillaAsignatura();
    limpiarComponentesAsignaturaPlan();
});

function limpiarComponentesPlantillaAsignatura() {
    $('#lb-codigo-plantilla-asignatura').text('');
    $('#lb-id-plantilla-asignatura').text('-');
    $('#lb-tipo-asignatura').text('-');
    $('#lb-creditos').text('-');
    $('#lb-plantilla-estudio').text('-');
}

function limpiarComponentesAsignaturaPlan() {
    $('#lb-id-asignatura-plan-erp').text('-');
    $('#lb-tipo-asignatura-erp').text('-');
    $('#lb-creditos-erp').text('-');
    $('#lb-plan-estudio-erp').text('-');
}