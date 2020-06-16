$(function () {
    $.ajax({
        type: "POST",
        traditional: true,
        async: false,
        cache: false,
        url: '/Customize/GetLogo',
        context: document.body,
        success: function (result) {
            $('#main-logo').attr('src', result);
        },
        error: function (xhr) {
            $('#main-logo').attr('src', '/assets/images/logo.png');
        }
    });
})
