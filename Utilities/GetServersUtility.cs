using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCacheKeyValueClient.Utilities
{
    internal static class GetServersUtility{
        public static List<IServer> GetServers(ConnectionMultiplexer connection) {

            var conn = connection;
   
                var servers = new List<IServer>();

                foreach (var endpoint in conn.GetEndPoints()) {
                    var server = conn.GetServer(endpoint);

                    servers.Add(server);
                }

                return servers;   
        }
    }
}
