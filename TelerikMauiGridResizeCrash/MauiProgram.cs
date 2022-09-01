using Telerik.Maui.Controls.Compatibility;
using ServiceProvider = TelerikMauiGridResizeCrash.Helpers.ServiceProvider;

namespace TelerikMauiGridResizeCrash;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseTelerik()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        ServiceProvider.Init(new ServiceProviderInitializer().GetInitializer());
        var settingsService = (ISettingsService)ServiceProvider.Instance.GetService(typeof(ISettingsService));

        return builder.Build();
	}
}

public class ServiceProviderInitializer
{
    public Func<IServiceProvider> GetInitializer() => () => Init();

    private IServiceProvider Init()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        return serviceCollection.BuildServiceProvider();
    }

    private IServiceCollection ConfigureServices(IServiceCollection services)
    {
        var settingsService = new SettingsService();
        settingsService.PauseUpdatesOnRearrange = RearrangeUpdateBehavior.DoNothing;
        return services;
    }
}
