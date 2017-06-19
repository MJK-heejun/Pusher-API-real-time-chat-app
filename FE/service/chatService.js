angular.module('myChat2')
    .factory('chatService', ['$http', '$rootScope', '$q', function($http, $rootScope, $q) {

        var chatService = {};

        //var rootUrl = "http://localhost:61968";
        var rootUrl = "http://mychat2.azurewebsites.net";

        chatService.getChannelList = function(){
            var url = rootUrl + '/main/channel';
            return $http.get(url);
        };

        chatService.addChannel = function(name){
            var url = rootUrl + '/main/channel/'+name;
            return $http.post(url, {});
        };

        chatService.getOldMessage = function(channelName){
            var url = rootUrl + '/main/message/channel/'+channelName;
            return $http.get(url);
        };

        chatService.sendMessage = function(data){
            var url = rootUrl + '/main/message';
            return $http.post(url, data);
        };
        
        return chatService;
    }]);
