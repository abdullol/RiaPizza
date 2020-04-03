App.controller("ShopScheduleCtrl", function ($scope, $http, Upload, $timeout) {

    $scope.allCategories = [];
    $scope.addCategoryObj = { categoryName: '', orderBy: '', isAvailable: true };
    $scope.editCategoryObj = { dishCategoryId: 0, categoryName: '', orderBy: 0, isAvailable: true };
    $scope.category = new FormData();


    //image file upload
    $scope.ShopLogoUpdate = function () {
        var formdata = new FormData();
        var fileInput = document.getElementById('fileInput');
        formdata.append(fileInput.files[0].name, fileInput.files[0]);
        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/ShopSchedule/LogoUpdate');
        xhr.send(formdata);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                $("#addModal").modal("hide");
                swal("Added", "Shop Logo Updated!", "success");
                location.reload();
            }
        };
        return false;
    };

});
