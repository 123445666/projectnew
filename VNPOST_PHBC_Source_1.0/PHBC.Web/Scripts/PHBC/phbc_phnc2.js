function search_phnc(){
    $('[name="provinceName"]').keyup(function (e) {

        var code = e.keyCode || e.which;
        if (code == '9') return;
        if (code == '27') $(this).val(null);
        //var $rows = $(this).closest('.dual-list').find('.list-group li');
        var $rows = $(this).parents('.panel-body').find('table td');

        //var $selector = $(this).parents('.cslide-slide').find('.selector');
        //$selector.removeClass('selected');
        //$selector.children('i').removeClass('glyphicon-check').addClass('glyphicon-unchecked');
        var val = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();
        $rows.show().filter(function () {

            var text = $(this).attr('valname').replace(/\s+/g, ' ').toLowerCase();

            val = lib.locdau(val);
            return !~text.indexOf(val);
            return !lib.containstxt($(this).attr('valname'), val);

        }).hide();

    });
}