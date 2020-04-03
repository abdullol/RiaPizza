App.controller("Auth", ['$scope', '$rootScope', function ($scope, $rootScope) {
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

    $scope.registerUser = { fullName: "", firstName: "", lastName: "", address: "", city: "", email: "", userName: "", password: "", phoneNumber: "" };
    $scope.loginUser = { username: "", password: "" };

    $scope.usernameExists = false;
    $scope.isLoggedIn = false;
    $scope.user = {};
    $scope.userOrders = [];
    $scope.register = function () {
        JsonCallParam("Auth", "RegisterUser", { "registerDto": JSON.stringify($scope.registerUser) });
        if (list == "Success") {
            $('#signModal').modal('hide');
            $('#loginModal').modal();
        }
        else if (list == "UsernameExists") {
            $scope.usernameExists = true;
        }
    }
    $scope.login = function () {
        JsonCallParam("Auth", "LoginUser", { "loginDto": JSON.stringify($scope.loginUser) });
        if (list == "Success") {
            $('#loginModal').modal('hide');
            $scope.loadUser();
        }
        else
            $scope.loginUnsuccessful = true;
    }
    $scope.logout = function () {
        JsonCall("Auth", "LogoutUser");
        $scope.isLoggedIn = false;
        $scope.user = {};
    }
    $scope.loadUser = function () {
        JsonCall("Auth", "GetLoggedInUser");
        if (list != false) {
            $scope.isLoggedIn = true;
            $scope.user = list;
            $rootScope.$emit("LoadUserData", true);
        }
        else {
            $scope.isLoggedIn = false;
            $scope.user = {};
        }
    }
    $scope.routeToPersonalData = function () {
        if ($scope.isLoggedIn) {
            window.load
            JsonCall("MyAccount", "PersonalData");
        }
    }

    $scope.loadOrders = function () {
        if ($scope.isLoggedIn) {
            JsonCallParam("Orders", "GetUserOrders", { id: $scope.user.id });
            $scope.userOrders = list;
        }
        else {
            var orders = localStorage.getItem('orders');
            if (orders) {
                var ordersList = JSON.parse(localStorage.orders);
                JsonCallParam("Orders", "GetUserOrdersFromCodes", { "orderCodes": JSON.stringify(ordersList) });
                $scope.userOrders = list;
            } else $scope.userOrders = [];

        }
    }
}]);