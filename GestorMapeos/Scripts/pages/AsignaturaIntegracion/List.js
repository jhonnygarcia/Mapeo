$(document).ready(function () {
    var tabla = $('#tb-asignatura-estudio-integracion');
    $('input:text', '.content > div > div.box-fieldset:first').autoNumeric(AutoNumericInteger);
    /**
     * Area de Filtros
     */
    $('#cbx-estudio').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-estudio',
        toolbar: {
            reset: function () { }
        },
        fnSelect: function () {
            $('#cbx-asignatura').combobox('reset');
        },
        fnTextChange: function () { }
    }));
    $('#cbx-asignatura').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-asignatura',
        toolbar: {
            reset: function () { }
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
            reset: function () { }
        },
        fnSelect: function () {
            $('#cbx-asignatura-plan-erp').combobox('reset');
        },
        fnTextChange: function () { }
    }));
    $('#cbx-asignatura-plan-erp').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-asignatura-plan',
        toolbar: {
            reset: function () { }
        },
        fnParams: function () {
            return {
                IdPlanEstudio: $('#cbx-plan-estudio-erp').combobox('getId')
            };
        }
    }));

    $('#btn-buscar').click(function () {
        tabla.table('update');
    });
    $('#btn-limpiar').click(function () {
        $('#cbx-estudio').combobox('reset');
        $('#cbx-asignatura').combobox('reset');
        $('#cbx-plan-estudio-erp').combobox('reset');
        $('#cbx-asignatura-plan-erp').combobox('reset');
        $('#txt-id-asignatura').val('');
        $('#txt-id-estudio').val('');
        $('#txt-id-asignatura-plan-erp').val('');
        $('#txt-id-plan-estudio-erp').val('');
        $('#chk-todos').prop('checked', false);
        tabla.table('update');
    });
    /**
     * Area de la Grilla
    */
    $('#btn-crear').click(function () {
        gotoController('AsignaturaIntegracion/Create');
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
            sTitle: $.Globalization.localize('ColumnaIdAsignatura'),
            sWidth: "70px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaAsignatura'),
            sWidth: "100px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaEstudio'),
            sWidth: "80px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaTipo'),
            sWidth: "30px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaCreditos'),
            sWidth: "30px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaIdAsignaturaPlan'),
            sWidth: "30px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaAsignaturaPlan'),
            sWidth: "100px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPlanEstudio'),
            sWidth: "80px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaTipoErp'),
            sWidth: "30px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaCreditosErp'),
            sWidth: "30px",
            bSortable: false
        },
        {
            sTitle: 'Acciones',
            sWidth: "50px",
            bSortable: false
        }],
        bServerSide: true,
        sAjaxSource: SiteUrl + 'api/v1/mapeo-asignatura/advanced-search',
        fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('.btn-opciones', nRow).contextMenu({
                fnLoadServerData: function (callbackRender) {
                    var menu = [
                        { value: 'Editar', url: SiteUrl + 'AsignaturaIntegracion/Edit/' + $(nRow).data('data').Id },
                        {
                            value: 'Eliminar',
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
                1, 'id-asignatura',
                2, 'asignatura',
                3, 'estudio',
                4, 'tipo',
                5, 'creditos',
                6, 'id-asignatura-plan', 'id-asignatura'
            ]);
            params.OrderDirection = paramsTabla.sSortDir_0;
            /******************************************************************/
            params.IdEstudio = isNull($('#cbx-estudio').combobox('getId'))
                ? $('#txt-id-estudio').val()
                : $('#cbx-estudio').combobox('getId');
            params.IdAsignatura = isNull($('#cbx-asignatura').combobox('getId'))
                ? $('#txt-id-asignatura').val()
                : $('#cbx-asignatura').combobox('getId');
            params.IdPlanEstudio = isNull($('#cbx-plan-estudio-erp').combobox('getId'))
                ? $('#txt-id-plan-estudio-erp').val()
                : $('#cbx-plan-estudio-erp').combobox('getId');
            params.IdAsignaturaPlan = isNull($('#cbx-asignatura-plan-erp').combobox('getId'))
                ? $('#txt-id-asignatura-plan-erp').val()
                : $('#cbx-asignatura-plan-erp').combobox('getId');
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
                        row.push(value.AsignaturaUnir.Id);
                        row.push(value.AsignaturaUnir.Nombre);
                        row.push(value.AsignaturaUnir.EstudioUnir.Nombre);
                        row.push(value.AsignaturaUnir.TipoAsignatura);
                        row.push(parseFloat(value.AsignaturaUnir.Creditos).toFixed(2));
                        row.push(value.AsignaturaPlanIntegracionId);
                        if (!isNull(value.AsignaturaPlan)) {
                            row.push(value.AsignaturaPlan.Asignatura.DisplayName);
                            row.push(value.AsignaturaPlan.Plan.DisplayName);
                            row.push(value.AsignaturaPlan.Asignatura.TipoAsignatura.Nombre);
                            row.push(parseFloat(value.AsignaturaPlan.Asignatura.Creditos).toFixed(2));
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
                    url: SiteUrl + 'api/v1/mapeo-asignatura/delete',
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
                        $('#tb-asignatura-estudio-integracion').table('update');
                    }
                });
            }
        });
    }
}