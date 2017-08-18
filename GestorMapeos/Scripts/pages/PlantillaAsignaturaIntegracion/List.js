$(document).ready(function () {
    var tabla = $('#tb-asignatura-plan-integracion');
    $('input:text', '.content > div > div.box-fieldset:first').autoNumeric(AutoNumericInteger);
    /**
     * Area de Filtros
     */
    $('#cbx-plantilla-estudio').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-plantilla-estudio',
        toolbar: {
            reset: function () { }
        },
        fnSelect: function () {
            $('#cbx-plantilla-asignatura').combobox('reset');
            $('#lb-codigo-plantilla-asignatura').text('');
        },
        fnTextChange: function () { }
    }));
    $('#cbx-plantilla-asignatura').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-plantiila-asignatura',
        toolbar: {
            reset: function () {
                $('#lb-codigo-plantilla-asignatura').text('');
            }
        },
        fnParams: function () {
            return {
                IdPlantillaEstudio: $('#cbx-plantilla-estudio').combobox('getId')
            };
        },
        fnSelect: function () {
            var itemData = $('#cbx-plantilla-asignatura').combobox('getData');
            $('#lb-codigo-plantilla-asignatura').text($.Globalization.localize('EtiquetaCodigo') + itemData.Codigo);
        },
        fnTextChange: function () {
            $('#lb-codigo-plantilla-asignatura').text('');
        },
        fnPreloadLabel: function (itemData) {
            return itemData.Codigo + " - " + itemData.Descripcion;
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
        $('#cbx-plantilla-estudio').combobox('reset');
        $('#cbx-plantilla-asignatura').combobox('reset');
        $('#cbx-plan-estudio-erp').combobox('reset');
        $('#cbx-asignatura-plan-erp').combobox('reset');

        $('#txt-id-plantilla-asignatura').val('');
        $('#txt-id-plantilla-estudio').val('');
        $('#txt-id-asignatura-plan-erp').val('');
        $('#txt-id-plan-estudio-erp').val('');
        $('#lb-codigo-plantilla-asignatura').text('');
        tabla.table('update');
        $('#chk-todos').prop('checked', false);
    });
    /**
     * Area de la Grilla
    */
    $('#btn-crear').click(function () {
        gotoController('PlantillaAsignaturaIntegracion/Create');
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
            sTitle: $.Globalization.localize('ColumnaIdPlantilla'),
            sWidth: "70px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPlantillaAsignatura'),
            sWidth: "30px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPlantillaEstudio'),
            sWidth: "100px",
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaTipo'),
            sWidth: "60px",
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
            sWidth: "30px",
            bSortable: false
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPlanEstudio'),
            sWidth: "100px",
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
        sAjaxSource: SiteUrl + 'api/v1/mapeo-plantilla-asignatura/advanced-search',
        fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('.btn-opciones', nRow).contextMenu({
                fnLoadServerData: function (callbackRender) {
                    var menu = [
                        { value: 'Editar', url: SiteUrl + 'PlantillaAsignaturaIntegracion/Edit/' + $(nRow).data('data').Id},
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
                        { Positions: [1, 2, 3, 4, 5], ClassName: 'color-gestor' },
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
                1, 'id-plantilla',
                2, 'plantilla-asignatura',
                3, 'plantilla-estudio',
                4, 'tipo',
                5, 'creditos',
                6, 'id-asignatura-plan', 'id-plantilla'
            ]);
            params.OrderDirection = paramsTabla.sSortDir_0;
            /******************************************************************/
            params.IdPlantillaEstudio = $('#txt-id-plantilla-estudio').val().isEmpty()
                ? null
                : parseInt($('#txt-id-plantilla-estudio').val());

            params.IdPlantillaAsignatura = $('#txt-id-plantilla-asignatura').val().isEmpty()
                ? null
                : parseInt($('#txt-id-plantilla-asignatura').val());

            params.IdPlanEstudio = $('#txt-id-plan-estudio-erp').val().isEmpty()
                ? null
                : parseInt($('#txt-id-plan-estudio-erp').val());
            params.IdAsignaturaPlan = $('#txt-id-asignatura-plan-erp').val().isEmpty()
                ? null
                : parseInt($('#txt-id-asignatura-plan-erp').val());


            params.FilterIdPlantillaEstudio = $('#cbx-plantilla-estudio').combobox('getId');
            params.FilterIdPlantillaAsignatura = $('#cbx-plantilla-asignatura').combobox('getId');
            params.FilterIdPlanEstudio = $('#cbx-plan-estudio-erp').combobox('getId');
            params.FilterIdAsignaturaPlan = $('#cbx-asignatura-plan-erp').combobox('getId');
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
                        row.push(value.PlantillaAsignatura.Id);
                        row.push(value.PlantillaAsignatura.NombreAsignatura);
                        row.push(value.PlantillaAsignatura.PlantillaEstudio.DisplayName);
                        row.push(value.PlantillaAsignatura.TipoAsignatura.Nombre);
                        row.push(parseFloat(value.PlantillaAsignatura.Creditos).toFixed(2));
                        row.push(value.Id);
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
                    url: SiteUrl + 'api/v1/mapeo-plantilla-asignatura/delete',
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
                        $('#tb-asignatura-plan-integracion').table('update');
                    }
                });
            }
        });
    }
}