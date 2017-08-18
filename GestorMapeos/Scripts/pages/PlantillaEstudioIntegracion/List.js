$(document).ready(function () {
    $('input:text', '.content > div > div.box-fieldset:first').autoNumeric(AutoNumericInteger);
    var tabla = $('#tb-plan-integracion');
    $('#cbx-plan').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-plan',
        toolbar: {
            reset: true
        }
    }));
    $('#cbx-plantilla-estudio').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-plantilla-estudio',
        toolbar: {
            reset: true
        }
    }));
    $('#btn-buscar').click(function () {
        tabla.table('update');
    });
    $('#btn-limpiar').click(function () {
        $('#cbx-plan').combobox('reset');
        $('#cbx-plantilla-estudio').combobox('reset');
        $('#txt-id-plan').val('');
        $('#txt-id-plantilla-estudio').val('');
        tabla.table('update');
        $('#chk-todos').prop('checked', false);
    });
    $('#btn-crear').click(function () {
        gotoController('PlantillaEstudioIntegracion/Create');
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
    /*
    Area de la Grilla de resultados
    */
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
            sTitle: $.Globalization.localize('ColumnIdPlantilla'),
            sWidth: "70px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnPlantillaEstudio'),
            sWidth: "280px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnIdPlan'),
            sWidth: "60px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnPlanEstudio'),
            sWidth: "280px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnAnyo'),
            sWidth: "50px",
            bSortable: false
        },
        {
            sTitle: $.Globalization.localize('TextAcciones'),
            sWidth: "50px",
            bSortable: false
        }],
        bServerSide: true,
        sAjaxSource: SiteUrl + 'api/v1/mapeo-plantilla-estudio/advanced-search',
        fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('.btn-opciones', nRow).contextMenu({
                fnLoadServerData: function (callbackRender) {
                    var menu = [
                        { value: 'Editar', url: SiteUrl + 'PlantillaEstudioIntegracion/Edit/' + $(nRow).data('data').Id },
                        {
                            value: 'Eliminar',
                            fnClick: function () {
                                fnEliminar([+$(nRow).data('data').Id]);
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
                        { Positions: [0, 1, 2, 6], ClassName: 'color-gestor' },
                        { Positions: [3, 4, 5], ClassName: 'color-erp' }
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
                1, 'id-plantilla',
                2, 'plantilla-estudio',
                3, 'id-plan', 'id-plantilla'
            ]);
            params.OrderDirection = paramsTabla.sSortDir_0;
            /******************************************************************/

            params.IdPlan = $('#txt-id-plan').val().isEmpty() ? null : parseInt($('#txt-id-plan').val());
            params.FilterIdPlan = $('#cbx-plan').combobox('getId');
            params.IdPlantillaEstudio = $('#txt-id-plantilla-estudio').val().isEmpty()
                ? null
                : parseInt($('#txt-id-plantilla-estudio').val());
            params.FilterIdPlantillaEstudio = $('#cbx-plantilla-estudio').combobox('getId');

            /******************************************************************/
            tabla.table('block');
            $('#chk-todos').prop('checked', false);
            $.ajax({
                url: sSource,
                data: $.toJSON(params),
                success: function (data, status, xhr) {
                    tabla.table('unblock');
                    var rows = [];
                    $.each(data.Content.Elements, function (index, value) {
                        var row = [];
                        row.push('<input type="checkbox" />');
                        row.push(value.PlantillaEstudio.Id);
                        row.push(value.PlantillaEstudio.DisplayName);
                        row.push(value.Id);
                        if (!isNull(value.Plan)) {
                            row.push(value.Plan.DisplayName);
                            row.push(value.Plan.Anyo);
                        } else {
                            row.push('');
                            row.push('');
                        }
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
                    url: SiteUrl + 'api/v1/mapeo-plantilla-estudio/delete',
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
                        $('#tb-plan-integracion').table('update');
                    }
                });
            }
        });
    }
}