function messageSuccessShow(content) {
    var _popupshow = new jPopup({
        show: true,
        title: "Thông báo",
        width: 350,
        body: '<div style="text-align:center;margin:5px 0px;">' + content + '</div>'
    });
    _popupshow.show()
}

function messageErrorShow(content) {
    var _popupshow = new jPopup({
        show: true,
        title: "Thông báo lỗi",
        width: 350,
        body: '<div style="text-align:center;margin:5px 0px;">' + content + '</div>'
    });
    _popupshow.show()
}

function messageShow(title, content) {
    var _popupshow = new jPopup({
        show: true,
        title: title,
        width: 350,
        body: '<div style="text-align:center;margin:5px 0px;">' + content + '</div>'
    });
    _popupshow.show()
}
//Trong trường hợp 'show:false' muốn hiển thị popup viết như sau:
//var _popupshow = new jPopup({....});
//_popupshow.show();
