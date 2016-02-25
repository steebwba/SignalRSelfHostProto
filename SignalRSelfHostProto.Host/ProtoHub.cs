using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SignalRSelfHostProto
{
    public class ProtoHub : Hub
    {
        public static ConcurrentDictionary<string, Guid> _connections = new ConcurrentDictionary<string, Guid>();
        public override Task OnConnected() => Task.Run(() =>
                                                        {
                                                            _connections.TryAdd(Context.ConnectionId, Guid.NewGuid());
                                                          
                                                            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ProtoHub>();
                                                            context.Clients.All.newUserConnected("A new connection has been established");
                                                        })
                                                    .ContinueWith((task) => base.OnConnected());

        public void Ping()
        {
            Guid userId;
            _connections.TryGetValue(Context.ConnectionId, out userId);
            _connections.TryAdd(Context.ConnectionId, userId);
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ProtoHub>();
            context.Clients.All.broadcast($"Ping from {userId}!");
        }

    }
}
