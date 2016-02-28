using System;
using System.Collections.Generic;
using System.Linq;
using SignalRConsole.Model;

namespace SignalRConsole.SignalR
{
    public class ConnectManage
    {
        public static ConnectManage Instance => Lazy.Value;

        private static readonly Lazy<ConnectManage> Lazy = new Lazy<ConnectManage>(() => new ConnectManage());
        private readonly List<Apps> _appses;
        private ConnectManage()
        {
            _appses = new List<Apps>();
        }

        public Apps GetAppsById(string appId)
        {
            return _appses.FirstOrDefault(x => x.AppId == appId);
        }

        public List<Apps> GetOnlineAppses()
        {
            return _appses.AsReadOnly().ToList();
        }

        public void AddApp(Apps app)
        {
            _appses.Add(app);
        }

        public bool IsContais(Apps app)
        {
            return _appses.Contains(app);
        }

        public bool IsContais(string appId)
        {
            return _appses.Any(x => x.AppId == appId);
        }

        public void RemoveApp(Apps app)
        {
            lock (Instance)
            {
                if (IsContais(app))
                {
                    _appses.Remove(app);
                }
            }
        }

        public void RemoveApp(string connectId)
        {
            lock (Instance)
            {
                var app = _appses.FirstOrDefault(x => x.ConnectId == connectId);
                if (app != null)
                {
                    _appses.Remove(app);

                }
            }
           
        }
    }
}