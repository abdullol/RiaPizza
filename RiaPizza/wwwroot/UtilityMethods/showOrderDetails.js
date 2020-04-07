function showOrderDetails(Id) {

    $.ajax({
        type: "POST",
        url: '/Orders/GetOrderDetails?id=' + Id,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            $('#orderDetailsModal').empty();
            $('#orderDetailsModal').html(response);
            $('.orderDetails-modal').modal();
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });

}