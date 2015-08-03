$(document).ready(function () {
    lib.searchtable();
    lib.fromcontroler();
});

var lib = new Lib();
function Lib() {
    this.RGBColor = function (hex) {
        var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
        return result ? {
            r: parseInt(result[1], 16),
            g: parseInt(result[2], 16),
            b: parseInt(result[3], 16)
        } : null;
    }
    this.foregroundColor = function (hex) {
        var color = this.RGBColor(hex);
        var number = Math.sqrt(color.r * color.r * .299 + color.g * color.g * .587 + color.b * color.b * .114);
        return (number <= 170) ? "#FFFFFF" : "#000";
    }
    this.randomcolor = function () {
        return "#"+((1<<24)*Math.random()|0).toString(16);
    }
    this.searchtable = function () {
        $('[table-search]').each(function () {
            if (typeof $(this).attr('valid') == 'undefined') {
                var idGuid = lib.newGuid()
                var tableSearch = $(this).attr('valid', idGuid);
                tableSearch.scroll(function () {
                    $(this).children('[attr-top-scroll]').css({ 'top': $(this).scrollTop() + 'px' });
                }).scroll();
                var elementSearch = $('<div table-seach-input valid="' + idGuid + '"><input type="text" placeholder="Tìm kiếm"/></div>').insertBefore(tableSearch);
                elementSearch.children('input:text').keyup(function () {
                    var _input = $(this);
                    if (window["search-time-" + idGuid])
                        clearTimeout(window["search-" + idGuid]);
                    console.log(_input.val());
                    window["search-time-" + idGuid] = setTimeout(function () {
                        var txt = $.trim(_input.val());
                        if (typeof window["search-data-" + idGuid] == 'undefined') {
                            window["search-data-" + idGuid] = [];
                            tableSearch.find('tr').filter(function (index) {
                                var elementTD = $(this).children('td');
                                if (elementTD.length > 0) {
                                    var style = typeof $(this).attr('style') == 'undefined' ? '' : $(this).attr('style');
                                    $(this).attr('valstyle', style);
                                    var dataTr = {
                                        index: index,
                                        data: []
                                    };
                                    elementTD.filter(function () {
                                        var value = $.trim($(this).text());
                                        if (value.length > 0)
                                            dataTr.data.push({
                                                index: index,
                                                value: $.trim($(this).text())
                                            });
                                    });
                                    window["search-data-" + idGuid].push(dataTr);

                                }
                            });
                        }
                        if (txt.length > 0) {
                            window["search-data-" + idGuid].filter(function (data, index) {
                                var isShow = false;
                                for (var i = 0; i < data.data.length; i++) {
                                    if (lib.containstxt(data.data[i].value, txt)) {
                                        isShow = true;
                                        break;
                                    }
                                }
                                var row = tableSearch.find('tr:eq(' + data.index + ')');
                                if (isShow)
                                    row.attr('style', row.attr('valstyle'));
                                else
                                    row.attr('style', 'display:none !important;' + row.attr('valstyle'));
                            });
                        } else {
                            tableSearch.find('tr').filter(function (index, element) {
                                $(this).css({ 'display': 'table-row' });
                            });
                        }
                    }, 300);
                });
            }
        });
    }
    this.fromcontroler = function () {
        $('.table td[title]').each(function () {
            $(this).attr('attr-title', $(this).attr('title'));
            $(this).removeAttr('title');
        });
        $('.form-group').find('.control-label + div > input:checkbox').each(function () {
            //var width = $(this).parent().prev('label').css('width');
            //$(this).parent().css({ 'width': '50px', 'right': 'calc(100% - ' + width + ' - 62px)' });
            $(this).parent().attr('checkbox', '').parent().attr('panelcheckbox', '');
        });
        $('.form-group').find('.control-label + div > input:checkbox').unbind("click");
        $('.form-group').find('.control-label + div > input:checkbox').on('click', function () {
            // in the handler, 'this' refers to the box clicked on
            var $box = $(this);
            var group = $("input:checkbox[groupname='" + $box.attr("groupname") + "']");
            if (group.size() > 1) {
                if ($box.is(":checked")) {
                    // the name of the box is retrieved using the .attr() method
                    // as it is assumed and expected to be immutable
                    // the checked state of the group/box on the other hand will change
                    // and the current value is retrieved using .prop() method
                    group.prop("checked", false);
                    $box.prop("checked", true);
                } else {
                    if ($("input:checkbox[groupname='" + $box.attr("groupname") + "']:checked").size() == 0)
                        $box.prop("checked", true);
                    else
                        $box.prop("checked", false);
                }
            }
        });
        $('.form-group').find('.control-label + div > textarea').each(function () {
            $(this).parents('.form-group').attr('style-textarea', '');
        });
        //$('.form-group').find('.btn:eq(0)').each(function () {
        //    $(this).parent().css('margin-left', '-=15px');
        //});
        $('.dl-horizontal').find('dt > label').each(function () {
            $(this).removeClass('control-label');
        });
    }
    this.newGuid = function () {
        var d = new Date().getTime();
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        return uuid;
    }
    this.locdau = function (txt) {
        var str = txt.toLowerCase(); str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a"); str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e"); str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i"); str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o"); str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u"); str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y"); str = str.replace(/đ/g, "d");
        return str;
    }
    this.containstxt = function (txt1, txt2) {
        txt1 = lib.locdau(txt1);
        txt2 = lib.locdau(txt2);
        if (txt1 == txt2)
            return true;
        if (txt1.indexOf(txt2) > -1)
            return true;

        var arrayTxt1 = txt1.split(' ');
        var arrayTxt2 = txt2.split(' ');
        //var countTxt1 = arrayTxt1.filter(function (x, index) { return arrayTxt2.indexOf(x) > -1; }).length;
        var countTxt2 = arrayTxt2.filter(function (x, index) { return arrayTxt1.indexOf(x) > -1; }).length;

        //if (countTxt1 / arrayTxt1.length * 100 > 50)
        //    return true;
        //console.log((countTxt2 / arrayTxt2.length * 100) + " - " + txt1 + " - " + txt2);
        if (countTxt2 / arrayTxt1.length * 100 > 50)
            return true;
        return false;
    }
}