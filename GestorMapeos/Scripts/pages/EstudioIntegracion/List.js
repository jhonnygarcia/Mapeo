$(document).ready(function () {
    var tabla = $('#tb-estudio-integracion');
    $('input:text', '.content > div > div.box-fieldset:first').autoNumeric(AutoNumericInteger);
    $('#txt-id-estudio').autoNumeric();
    $("#cbx-estudio").combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-estudio',
        toolbar: {
            reset: true
        }
    }));
    $("#txt-id-plan-estudio").autoNumeric();
    $('#cbx-plan-estudio').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-plan',
        toolbar: {
            reset: true
        }
    }));
    $("#cbx-titulo").combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-titulo',
        fnSelect: function () {
            $('#cbx-especializacion').combobox('reset');
            $('#cbx-especializacion').combobox('enable');
        },
        fnTextChange: function () {
            $('#cbx-especializacion').combobox('reset');
            $('#cbx-especializacion').combobox('disable');
        },
        toolbar: {
            reset: function () {
                $('#cbx-especializacion').combobox('reset');
                $('#cbx-especializacion').combobox('disable');
            }
        }
    }));
    $('#cbx-especializacion').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-especializacion',
        fnParams: function () {
            return {
                FilterTitulo: $('#cbx-titulo').combobox('getId')
            };
        },
        toolbar: {
            reset: true
        }
    }));

    $('#cbx-especializacion').combobox('disable');

    $('#btn-buscar').click(function () {
        if (!isNull($("#cbx-titulo").combobox('getId')) && isNull($('#cbx-especializacion').combobox('getId'))) {
            var errors = [];
            errors.push($.Globalization.localize('ErrorNoTitulo'));
            showErrors(errors);
        } else {
            tabla.table('update');
        }

    });
    $('#btn-limpiar').click(function () {
        $('#txt-id-estudio').val('');
        $('#cbx-estudio').combobox('reset');
        $('#txt-id-plan-estudio').val('');
        $('#cbx-plan-estudio').combobox('reset');
        $('#cbx-titulo').combobox('reset');
        $('#cbx-especializacion').combobox('reset');
        tabla.table('update');
        $('#chk-todos').prop('checked', false);
    });
    $('#btn-crear').click(function () {
        gotoController('EstudioIntegracion/Create');
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
            sTitle: $.Globalization.localize('ColumnIdEstudio'),
            sWidth: "70px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnEstudio'),
            sWidth: "250px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnIdPlan'),
            sWidth: "50px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnPlanEstudio'),
            sWidth: "250px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnAnyo'),
            sWidth: "50px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnEspecializacion'),
            sWidth: "100px",
            bSortable: false
        },
        {
            sTitle: 'Acciones',
            sWidth: "50px",
            bSortable: false
        }],
        bServerSide: true,
        sAjaxSource: SiteUrl + 'api/v1/mapeo-estudio/advanced-search',
        fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('.btn-opciones', nRow).contextMenu({
                fnLoadServerData: function (callbackRender) {
                    var menu = [
                        {
                            value: $.Globalization.localize('TextEdit'),
                            url: SiteUrl + 'EstudioIntegracion/Edit/' + $(nRow).data('data').Id
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
                        { Positions: [1, 2, 7], ClassName: 'color-gestor' },
                        { Positions: [3, 4, 5, 6], ClassName: 'color-erp' }
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
                1, 'id-estudio',
                2, 'estudio',
                3, 'id-plan', 'id-estudio'
            ]);
            params.OrderDirection = paramsTabla.sSortDir_0;
            /******************************************************************/
            params.IdEstudio = $('#txt-id-estudio').val().isEmpty() ? null : parseInt($('#txt-id-estudio').val());
            params.FilterEstudio = $("#cbx-estudio").combobox('getId');
            params.IdPlanEstudio = $("#txt-id-plan-estudio").val().isEmpty() ? null : parseInt($('#txt-id-plan-estudio').val());;
            params.FilterPlanEstudio = $('#cbx-plan-estudio').combobox('getId');
            params.IdEspecializacion = $('#cbx-especializacion').combobox('getId');
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
                        row.push(value.Estudio.Id);
                        row.push(value.Estudio.Nombre);
                        row.push(value.PlantillaEstudioIntegracion.Id);
                        if (!isNull(value.Plan)) {
                            row.push(value.Plan.DisplayName);
                            row.push(value.Plan.Anyo);
                        } else {
                            row.push('');
                            row.push('');
                        }
                        row.push(!isNull(value.Especializacion) ? value.Especializacion.Nombre : '');
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
                    url: SiteUrl + 'api/v1/mapeo-estudio/delete',
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
                        $('#tb-estudio-integracion').table('update');
                    }
                });
            }
        });
    }
}