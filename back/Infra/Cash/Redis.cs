using StackExchange.Redis;
namespace Back.Infra.Cash;

public class Cash
{
    private ConnectionMultiplexer _connection { get; set; }
    private IDatabase _db { get; set; }

    public Cash(ConnectionMultiplexer connection, IDatabase db)
    {
        _connection = connection;
        _db = db;
    }

    public static Cash Connect(Settings settings)
    {
        var connection = ConnectionMultiplexer.Connect(settings.RedisHost);
        var db = connection.GetDatabase();
        return new Cash(connection, db);
    }

    public void Disconnect()
    {
        if (_connection != null)
            _connection.Close();

    }


    public async void Set(string key, string value)
    {
        if (_db == null) return;
        await _db.StringSetAsync(key, value);
    }

    public async Task<string?> Get(string key)
    {
        if (_db == null) return null;
        string? value = await _db.StringGetAsync(key);
        return value;
    }
}
