App.controller("DishCategoriesCtrl", function ($scope, $http, Upload, $timeout) {
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

    $scope.allCategories = [];
    $scope.addCategoryObj = { categoryName: '', description: "", orderBy: '', isAvailable: true };
    $scope.editCategoryObj = { dishCategoryId: 0, description: "", categoryName: '', orderBy: 0, isAvailable: true };
    $scope.category = new FormData();

    $scope.getAllCategories = function () {
        JsonCall("DishCategories", "GetAllCategories");
        if (list !== null) {
            $scope.allCategories = list;
        }
    }

    $scope.openEditModal = function (category) {
        $scope.editCategoryObj = {
            dishCategoryId: category.dishCategoryId,
            categoryName: category.categoryName,
            orderBy: category.orderBy,
            isAvailable: category.isAvailable,
            description: category.description
        };

        $('#ImageTabEdit').append('<img style="cursor:pointer" src="/Uploads/' + category.image + '" alt="' + category.categoryName + '" /> <i onclick="ClearFileEdit()" class="fas fa-window-close centerBtn"></i>');
        $('#selectImageBtnEdit').hide();

        $("#editModal").modal();
    }

    //image file upload
    $scope.addCategory = function () {
        var formdata = new FormData();
        var fileInput = document.getElementById('fileInput');
        formdata.append(fileInput.files[0].name, fileInput.files[0]);
        formdata.append("category", JSON.stringify($scope.addCategoryObj));
        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/DishCategories/Create');
        xhr.send(formdata);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                $("#addModal").modal("hide");
                swal("Added", "Dish Category Added!", "success");
                $scope.addCategoryObj = { categoryName: '', isAvailable: true };
                $scope.getAllCategories();
                location.reload();
            }
        };
        return false;
    };

    $scope.delDishCategory = function (cat) {
        swal({
            title: "Are you sure?",
            text: "Your Category will be deleted!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, delete it!",
            closeOnConfirm: false
        }, function () {
            JsonCallParam("DishCategories", "Delete", { id: cat });
            if (list === "Success") {
                swal("Deleted", "Dish Category Deleted!", "success");
                $scope.getAllCategories();
            }
            else {
                swal("Error", "Please Try again later!", "error");
            }
        });
    };

    $scope.editCategory = function () {
        var formdata = new FormData();
        var fileInput = document.getElementById('fileInputEdit');
        if (fileInput.files.length) {
            formdata.append(fileInput.files[0].name, fileInput.files[0]);
        }
        formdata.append("dishcategory", JSON.stringify($scope.editCategoryObj));
        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/DishCategories/Edit');
        xhr.send(formdata);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                $("#editModal").modal("hide");
                swal("Added", "Dish Category Edit!", "success");
                $scope.editCategoryObj = { dishCategoryId: 0, categoryName: '', isAvailable: true };
                location.reload();
            }
        };
        return false;
    };
});
