App.controller("DeliveryAreasCtrl", function ($scope, $http) {
    function JsonCallParam(Controller, Action, Parameters) {
        $.ajax({
            type: "POST",
            traditional: true,
            async: false,
            cache: false,
            url: '/' + Controller + '/' + Action,
            context: document.body,
            data: Parameters,
            success: function (json) {
                list = null;
                list = json;
            }
            ,
            error: function (xhr) {
                list = null;
            }
        });
    }
    function JsonCall(Controller, Action) {
        $.ajax({
            type: "POST",
            traditional: true,
            async: false,
            cache: false,
            url: '/' + Controller + '/' + Action,
            context: document.body,
            success: function (json) {
                list = null; list = json;
            },
            error: function (xhr) {
                list = null;
                //debugger;
            }
        });
    }

    $scope.allAreas = [];
    $scope.addAreaObj = { areaName: '', postalCode: '', city: 'Herrenberg', status: true, isDeliveryAvailable: true, deliveryCharges: 0, minOrderCharges: 0 };
    $scope.editAreaObj = { deliveryAreaId: 0, areaName: '', postalCode: '', city: '', status: false, isDeliveryAvailable: false, deliveryCharges: 0, minOrderCharges: 0 };

    $scope.getAllAreas = function () {
        JsonCall("DeliveryAreas", "GetAllAreas");
        if (list !== null) {
            $scope.allAreas = list;
        }
    };
    //$scope.getAllAreas();

    $scope.addArea = function () {
        if ($scope.addAreaObj.areaName !== "" && $scope.addAreaObj.postalCode !== "" && $scope.addAreaObj.city !== "" && $scope.addAreaObj.deliveryCharges !== null && $scope.addAreaObj.minOrderCharges !== null) {
            var pram = { "deliveryArea": JSON.stringify($scope.addAreaObj) };
            JsonCallParam("DeliveryAreas", "Create", pram);
            if (list === "Success") {
                $("#addModal").modal('hide');
                swal("Added", "Delivery Area Added!", "success");
                $scope.addAreaObj = { areaName: '', postalCode: '', city: '', status: true, isDeliveryAvailable: true, deliveryCharges: 0, minOrderCharges: 0 };
                $scope.getAllAreas();
            } else if (list === "PostalCodeExists") {
                swal("Error", "Postal Code already exists", "error");
            }
        }
    };
    //edit
    $scope.editArea = function () {
        if ($scope.editAreaObj.areaName !== "" && $scope.editAreaObj.postalCode !== "" && $scope.editAreaObj.city !== "") {
            var pram = { "DeliveryArea": JSON.stringify($scope.editAreaObj) };
            JsonCallParam("DeliveryAreas", "Edit", pram);
            if (list === "Success") {
                $scope.getAllAreas();
                $("#editModal").modal("hide");
                $scope.editAreaObj = { areaName: '', postalCode: '', city: '', status: false, isDeliveryAvailable: false, deliveryCharges: 0, minOrderCharges: 0 };
                swal("Edited", "Delivery Area Edited!", "success");
            } else if (list == "PostalCodeExists") {
                swal("Error", "Please Try again later!", "error");
            }
        }
    };
    //delete
    
    $scope.delDeliveryArea = function (Id) {
        swal({
            title: "Are you sure?",
            text: "Your Delivery Area will be deleted!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, delete it!",
            closeOnConfirm: false
        }, function () {
                JsonCallParam("DeliveryAreas", "Delete", { id: Id });
                if (list === "Success") {
                    $scope.getAllAreas();
                swal("Deleted", "DeliveryArea Deleted!", "success");
            }
            else {
                swal("Error", "Please Try again later!", "error");
            }
        });

    };


    $scope.openEditModal = function (area) {
        $scope.editAreaObj = { deliveryAreaId: area.deliveryAreaId, areaName: area.areaName, postalCode: area.postalCode, city: area.city, status: area.status, isDeliveryAvailable: area.isDeliveryAvailable, deliveryCharges: area.deliveryCharges, minOrderCharges: area.minOrderCharges };
        $("#editModal").modal();
    };

});
