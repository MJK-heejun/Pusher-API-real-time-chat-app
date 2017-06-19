angular.module('myChat2').directive('loginPage', function() {

	var loginPageCtrl = function($scope, $rootScope){

		$scope.login = function(){			
			if($scope.username && $scope.password){
				if(($scope.username === "james" && $scope.password === "james123") ||
					($scope.username === "gary" && $scope.password === "gary123")){
					$rootScope.currentUser = $scope.username;
					$scope.$emit("loginEvent");
				}else{
					$rootScope.popup("error","wrong username and password");
				}
			}else{
				$rootScope.popup("error","please login with proper input");
			}			
		};


	};

	return {
		restrict: 'E',
		replace: true,
		scope: {
			isLoggedIn: "="
		},
		controller: loginPageCtrl,
		templateUrl: 'directive/loginPage/loginPage.html',
		link: function(scope, element, attrs, fn) {


		}
	};
});
