class ConfigurationManagerBuilder
{
    public static IConfiguration AppSetting
    {
        get;
    }
    static ConfigurationManagerBuilder()
    {
        AppSetting = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
    }
}