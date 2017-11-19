var app = angular.module("SmartRentalApp", []);

app.controller('CreateRealEstate', ['$scope', function ($scope) {
    $scope.$watch('selectedType', function (selectedValue) {
        if (selectedValue in { '1': '', '2': '' }) {
            $scope.residential = true;
            $('#roomsNo').prop('required', true);
            $('#bathsNo').prop('required', true);
            $('#levelNo').prop('required', true);
            $('#houseNo').prop('required', true);
        }
        else {
            $('#roomsNo').removeAttr('required');
            $('#bathsNo').removeAttr('required');
            $('#levelNo').removeAttr('required');
            $('#houseNo').removeAttr('required');
            $scope.residential = false;
        }
    });
}]);