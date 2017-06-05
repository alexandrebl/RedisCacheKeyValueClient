using System;

namespace RedisCacheKeyValueClient {

    /// <summary>
    /// Redis event Manager
    /// </summary>
    /// <typeparam name="T">object type</typeparam>
    public class RedisManager<T> : IRedisManager<T> {

        /// <summary>
        /// constructor method
        /// </summary>
        /// <param name="host">host and port</param>
        public RedisManager(string host) {
        
        }
    }
}