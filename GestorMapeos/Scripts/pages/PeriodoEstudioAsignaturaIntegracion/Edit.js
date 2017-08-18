$(document).ready(function () {
    $("#btn-buscar-asignatura-ofertada").click(function () {
        PopupCargarAsignaturaOfertada();
    });
    $('#btn-guardar').click(function () {
        var params = {};
        var errors = new Array();
        params.IdPeriodoEstudioAsignatura = $('#hd-id-periodo-estudio-asignatura').val();
        params.IdAsignaturaOfertada = $('#hd-id-asignatura-ofertada').val();

        if (isNull(params.IdPeriodoEstudioAsignatura) || params.IdPeriodoEstudioAsignatura === "") {
            errors.push($.Globalization.localize('ErrorNoSeleccionPeriodoEstudioAsignatura'));
        }
        if (isNull(params.IdAsignaturaOfertada) || params.IdAsignaturaOfertada === "") {
            errors.push($.Globalization.localize('ErrorNoSeleccionAsignaturaOfertada'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-periodo-estudio-asignatura/' + params.IdAsignaturaOfertada,
                data: $.toJSON(params),
                success: function (data, status, xhr) {
                    showMessage($.Globalization.localize('MessageOperacionExitosamente'), true);
                    gotoController('PeriodoEstudioAsignaturaIntegracion/Index');
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
        gotoController('PeriodoEstudioAsignaturaIntegracion/Index');
    });
    LoadData();
});

function LoadData() {
    var id = $('#hd-id-periodo-estudio-asignatura').val();
    $.ajax({
        url: SiteUrl + 'api/v1/mapeo-periodo-estudio-asignatura/' + id,
        type: 'GET',
        success: function (data, status, xhr) {
            $('#page-asignatura-estudio-editar').unblock();
            $("#lb-id-periodo-estudio-asignatura").text(data.Id);
            $("#lb-asignatura").text(data.PeriodoEstudioAsignatura.Asignatura.Nombre);
            $("#lb-tipo-asignatura").text(data.PeriodoEstudioAsignatura.Asignatura.TipoAsignatura);
            $("#lb-creditos").text(parseFloat(data.PeriodoEstudioAsignatura.Asignatura.Creditos).toFixed(2));
            $("#lb-periodo-lectivo").text(data.PeriodoEstudioAsignatura.Asignatura.PeriodoLectivo);
            $("#lb-curso").text(data.PeriodoEstudioAsignatura.Asignatura.Curso);
            $("#lb-activa").text(data.PeriodoEstudioAsignatura.Asignatura.Activo);
            $("#lb-id-periodo-estudio").text(data.PeriodoEstudioAsignatura.PeriodoEstudio.Id);
            $("#lb-estudio").text(data.PeriodoEstudioAsignatura.PeriodoEstudio.Estudio.Nombre);
            $("#lb-periodo-matriculacion").text(data.PeriodoEstudioAsignatura.PeriodoEstudio.PeriodoMatriculacion.Nombre);

            $("#lb-id-asignatura-ofertada-erp").text(data.AsignaturaOfertada.Id);
            $("#lb-asignatura-ofertada-erp").text(data.AsignaturaOfertada.DisplayName);
            $("#lb-asignatura-plan-erp").text(data.AsignaturaOfertada.AsignaturaPlan.Asignatura.DisplayName);
            $("#lb-periodo-lectivo-erp").text(data.AsignaturaOfertada.PeriodoLectivo.DisplayName);
            $("#lb-tipo-asignatura-erp").text(data.AsignaturaOfertada.TipoAsignatura.Nombre);
            $("#lb-creditos-erp").text(parseFloat(data.AsignaturaOfertada.AsignaturaPlan.Asignatura.Creditos).toFixed(2));
            $("#lb-duracion-periodo-lectivo-erp").text(data.AsignaturaOfertada.PeriodoLectivo.DuracionPeriodoLectivo.Nombre);
            $("#lb-ubicacion-periodo-lectivo-erp").text(data.AsignaturaOfertada.UbicacionPeriodoLectivo);
            $("#lb-curso-erp").text(isNull(data.AsignaturaOfertada.Curso) ? '-': data.AsignaturaOfertada.Curso.Numero);
            $("#lb-id-plan-ofertado-erp").text(data.AsignaturaOfertada.PlanOfertado.Id);
            $("#lb-plan-estudio-erp").text(data.AsignaturaOfertada.PlanOfertado.Plan.DisplayName);
            $("#lb-periodo-academico-erp").text(data.AsignaturaOfertada.PlanOfertado.PeriodoAcademico.DisplayName);

            $('#hd-id-periodo-estudio-asignatura').val(data.PeriodoEstudioAsignatura.Id);
            $('#hd-id-asignatura-ofertada').val(data.AsignaturaOfertada.Id);
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

/**
 * Popup AsignaturaOfertadaErp
 */
function PopupCargarAsignaturaOfertada() {
    $.blockUI();
    var popup = null;
    var buttons = {};
    /***************************************************************************/
    buttons[$.Globalization.localize('Select')] = function () {
        var params = {};
        var errors = new Array();
        var selecciones = new Array();
        $.each($('#tb-asignatura-ofertada tbody input:checkbox'), function (index, value) {
            if ($(value).is(':checked')) {
                var data = $(value).closest('tr').data('data');
                var asignaturaOfertada = {};
                asignaturaOfertada.Id = data.Id;
                asignaturaOfertada.AsignaturaOfertada = data.DisplayName;
                asignaturaOfertada.AsignaturaPlan = data.AsignaturaPlan.Asignatura.DisplayName;
                asignaturaOfertada.PeriodoLectivo = data.PeriodoLectivo.DisplayName;
                asignaturaOfertada.TipoAsignatura = data.TipoAsignatura.Nombre;
                asignaturaOfertada.Creditos = data.AsignaturaPlan.Asignatura.Creditos;
                asignaturaOfertada.DuracionPeriodoLectivo = data.PeriodoLectivo.DuracionPeriodoLectivo.Nombre;
                asignaturaOfertada.UbicacionPeriodoLectivo = data.UbicacionPeriodoLectivo;
                asignaturaOfertada.Curso = !isNull(data.Curso) ? data.Curso.Numero : '-';
                asignaturaOfertada.IdPlanOfertado = data.PlanOfertado.Id;
                asignaturaOfertada.PlanEstudio = data.PlanOfertado.Plan.DisplayName;
                asignaturaOfertada.PeriodoAcademico = data.PlanOfertado.PeriodoAcademico.DisplayName;
                selecciones.push(asignaturaOfertada);
            }
        });

        if (selecciones.length > 0) {
            var ultimaSeleccion = selecciones[selecciones.length - 1];
            params.Id = ultimaSeleccion.Id;
            params.AsignaturaOfertada = ultimaSeleccion.AsignaturaOfertada;
            params.AsignaturaPlan = ultimaSeleccion.AsignaturaPlan;
            params.PeriodoLectivo = ultimaSeleccion.PeriodoLectivo;
            params.TipoAsignatura = ultimaSeleccion.TipoAsignatura;
            params.Creditos = ultimaSeleccion.Creditos;
            params.DuracionPeriodoLectivo = ultimaSeleccion.DuracionPeriodoLectivo;
            params.UbicacionPeriodoLectivo = ultimaSeleccion.UbicacionPeriodoLectivo;
            params.Curso = ultimaSeleccion.Curso;
            params.IdPlanOfertado = ultimaSeleccion.IdPlanOfertado;
            params.PlanEstudio = ultimaSeleccion.PlanEstudio;
            params.PeriodoAcademico = ultimaSeleccion.PeriodoAcademico;
        }

        if (isNull(params.Id) || isNull(params.AsignaturaOfertada) || isNull(params.AsignaturaPlan)
            || isNull(params.PeriodoLectivo) || isNull(params.TipoAsignatura) || isNull(params.Creditos)
            || isNull(params.DuracionPeriodoLectivo) || isNull(params.UbicacionPeriodoLectivo) || isNull(params.Curso)
            || isNull(params.IdPlanOfertado) || isNull(params.PlanEstudio) || isNull(params.PeriodoAcademico)) {
            errors.push($.Globalization.localize('ErrorNoSeleccionAsignaturaOfertada'));
        }
        if (errors.length == 0) {
            $('#hd-id-asignatura-ofertada').val(params.Id);
            $("#lb-id-asignatura-ofertada-erp").text(params.Id);
            $("#lb-asignatura-ofertada-erp").text(params.AsignaturaOfertada);
            $("#lb-asignatura-plan-erp").text(params.AsignaturaPlan);
            $("#lb-periodo-lectivo-erp").text(params.PeriodoLectivo);
            $("#lb-tipo-asignatura-erp").text(params.TipoAsignatura);
            $("#lb-creditos-erp").text(parseFloat(params.Creditos).toFixed(2));
            $("#lb-duracion-periodo-lectivo-erp").text(params.DuracionPeriodoLectivo);
            $("#lb-ubicacion-periodo-lectivo-erp").text(params.UbicacionPeriodoLectivo);
            $("#lb-curso-erp").text(params.Curso);
            $("#lb-id-plan-ofertado-erp").text(params.IdPlanOfertado);
            $("#lb-plan-estudio-erp").text(params.PlanEstudio);
            $("#lb-periodo-academico-erp").text(params.PeriodoAcademico);
            popup.dialog('close');
        } else {
            showErrors(errors);
        }
    };
    buttons[$.Globalization.localize('Cancel')] = function () {
        popup.dialog('close');
    };
    /***************************************************************************/
    showPopupPage({
        title: $.Globalization.localize('TituloPopupAsignaturaOfertada'),
        url: SiteUrl + 'PeriodoEstudioAsignaturaIntegracion/PopupAsignaturaOfertada',
        open: function () {
            popup = $(this);
            $.unblockUI();
        },
        buttons: buttons,
        width: 700,
        height: 800
    }, false, function () {
        inicializarPopupAsignaturaOfertada(popup);
        loadAsignaturasOfertadas($("#tb-asignatura-ofertada"));
    });
}

function inicializarPopupAsignaturaOfertada(popup) {
    $("#txt-id-asignatura-ofertada", popup).autoNumeric(AutoNumericInteger);
    $("#cbx-periodo-academico", popup).combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-periodo-academico',
        toolbar: {
            reset: true
        },
        fnSelect: function () {
            $('#cbx-plan-estudio').combobox('reset');
            $('#cbx-asignatura-ofertada').combobox('reset');
        },
        fnPreloadLabel: function (itemData) {
            return itemData.Descripcion + ' <strong>(' + itemData.FechaInicio + ' - ' + itemData.FechaFin + ')</strong>';
        },
        fnPreloadValue: function (itemData) {
            return itemData.Descripcion;
        }
    }));
    $("#cbx-plan-estudio", popup).combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-plan',
        toolbar: {
            reset: true
        },
        fnParams: function () {
            return {
                IdPeriodoAcademico: $('#cbx-periodo-academico').combobox('getId')
            };
        },
        fnSelect: function () {
            $('#cbx-asignatura-ofertada').combobox('reset');
        }
    }));
    $("#cbx-asignatura-ofertada", popup).combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-asignatura-ofertada',
        toolbar: {
            reset: true
        },
        fnParams: function () {
            return {
                IdPeriodoAcademico: $('#cbx-periodo-academico').combobox('getId'),
                IdPlanEstudio: $('#cbx-plan-estudio').combobox('getId')
            }
        }
    }));

    $('#btn-buscar', popup).button();
    $('#btn-buscar', popup).click(function () {
        $("#tb-asignatura-ofertada", popup).table('update');
    });
    $('#btn-limpiar', popup).button();
    $('#btn-limpiar', popup).click(function () {
        $('#cbx-periodo-academico', popup).combobox('reset');
        $('#cbx-plan-estudio', popup).combobox('reset');
        $('#cbx-asignatura-ofertada', popup).combobox('reset');
        $('#txt-id-asignatura-ofertada', popup).val('');
        $("#tb-asignatura-ofertada", popup).table('update');
    });
}

function loadAsignaturasOfertadas(table) {
    table.table({
        bInfo: true,
        responsive: {
            details: {
                type: 'inline'
            }
        },
        aaSorting: [[1, 'asc']],
        aoColumns: [
        {
            className: '',
            sTitle: '<input type="checkbox" id="chk-todos">',
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPopupIdAsignaturaOfertada'),
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPopupAsignaturaOfertada'),
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPopupPeriodoLectivo'),
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPopupPlanEstudio'),
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPopupPeriodoAcademico'),
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPopupTipoAsignatura'),
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPopupCreditos'),
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPopupDuracionPeriodoLectivo'),
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPopupUbicacionPeriodoLectivo'),
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPopupCurso'),
            bSortable: true
        }],
        bServerSide: true,
        sAjaxSource: SiteUrl + 'api/v1/erp-commons/search-asignatura-ofertada',
        fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            return nRow;
        },
        fnServerData: function (sSource, aoData, fnCallback) {
            var paramsTabla = {};
            $.each(aoData, function (index, value) {
                paramsTabla[value.name] = value.value;
            });
            var params = {};
            params.PageIndex = (paramsTabla.iDisplayStart / paramsTabla.iDisplayLength) + 1;
            params.ItemsPerPage = paramsTabla.iDisplayLength;
            params.OrderColumnPosition = paramsTabla.iSortCol_0;
            params.OrderColumnName = decode(paramsTabla.iSortCol_0,
            [
                1, 'id-asignatura-ofertada',
                2, 'asignatura-ofertada',
                3, 'periodo-lectivo',
                4, 'plan-estudio',
                5, 'periodo-academico',
                6, 'tipo-asignatura',
                7, 'creditos',
                8, 'duracion-periodo-lectivo',
                9, 'ubicacion-periodo-lectivo',
                10, 'curso', 'id-asignatura-ofertada'
            ]);
            params.OrderDirection = paramsTabla.sSortDir_0;
            /******************************************************************/

            params.IdAsignaturaOfertada = $('#txt-id-asignatura-ofertada').val().isEmpty() ? null : parseInt($('#txt-id-asignatura-ofertada').val());
            params.FilterIdAsignaturaOfertada = $('#cbx-asignatura-ofertada').combobox('getId');
            params.IdPeriodoAcademico = $('#cbx-periodo-academico').combobox('getId');
            params.IdPlanEstudio = $('#cbx-plan-estudio').combobox('getId');

            /******************************************************************/
            table.table('block');
            $.ajax({
                url: sSource,
                data: $.toJSON(params),
                success: function (data, status, xhr) {
                    table.table('unblock');
                    var rows = [];
                    $.each(data, function (index, value) {
                        var row = [];
                        row.push('<input type="checkbox" />');
                        row.push(value.Id);
                        row.push(value.DisplayName);
                        row.push(value.PeriodoLectivo.DisplayName);
                        row.push(value.PlanOfertado.Plan.DisplayName);
                        row.push(value.PlanOfertado.PeriodoAcademico.DisplayName);
                        row.push(value.TipoAsignatura.Nombre);
                        row.push(parseFloat(value.AsignaturaPlan.Asignatura.Creditos).toFixed(2));
                        row.push(value.PeriodoLectivo.DuracionPeriodoLectivo.Nombre);
                        row.push(value.UbicacionPeriodoLectivo);
                        row.push(isNull(value.Curso) ? '-' : value.Curso.Numero);
                        rows.push(row);
                    });
                    fnCallback({
                        "sEcho": paramsTabla.sEcho,
                        "aaData": rows,
                        "iTotalRecords": xhr.getResponseHeader("X-TotalPages"),
                        "iTotalDisplayRecords": xhr.getResponseHeader("X-TotalElements")
                    });
                    table.table('setData', data);
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
    });
    $('#chk-todos').click(function () {
        var checked = $(this).prop('checked');
        if (checked) {
            $('tbody input:checkbox', table).prop('checked', true);
        } else {
            $('tbody input:checkbox', table).prop('checked', false);
        }
    });
}