class ConfigurationManagerBuilder
{
    public static IConfiguration AppSetting
    {
        get;
    }
    static ConfigurationManagerBuilder()
    {
        //Để lấy cái config ở appsetting.json
        AppSetting = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
    }
}