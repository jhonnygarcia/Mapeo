$(document).ready(function () {
    $('#cbx-estudio').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-estudio',
        toolbar: {
            reset: function () { }
        },
        fnSelect: function () {
            $('#cbx-asignatura').combobox('reset');
            limpiarComponentesAsignaturaUnir();
        },
        fnTextChange: function () { }
    }));
    $('#cbx-asignatura').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-asignatura',
        toolbar: {
            reset: function () { }
        },
        fnSelect: function () {
            var itemData = $('#cbx-asignatura').combobox('getData');
            $('#lb-id-asignatura').text(itemData.Id);
            $('#lb-estudio').text(itemData.Estudio);
            $('#lb-tipo-asignatura').text(itemData.TipoAsignatura);
            $('#lb-creditos').text(parseFloat(itemData.Creditos).toFixed(2));
            $('#lb-periodo-lectivo').text(itemData.PeriodoLectivo);
            $('#lb-curso').text(itemData.Curso);
            $('#lb-activa').text(itemData.Activo);
        },
        fnTextChange: function () {
            limpiarComponentesAsignaturaUnir();
        },
        fnParams: function () {
            return {
                IdEstudio: $('#cbx-estudio').combobox('getId')
            };
        }
    }));
    $('#cbx-plan-estudio-erp').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-plan',
        toolbar: {
            reset: function() {}
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
            $('#lb-plan-estudio-erp').text(itemData.PlanEstudio);
            $('#lb-tipo-asignatura-erp').text(itemData.TipoAsignatura);
            $('#lb-creditos-erp').text(parseFloat(itemData.Creditos).toFixed(2));
            $('#lb-duracion-periodo-lectivo-erp').text(itemData.DuracionPeriodoLectivo);
            $('#lb-ubicacion-periodo-lectivo-erp').text(!isNull(itemData.UbicacionPeriodoLectivo) ? itemData.UbicacionPeriodoLectivo : '-');
            $('#lb-curso-erp').text(!isNull(itemData.Curso) ? itemData.Curso : '-');
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
        params.IdAsignatura = $('#cbx-asignatura').combobox('getId');

        if (isNull(params.IdAsignatura)) {
            errors.push($.Globalization.localize('ErrorNoAsignatura'));
        }
        if (isNull(params.IdAsignaturaPlan)) {
            errors.push($.Globalization.localize('ErrorNoAsignaturaPlan'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-asignatura',
                type: 'PUT',
                data: $.toJSON(params),
                success: function (data, status, xhr) {
                    showMessage($.Globalization.localize('MessageOperacionExitosamente'), true);
                    gotoController('AsignaturaIntegracion/Index');
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
        params.IdAsignatura = $('#cbx-asignatura').combobox('getId');

        if (isNull(params.IdAsignatura)) {
            errors.push($.Globalization.localize('ErrorNoAsignatura'));
        }
        if (isNull(params.IdAsignaturaPlan)) {
            errors.push($.Globalization.localize('ErrorNoAsignaturaPlan'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-asignatura',
                type: 'PUT',
                data: $.toJSON(params),
                success: function (data, status, xhr) {
                    showMessage($.Globalization.localize('MessageOperacionExitosamente'), true);
                    $('#cbx-asignatura').combobox('reset');
                    limpiarComponentesAsignaturaUnir();
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
        gotoController('AsignaturaIntegracion/Index');
    });

    limpiarComponentesAsignaturaUnir();
    limpiarComponentesAsignaturaPlan();
});

function limpiarComponentesAsignaturaUnir() {
    $('#lb-id-asignatura').text('-');
    $('#lb-estudio').text('-');
    $('#lb-tipo-asignatura').text('-');
    $('#lb-creditos').text('-');
    $('#lb-periodo-lectivo').text('-');
    $('#lb-curso').text('-');
    $('#lb-activa').text('-');
}

function limpiarComponentesAsignaturaPlan() {
    $('#lb-id-asignatura-plan-erp').text('-');
    $('#lb-plan-estudio-erp').text('-');
    $('#lb-tipo-asignatura-erp').text('-');
    $('#lb-creditos-erp').text('-');
    $('#lb-duracion-periodo-lectivo-erp').text('-');
    $('#lb-ubicacion-periodo-lectivo-erp').text('-');
    $('#lb-curso-erp').text('-');
}