using System;
using ClientCore;
using ClientCore.CnCNet5;
using ClientCore.Extensions;
using ClientGUI;
using DTAClient.Domain;
using DTAClient.Domain.Multiplayer;
using DTAClient.Online;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rampastring.Tools;

namespace DTAClient.DXGUI;

/// <summary>
/// The main class for the game. Sets up asset search paths
/// and initializes components.
/// </summary>
public class GameClass
{
    public GameClass()
    {
        Initialize();
    }

    protected void Initialize()
    {
        Logger.Log("Initializing GameClass.");

        string windowTitle = ClientConfiguration.Instance.WindowTitle;
        Console.Title = string.IsNullOrEmpty(windowTitle) ?
            string.Format("{0} Client", MainClientConstants.GAME_NAME_SHORT) : windowTitle;

        ProgramConstants.DisplayErrorAction = (title, error, exit) =>
        {
            Console.WriteLine(error);
            if (exit)
                Environment.Exit(1);
        };

        string playerName = UserINISettings.Instance.PlayerName.Value.Trim();

        if (UserINISettings.Instance.AutoRemoveUnderscoresFromName)
        {
            while (playerName.EndsWith("_"))
                playerName = playerName.Substring(0, playerName.Length - 1);
        }

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = Environment.UserName;

            playerName = playerName.Substring(playerName.IndexOf("\\") + 1);
        }

        playerName = NameValidator.GetValidOfflineName(playerName);

        ProgramConstants.PLAYERNAME = playerName;
        UserINISettings.Instance.PlayerName.Value = playerName;

        _ = BuildServiceProvider();
    }

    // TEMP: TODO, CLI menus
    public void Run()
    {
        GameProcessLogic.StartGameProcess();
    }

    private IServiceProvider BuildServiceProvider()
    {
        // Create host - this allows for things like DependencyInjection
        IHost host = Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
                {
                    // services (or service-like)
                    services
                        .AddSingleton<ServiceProvider>()
                        // .AddSingleton(GraphicsDevice)
                        .AddSingleton<GameCollection>()
                        // .AddSingleton<CnCNetUserData>()
                        // .AddSingleton<CnCNetManager>()
                        // .AddSingleton<TunnelHandler>()
                        .AddSingleton<DiscordHandler>()
                        // .AddSingleton<PrivateMessageHandler>()
                        .AddSingleton<MapLoader>();

                    // singleton xna controls - same instance on each request
                    // services.AddSingletonXnaControl<LoadingScreen>()

                    // transient xna controls - new instance on each request
                    // services.AddTransientXnaControl<XNAControl>();
                }
            )
            .Build();

        return host.Services.GetService<IServiceProvider>();
    }
}

/// <summary>
/// An exception that is thrown when initializing display / graphics mode fails.
/// </summary>
class GraphicsModeInitializationException : Exception
{
    public GraphicsModeInitializationException(string message) : base(message)
    {
    }
}