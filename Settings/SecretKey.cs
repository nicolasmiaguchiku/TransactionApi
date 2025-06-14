using DotNetEnv;

namespace TransactionsApi.Settings
{
    public static class SecretKey
    {
        public static string Key { get; }
        static SecretKey()
        {
            Env.Load();
            Key = Environment.GetEnvironmentVariable("SECRET_KEY") ?? string.Empty;
        }
    }
}
