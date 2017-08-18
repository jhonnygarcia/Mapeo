$(document).ready(function () {
    $("#cbx-periodo-matriculacion").combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-periodo-matriculacion',
        toolbar: {
            reset: function () {
                limpiarComponentesPeriodoMatriculacionUnir();
            }
        },
        fnSelect: function () {
            var itemData = $('#cbx-periodo-matriculacion').combobox('getData');
            $("#lb-id-periodo-matriculacion").text(itemData.Id);
            $("#lb-anyo-matriculacion").text(itemData.AnyoAcademico);
            $("#lb-fecha-inicio-matriculacion").text(itemData.FechaInicio);
            $("#lb-fecha-fin-matriculacion").text(itemData.FechaFin);
            $("#lb-nro-matriculacion").text(itemData.Nro);
        },
        fnTextChange: function () {
            limpiarComponentesPeriodoMatriculacionUnir();
        }
    }));
    $('#cbx-periodo-academico').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-periodo-academico',
        fnPreloadLabel: function (itemData) {
            return itemData.Descripcion + ' <strong>(' + itemData.FechaInicio + ' ' + itemData.FechaFin + ')</strong>';
        },
        fnPreloadValue: function(itemData) {
            return itemData.Descripcion;
        },
        toolbar: {
            reset: function () {
                limpiarComponentesPeriodoAcademico();
            }
        },
        fnSelect: function () {
            var itemData = $('#cbx-periodo-academico').combobox('getData');
            $("#lb-id-periodo-academico").text(itemData.Id);
            $("#lb-anyo-academico").text(itemData.AnyoAcademico.DisplayName);
            $("#lb-fecha-inicio-academico").text(itemData.FechaInicio);
            $("#lb-fecha-fin-academico").text(itemData.FechaFin);
            $("#lb-nro-academico").text(itemData.Nro);
        },
        fnTextChange: function () {
            limpiarComponentesPeriodoAcademico();
        }
    }));

    $('#btn-guardar').click(function () {
        var params = {};
        var errors = new Array();
        params.IdPeriodoMatriculacionGestor = $('#cbx-periodo-matriculacion').combobox('getId');
        params.IdRefPeriodoAcademicoErp = $('#cbx-periodo-academico').combobox('getId');

        if (isNull(params.IdPeriodoMatriculacionGestor)) {
            errors.push($.Globalization.localize('ErrorNoPeriodoMatriculacion'));
        }
        if (isNull(params.IdRefPeriodoAcademicoErp)) {
            errors.push($.Globalization.localize('ErrorNoPeriodoAcademico'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-periodo-matriculacion',
                type: 'PUT',
                data: $.toJSON(params),
                success: function (data, status, xhr) {
                    showMessage($.Globalization.localize('MessageOperacionExitosamente'), true);
                    gotoController('PeriodoMatriculacionIntegracion/Index');
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
        params.IdPeriodoMatriculacionGestor = $('#cbx-periodo-matriculacion').combobox('getId');
        params.IdRefPeriodoAcademicoErp = $('#cbx-periodo-academico').combobox('getId');

        if (isNull(params.IdPeriodoMatriculacionGestor)) {
            errors.push($.Globalization.localize('ErrorNoPeriodoMatriculacion'));
        }
        if (isNull(params.IdRefPeriodoAcademicoErp)) {
            errors.push($.Globalization.localize('ErrorNoPeriodoAcademico'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-periodo-matriculacion',
                type: 'PUT',
                data: $.toJSON(params),
                success: function (data) {
                    showMessage($.Globalization.localize('MessageOperacionExitosamente'), true);
                    $("#cbx-periodo-matriculacion").combobox('reset');
                    limpiarComponentesPeriodoMatriculacionUnir();
                    $('#cbx-periodo-academico').combobox('reset');
                    limpiarComponentesPeriodoAcademico();
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
        gotoController('PeriodoMatriculacionIntegracion/Index');
    });

    limpiarComponentesPeriodoMatriculacionUnir();
    limpiarComponentesPeriodoAcademico();
});

function limpiarComponentesPeriodoMatriculacionUnir() {
    $("#lb-id-periodo-matriculacion").text('-');
    $("#lb-anyo-matriculacion").text('-');
    $("#lb-fecha-inicio-matriculacion").text('-');
    $("#lb-fecha-fin-matriculacion").text('-');
    $("#lb-nro-matriculacion").text('-');
}

function limpiarComponentesPeriodoAcademico() {
    $("#lb-id-periodo-academico").text('-');
    $("#lb-anyo-academico").text('-');
    $("#lb-fecha-inicio-academico").text('-');
    $("#lb-fecha-fin-academico").text('-');
    $("#lb-nro-academico").text('-');
}