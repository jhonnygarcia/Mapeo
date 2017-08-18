$(document).ready(function () {
    var tabla = $('#tb-periodo-estudio-integracion');
    $('input:text', '.content > div > div.box-fieldset:first').autoNumeric(AutoNumericInteger);
    /**
     * Area de Filtros
     */
    $('#cbx-periodo-matriculacion').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-periodo-matriculacion',
        toolbar: {
            reset: function () { }
        },
        fnSelect: function () {
            $('#cbx-estudio').combobox('reset');
        },
        fnTextChange: function () { }
    }));
    $('#cbx-estudio').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-estudio',
        toolbar: {
            reset: function () { }
        },
        fnParams: function () {
            return {
                IdPeriodoMatriculacion: $('#cbx-periodo-matriculacion').combobox('getId')
            };
        }
    }));
    $('#cbx-periodo-academico-erp').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-periodo-academico',
        toolbar: {
            reset: true
        },
        fnSelect: function () {
            $('#cbx-plan-estudio-erp').combobox('reset');
        },
        fnPreloadLabel: function (itemData) {
            return itemData.Descripcion + ' <strong>(' + itemData.FechaInicio + ' - ' + itemData.FechaFin + ')</strong>';
        },
        fnPreloadValue: function (itemData) {
            return itemData.Descripcion;
        }
    }));
    $('#cbx-plan-estudio-erp').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-plan',
        toolbar: {
            reset: function () { }
        },
        fnParams: function () {
            return {
                IdPeriodoAcademico: $('#cbx-periodo-academico-erp').combobox('getId')
            };
        }
    }));

    $('#btn-buscar').click(function () {
        tabla.table('update');
    });
    $('#btn-limpiar').click(function () {
        $('#cbx-periodo-matriculacion').combobox('reset');
        $('#cbx-estudio').combobox('reset');
        $('#cbx-periodo-academico-erp').combobox('reset');
        $('#cbx-plan-estudio-erp').combobox('reset');
        $('#txt-id-periodo-estudio').val('');
        $('#txt-id-plan-ofertado-erp').val('');
        tabla.table('update');
        $('#chk-todos').prop('checked', false);
    });
    /**
     * Area de la Grilla
    */
    $('#btn-crear').click(function () {
        gotoController('PeriodoEstudioIntegracion/Create');
    });
    $('#btn-eliminar').click(function () {
        var ids = [];
        $.each($('tbody input:checkbox', 'table'), function (index, value) {
            if ($(value).is(':checked')) {
                ids.push($(value).closest('tr').data('data').Id);
            }
        });
        fnEliminar([ids]);
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
            sTitle: $.Globalization.localize('ColumnaIdPeriodoEstudio'),
            sWidth: "70px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaEstudio'),
            sWidth: "70px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPeriodoMatriculacion'),
            sWidth: "70px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaIdPlanOfertado'),
            sWidth: "70px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPlanEstudio'),
            sWidth: "70px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPeriodoAcademico'),
            sWidth: "70px",
            bSortable: false
        },
        {
            sTitle: 'Acciones',
            sWidth: "50px",
            bSortable: false
        }],
        bServerSide: true,
        sAjaxSource: SiteUrl + 'api/v1/mapeo-periodo-estudio/advanced-search',
        fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('.btn-opciones', nRow).contextMenu({
                fnLoadServerData: function (callbackRender) {
                    var menu = [
                        { value: 'Editar', url: SiteUrl + 'PeriodoEstudioIntegracion/Edit/' + $(nRow).data('data').Id },
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
                        { Positions: [1, 2, 3, 7], ClassName: 'color-gestor' },
                        { Positions: [4, 5, 6], ClassName: 'color-erp' }
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
                1, 'id-periodo-estudio',
                2, 'estudio',
                3, 'periodo-matriculacion',
                4, 'id-plan-ofertado', 'id-periodo-estudio'
            ]);
            params.OrderDirection = paramsTabla.sSortDir_0;
            /******************************************************************/
            params.IdPeriodoEstudio = $('#txt-id-periodo-estudio').val();
            params.IdPeriodoMatriculacion = $('#cbx-periodo-matriculacion').combobox('getId');
            params.IdEstudio = $('#cbx-estudio').combobox('getId');
            params.IdPlanOfertado = $('#txt-id-plan-ofertado-erp').val();
            params.IdPeriodoAcademico = $('#cbx-periodo-academico-erp').combobox('getId');
            params.IdPlanEstudio = $('#cbx-plan-estudio-erp').combobox('getId');
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
                        row.push(value.PeriodoEstudio.Id);
                        row.push(value.PeriodoEstudio.Estudio.Nombre);
                        row.push(value.PeriodoEstudio.PeriodoMatriculacion.Nombre);
                        row.push(value.PlanOfertadoId);
                        if (!isNull(value.PlanOfertado)) {
                            row.push(value.PlanOfertado.Plan.DisplayName);
                            row.push(value.PlanOfertado.PeriodoAcademico.DisplayName);
                        } else {
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
                    url: SiteUrl + 'api/v1/mapeo-periodo-estudio/delete',
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
                        $('#tb-periodo-estudio-integracion').table('update');
                    }
                });
            }
        });
    }
}