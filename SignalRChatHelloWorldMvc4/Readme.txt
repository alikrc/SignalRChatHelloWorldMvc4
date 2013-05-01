1) Package Manager Console >
	Install-Package Microsoft.AspNet.SignalR

2) Global.asax > 
	Add the following code before registering any routes
	
	RouteTable.Routes.MapHubs();

	Example:

	public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RouteTable.Routes.MapHubs();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }

3) Add a new class to project - (HubClass.cs)
	
	Example:

	using System;
	using Microsoft.AspNet.SignalR;

	namespace SignalRChatHelloWorldMvc4
	{
	    public class HubClass : Hub
	    {
	        public void ServerSendMessageMethod(string message)
	        {
	            // adding time stamp to the message 
	            message += " | " + DateTime.Now.ToShortTimeString();
	
	            // this is a client function
	            Clients.All.clientReceiveMessageMethod(message);
	        }
	    }
	}

4) Add a new html page to the project - (index.html) or mvc view

@{
    Layout = null;
}

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
</head>
<body>
    <div>
        <input type="text" id="txtMsg" />
        <input type="button" id="btnBroadcast" value="send" />
        <ul id="listMessages"></ul>
    </div>

    <!-- reference script order is important | "/signalr/hubs" must come after signalR.js referenced-->
    <script src="//ajax.googleapis.com/ajax/libs/jquery/2.0.0/jquery.min.js"></script>
    <script src="/Scripts/jquery.signalR-1.0.1.js"></script>
    <script src="/signalr/hubs"></script>

    <!-- this script calls and receives messages -->
    <script>
        $(function () {
            // this is the proxy for connection hub
            var proxy = $.connection.hubClass;

            // this needs to be defined before calling from server. Server'll call this method
            proxy.client.clientReceiveMessageMethod = function (msg) {
                $("#listMessages").append("<li>" + msg + "</li>");
            }

            // start the connection and call the server method
            $.connection.hub.start().done(function () {

                // button binded for calling the server method
                $("#btnBroadcast").click(function () {

                    // call the server method and pass the txtBox's value as a parameter
                    proxy.server.serverSendMessageMethod($("#txtMsg").val());

                    // clear the txtbox
                    $("#txtMsg").val("");
                });
            })
        });
    </script>
</body>
</html>
