App.controller("DishesCtrl", function ($scope, $http) {

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

    $scope.allDishes = [];
    $scope.viewDishExtraType = [];
    $scope.viewDishExtras = [];
    $scope.dishCategories = [];

    //Add Dish Objects
    $scope.addDish = { dishName: '', subName: '', description: '', dishCategoryId: 0, basePrice: 0, allergies: '' };
    $scope.addDishExtraTypes = [];
    $scope.getDishCategories = function () {
        JsonCall("DishCategories", "GetAllCategories");
        if (list !== null) {
            $scope.dishCategories = list;
            $scope.addDish.dishCategoryId = 0;
        }
    }

    //Start Dish Only Function
    $scope.saveDish = function () {
        if ($scope.addDish.dishName === "" || Number($scope.addDish.dishCategoryId) === 0 || $scope.addDish.basePrice === 0) { return; }
        for (var i = 0; i < $scope.addDishExtraTypes.length; i++) {
            if ($scope.addDishExtraTypes[i].typeName === "") { return; }
            for (var j = 0; j < $scope.addDishExtraTypes[i].dishExtras.length; j++)
            { if ($scope.addDishExtraTypes[i].dishExtras[j].extraName === "") { return; } }
        }

        var pram = { "dish": JSON.stringify($scope.addDish), "dishExtraTypes": JSON.stringify($scope.addDishExtraTypes) };
        JsonCallParam("Dishes", "CreateDish", pram);
        if (list === "Success") {
            $("#addModal").modal("hide");
            swal("Added", "Dish Added!", "success");

            $scope.addDish = { dishName: '', subName: '', description: '', dishCategoryId: 0, basePrice: 0 };
            $scope.addDishExtraTypes = [];
        }
    }
    $scope.saveDishExtraType = function () {
        for (var i = 0; i < $scope.addDishExtraTypes.length; i++) {
            if ($scope.addDishExtraTypes[i].typeName === "") { return; }
            for (var j = 0; j < $scope.addDishExtraTypes[i].dishExtras.length; j++) { if ($scope.addDishExtraTypes[i].dishExtras[j].extraName === "") { return; } }
        }

        var pram = { "dishId": JSON.stringify(document.getElementById('dishId').value), "dishExtraTypes": JSON.stringify($scope.addDishExtraTypes) };
        JsonCallParam("Dishes", "AddDishExtras", pram);
        if (list === "Success") {
            $("#addDishExtra").modal("hide");
            swal("Added", "Dish Topping Added!", "success");

            $scope.addDishExtraTypes = [];
            location.reload();
        }
    }
    $scope.pushDishExtraType = function () {
        var length = $scope.addDishExtraTypes.length;
        if (length === 0) {
            $scope.addDishExtraTypes.push({ typeName: '', chooseMultiple: true, dishExtras: [{ extraName: '', extraPrice: 0 }] });
        }
        else if ($scope.addDishExtraTypes[length - 1].typeName !== '') {
            $scope.addDishExtraTypes.push({ typeName: '', chooseMultiple: true, dishExtras: [{ extraName: '', extraPrice: 0 }] });
        }
    }
    $scope.pushDishExtra = function (i) {
        var length = $scope.addDishExtraTypes[i].dishExtras.length;
        if (length === 0) {
            $scope.addDishExtraTypes[i].dishExtras.push({ extraName: '', extraPrice: 0 });
        }
        else if ($scope.addDishExtraTypes[i].dishExtras[length - 1].extraName !== "") {
            $scope.addDishExtraTypes[i].dishExtras.push({ extraName: '', extraPrice: 0 });
        }
    }
    $scope.removeExtraType = function (i) {
        $scope.addDishExtraTypes.splice(i, 1);
    }
    $scope.removeExtra = function (extratype, i) {
        $scope.addDishExtraTypes[extratype].dishExtras.splice(i, 1);
    }
    //End Dish Only Function
    
    //Start UI Functions
    $scope.openDishEditModal = function (dish) {
        $scope.editDish = { dishId: dish.dishId, dishName: dish.dishName, subName: dish.subName, allergies: dish.allergies, description: dish.description, dishCategoryId: dish.dishCategoryId, basePrice: dish.basePrice };
        $("#editDishModal").modal();
    }
    $scope.openDishExtraTypeEditModal = function (dishExtraType) {
        $scope.editDishExtraType = { dishExtraTypeId: dishExtraType.dishExtraTypeId, typeName: dishExtraType.typeName, dishId: dishExtraType.dishId, chooseMultiple: dishExtraType.chooseMultiple };
        $("#editDishModal").modal();
    }
    $scope.openDishExtraEditModal = function (dishextra) {
        $scope.editDishExtra = { dishExtraId: dishextra.dishExtraId, extraName: dishextra.extraName, dishExtraTypeId: dishextra.dishExtraTypeId, extraPrice: dishextra.extraPrice };
        $("#editDishExtraModal").modal();
    }
    //End UI Functions
    $scope.delDishCategory = function (cat) {
        $scope.delDishCategoryObj = { dishCategoryId: cat.dishCategoryId };
        var pram = { "dishCategory": JSON.stringify($scope.delDishCategoryObj) };
        JsonCallParam("DishCategories", "Delete", pram);
        if (list == "Success") {
            $scope.getAllCategories();
            swal("Removed", "Dish Category Deleted!", "success");
        }
    }
});
