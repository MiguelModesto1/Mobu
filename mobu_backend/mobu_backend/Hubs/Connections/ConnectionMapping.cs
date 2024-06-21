// https://learn.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/mapping-users-to-connections

namespace mobu_backend.Hubs.Connections
{
    /// <summary>
    /// Classe para guardar conexões signalR in-memory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> _connections =
            new Dictionary<T, HashSet<string>>();

        /// <summary>
        /// Método para contagem das conexões
        /// </summary>
        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        /// <summary>
        /// Método para adicionar conexões
        /// </summary>
        /// <param name="key">Chave identificadora</param>
        /// <param name="connectionId">ID de conexão</param>
        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        /// <summary>
        /// Método para obter conexões
        /// </summary>
        /// <param name="key">Chave identificadora</param>
        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Método para eliminar conexões
        /// </summary>
        /// <param name="key">Chave identificadora</param>
        /// <param name="connectionId">ID de conexão</param>
        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }
}
