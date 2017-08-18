$(document).ready(function () {
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
        params.IdAsignatura = $('#hd-id-asignatura').val();

        if (isNull(params.IdAsignaturaPlan)) {
            errors.push($.Globalization.localize('ErrorNoAsignaturaPlan'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-asignatura/' + params.IdAsignatura,
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
    $('#btn-cancelar').click(function () {
        gotoController('AsignaturaIntegracion/Index');
    });
    LoadData();
});

function LoadData() {
    var id = $('#hd-id-asignatura').val();
    $('#page-asignatura-estudio-editar').block();
    $.ajax({
        url: SiteUrl + 'api/v1/mapeo-asignatura/' + id,
        type: 'GET',
        success: function (data, status, xhr) {
            $('#page-asignatura-estudio-editar').unblock();
            $('#lb-id-asignatura').text(data.AsignaturaUnir.Id);
            $('#lb-asignatura').text(data.AsignaturaUnir.Nombre);
            $('#lb-tipo-asignatura').text(data.AsignaturaUnir.TipoAsignatura);
            $('#lb-creditos').text(parseFloat(data.AsignaturaUnir.Creditos).toFixed(2));
            $('#lb-periodo-lectivo').text(data.AsignaturaUnir.PeriodoLectivo);
            $('#lb-curso').text(data.AsignaturaUnir.Curso);
            $('#lb-activa').text(data.AsignaturaUnir.Activo);
            $('#lb-estudio').text(data.AsignaturaUnir.EstudioUnir.Nombre);

            $('#cbx-plan-estudio-erp')
                .combobox('setValue', data.AsignaturaPlan.Plan.DisplayName)
                .combobox('setId', data.AsignaturaPlan.Plan.Id);
            $('#cbx-asignatura-plan-erp')
                .combobox('setValue', data.AsignaturaPlan.Asignatura.DisplayName)
                .combobox('setId', data.AsignaturaPlan.Id);
            $('#lb-id-asignatura-plan-erp').text(data.AsignaturaPlan.Id);
            $('#lb-plan-estudio-erp').text(data.AsignaturaPlan.Plan.DisplayName);
            $('#lb-tipo-asignatura-erp').text(data.AsignaturaPlan.Asignatura.TipoAsignatura.Nombre);
            $('#lb-creditos-erp').text(parseFloat(data.AsignaturaPlan.Asignatura.Creditos).toFixed(2));
            $('#lb-duracion-periodo-lectivo-erp').text(data.AsignaturaPlan.DuracionPeriodoLectivo.Nombre);
            $('#lb-ubicacion-periodo-lectivo-erp').text(!isNull(data.AsignaturaPlan.UbicacionPeriodoLectivo) ? data.AsignaturaPlan.UbicacionPeriodoLectivo : '-');
            $('#lb-curso-erp').text(!isNull(data.AsignaturaPlan.Curso) ? data.AsignaturaPlan.Curso.Numero : '-');
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

function limpiarComponentesAsignaturaPlan() {
    $('#lb-id-asignatura-plan-erp').text('-');
    $('#lb-plan-estudio-erp').text('-');
    $('#lb-tipo-asignatura-erp').text('-');
    $('#lb-creditos-erp').text('-');
    $('#lb-duracion-periodo-lectivo-erp').text('-');
    $('#lb-ubicacion-periodo-lectivo-erp').text('-');
    $('#lb-curso-erp').text('-');
}