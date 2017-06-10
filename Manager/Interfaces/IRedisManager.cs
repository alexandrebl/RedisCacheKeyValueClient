namespace RedisCacheKeyValueClient.Manager.Interfaces {

    /// <summary>
    /// Redis event Manager
    /// </summary>
    /// <typeparam name="T">object type</typeparam>
    public interface IRedisManager<T> {

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