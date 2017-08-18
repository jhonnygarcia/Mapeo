$(document).ready(function () {
    $('#cbx-periodo-academico').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-periodo-academico',
        fnPreloadLabel: function (itemData) {
            return itemData.Descripcion + ' <strong>(' + itemData.FechaInicio + ' ' + itemData.FechaFin + ')</strong>';
        },
        fnPreloadValue: function (itemData) {
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
        params.IdPeriodoMatriculacionGestor = $('#hd-id-periodo-matriculacion').val();
        params.IdRefPeriodoAcademicoErp = $('#cbx-periodo-academico').combobox('getId');

        if (isNull(params.IdRefPeriodoAcademicoErp)) {
            errors.push($.Globalization.localize('ErrorNoPeriodoAcademico'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-periodo-matriculacion/' + params.IdPeriodoMatriculacionGestor,
                data: $.toJSON(params),
                success: function (data) {
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
    $('#btn-cancelar').click(function () {
        gotoController('PeriodoMatriculacionIntegracion/Index');
    });
    LoadData();
});

function LoadData() {
    $('#page-asignatura-plan-listar').block();
    var id = $('#hd-id-periodo-matriculacion').val();
    $.ajax({
        url: SiteUrl + 'api/v1/mapeo-periodo-matriculacion/' + id,
        type: 'GET',
        success: function (data, status, xhr) {
            $('#page-asignatura-plan-listar').unblock();
            //Periodo Matriculacion
            $("#lb-periodo-matriculacion").text(data.PeriodoMatriculacion.Nombre);
            $("#lb-id-periodo-matriculacion").text(data.PeriodoMatriculacion.Id);
            $("#lb-anyo-matriculacion").text(data.PeriodoMatriculacion.AnyoAcademico);
            $("#lb-fecha-inicio-matriculacion").text(data.PeriodoMatriculacion.FechaInicio);
            $("#lb-fecha-fin-matriculacion").text(data.PeriodoMatriculacion.FechaFin);
            $("#lb-nro-matriculacion").text(data.PeriodoMatriculacion.Nro);

            //Periodo Academico
            $('#cbx-periodo-academico')
                .combobox("setId", data.PeriodoAcademico.Id)
                .combobox("setValue", data.PeriodoAcademico.Descripcion);
            $("#lb-id-periodo-academico").text(data.PeriodoAcademico.Id);
            $("#lb-anyo-academico").text(data.PeriodoAcademico.AnyoAcademico.DisplayName);
            $("#lb-fecha-inicio-academico").text(data.PeriodoAcademico.FechaInicio);
            $("#lb-fecha-fin-academico").text(data.PeriodoAcademico.FechaFin);
            $("#lb-nro-academico").text(data.PeriodoAcademico.Nro);
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

function limpiarComponentesPeriodoAcademico() {
    $("#lb-id-periodo-academico").text('-');
    $("#lb-anyo-academico").text('-');
    $("#lb-fecha-inicio-academico").text('-');
    $("#lb-fecha-fin-academico").text('-');
    $("#lb-nro-academico").text('-');
}