var app = angular.module("SmartRentalApp", []);

app.controller('CreateRealEstate', ['$scope', function ($scope) {
    $scope.$watch('selectedType', function (selectedValue) {
        if (selectedValue in { '1': '', '2': '' })
            $scope.residential = true;
        else
            $scope.residential = false;
    });
}]);