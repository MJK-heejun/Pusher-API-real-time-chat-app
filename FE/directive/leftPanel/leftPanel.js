angular.module('myChat2').directive('leftPanel', function() {

	var leftPanelCtrl = function($scope,$rootScope, chatService){

		chatService.getChannelList()
            .then(function(response) {
				$scope.channelList = response.data;
            }, function(response) {
                $rootScope.popup("Error", response.data);
            });



        $scope.selectChannel = function(channel){
            $scope.selectedChannel = channel;
        };

        $scope.addChannel = function(){
			if($scope.newChannelName){
				chatService.addChannel($scope.newChannelName)
					.then(function(response) {
							$scope.channelList.push($scope.newChannelName);
							$scope.newChannelName = "";
						}, function(response) {
							$rootScope.popup("Error", response.data);
					});
			}
        };



	};

	return {
		restrict: 'E',
		replace: true,
		scope: {
			selectedChannel: "="
		},
		controller: leftPanelCtrl,
		templateUrl: 'directive/leftPanel/leftPanel.html',
		link: function(scope, element, attrs, fn) {


		}
	};
});
