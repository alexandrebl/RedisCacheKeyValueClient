using StackExchange.Redis;
using System;

namespace RedisCacheKeyValueClient.Utilities.Interfaces {

    /// <summary>
    /// Utility event manager
    /// </summary>
    /// <typeparam name="T">type</typeparam>
    internal interface ICacheUtility<T> {

        /// <summary>
        /// Receive event
        /// </summary>
        event Action<T, int> OnReceiveMessage;

        /// <summary>
        /// Send message event
        /// </summary>
        event Action<T> OnSendMessage;

        /// <summary>
        /// Error event
        /// </summary>
        event Action<Exception> OnErrorMessage;

        /// <summary>
        /// Information event
        /// </summary>
        event Action<string> OnInfoMessage;

        /// <summary>
        /// Connection event
        /// </summary>
        event Action<string, ConnectionFailedEventArgs> OnConnectionMessage;

        /// <summary>
        /// flag that indicate is connected
        /// </summary>
        /// <returns>success flag</returns>
        bool IsConnected();

        /// <summary>
        /// Set object and value
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="obj">object</param>
        /// <returns>success flag</returns>
        bool SetKey(string key, T obj);

        /// <summary>
        /// Get object value
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>object</returns>
        T GetValue(string key);

        /// <summary>
        /// Define database to manage
        /// </summary>
        /// <param name="database"></param>
        /// <returns>success flag</returns>
        bool SetDatabase(int database);
    }
}