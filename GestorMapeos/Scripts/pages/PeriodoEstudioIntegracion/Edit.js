$(document).ready(function () {
    $("#btn-search-plan-ofertado").click(function () {
        PopupCargarPlanOfertado();
    });
    $('#btn-guardar').click(function () {
        var params = {};
        var errors = new Array();
        params.IdPeriodoEstudio = $('#hd-id-periodo-estudio').val();
        params.IdPlanOfertado = $('#hd-id-plan-ofertado').val();

        if (isNull(params.IdPeriodoEstudio) || params.IdPeriodoEstudio === "") {
            errors.push($.Globalization.localize('ErrorNoSeleccionPeriodoEstudio'));
        }
        if (isNull(params.IdPlanOfertado) || params.IdPlanOfertado === "") {
            errors.push($.Globalization.localize('ErrorNoSeleccionPlanOfertado'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-periodo-estudio/' + params.IdPlanOfertado,
                data: $.toJSON(params),
                success: function (data, status, xhr) {
                    showMessage($.Globalization.localize('MessageOperacionExitosamente'), true);
                    gotoController('PeriodoEstudioIntegracion/Index');
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
        gotoController('PeriodoEstudioIntegracion/Index');
    });
    LoadData();
});

function LoadData() {
    var id = $('#hd-id-periodo-estudio').val();
    $.ajax({
        url: SiteUrl + 'api/v1/mapeo-periodo-estudio/' + id,
        type: 'GET',
        success: function (data, status, xhr) {
            $("#lb-id-periodo-estudio").text(data.PeriodoEstudio.Id);
            $("#lb-estudio").text(data.PeriodoEstudio.Estudio.Nombre);
            $("#lb-periodo-matriculacion").text(data.PeriodoEstudio.PeriodoMatriculacion.Nombre);

            $("#lb-id-plan-ofertado").text(data.PlanOfertado.Id);
            $("#lb-plan-estudio").text(data.PlanOfertado.Plan.DisplayName);
            $("#lb-periodo-academico").text(data.PlanOfertado.PeriodoAcademico.DisplayName);

            $('#hd-id-periodo-estudio').val(data.PeriodoEstudio.Id);
            $('#hd-id-plan-ofertado').val(data.PlanOfertado.Id);
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
 * Popup PlanOfertadoErp
 */
function PopupCargarPlanOfertado() {
    $.blockUI();
    var popup = null;
    var buttons = {};
    /***************************************************************************/
    buttons[$.Globalization.localize('Select')] = function () {
        var params = {};
        var errors = [];
        var selecciones = [];
        $.each($('#tb-plan-ofertado tbody input:checkbox'), function (index, value) {
            if ($(value).is(':checked')) {
                var planOfertado = {};
                planOfertado.Id = $(value).closest('tr').data('data').Id;
                planOfertado.Plan = $(value).closest('tr').data('data').Plan.DisplayName;
                planOfertado.PeriodoAcademico = $(value).closest('tr').data('data').PeriodoAcademico.DisplayName;
                selecciones.push(planOfertado);
            }
        });

        if (selecciones.length > 0) {
            params.Id = selecciones[selecciones.length - 1].Id;
            params.Plan = selecciones[selecciones.length - 1].Plan;
            params.PeriodoAcademico = selecciones[selecciones.length - 1].PeriodoAcademico;
        }

        if (isNull(params.Id) || isNull(params.Plan) || isNull(params.PeriodoAcademico)) {
            errors.push($.Globalization.localize('ErrorNoSeleccionPlanOfertado'));
        }
        if (errors.length == 0) {
            $('#hd-id-plan-ofertado').val(params.Id);
            $('#lb-id-plan-ofertado').text(params.Id);
            $('#lb-plan-estudio').text(params.Plan);
            $('#lb-periodo-academico').text(params.PeriodoAcademico);
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
        title: $.Globalization.localize('TituloPopupPlanOfertado'),
        url: SiteUrl + 'PeriodoEstudioIntegracion/PopupPlanesOfertados',
        open: function () {
            popup = $(this);
            $.unblockUI();
        },
        buttons: buttons,
        width: 800,
        close: function () {

        },
    }, false, function () {
        inicializarPopupPlanOfertado(popup);
        loadPlanesOfertados($("#tb-plan-ofertado"));
    });
}

function inicializarPopupPlanOfertado(popup) {
    $("#txt-id-plan-ofertado", popup).autoNumeric(AutoNumericInteger);
    $("#cbx-periodo-academico", popup).combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/erp-commons/search-periodo-academico',
        toolbar: {
            reset: true
        },
        fnSelect: function () {
            $('#cbx-plan-estudio').combobox('reset');
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
        }
    }));

    $('#btn-buscar', popup).button();
    $('#btn-buscar', popup).click(function () {
        $("#tb-plan-ofertado", popup).table('update');
    });
    $('#btn-limpiar', popup).button();
    $('#btn-limpiar', popup).click(function () {
        $('#cbx-periodo-academico', popup).combobox('reset');
        $('#cbx-plan-estudio', popup).combobox('reset');
        $('#txt-id-plan-ofertado', popup).val('');
        $("#tb-plan-ofertado", popup).table('update');
    });
}

function loadPlanesOfertados(table) {
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
            sTitle: $.Globalization.localize('ColumnaIdPlanOfertado'),
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPlanEstudio'),
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPeriodoAcademico'),
            bSortable: true
        }],
        bServerSide: true,
        sAjaxSource: SiteUrl + 'api/v1/erp-commons/search-plan-ofertado',
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
                1, 'id-plan-ofertado',
                2, 'plan-estudio',
                3, 'periodo-academico', 'id-plan-ofertado'
            ]);
            params.OrderDirection = paramsTabla.sSortDir_0;
            /******************************************************************/
            params.IdPlanOfertado = $('#txt-id-plan-ofertado').val();
            params.IdPlanEstudio = $('#cbx-plan-estudio').combobox('getId');
            params.IdPeriodoAcademico = $('#cbx-periodo-academico').combobox('getId');
            /******************************************************************/
            table.table('block');
            $.ajax({
                url: sSource,
                data: $.toJSON(params),
                success: function (data, status, xhr) {
                    table.table('unblock');
                    var rows = [];
                    $.each(data.Content.Elements, function (index, value) {
                        var row = [];
                        row.push('<input type="checkbox" />');
                        row.push(value.Id);
                        row.push(value.Plan.DisplayName);
                        row.push(value.PeriodoAcademico.DisplayName);
                        rows.push(row);
                    });
                    fnCallback({
                        "sEcho": paramsTabla.sEcho,
                        "aaData": rows,
                        "iTotalRecords": data.Content.TotalPages,
                        "iTotalDisplayRecords": data.Content.TotalElements
                    });
                    table.table('setData', data.Content.Elements);
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