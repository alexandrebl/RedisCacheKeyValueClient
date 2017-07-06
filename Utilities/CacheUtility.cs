using RedisCacheKeyValueClient.Library;
using RedisCacheKeyValueClient.Utilities.Interfaces;
using StackExchange.Redis;
using StackExchange.Redis.Extender;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedisCacheKeyValueClient.Utilities {

    /// <summary>
    /// Utility event manager
    /// </summary>
    /// <typeparam name="T">type</typeparam>
    internal class CacheUtility<T> : ICacheUtility<T> {

        /// <summary>
        /// Connection
        /// </summary>
        private readonly ConnectionMultiplexer _connection;

        /// <summary>
        /// 
        /// </summary>
        private readonly StackExchangeRedisCacheClient _client;

        /// <summary>
        /// Define database to manage data
        /// </summary>
        private int _database;

        /// <summary>
        /// Receive event
        /// </summary>
        public event Action<T, int> OnReceiveMessage;

        /// <summary>
        /// Send message event
        /// </summary>
        public event Action<T> OnSendMessage;

        /// <summary>
        /// Error event
        /// </summary>
        public event Action<Exception> OnErrorMessage;

        /// <summary>
        /// Information event
        /// </summary>
        public event Action<string> OnInfoMessage;

        /// <summary>
        /// Connection event
        /// </summary>
        public event Action<string, ConnectionFailedEventArgs> OnConnectionMessage;

        /// <summary>
        /// Sync object
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object SyncObj = new object();

        /// <summary>
        /// Constructor method
        /// </summary>
        /// <param name="host">server conection string</param>
        /// <param name="database">database index</param>
        public CacheUtility(string host, int database = 0) {
            //Define connection
            ConnectionManager.SetConnectionString(host);
            //Open connection
            _connection = ConnectionManager.OpenConnection(true);

            //Connection fail
            ConnectionManager.OnFailed += delegate (ConnectionFailedEventArgs args) {
                //Invoke event
                OnConnectionHandle("Connection error", args);
            };

            //Connection Restored
            ConnectionManager.OnRestored += delegate (ConnectionFailedEventArgs args) {
                //Invoke event
                OnConnectionHandle("Connection restored", args);
            };

            //Error event
            ConnectionManager.OnError += delegate (Exception exception) {
                //Invoke event
                OnErrorMessageHandle(new Exception($"Connection error. Exception: {exception}"));
            };

            //Set database
            _database = database;

            //Information event
            ConnectionManager.OnInfo += OnInfoMessageHandle;
        }

        /// <summary>
        /// flag that indicate is connected
        /// </summary>
        /// <returns>success flag</returns>
        public bool IsConnected() {
            //Sync context
            lock (SyncObj) {
                //Return flag data
                return _connection?.IsConnected ?? false;
            }
        }

        /// <summary>
        /// Receive messaage
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="database">database index</param>
        protected virtual void OnReceiveMessageHandle(T obj, int database) {
            //Invoke event
            OnReceiveMessage?.Invoke(obj, database);
        }

        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="obj">object</param>
        protected virtual void OnSendMessageHadle(T obj) {
            //Invoke event
            OnSendMessage?.Invoke(obj);
        }

        /// <summary>
        /// Emensagem de erro
        /// </summary>
        /// <param name="ex">erro</param>
        protected virtual void OnErrorMessageHandle(Exception ex) {
            //Invoke event
            OnErrorMessage?.Invoke(ex);
        }

        /// <summary>
        /// Information message
        /// </summary>
        /// <param name="message">mensagem</param>
        protected virtual void OnInfoMessageHandle(string message) {
            //Invoke event
            OnInfoMessage?.Invoke(message);
        }

        /// <summary>
        /// Connection event
        /// </summary>
        /// <param name="mensagem">message</param>
        /// <param name="eventArgs">event arguments</param>
        protected virtual void OnConnectionHandle(string mensagem, ConnectionFailedEventArgs eventArgs) {
            //Invoke event
            OnConnectionMessage?.Invoke(mensagem, eventArgs);
        }

        /// <summary>
        /// Set object and value
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">object</param>
        /// <returns>success flag</returns>
        public bool SetKey(string key, T obj) {
            //Flag
            if (!IsConnected()) {
                //Invoke event
                OnErrorMessageHandle(new Exception("Connection is not ready to set key"));
                return false;
            }

            //Obtem o objeto
            var result = _connection.GetDatabase(_database).Set(key, obj);

            //Send event
            OnSendMessageHadle(obj);

            //Return
            return result;
        }

        /// <summary>
        /// Get object value
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>object</returns>
        public T GetValue(string key) {
            //Flag
            if (!IsConnected()) {
                //Invoke event
                OnErrorMessageHandle(new Exception("Connection is not ready to get value"));
                return default(T);
            }

            //Obtem o objeto
            var obj = _connection.GetDatabase(_database).Get<T>(key);

            //Receive event
            OnReceiveMessageHandle(obj, _database);

            //Return
            return obj;
        }

        /// <summary>
        /// Define database to manage
        /// </summary>
        /// <param name="database"></param>
        /// <returns>success flag</returns>
        public bool SetDatabase(int database) {
            //Flag
            if (!IsConnected()) {
                //Invoke event
                OnErrorMessageHandle(new Exception("Connection is not ready to set database"));
                //Return
                return false;
            }

            //Sync data region
            lock (SyncObj) {
                //Set database
                _database = database;
            }

            //Return
            return true;
        }

        /// <summary>
        /// Search for key in databases using pattern.
        /// </summary>
        /// <param name="patterns"></param>
        /// <returns>List containing founded keys</returns>
        public List<RedisKey> SearchKeys(params string[] patterns) {

            if (!IsConnected()) {
                OnErrorMessageHandle(new Exception("Connection is not ready to search keys"));
            }

            // Get all servers in connection.
            var servers = GetServersUtility.GetServers(_connection);

            if (!servers.Any()) {
                OnErrorMessageHandle(new Exception("Could not locate any server in connection"));
            }

            var keysList = new List<RedisKey>();

            // Foreach server in the list of founded servers, it will search for a key in database based in param patterns.
            foreach (var server in servers) {
                foreach (var pattern in patterns) {
                    var keys = server.Keys(_database, pattern);

                    // Add founded keys if it does not already exist in the list.
                    foreach (var key in keys) {
                        if (!keysList.Contains(key)) {
                            keysList.Add(key);
                        }
                    }
                }
            }

            return keysList;
        }
    }
}