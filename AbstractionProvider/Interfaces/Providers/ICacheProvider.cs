namespace AbstractionProvider.Interfaces.Providers
{
    public interface ICacheProvider
    {
        void Add<T>(T objInfo, string key, int experationTimeInSeconds = 0);

        void Clear(string key);

        T Get<T>(string key) where T : class;
    }
}