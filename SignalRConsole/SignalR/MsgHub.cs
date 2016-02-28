using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRConsole.Model;

namespace SignalRConsole.SignalR
{
    [HubName("msgHub")]
    public class MsgHub : Hub
    {
        public void Login(string appId)
        {
            try
            {
                var app = new Apps()
                {
                    AppId = appId,
                    ConnectId = Context.ConnectionId,
                    //Token = token
                };
                var result = new ResultStatus<string>();

                if (!ConnectManage.Instance.IsContais(app.AppId))
                {
                    ConnectManage.Instance.AddApp(app);

                    result.ResultModel = "登录成功";
                    Console.WriteLine("app[{0}]:登录",app.AppId);
                }
                else
                {
                    result.ErrorMsg = "重复登录！";
                    result.IsError = true;
                }
                Clients.Caller.receiveMsg(result);


            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + ex.ToString());
            }

        }

        public void SendMsg(string appId, string msg)
        {
            try
            {
                var result = new ResultStatus<dynamic>();
                //验证权限，可根据实际业务在登录时进行验证
                var fromApp = ConnectManage.Instance.GetOnlineAppses().FirstOrDefault(x => x.ConnectId != Context.ConnectionId);
                if (fromApp==null)
                {
                    result.ErrorMsg = "请先登录";
                    result.IsError = true;

                    Clients.Caller.receiveMsg(result);
                    return;
                }

                var app = ConnectManage.Instance.GetAppsById(appId);
                if (app == null)
                {
                    result.ErrorMsg = "没有你要发送的app";
                    result.IsError = true;

                    Clients.Caller.receiveMsg(result);
                }
                else
                {
                    result.ResultModel = msg;
                    Clients.Client(app.ConnectId).receiveMsg(result);
                    Console.WriteLine("app[{0}]向app[{1}]发送消息[{2}],",fromApp.AppId,app.AppId,msg); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+":"+ex.ToString());
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            ConnectManage.Instance.RemoveApp(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}