$(function () {
    $.ajax({
        type: "POST",
        traditional: true,
        async: false,
        cache: false,
        url: '/Customize/UpdateDishImage',
        context: document.body,
        success: function (result) {
            $('#showcase-dish').css('background', 'url(' + result + ')');
        },
        error: function (xhr) {
            $('#showcase-dish').css('background', '/client-assets/res/img/generic.jpg');
        }
    });
})

