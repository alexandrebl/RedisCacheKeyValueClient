using RedisCacheKeyValueClient.Manager.Interfaces;
using RedisCacheKeyValueClient.Utilities;
using RedisCacheKeyValueClient.Utilities.Interfaces;
using System;
using System.Threading.Tasks;

namespace RedisCacheKeyValueClient.Manager {

    /// <summary>
    /// Redis event Manager
    /// </summary>
    /// <typeparam name="T">object type</typeparam>
    public class RedisManager<T> : IRedisManager<T> {

        /// <summary>
        /// Observer manager
        /// </summary>
        private readonly ICacheUtility<T> _cacheManagerUtility;

        /// <summary>
        /// Database index
        /// </summary>
        private int _database;

        /// <summary>
        /// constructor method
        /// </summary>
        /// <param name="host">host and port</param>
        public RedisManager(string host) {
            //Cache initilization
            _cacheManagerUtility = new CacheUtility<T>(host);
            //Set receive event
            _cacheManagerUtility.OnReceiveMessage += ReceiveMessage;
            //Initialize
            _database = 0;
        }

        /// <summary>
        /// Set object and value
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">object</param>
        /// <returns>success flag</returns>
        public bool SetKey(string key, T obj) {
            //Set value
            var result = _cacheManagerUtility.SetKey(key, obj);

            return result;
        }

        /// <summary>
        /// Get object value
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>object</returns>
        public T GetValue(string key) {
            //Get value
            var obj = _cacheManagerUtility.GetValue(key);

            //Receive event
            Task.Factory.StartNew(() => {
                OnReceiveMessage(obj, _database);
            });

            return obj;
        }

        /// <summary>
        /// Message received
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="database">database index</param>
        private void OnReceiveMessage(T obj, int database) {
            //Event invoker
            ReceiveMessage?.Invoke(obj, database);
        }

        /// <summary>
        /// Reception event
        /// </summary>
        public event Action<T, int> ReceiveMessage;

        /// <summary>
        /// Define database to manage
        /// </summary>
        /// <param name="database"></param>
        /// <returns>success flag</returns>
        public bool SetDatabase(int database) {
            //Define database
            _database = database;

            return true;
        }
    }
}