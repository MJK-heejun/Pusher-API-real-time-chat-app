angular.module('myChat2').directive('rightPanel', function() {

	var rightPanelCtrl = function($scope, $rootScope, chatService, Pusher){

		$scope.currentChannel = null;
		$scope.messageList = []; //{username:, message }
		$scope.inputMessage = "";

		$scope.$watch('selectedChannel', function(){			

			if($scope.selectedChannel){
				if($scope.currentChannel){				
					Pusher.unsubscribe($scope.currentChannel); //unsubscribe from the old channel
					$scope.messageList = []; //empty message list				
				}

				//get previous message
				chatService.getOldMessage($scope.selectedChannel)
					.then(function(response) {
						$scope.messageList = response.data;		
						console.log("old messages");				
						console.log($scope.messageList);
					}, function(response) {
						$rootScope.popup("Error", response.data);
					});	

				$scope.currentChannel = $scope.selectedChannel; //reassign current channel

				var my_channel = Pusher.subscribe($scope.selectedChannel, 'message_event', function(data){
					$scope.messageList.push(data);
					$scope.$apply();
				});

			}
		});
			

		$scope.sendMessage = function(){
			console.log("send message started");
			console.log($scope.inputMessage);

			if($scope.inputMessage){
				var payload = {
					username: $rootScope.currentUser,
					channel: $scope.currentChannel,
					message: $scope.inputMessage
				};

				console.log("let's send message");
				chatService.sendMessage(payload)
					.then(function(response) {
						$scope.inputMessage = "";

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
		controller: rightPanelCtrl,
		templateUrl: 'directive/rightPanel/rightPanel.html',
		link: function(scope, element, attrs, fn) {


		}
	};
});
