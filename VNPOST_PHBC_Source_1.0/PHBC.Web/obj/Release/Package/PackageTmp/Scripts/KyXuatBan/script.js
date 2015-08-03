var arayMonthText = ['nhất', 'hai', 'ba', 'tư', 'năm', 'sáu', 'bảy', 'tám', 'chín', 'mười', 'mười một', 'mười hai'];
function getDataKyXuatBan() {
    var id = parseInt($('.parent-kxb > .parent-kxb-btn > [btn-select][active]').attr('valid'));
    var box_data = $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"]');
    var data = {};
    //data['title'] = $.trim($('.parent-kxb > .parent-kxb-btn > [btn-select][active]').text());
    data['id'] = id;
    if (id >= 0 && id <= 3) {
        data['data'] = [];
        box_data.children('[active]').filter(function () { data['data'].push($(this).attr('valid')) });
        if (id == 1 && data['data'].length <= 0)
            return [false, "Vui lòng chọn <b>Thứ</b> xuất bản"];
        else if ((id >= 2 && id <= 3) && data['data'].length <= 0)
            return [false, "Vui lòng chọn <b>Ngày</b> xuất bản"];
    } else {
        var number = parseInt(box_data.children('[box-kxb-input]').children('input:text:eq(0)').val());
        var ky = parseInt(box_data.children('[box-kxb-input]').children('input:text:eq(1)').val());
        data['data'] = {};
        data['data']["number"] = number;
        data['data']["ky"] = ky;

        data['data']["data"] = [];

        for (var i = 0; i < ky; i++) {
            var _pa = box_data.children('[append-data-box]').children('[attr-ky]:eq(' + i + ')');
            data['data']["data"][i] = {};
            //data['data']["data"][i]["title"] = _pa.attr('valtitle');
            if (_pa.children('[attr-month]').children('select').val() == null || _pa.children('[attr-day]').children('select').val()==null)
                return [false, "Vui lòng chọn <b>ngày tháng</b> của <b>Kỳ " + (i + 1) + "</b> lớn hơn <b>Kỳ " + (i) + "</b>"];
            data['data']["data"][i]["month"] = _pa.children('[attr-month]').children('select').val();
            data['data']["data"][i]["day"] = _pa.children('[attr-day]').children('select').val();
        }
    }
    return [true, JSON.stringify(data)];
}
function loadDataKyXuatBan(data) {
    var id = parseInt(data.id);
    $('.parent-kxb > .parent-kxb-btn > [btn-select]').removeAttr('active');
    $('.parent-kxb').attr('valselect', id);
    $('.parent-kxb > .parent-kxb-btn > [btn-select][valid="' + id + '"]').attr('active', '');
    var box_data = $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"]');
    if (id >= 0 && id <= 3) {
        $.each(data.data, function (key, value) {
            box_data.children('[valid="' + value + '"]').attr('active', '');
        });
    } else {

        var _num = parseInt(data['data']["number"]);
        var _ky = parseInt(data['data']["ky"]);

        box_data.children('[box-kxb-input]').children('input:text:eq(0)').val(_num);
        box_data.children('[box-kxb-input]').children('input:text:eq(1)').val(_ky);



        var html_day = '<span select-option attr-day valtitle="Ngày"><select>';
        for (var i = 1; i <= 31 ; i++) {
            html_day += '<option value="' + i + '">Ngày ' + i + '</option>';
        }
        html_day += '</select></span>';
        if (id == 4) {
            var html_month = '<span select-option attr-month valtitle="Tháng"><select>';
            for (var i = 1; i <= _num ; i++) {
                html_month += '<option value="' + i + '">Tháng thứ ' + arayMonthText[i-1] + '</option>';
            }
            html_month += '</select></span>';

            $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"] > [append-data-box]').html('');
            for (var i = 1; i <= _ky ; i++) {
                $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"] > [append-data-box]').append('<div attr-ky valtitle="Kỳ ' + i + '">' + html_month + html_day + '</div>');
            }
        } else if (id == 5) {
            var html_month = '<span select-option attr-month valtitle="Tháng"><select>';
            for (var i = 1; i <= _num * 3 ; i++) {
                html_month += '<option value="' + i + '">Tháng thứ ' + arayMonthText[i - 1] + '</option>';
            }
            html_month += '</select></span>';

            $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"] > [append-data-box]').html('');
            for (var i = 1; i <= _ky ; i++) {
                $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"] > [append-data-box]').append('<div attr-ky valtitle="Kỳ ' + i + '">' + html_month + html_day + '</div>');
            }
        } else if (id == 6) {
            var html_month = '<span select-option attr-month valtitle="Tháng"><select>';
            for (var i = 1; i <= 12 ; i++) {
                html_month += '<option value="' + i + '">Tháng thứ ' + arayMonthText[i - 1] + '</option>';
            }
            html_month += '</select></span>';

            $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"] > [append-data-box]').html('');
            for (var i = 1; i <= _ky ; i++) {
                $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"] > [append-data-box]').append('<div attr-ky valtitle="Kỳ ' + i + '">' + html_month + html_day + '</div>');
            }
        }


        for (var i = 0; i < _ky; i++) {
            var _pa = box_data.children('[append-data-box]').children('[attr-ky]:eq(' + i + ')');
            _pa.children('[attr-month]').children('select').children('option[value="' + data['data']["data"][i]["month"] + '"]').prop('selected', true);
            _pa.children('[attr-day]').children('select').children('option[value="' + data['data']["data"][i]["day"] + '"]').prop('selected', true);
        }
        createEventSelect(id);
    }

}
function actionEventSelect(arr_select) {
    for (var i = 1; i < arr_select.length; i++) {
        var month = parseInt(arr_select.eq(i - 1).children('[attr-month]').children('select').val());
        var day = parseInt(arr_select.eq(i - 1).children('[attr-day]').children('select').val());

        var monthNow = parseInt(arr_select.eq(i).children('[attr-month]').children('select').val());
        var dayNow = parseInt(arr_select.eq(i).children('[attr-day]').children('select').val());
        console.log(monthNow + '  ' + month);
        arr_select.eq(i).children('[attr-month]').children('select').children('option').filter(function () {
            if (day == 31) {
                if (parseInt($(this).val()) <= month)
                    $(this).attr('disabled', 'disabled');
                else
                    $(this).removeAttr('disabled');
            } else {
                if (parseInt($(this).val()) < month)
                    $(this).attr('disabled', 'disabled');
                else
                    $(this).removeAttr('disabled');
            }
        });

        if (monthNow == month) {
            arr_select.eq(i).children('[attr-day]').children('select').children('option').filter(function () {
                if (parseInt($(this).val()) <= day)
                    $(this).attr('disabled', 'disabled');
                else
                    $(this).removeAttr('disabled');
            });
        } else if (monthNow < month) {
            arr_select.eq(i).children('[attr-day]').children('select').children('option').filter(function () {
                $(this).attr('disabled', 'disabled');
            });
        } else if (monthNow > month) {
            arr_select.eq(i).children('[attr-day]').children('select').children('option').filter(function () {
                $(this).removeAttr('disabled');
            });
        }
    }
}
function createEventSelect(id) {
    var arr_select = $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"] > [append-data-box] > [attr-ky]');
    arr_select.find('select').change(function () {
        actionEventSelect(arr_select);
    });
    actionEventSelect(arr_select);
}
function buildKxb() {
    for (var i = 2; i <= 8; i++) {
        $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="1"]').append('<span valid="' + i + '">' + (i == 8 ? 'Chủ nhật' : 'Thứ ' + i) + '</span>');
    }
    //for (var i = 1; i <= 4; i++) {
    //    $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="1"]').append('<span valid="' + i + '">Tuần '+i+'</span>');
    //}
    for (var i = 1; i <= 31; i++) {
        $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="2"]').append('<span valid="' + i + '">Ngày ' + i + '</span>');
        $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="3"]').append('<span valid="' + i + '">Ngày ' + i + '</span>');
    }


    /**/

    /*Event*/

    $('.parent-kxb > .parent-kxb-btn > [btn-select]').click(function () {
        $('.parent-kxb > .parent-kxb-btn > [btn-select]').removeAttr('active');
        $(this).attr('active', '');
        var id = parseInt($(this).attr('valid'));
        $(this).parents('.parent-kxb').attr('valselect', id);
        if (id >= 4 && id <= 6 && $.trim($('.parent-kxb .parent-kxb-box > [box-kxb][valid="' + id + '"] [append-data-box]').html()).length <= 0) {
            var _parent = $('.parent-kxb .parent-kxb-box > [box-kxb][valid="' + id + '"] [box-kxb-input]');
            _parent.children('input:text').filter(function () {
                if ($.trim($(this).val()).length == 0)
                    $(this).val(1);
            });
            $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"] > [box-kxb-input] > input[type=button]').click();
        }
    });

    $('.parent-kxb > .parent-kxb-box > [box-kxb] > [box-kxb-input] > input[type=button]').click(function () {
        var _this = $(this);
        var _parent = _this.parent('[box-kxb-input]');
        var id = parseInt(_parent.parent('[box-kxb]').attr('valid'));
        if (_parent.children('input:text').filter(function () { return $.trim($(this).val()).length == 0; }).length == 0) {
            var _num = parseInt(_parent.children('input:text:eq(0)').val());
            var _numMax = parseInt(_parent.children('input:text:eq(0)').attr('max'));
            var _ky = parseInt(_parent.children('input:text:eq(1)').val());
            if (_num > _numMax) {
                _num = _numMax;
                _parent.children('input:text:eq(0)').val(_num);
            } else if (_num <= 0) {
                _num = 1;
                _parent.children('input:text:eq(0)').val(_num);
            }
            if (_ky <= 0) {
                _ky = 1;
                _parent.children('input:text:eq(1)').val(_ky);
            }
            var html_day = '<span select-option attr-day valtitle="Ngày"><select>';
            for (var i = 1; i <= 31 ; i++) {
                html_day += '<option value="' + i + '">Ngày ' + i + '</option>';
            }
            html_day += '</select></span>';
            if (id == 4) {
                var html_month = '<span select-option attr-month valtitle="Tháng"><select>';
                for (var i = 1; i <= _num ; i++) {
                    html_month += '<option value="' + i + '">Tháng thứ ' + arayMonthText[i - 1] + '</option>';
                }
                html_month += '</select></span>';

                $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"] > [append-data-box]').html('');
                for (var i = 1; i <= _ky ; i++) {
                    $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"] > [append-data-box]').append('<div attr-ky valtitle="Kỳ ' + i + '">' + html_month + html_day + '</div>');
                }
            } else if (id == 5) {
                var html_month = '<span select-option attr-month valtitle="Tháng"><select>';
                for (var i = 1; i <= _num * 3 ; i++) {
                    html_month += '<option value="' + i + '">Tháng thứ ' + arayMonthText[i - 1] + '</option>';
                }
                html_month += '</select></span>';

                $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"] > [append-data-box]').html('');
                for (var i = 1; i <= _ky ; i++) {
                    $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"] > [append-data-box]').append('<div attr-ky valtitle="Kỳ ' + i + '">' + html_month + html_day + '</div>');
                }
            } else if (id == 6) {
                var html_month = '<span select-option attr-month valtitle="Tháng"><select>';
                for (var i = 1; i <= 12 ; i++) {
                    html_month += '<option value="' + i + '">Tháng thứ ' + arayMonthText[i - 1] + '</option>';
                }
                html_month += '</select></span>';

                $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"] > [append-data-box]').html('');
                for (var i = 1; i <= _ky ; i++) {
                    $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"] > [append-data-box]').append('<div attr-ky valtitle="Kỳ ' + i + '">' + html_month + html_day + '</div>');
                }
            }
            var total_day = id == 4 ? _num * 31 : id == 5 ? _num * 3 * 31 : 12 * 31;
            var stepSelect = total_day / _ky;
            var inMonth = 0, inDay = 0;
            var _arrKy = $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="' + id + '"] > [append-data-box] > [attr-ky]');
            for (var i = 0; i < _arrKy.size() ; i++) {
                _arrKy.eq(i).children('[attr-month]').children('select').children('option:eq(' + parseInt(inDay / 31) + ')').prop('selected', true);
                _arrKy.eq(i).children('[attr-day]').children('select').children('option:eq(' + parseInt((inDay - (31 * (parseInt(inDay / 31))))) + ')').prop('selected', true);
                inDay += stepSelect;
            }
            createEventSelect(id);
        }
    });
    $('.parent-kxb > .parent-kxb-box > [box-kxb] > [box-kxb-input] > input:text').keypress(function (e) {
        if (e.which == 13) {
            $(this).parent('[box-kxb-input]').children('span').click();
            e.preventDefault();
            return;
        }
        if (((e.which == 97 || e.which == 99 || e.which == 118) && e.ctrlKey) || e.which == 8 || (e.shiftKey && e.keyCode >= 37 && e.keyCode <= 40) || (e.keyCode >= 37 && e.keyCode <= 40) || e.keyCode <= 9)
            return;
        //if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
        //    (e.keyCode == 65 && e.ctrlKey === true) ||
        //    (e.keyCode >= 35 && e.keyCode <= 40)) {
        //    return;
        //}
        //console.log(e);
        if ((e.shiftKey || (e.which < 48 || e.which > 57))) {
            e.preventDefault();
        }
    });



    $('.parent-kxb > .parent-kxb-box > [box-kxb][valid="0"] > span,.parent-kxb > .parent-kxb-box > [box-kxb][valid="1"] > span,.parent-kxb > .parent-kxb-box > [box-kxb][valid="2"] > span,.parent-kxb > .parent-kxb-box > [box-kxb][valid="3"] > span').click(function () {
        if (typeof $(this).attr('active') === 'undefined')
            $(this).attr('active', '');
        else
            $(this).removeAttr('active');
    });


    /*End Event*/

}