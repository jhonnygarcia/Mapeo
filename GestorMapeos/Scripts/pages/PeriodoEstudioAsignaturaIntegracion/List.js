$(document).ready(function () {
    var tabla = $('#tb-asignatura-ofertada-integracion');
    $('input:text', '.content > div > div.box-fieldset:first').autoNumeric(AutoNumericInteger);
    //Filtros Gestor UNIR
    $('#txt-id-periodo-matriculacion').autoNumeric();
    $("#cbx-periodo-matriculacion").combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-periodo-matriculacion',
        toolbar: {
            reset: true
        },
        fnSelect: function () {
            $('#cbx-estudio').combobox('reset');
            $('#cbx-asignatura').combobox('reset');
        }
    }));
    $("#cbx-estudio").combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-estudio',
        fnParams: function () {
            return {
                IdPeriodoMatriculacion: $("#cbx-periodo-matriculacion").combobox('getId')
            };
        },
        toolbar: {
            reset: true
        },
        fnSelect: function () {
            $('#cbx-asignatura').combobox('reset');
        }
    }));
    $("#cbx-asignatura").combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-asignatura',
        fnParams: function () {
            return {
                IdPeriodoMatriculacion: $("#cbx-periodo-matriculacion").combobox('getId'),
                IdEstudio: $("#cbx-estudio").combobox('getId')
            };
        },
        toolbar: {
            reset: true
        }
    }));
    //Filtros ERP Academico
    $('#txt-id-asignatura-ofertada').autoNumeric();
    $('#cbx-periodo-academico').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-periodo-academico',
        toolbar: {
            reset: true
        },
        fnSelect: function () {
            $('#cbx-plan-estudio').combobox('reset');
            $('#cbx-asignatura-ofertada').combobox('reset');
        },
        fnPreloadLabel: function (itemData) {
            return itemData.Descripcion + ' <strong>(' + itemData.FechaInicio + ' ' + itemData.FechaFin + ')</strong>';
        },
        fnPreloadValue: function (itemData) {
            return itemData.Descripcion;
        }
    }));
    $("#cbx-plan-estudio").combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-plan',
        fnParams: function () {
            return {
                IdPeriodoAcademico: $('#cbx-periodo-academico').combobox('getId')
            };
        },
        toolbar: {
            reset: true
        },
        fnSelect: function () {
            $('#cbx-asignatura-ofertada').combobox('reset');
        }
    }));
    $("#cbx-asignatura-ofertada").combobox(CustomDefaultCombobox({
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

    $('#btn-buscar').click(function () {
        tabla.table('update');
    });
    $('#btn-limpiar').click(function () {
        $('#txt-id-periodo-matriculacion').val('');
        $("#cbx-periodo-matriculacion").combobox('reset');
        $("#cbx-estudio").combobox('reset');
        $("#cbx-asignatura").combobox('reset');
        $('#txt-id-asignatura-ofertada').val('');
        $('#cbx-periodo-academico').combobox('reset');
        $("#cbx-plan-estudio").combobox('reset');
        $("#cbx-asignatura-ofertada").combobox('reset');
        tabla.table('update');
        $('#chk-todos').prop('checked', false);
    });
    $('#btn-crear').click(function () {
        gotoController('PeriodoEstudioAsignaturaIntegracion/Create');
    });
    $('#btn-eliminar').click(function () {
        var ids = [];
        $.each($('tbody input:checkbox', 'table'), function (index, value) {
            if ($(value).is(':checked')) {
                ids.push($(value).closest('tr').data('data').Id);
            }
        });
        fnEliminar(ids);
    });
    tabla.table({
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
            sWidth: "15px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnIdPerEstAsig'),
            sWidth: "50px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnAsignatura'),
            sWidth: "50px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnEstudio'),
            sWidth: "70px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnPeriodoMatriculacion'),
            sWidth: '70px',
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnIdAsigOf'),
            sWidth: "50px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnAsignaturaOfertada'),
            sWidth: "50px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnPeriodoLectivo'),
            sWidth: "50px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnPlanEstudio'),
            sWidth: "50px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnPeriodoAcademico'),
            sWidth: '70px',
            bSortable: false
        },
        {
            sTitle: $.Globalization.localize('TextAcciones'),
            sWidth: "50px",
            bSortable: false
        }],
        bServerSide: true,
        sAjaxSource: SiteUrl + 'api/v1/mapeo-periodo-estudio-asignatura/advanced-search',
        fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('.btn-opciones', nRow).contextMenu({
                fnLoadServerData: function (callbackRender) {
                    var menu = [
                        {
                            value: $.Globalization.localize('TextEdit'),
                            url: SiteUrl + 'PeriodoEstudioAsignaturaIntegracion/Edit/' + $(nRow).data('data').Id
                        },
                        {
                            value: $.Globalization.localize('TextDelete'),
                            fnClick: function () {
                                fnEliminar([$(nRow).data('data').Id]);
                                return false;
                            }
                        }
                    ];
                    callbackRender(menu);
                }
            });
            $(nRow).colorize({
                Columns:
                    [
                        { Positions: [0, 1, 2, 3, 4, 10], ClassName: 'color-gestor' },
                        { Positions: [5, 6, 7, 8, 9], ClassName: 'color-erp' }
                    ]
            });
            $.each($('td', nRow), function (index, td) {
                var celda = $(td);
                var countColumns = $('td', nRow).length;
                var th = $('thead tr:first th:eq(' + index + ')', tabla);
                if (index != 0 && index != countColumns - 1) {
                    var texto = celda.text();
                    celda.html('');
                    $('<div style="word-wrap: break-word;max-width:' + th.css('width') + ';" />').appendTo(celda).text(texto);
                }
            });
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
                1, 'id-periodo-matriculacion',
                2, 'periodo-matriculacion',
                3, 'fecha-inicio',
                4, 'fecha-fin', 'id-periodo-matriculacion'
            ]);
            params.OrderDirection = paramsTabla.sSortDir_0;
            /******************************************************************/

            params.IdPeriodoEstudioAsignatura = $('#txt-id-periodo-matriculacion').val().isEmpty() ? null : parseInt($('#txt-id-periodo-matriculacion').val());
            params.IdPeriodoMatriculacion = $("#cbx-periodo-matriculacion").combobox('getId');
            params.IdEstudio = $("#cbx-estudio").combobox('getId');
            params.IdAsignatura = $("#cbx-asignatura").combobox('getId');

            params.IdAsignaturaOfertada = $('#txt-id-asignatura-ofertada').val().isEmpty() ? null : parseInt($('#txt-id-asignatura-ofertada').val());
            params.IdPeriodoAcademico = $('#cbx-periodo-academico').combobox('getId');
            params.IdPlanEstudio = $("#cbx-plan-estudio").combobox('getId');
            params.FilterIdAsignaturaOfertada = $("#cbx-asignatura-ofertada").combobox('getId');

            /******************************************************************/
            tabla.table('block');
            $('#chk-todos').prop('checked', false);
            $.ajax({
                url: sSource,
                data: $.toJSON(params),
                success: function (data) {
                    tabla.table('unblock');
                    var rows = [];
                    $.each(data.Content.Elements, function (index, value) {
                        var row = [];
                        row.push('<input type="checkbox" />');
                        row.push(value.PeriodoEstudioAsignatura.Id);
                        row.push(value.PeriodoEstudioAsignatura.Asignatura.Nombre);
                        row.push(value.PeriodoEstudioAsignatura.Asignatura.Estudio.Nombre);
                        row.push(value.PeriodoEstudioAsignatura.PeriodoEstudio.PeriodoMatriculacion.Nombre);
                        row.push(value.AsignaturaOfertadaId);
                        if (!isNull(value.AsignaturaOfertada)) {
                            row.push(value.AsignaturaOfertada.Codigo + ' - ' + value.AsignaturaOfertada.Nombre);
                            row.push(value.AsignaturaOfertada.PeriodoLectivo.Nombre +
                                ' (' +
                                value.AsignaturaOfertada.PeriodoLectivo.FechaInicio +
                                ' - ' +
                                value.AsignaturaOfertada.PeriodoLectivo.FechaFin +
                                ')');
                            row.push(value.AsignaturaOfertada.PlanOfertado.Plan.Codigo +
                                ' - ' +
                                value.AsignaturaOfertada.PlanOfertado.Plan.Nombre);
                            row.push(value.AsignaturaOfertada.PlanOfertado.PeriodoAcademico.Nombre +
                                ' (' +
                                value.AsignaturaOfertada.PlanOfertado.PeriodoAcademico.FechaInicio +
                                ' - ' +
                                value.AsignaturaOfertada.PlanOfertado.PeriodoAcademico.FechaFin +
                                ')');
                        } else {
                            row.push('');
                            row.push('');
                            row.push('');
                            row.push('');
                        }
                        row.push('<span class="btn-opciones">Opciones</span>');
                        rows.push(row);
                    });
                    fnCallback({
                        "sEcho": paramsTabla.sEcho,
                        "aaData": rows,
                        "iTotalRecords": data.Content.TotalPages,
                        "iTotalDisplayRecords": data.Content.TotalElements
                    });
                    tabla.table('setData', data.Content.Elements);
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
            $('tbody input:checkbox', tabla).prop('checked', true);
        } else {
            $('tbody input:checkbox', tabla).prop('checked', false);
        }
    });
});

function fnEliminar(ids) {
    if (ids.length == 0) {
        showErrors([$.Globalization.localize('MessageSeleccioneUnaFila')]);
    } else {
        var popup = null;
        showConfirmation({
            open: function () {
                popup = $(this);
            },
            message: $.Globalization.localize('MessageEliminarElementos'),
            buttonFunctionYes: function () {
                popup.block();
                $.ajax({
                    url: SiteUrl + 'api/v1/mapeo-periodo-estudio-asignatura/delete',
                    data: $.toJSON({ Ids: ids }),
                    success: function (data, status, xhr) {
                        showMessage($.Globalization.localize('MessageOperacionExitosamente'), true);
                    },
                    error: function (xhr) {
                        if (xhr.status !== 500) {
                            var result = $.parseJSON(xhr.responseText);
                            showErrors(result.Errors);
                        } else {
                            showApplicationFatalErrorMessage();
                        }
                    },
                    complete: function () {
                        popup.dialog('close');
                        $('#tb-asignatura-ofertada-integracion').table('update');
                    }
                });
            }
        });
    }
}