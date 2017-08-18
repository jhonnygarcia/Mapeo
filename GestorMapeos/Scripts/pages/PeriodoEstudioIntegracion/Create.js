$(document).ready(function () {
    $('#btn-buscar-periodo-estudio').click(function () {
        PopupCargarPeriodoEstudio();
    });
    $('#btn-buscar-plan-ofertado').click(function () {
        PopupCargarPlanOfertado();
    });

    $('#btn-guardar').click(function () {
        var params = {};
        var errors = new Array();
        params.IdPeriodoEstudio = $('#hd-id-periodo-estudio').val();
        params.IdPlanOfertado = $('#hd-id-plan-ofertado-erp').val();

        if (isNull(params.IdPeriodoEstudio) || params.IdPeriodoEstudio === "") {
            errors.push($.Globalization.localize('ErrorNoSeleccionPeriodoEstudio'));
        }
        if (isNull(params.IdPlanOfertado) || params.IdPlanOfertado === "") {
            errors.push($.Globalization.localize('ErrorNoSeleccionPlanOfertado'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-periodo-estudio',
                type: 'PUT',
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
    $('#btn-guardar-continuar').click(function () {
        var params = {};
        var errors = new Array();
        params.IdPeriodoEstudio = $('#hd-id-periodo-estudio').val();
        params.IdPlanOfertado = $('#hd-id-plan-ofertado-erp').val();

        if (isNull(params.IdPeriodoEstudio) || params.IdPeriodoEstudio === "") {
            errors.push($.Globalization.localize('ErrorNoSeleccionPeriodoEstudio'));
        }
        if (isNull(params.IdPlanOfertado) || params.IdPlanOfertado === "") {
            errors.push($.Globalization.localize('ErrorNoSeleccionPlanOfertado'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-periodo-estudio',
                type: 'PUT',
                data: $.toJSON(params),
                success: function (data, status, xhr) {
                    showMessage($.Globalization.localize('MessageOperacionExitosamente'), true);
                    limpiarComponentesPeriodoEstudioUnir();
                    limpiarComponentesPlanOfertado();
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

    limpiarComponentesPeriodoEstudioUnir();
    limpiarComponentesPlanOfertado();
});

function limpiarComponentesPeriodoEstudioUnir() {
    $('#lb-id-periodo-estudio').text('-');
    $('#lb-estudio').text('-');
    $('#lb-periodo-matriculacion').text('-');
}

function limpiarComponentesPlanOfertado() {
    $('#lb-id-plan-ofertado-erp').text('-');
    $('#lb-plan-estudio-erp').text('-');
    $('#lb-periodo-academico-erp').text('-');
}

/**
 * Popup PeriodoEstudioUnir
 */
function PopupCargarPeriodoEstudio() {
    $.blockUI();
    var popup = null;
    var buttons = {};
    /***************************************************************************/
    buttons[$.Globalization.localize('Select')] = function () {
        var params = {};
        var errors = new Array();
        var selecciones = new Array();
        $.each($('#tb-periodo-estudio tbody input:checkbox'), function (index, value) {
            if ($(value).is(':checked')) {
                var periodoEstudio = {};
                periodoEstudio.Id = $(value).closest('tr').data('data').Id;
                periodoEstudio.Estudio = $(value).closest('tr').data('data').Estudio.Nombre;
                periodoEstudio.PeriodoMatriculacion = $(value).closest('tr').data('data').PeriodoMatriculacion.Nombre;
                selecciones.push(periodoEstudio);
            }
        });

        if (selecciones.length > 0) {
            params.Id = selecciones[selecciones.length - 1].Id;
            params.Estudio = selecciones[selecciones.length - 1].Estudio;
            params.PeriodoMatriculacion = selecciones[selecciones.length - 1].PeriodoMatriculacion;
        }

        if (isNull(params.Id) || isNull(params.Estudio) || isNull(params.PeriodoMatriculacion)) {
            errors.push($.Globalization.localize('ErrorNoSeleccionPeriodoEstudio'));
        }
        if (errors.length == 0) {
            $('#hd-id-periodo-estudio').val(params.Id);
            $('#lb-id-periodo-estudio').text(params.Id);
            $('#lb-estudio').text(params.Estudio);
            $('#lb-periodo-matriculacion').text(params.PeriodoMatriculacion);
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
        title: $.Globalization.localize('TituloPopupPeriodoEstudio'),
        url: SiteUrl + 'PeriodoEstudioIntegracion/PopupPeriodosEstudio',
        open: function () {
            popup = $(this);
            $.unblockUI();
        },
        buttons: buttons,
        width: 800
    }, false, function () {
        inicializarPopupPeriodoEstudio(popup);
        loadPeriodosEstudio($("#tb-periodo-estudio"));
    });
}

function inicializarPopupPeriodoEstudio(popup) {
    $("#txt-id-periodo-estudio", popup).autoNumeric(AutoNumericInteger);
    $("#cbx-periodo-matriculacion", popup).combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-periodo-matriculacion',
        toolbar: {
            reset: true
        },
        fnSelect: function () {
            $('#cbx-estudio').combobox('reset');
        }
    }));
    $("#cbx-estudio", popup).combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-estudio',
        toolbar: {
            reset: true
        },
        fnParams: function () {
            return {
                IdPeriodoMatriculacion: $('#cbx-periodo-matriculacion').combobox('getId')
            };
        }
    }));

    $('#btn-buscar', popup).button();
    $('#btn-buscar', popup).click(function () {
        $("#tb-periodo-estudio", popup).table('update');
    });
    $('#btn-limpiar', popup).button();
    $('#btn-limpiar', popup).click(function () {
        $('#cbx-periodo-matriculacion', popup).combobox('reset');
        $('#cbx-estudio', popup).combobox('reset');
        $('#txt-id-periodo-estudio', popup).val('');
        $("#tb-periodo-estudio", popup).table('update');
    });
}

function loadPeriodosEstudio(table) {
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
            sTitle: $.Globalization.localize('ColumnaIdPeriodoEstudio'),
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaEstudio'),
            bSortable: true
        },
        {
            className: '',
            sTitle: $.Globalization.localize('ColumnaPeriodoMatriculacion'),
            bSortable: true
        }],
        bServerSide: true,
        sAjaxSource: SiteUrl + 'api/v1/gestor-commons/search-periodo-estudio',
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
                1, 'id-periodo-estudio',
                2, 'estudio',
                3, 'periodo-matriculacion', 'id-periodo-estudio'
            ]);
            params.OrderDirection = paramsTabla.sSortDir_0;
            /******************************************************************/
            params.IdPeriodoEstudio = $('#txt-id-periodo-estudio').val();
            params.IdEstudio = $('#cbx-estudio').combobox('getId');
            params.IdPeriodoMatriculacion = $('#cbx-periodo-matriculacion').combobox('getId');
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
                        row.push(value.Estudio.Nombre);
                        row.push(value.PeriodoMatriculacion.Nombre);
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
            $('#hd-id-plan-ofertado-erp').val(params.Id);
            $('#lb-id-plan-ofertado-erp').text(params.Id);
            $('#lb-plan-estudio-erp').text(params.Plan);
            $('#lb-periodo-academico-erp').text(params.PeriodoAcademico);
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