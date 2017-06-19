angular.module('myChat2').directive('mainPanel', function() {

	var mainPanelCtrl = function($scope, $rootScope){
		$scope.selectedChannel = null;
	};

	return {
		restrict: 'E',
		replace: true,
		scope: {

		},
		controller: mainPanelCtrl,
		templateUrl: 'directive/mainPanel/mainPanel.html',
		link: function(scope, element, attrs, fn) {


		}
	};
});
