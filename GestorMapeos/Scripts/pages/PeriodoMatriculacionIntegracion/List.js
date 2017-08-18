$(document).ready(function () {
    var tabla = $('#tb-periodo-matriculacion-integracion');
    $('input:text', '.content > div > div.box-fieldset:first').autoNumeric(AutoNumericInteger);
    $('#txt-id-periodo-matriculacion').autoNumeric();
    $("#cbx-periodo-matriculacion").combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-periodo-matriculacion',
        toolbar: {
            reset: true
        }
    }));
    $('#txt-id-periodo-academico').autoNumeric();
    $('#cbx-periodo-academico').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-periodo-academico',
        toolbar: {
            reset: true
        },
        fnPreloadLabel: function (itemData) {
            return itemData.Descripcion + ' <strong>(' + itemData.FechaInicio + ' ' + itemData.FechaFin + ')</strong>';
        },
        fnPreloadValue: function(itemData) {
            return itemData.Descripcion;
        }
    }));
    
    $('#btn-buscar').click(function () {
        tabla.table('update');
    });
    $('#btn-limpiar').click(function () {
        $('#txt-id-periodo-matriculacion').val('');
        $("#cbx-periodo-matriculacion").combobox('reset');
        $('#txt-id-periodo-academico').val('');
        $('#cbx-periodo-academico').combobox('reset');
        tabla.table('update');
        $('#chk-todos').prop('checked', false);
    });
    $('#btn-crear').click(function () {
        gotoController('PeriodoMatriculacionIntegracion/Create');
    });
    $('#btn-eliminar').click(function() {
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
        //responsive: {
        //    details: {
        //        type: 'inline'
        //    }
        //},
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
            sTitle: $.Globalization.localize('ColumnIdPerMatriculacion'),
            sWidth: "30px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnPerMatriculacion'),
            sWidth: "150px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnFechaInicio'),
            sWidth: "70px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnFechaFin'),
            sWidth: "70px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnNro'),
            sWidth: "30px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnIdPerAcademico'),
            sWidth: "70px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnPerAcademico'),
            sWidth: "150px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnFechaInicio'),
            sWidth: "70px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnFechaFin'),
            sWidth: "70px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnNro'),
            sWidth: "30px",
            bSortable: false
        },
        {
            sTitle: 'Acciones',
            sWidth: "50px",
            bSortable: false
        }],
        bServerSide: true,
        sAjaxSource: SiteUrl + 'api/v1/mapeo-periodo-matriculacion/advanced-search',
        fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('.btn-opciones', nRow).contextMenu({
                fnLoadServerData: function (callbackRender) {
                    var menu = [
                        {
                            value: $.Globalization.localize('TextEdit'),
                            url: SiteUrl + 'PeriodoMatriculacionIntegracion/Edit/' + $(nRow).data('data').Id
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
                        { Positions: [1, 2, 3, 4, 5, 11], ClassName: 'color-gestor' },
                        { Positions: [6, 7, 8, 9, 10], ClassName: 'color-erp' }
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
            params.IdPeriodoMatriculacion = $('#txt-id-periodo-matriculacion').val().isEmpty() ? null : parseInt($('#txt-id-periodo-matriculacion').val());
            params.IdPeriodoAcademico = $('#txt-id-periodo-academico').val().isEmpty() ? null : parseInt($('#txt-id-periodo-academico').val());

            params.FilterIdPeriodoMatriculacion = $('#cbx-periodo-matriculacion').combobox('getId');
            params.FilterIdPeriodoAcademico = $('#cbx-periodo-academico').combobox('getId');
            /******************************************************************/
            tabla.table('block');
            $('#chk-todos').prop('checked', false);
            $.ajax({
                url: sSource,
                data: $.toJSON(params),
                success: function (data) {
                    var rows = [];
                    $.each(data.Content.Elements, function (index, value) {
                        tabla.table('unblock');
                        var row = [];
                        row.push('<input type="checkbox" />');
                        row.push(value.Id);
                        row.push(value.PeriodoMatriculacion.Nombre);
                        row.push(value.PeriodoMatriculacion.FechaInicio);
                        row.push(value.PeriodoMatriculacion.FechaFin);
                        row.push(isNull(value.PeriodoMatriculacion.Nro) ? '' : value.PeriodoMatriculacion.Nro);
                        row.push(value.PeriodoAcademicoId);

                        row.push(value.PeriodoAcademico != null ? value.PeriodoAcademico.Nombre : "");
                        row.push(value.PeriodoAcademico != null ? value.PeriodoAcademico.FechaInicio : "");
                        row.push(value.PeriodoAcademico != null ? value.PeriodoAcademico.FechaFin : "");
                        row.push(value.PeriodoAcademico != null ? value.PeriodoAcademico.Nro : "");
                        row.push('<span class="btn-opciones">' + $.Globalization.localize('TextOpciones') + '</span>');
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
                    url: SiteUrl + 'api/v1/mapeo-periodo-matriculacion/delete',
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
                        $('#tb-periodo-matriculacion-integracion').table('update');
                    }
                });
            }
        });
    }
}