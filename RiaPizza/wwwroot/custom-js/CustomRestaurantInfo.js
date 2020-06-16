$(function () {
    $.ajax({
        type: "POST",
        traditional: true,
        async: false,
        cache: false,
        url: '/RestaurantInfo/GetLocation',
        context: document.body,
        success: function (result) {
            $('#RestaurantMap').attr('src', result);
        },
        error: function (xhr) {
            $('#RestaurantMap').attr('src', 'https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2524.5710064952273!2d8.076955115302928!3d50.74644507387427!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x47bc186e9ba0a223%3A0x6a79ca7f91176277!2sAgentur%20f%C3%BCr%20Arbeit%20Burbach!5e0!3m2!1sen!2s!4v1582018821947!5m2!1sen!2s');
        }
    });
})

$(function () {
    $.ajax({
        type: "POST",
        traditional: true,
        async: false,
        cache: false,
        url: '/RestaurantInfo/GetOwnerDetails',
        context: document.body,
        success: function (result) {
            $('#OwnerDetails').html(result);
        },
        error: function (xhr) {
            $('#OwnerDetails').html('<p>Inayat Ullah Begum handelt im Namen von Ria Pizza Herrenberg</p>');
        }
    });
})
