using Files;
using Application;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

//VS
Directory.SetCurrentDirectory("./../../../");
try{
    await ConsoleApp.instance.Run();
    Console.ReadKey();
}
catch (Exception e) {
    System.Console.WriteLine("Oops, error occurred");
    System.Console.WriteLine("\t" + e.Message);
}

public class ConsoleApp
{
    public const string APP_NAME = "app";

    public static readonly ConsoleApp instance = new ConsoleApp();

    private Parser _parser;

    private CancellationTokenSource _source = new CancellationTokenSource();

    private App app;

    private ConsoleApp()
    {
        var builder = new CommandLineBuilder(AppRootCommand.root).UseDefaults();
        builder.AddMiddleware(async (context, next) => {
            try
            {
                await next(context);
            }
            catch (Exception e) 
            {
                Exit();
                System.Console.WriteLine("Oops, error occurred");
                System.Console.WriteLine("\t" + e.Message);
            }
        });
        _parser = builder.Build();
    }

    public void Exit() {
        _source.Cancel();
    }

    public async Task Run(){
        System.Console.WriteLine("App is running.");
        string p = string.Format("{0}: ", APP_NAME);

        while(!_source.IsCancellationRequested) {
            System.Console.Write(p);
            await _parser.InvokeAsync(Console.ReadLine()).ConfigureAwait(false);
        }
    }

    public void Stop()
    {
        app.Stop();
    }

    public void Reset()
    {
        app = new App(new DataShaper());
        app.Run();
    }

    public void Strat()
    {
        app ??= new App(new DataShaper());
        app.Run();
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
        ConsoleApp.instance.Stop();
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