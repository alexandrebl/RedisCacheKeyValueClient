using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCacheKeyValueClient.Utilities
{
    /// <summary>
    /// Get all server based in all connection endpoints.
    /// </summary>
    /// <param name="connection"></param>
    /// <returns>List containing founded servers</returns>
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
