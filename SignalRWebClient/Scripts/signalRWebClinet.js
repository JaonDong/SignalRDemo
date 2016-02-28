(function ($, window, document, undefined) {
    //定义构造函数
    var clientObject = function (opt) {
        this.connectionClient = {},
            this.defaults = {
                'url': 'http://localhost:8088/signalr',
                'appId': '1',
                'error': function(msg) { console.log(msg); },
                'success': function(data) { console.log(data); }

    },
            this.option = $.extend({}, this.defaults, opt);
    }
    clientObject.prototype = {
        connect: function() {
            $.connection.hub.url = this.option.url;
            this.connectionClient = $.connection.msgHub;
            console.log("创建连接," + this.connectionClient);

            var errorMethod = this.option.error;
            var successMethod = this.option.success;
            this.connectionClient.client.receiveMsg = function (data) {
                if (data.IsError) {
                    errorMethod(data.ErrorMsg);
                } else {
                    //alert(data.ResultModel);
                    successMethod(data.ResultModel);
                }
            }
        },
        sendMsg: function(appId, msg) {
            this.connectionClient.server.sendMsg(appId, msg);
        },
        login: function() {
            this.connectionClient.server.login(this.option.appId);
        }
    }

    $.signalRClient = function (opt) {
        var signalObject = new clientObject(opt);
        return signalObject;
    };

})(jQuery, window, document);