using StackExchange.Redis;

namespace CPR2.Data.Redis;

public interface IPublisherConfirmsRepository
{
    Task SetKeyValue(string key, byte[] value);
    Task<byte[]> GetValueByKeyAndThenDelete(string key);
    Task<byte[]> GetValueByKey(string key);
}

public class PublisherConfirmsRepository : IPublisherConfirmsRepository
{
    private readonly ConnectionMultiplexer _redisConnectionMultiplexer;

    public PublisherConfirmsRepository(ConnectionMultiplexer redisConnectionMultiplexer)
    {
        _redisConnectionMultiplexer = redisConnectionMultiplexer;
    }

    public async Task SetKeyValue(string key, byte[] value)
    {
        IDatabase redisDb = _redisConnectionMultiplexer.GetDatabase();
        await redisDb.StringSetAsync(key, value);
    }

    public async Task<byte[]> GetValueByKeyAndThenDelete(string key)
    {
        IDatabase redisDb = _redisConnectionMultiplexer.GetDatabase();
        byte[] value = await redisDb.StringGetAsync(key);
        await redisDb.KeyDeleteAsync(key);
        return value;
    }

    public async Task<byte[]> GetValueByKey(string key)
    {
        IDatabase redisDb = _redisConnectionMultiplexer.GetDatabase();
        return await redisDb.StringGetAsync(key);
    }
}