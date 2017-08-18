$(document).ready(function () {
    $('#cbx-plantilla-estudio').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-plantilla-estudio',
        toolbar: {
            reset: function () {
            }
        },
        fnSelect: function () {
            $('#cbx-plantilla-asignatura').combobox('reset');
            limpiarComponentesPlantillaAsignatura();
        },
        fnTextChange: function () {
        }
    }));
    $('#cbx-plantilla-asignatura').combobox(CustomDefaultCombobox({
        url: SiteUrl + 'api/v1/gestor-commons/search-plantiila-asignatura',
        toolbar: {
            reset: function () {
                limpiarComponentesPlantillaAsignatura();
            }
        },
        fnSelect: function () {
            var itemData = $('#cbx-plantilla-asignatura').combobox('getData');
            $('#lb-codigo-plantilla-asignatura').text($.Globalization.localize('EtiquetaCodigo') + itemData.Codigo);
            $('#lb-id-plantilla-asignatura').text(itemData.Id);
            $('#lb-tipo-asignatura').text(itemData.TipoAsignatura);
            $('#lb-creditos').text(parseFloat(itemData.Creditos).toFixed(2));
            $('#lb-plantilla-estudio').text(itemData.PlantillaEstudio);
        },
        fnTextChange: function () {
            limpiarComponentesPlantillaAsignatura();
        },
        fnParams: function () {
            return {
                IdPlantillaEstudio: $('#cbx-plantilla-estudio').combobox('getId')
            };
        },
        fnPreloadLabel: function (itemData) {
            return itemData.Codigo + " - " + itemData.Descripcion;
        },
        fnPreloadValue: function (itemData) {
            return itemData.Descripcion;
        }
    }));
    $('#btn-guardar').click(function () {
        var params = {};
        var errors = new Array();
        params.IdAsignaturaPlan = $('#hd-id-asignatura-plan').val();
        params.IdPlantillaAsignatura = $('#cbx-plantilla-asignatura').combobox('getId');

        if (isNull(params.IdPlantillaAsignatura)) {
            errors.push($.Globalization.localize('ErrorNoPlantillaAsignatura'));
        }
        if (errors.length == 0) {
            $.blockUI();
            $.ajax({
                url: SiteUrl + 'api/v1/mapeo-plantilla-asignatura/' + params.IdAsignaturaPlan,
                data: $.toJSON(params),
                success: function (data) {
                    showMessage($.Globalization.localize('MessageOperacionExitosamente'), true);
                    gotoController('PlantillaAsignaturaIntegracion/Index');
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
        gotoController('PlantillaAsignaturaIntegracion/Index');
    });
    LoadData();
});

function LoadData() {
    var id = $('#hd-id-asignatura-plan').val();
    $('#page-asignatura-plan-editar').block();
    $.ajax({
        url: SiteUrl + 'api/v1/mapeo-plantilla-asignatura/' + id,
        type: 'GET',
        success: function (data) {
            $('#page-asignatura-plan-editar').unblock();

            $('#cbx-plantilla-estudio')
                .combobox('setValue', data.PlantillaAsignatura.PlantillaEstudio.DisplayName)
                .combobox('setId', data.PlantillaAsignatura.PlantillaEstudio.Id);
            $('#cbx-plantilla-asignatura')
                .combobox('setValue', data.PlantillaAsignatura.NombreAsignatura)
                .combobox('setId', data.PlantillaAsignatura.Id);
            $('#lb-codigo-plantilla-asignatura').text($.Globalization.localize('EtiquetaCodigo') + data.PlantillaAsignatura.Codigo);
            $('#lb-id-plantilla-asignatura').text(data.PlantillaAsignatura.Id);
            $('#lb-tipo-asignatura').text(data.PlantillaAsignatura.TipoAsignatura.Nombre);
            $('#lb-creditos').text(parseFloat(data.PlantillaAsignatura.Creditos).toFixed(2));
            $('#lb-plantilla-estudio').text(data.PlantillaAsignatura.PlantillaEstudio.DisplayName);

            $('#lb-asignatura-plan-erp').text(data.AsignaturaPlan.Asignatura.DisplayName);
            $('#lb-id-asignatura-plan-erp').text(data.AsignaturaPlan.Id);
            $('#lb-tipo-asignatura-erp').text(data.AsignaturaPlan.Asignatura.TipoAsignatura.Nombre);
            $('#lb-creditos-erp').text(parseFloat(data.AsignaturaPlan.Asignatura.Creditos).toFixed(2));
            $('#lb-plan-estudio-erp').text(data.AsignaturaPlan.Plan.DisplayName);
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

function limpiarComponentesPlantillaAsignatura() {
    $('#lb-codigo-plantilla-asignatura').text('');
    $('#lb-id-plantilla-asignatura').text('-');
    $('#lb-tipo-asignatura').text('-');
    $('#lb-creditos').text('-');
    $('#lb-plantilla-estudio').text('-');
}