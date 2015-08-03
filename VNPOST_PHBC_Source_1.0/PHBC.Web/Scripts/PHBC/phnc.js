var currentlisttinh = {};
var currentlisthuyen = {};
$('body').on('click', '.list-group .list-group-item', function () {
    $(this).toggleClass('active');
});

$('.dual-list .selector').click(function () {
    var $checkBox = $(this);
    var divcontainer;
    if ($checkBox.attr("customs") == "1") {
        divcontainer = ".tinh-partition";
    } else if ($checkBox.attr("customs") == "2") {
        divcontainer = ".huyen-partition";
    }
    else if ($checkBox.attr("customs") == "3") {
        divcontainer = ".buucuc-partition";
    }
    if (!$checkBox.hasClass('selected')) {
        $checkBox.addClass('selected').closest('.well ' + divcontainer).find('ul li:not(.active):visible').addClass('active');
        $checkBox.children('i').removeClass('glyphicon-unchecked').addClass('glyphicon-check');
    } else {
        $checkBox.removeClass('selected').closest('.well ' + divcontainer).find('ul li.active:visible').removeClass('active');
        $checkBox.children('i').removeClass('glyphicon-check').addClass('glyphicon-unchecked');
    }
});



$('[name="SearchDualList"]').keyup(function (e) {
    
    var code = e.keyCode || e.which;
    if (code == '9') return;
    if (code == '27') $(this).val(null);
    //var $rows = $(this).closest('.dual-list').find('.list-group li');
    var $rows = $(this).parents('.cslide-slide').find('.list-group li');

    var $selector = $(this).parents('.cslide-slide').find('.selector');
    $selector.removeClass('selected');
    $selector.children('i').removeClass('glyphicon-check').addClass('glyphicon-unchecked');

    var val = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();
    
    $rows.show().filter(function () {
        
        var text = $(this).attr('valname').replace(/\s+/g, ' ').toLowerCase();

        val = libIf.locdau(val);
        return !~text.indexOf(val);
        return !libIf.containstxt($(this).attr('valname'), val);

    }).hide();

});

var libIf = new LibIf();
function LibIf() {
    this.locdau = function (txt) {
        var str = txt.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a"); str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e"); str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i"); str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o"); str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u"); str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y"); str = str.replace(/đ/g, "d");
        return str;
    }
    this.containstxt = function (txt1, txt2) {
        if (!txt2) return false;
        txt2 = libIf.locdau(txt2);
        if (txt1 == txt2)
            return true;
        var arrayTxt1 = txt1.split(' ');
        var arrayTxt2 = txt2.split(' ');
        var countTxt1 = arrayTxt1.filter(function (x, index) { return arrayTxt2.indexOf(x) > -1; }).length;
        var countTxt2 = arrayTxt2.filter(function (x, index) { return arrayTxt1.indexOf(x) > -1; }).length;
        if (arrayTxt1.length / 2 <= countTxt1)
            return true;
        if (arrayTxt2.length / 2 <= countTxt2)
            return true;
        return false;
    }
}
