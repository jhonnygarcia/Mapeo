$.Globalization.localize = function(key) {
    var globalizationComplete = $.extend({}, $.Globalization.GlobalPage, $.Globalization.Page);
    var stringValue = globalizationComplete[key];
    return stringValue != "" && stringValue != null ? stringValue : "";
};

function toEntityHtml(string) {
    $.each(entity_table, function (index, value) {
        if (index != 38) {
            string = string.replace(String.fromCharCode(index), value);
        }
    });
    return string;
};

var AutoNumericInteger = {
    vMax: '9999999',
    aSep: '',
    mDec: 0,
    aDec: ','
};

var pageKey = "";
var SAVED_PARAMS = null;

function initStorage() {
    try {
        $.storage = new $.store();
        SAVED_PARAMS = isEmpty(pageKey) ? null : $.storage.get(pageKey);
        if (SAVED_PARAMS && typeof SAVED_PARAMS == "string") {
            SAVED_PARAMS = $.parseJSON(SAVED_PARAMS);
        }
    } catch (exception) {
        $.storage.set(pageKey, null);
    }
}
/******************************************************************************/
$(document).ready(function () {
    initStorage();
    /**************************************************************************/
    $.tableSetup({
        bJQueryUI: true,
        aLengthMenu: [
        $.map(RegistrosPorPagina.split("|"), function (item) {
            return parseInt(item);
        }),
        $.map(RegistrosPorPagina.split("|"), function (item) {
            return parseInt(item);
        })],
        iDisplayLength: parseInt(RegistrosPorPagina.split("|")[0])
        //bLengthChange  : false
    });
    /**************************************************************************/
    $.comboboxSetup({
        delay: 900,
        textOverlabel: "Seleccione",
        messageLoading: ImgLoading
            .attr("title", "Cargando")
            .attr("alt", "Cargando"),
        pageable: true,
        configButtonPrev: {
            label: "Anterior",
            text: true,
            icons: {}
        },
        configButtonNext: {
            label: "Siguiente",
            text: true,
            icons: {}
        }
    });

    /**************************************************************************/
    $.showCustomMessageSetup({
        title: "Gestor de Mapeos"
    });
    /**************************************************************************/
    $.showMessageSetup({
        title: "Gestor de Mapeos",
        minHeight: 80
    });
    /**************************************************************************/
    $.showPopupPageSetup({
        title: "Gestor de Mapeos"
    });
    $.showConfirmationSetup({
        title: "Gestor de Mapeos"
    });
    /**************************************************************************/
    $.blockUI.defaults.message = ImgLoading;
    $.blockUI.defaults.css.border = "0px";
    $.blockUI.defaults.css.backgroundColor = "transparent";
    $.blockUI.defaults.overlayCSS.backgroundColor = "#000000";
    $.blockUI.defaults.overlayCSS.opacity = 0.5;
    $.blockUI.defaults.filter = "Alpha(Opacity=50)";
    $.blockUI.defaults.baseZ = 10;
    /**************************************************************************/
    $.ajaxSetup({
        cache: false,
        type: "POST",
        contentType: "application/json"
    });
    /**************************************************************************/
    $(".box-current-user .btn-opciones").click(function () {
        $(".box-current-user .list-opciones").toggle("fade");
    });
    $("#sub-menu").menu();
    $(".button").button();
    $(".box-fieldset .field-title").collapsibleContainer();
});
function gotoController(controller) {
    if (controller) {
        window.location.href = SiteUrl + controller;
    } else {
        window.location.href = SiteUrl;
    }
}
function cleanState() {
    $.storage.set(pageKey, null);
}
/**
 * Devuelve los milisegundos que representa la fecha de un Datetime
 */
function convertDateServerToClient(ms) {
    if (isNull(ms)) {
        return null;
    } else {
        if (typeof ms === "string") {
            ms = parseInt(ms);
        }
    }

    var date = new Date(ms);
    var gmt = date.getTimezoneOffset() * (60 * 1000);
    date.setTime(date.getTime() + gmt);
    return date;
}
/**
 * Devuelve una fecha que representa los milisegundos de un Timestamp
 */
function convertDateClientToServer(date) {
    if (isNull(date)) {
        return null;
    }
    var gmt = date.getTimezoneOffset() * (60 * 1000);
    var newDate = new Date(date);
    newDate.setTime(date.getTime() - gmt);
    return newDate.getTime();
}

$(function () {
    $.widget("if.colorize",{
        options: {
            Columns: [{Positions: [-1,-2], ClassName: ''}]
        },
        _create:function() {
            var tr = this.element;
            var options = this.options;
            if ($(tr).is('tr') && !isNull(this.options.Columns) && typeof options.Columns == "object") {
                var celdas = $('td', tr);
                $.each(options.Columns, function (index, value) {
                    var posiciones = value.Positions;
                    var className = value.ClassName;
                    if (!isNull(posiciones) && !isNull(className) && className != '') {
                        $.each(posiciones, function (i, v) {
                            $.each(celdas, function (ii, td) {
                                if (v === ii) {
                                    $(td).addClass(className);
                                }
                            });
                        });
                    }
                });
            }
        }
    });
});

$(document).ajaxComplete(function (event, request, settings) {
    resizeDialog();
});

function resizeDialog() {
    var dialogs = $(".ui-dialog:visible");
    dialogs.each(function () {
        $(this).find(".ui-dialog-content").dialog("option", "maxHeight", $(window).height() - 55);
    });
}

function CustomDefaultCombobox(conf) {
    if (isNull(conf)) {
        return {};
    }

    if (!$.isFunction(conf.fnParams)) {
        conf.fnParams = function () {
            return {};
        };
    } else {
        if (!$.isPlainObject(conf.fnParams())) {
            conf.fnParams = function () {
                return {};
            };
        }
    }

    if (!$.isFunction(conf.fnPrecondition)) {
        conf.fnPrecondition = function () {
            return true;
        };
    }

    conf.changeParams = isNull(conf.changeParams) ? {} : conf.changeParams;
    conf.changeParams.SearchText = isNullOrEmpty(conf.changeParams.SearchText) ? 'Descripcion,Description' : conf.changeParams.SearchText;
    conf.changeParams.PageIndex = isNullOrEmpty(conf.changeParams.PageIndex) ? 'PageIndex,Pagina' : conf.changeParams.PageIndex;
    conf.successData = isNull(conf.successData) ? {} : conf.successData;
    conf.successData.Id = isNullOrEmpty(conf.successData.Id) ? 'Id' : conf.successData.Id;
    conf.successData.Description = isNullOrEmpty(conf.successData.Description) ? 'Description,Descripcion' : conf.successData.Description;

    var extraConfig = {
        fnLoadServerData: function(elem, request, response) {
            if (conf.fnPrecondition()) {
                if (elem.options.messageLoading) {
                    elem.box.append(elem.options.messageLoading);
                }

                var dataParams = conf.fnParams();
                $.each(conf.changeParams.SearchText.split(','), function (i, v) {
                    dataParams[v] = request.term;
                });
                $.each(conf.changeParams.PageIndex.split(','), function (i, v) {
                    dataParams[v] = elem.getCurrentPage();
                });

                $.ajax({
                    url: $.isFunction(conf.url) ? conf.url() : conf.url,
                    data: $.toJSON(dataParams),
                    type: 'POST',
                    success: function(data, status, xhr) {
                        if (elem.options.messageLoading) {
                            elem.options.messageLoading.remove();
                        }

                        elem.loadPagination({
                            totalPages: xhr.getResponseHeader("X-TotalPages")
                        });

                        var fnValueProperty = function(obj, propertyes, index) {
                            if (index > propertyes.length) return '';
                            if (!isNull(obj[propertyes[index]]) && obj[propertyes[index]].toString() != ''
                            ) return obj[propertyes[index]];
                            return fnValueProperty(obj, propertyes, index + 1);
                        };

                        var textMappings = conf.successData.Description.split(',');
                        var idMappings = conf.successData.Id.split(',');

                        response($.map(data, function(item) {
                                var text = fnValueProperty(item, textMappings, 0);
                                var id = fnValueProperty(item, idMappings, 0);
                                item.Description = text;

                                var itemLabel = highlightText(isNull(item.Descripcion)
                                        ? (!isNull(conf.itemLength)
                                            ? summary(item.Description, conf.itemLength, '...')
                                            : item.Description)
                                        : (!isNull(conf.itemLength)
                                            ? summary(item.Descripcion, conf.itemLength, '...')
                                            : item.Descripcion),
                                        request.term);
                                var itemValue = isNull(item.Descripcion) ? item.Description : item.Descripcion;

                                if (!isNull(conf.fnPreloadLabel)) {
                                    itemLabel = conf.fnPreloadLabel(item);
                                }
                                if (!isNull(conf.fnPreloadValue)) {
                                    itemValue = conf.fnPreloadValue(item);
                                }
                                return {
                                    label: itemLabel,
                                    value: itemValue,
                                    id: id,
                                    option: this,
                                    data: item
                                };
                            }));
                    }
                });
            }
        }
    };
    conf = $.extend({}, extraConfig, conf);
    return conf;
}