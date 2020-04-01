'user strict';

var connection = new signalR.HubConnectionBuilder().withUrl("/notifyHub").withAutomaticReconnect([5000, 1500, 50000, null]).build();
connection.start();
connection.on("notifyOrder", function (order) {
    var orderStatus = "";
    var isPaymentConfirmed = "";

    if (order.orderStatus == "Pending")
        orderStatus = '<span class="label bg-yellow shadow-style">Pending</span>'
    else if (order.orderStatus == "Confirmed")
        orderStatus = '<span class="label bg-orange shadow-style">Confirmed</span>'
    else if (order.orderStatus == "Processing")
        orderStatus = '<span class="label bg-teal shadow-style">Processing</span>'
    else if (order.orderStatus == "Shipped")
        orderStatus = '<span class="label bg-cyan shadow-style">Shipped</span>'
    else if (order.orderStatus == "Delivered")
        orderStatus = '<span class="label bg-blue shadow-style">Delivered</span>'
    else if (order.orderStatus == "Cancelled")
        orderStatus = '<span class="label bg-red shadow-style">Cancelled</span>'

    if (order.isPaymentConfirmed == true)
        isPaymentConfirmed = '<span class="label bg-green shadow-style">Confirmed</span>'
    else
        isPaymentConfirmed = '<span class="label bg-red shadow-style">Pending</span>'

    var notsound = document.getElementById("NotSound");
    notsound.play();

    $.notify({
        message: 'You recieved an order: ' + order.orderCode
    },
    {
        type: 'bg-teal',
        allow_dismiss: true,
        newest_on_top: true,
        timer: 1000,
        placement: {
            from: "bottom",
            align: "right"
        },
        template: '<div data-notify="container" class="bootstrap-notify-container alert alert-dismissible {0} ' + (true ? "p-r-35" : "") + '" role="alert">' +
            '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
            '<span data-notify="message">{2}</span>'
    });

    $('.ordersList').prepend(
        '<tr> ' +
            '<td> ' + order.orderCode + '</td> ' +
            '<td>' + order.orderBy.name + '</td>' +
            '<td>' + orderStatus + '</td>' +
            '<td>' + order.recievingTime + '</td>' +
            '<td>' + order.subTotal + '</td>' +
            '<td>' + order.discount + '</td>' +
            '<td>' + order.totalBill + '</td>' +
            '<td>' + order.paymentMethod + isPaymentConfirmed + ' </td>' +
            '<td>' +
                '<a target = "_blank" class= "btn btn-warning" href = "~/Orders/Invoice/' + order.orderId + '" > <i class="fas fa-print"></i></a>' +
                '<button data-toggle="modal" data-target="#ChangeStatusModal" class="btn btn-success" onclick="setValue(' + order.orderId + ',`' + order.orderStatus + '`)"><i class="fas fa-toggle-on"></i></button>' +
                '<button type="button" class="btn btn-primary" onclick="showOrderDetails(' + order.orderId + ')"><i class="fas fa-info-circle"></button>' +
            '</td>' +
        '</tr> ');
});