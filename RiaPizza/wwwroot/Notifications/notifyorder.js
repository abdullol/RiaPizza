'user strict';

var connection = new signalR.HubConnectionBuilder().withUrl("/notifyHub").withAutomaticReconnect([5000, 1500, 50000, null]).build();
connection.start();
connection.on("notifyOrder", function (order) {
    var notsound = document.getElementById("NotSound");
    notsound.play();

    swal({
        title: "New Order!",
        text: 'You recieved an order: ' + order.orderCode,
        imageUrl: "/assets/images/order.png"
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