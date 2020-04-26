'user strict';

var connection = new signalR.HubConnectionBuilder().withUrl("/notifyHub").withAutomaticReconnect([5000, 1500, 50000, null]).build();
connection.start();
connection.on("notifyOrder", function (order) {
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
            '<td> <strong class="font-bold">Ordered at: </strong>' + order.orderDateTime + '<strong class="font-bold"> Recieving: </strong>' + order.recievingTime + '</td>' +
            '<td>' + order.postalCode + '<strong class="font-bold"> | </strong>' + order.orderDeliveryAddress.city + '</td>' +
            '<td>' + order.orderDeliveryAddress.address + '</td>' +
            '<td>' + order.totalBill + '</td>' +
            '<td>' + order.orderBy.contact + ' </td>' +
            '<td>' +
                '<a target = "_blank" class= "btn tblActnBtn" href = "~/Orders/Invoice/' + order.orderId + '" > <i class="fas fa-print pt-2"></i> </a>' +
                '<button class="btn tblActnBtn" data-toggle="modal" data-target="#ChangeStatusModal" onclick="setValue(' + order.orderId + ',`' + order.orderStatus + '`)"><i class="fas fa-toggle-on"></i></button>' +
                '<button class="btn tblActnBtn" onclick="showOrderDetails(' + order.orderId + ')"><i class="fas fa-info-circle"></button>' +
            '</td>' +
        '</tr> ');
});