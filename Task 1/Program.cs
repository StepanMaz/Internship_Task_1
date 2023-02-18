using Files;
using Application;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

await ConsoleApp.instance.Run(new CancellationToken());

public class ConsoleApp
{
    public const string APP_NAME = "-|-";

    public static readonly ConsoleApp instance = new ConsoleApp();

    private Parser _parser;

    private bool _run = true;

    private App app;

    private ConsoleApp()
    {
        var builder = new CommandLineBuilder(AppRootCommand.root).UseDefaults();

        var parser = builder.Build();
    }

    public void Exit() {
        _run = false;
    }

    public async Task Run(CancellationToken cancellationToken){
        string p = string.Format("{0}: ", APP_NAME);

        while(_run) {
            System.Console.Write(p);
            await _parser.InvokeAsync(Console.ReadLine()).ConfigureAwait(false);
        }
    }

    public void Strat()
    {
        app = new App();
    }
}

internal static class AppRootCommand
{
    public static readonly RootCommand root = new RootCommand("Data processing service");

    static AppRootCommand() {
        root.AddCommand(AppStartCommand.command);
        root.AddCommand(AppStopCommand.command);
        root.AddCommand(AppResetCommand.command);
    }
}

internal static class AppStartCommand
{
    public static readonly Command command = new Command("start", "Starts data processing service for payment transactions.");

    static AppStartCommand() {
        command.SetHandler(Handler);
    }

    public static void Handler() {
        ConsoleApp.instance.Strat();
    }
}

internal static class AppExitCommand
{
    public static readonly Command command = new Command("exit", "Exits the program and shuts it down.");

    static AppExitCommand() {
        command.SetHandler(Handler);
    }

    public static void Handler() {
        ConsoleApp.instance.Exit();
    }
}

internal static class AppStopCommand
{
    public static readonly Command command = new Command("stop", "Stops data processing service for payment transactions.");

    static AppStopCommand() {
        command.SetHandler(Handler);
    }

    public static void Handler() {

    }
}

internal static class AppResetCommand
{
    public static readonly Command command = new Command("reset", "Resets data processing service for payment transactions.");

    static AppResetCommand() {
        command.SetHandler(Handler);
    }

    public static void Handler() {

    }
}