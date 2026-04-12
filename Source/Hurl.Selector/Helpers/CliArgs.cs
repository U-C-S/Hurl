using Hurl.Library;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Hurl.Selector.Helpers;

public partial class CliArgs
{
    // CLI Options and Arguments
    private static readonly Option<bool> MinimizedOption = new("--minimized");
    private static readonly Option<string?> UriOption = new("--uri");
    private static readonly Argument<string[]> ValuesArgument = new("values")
    {
        Arity = ArgumentArity.ZeroOrMore
    };

    private static readonly RootCommand CliRoot = CreateRootCommand();

    // Properties to store parsed info
    public bool IsSecondInstance = false;
    public bool IsRunAsMin = false;
    public bool IsProtocolActivated = false;
    public string Url { get; set; } = string.Empty;

    private CliArgs(ParseResult parseResult, bool isSecondInstance)
    {
        IsRunAsMin = parseResult.GetValue(MinimizedOption);

        var values = new List<string>(parseResult.GetValue(ValuesArgument) ?? []);
        string? uriValue = parseResult.GetValue(UriOption);

        if (IsRunAsMin)
        {
            return;
        }

        if (isSecondInstance)
        {
            IsSecondInstance = true;
            if (values.Count >= 2)
            {
                values.RemoveAt(0);
            }
            else if (values.Count == 1 && values[0].Contains("Hurl", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
        }

        TrimExecutablePath(values);

        if (!string.IsNullOrWhiteSpace(uriValue))
        {
            AssignUrl(uriValue);
            return;
        }

        if (values.Count == 0)
        {
            return;
        }

        AssignUrl(values[0]);
    }

    private void AssignUrl(string value)
    {
        if (value.StartsWith("hurl://", StringComparison.OrdinalIgnoreCase))
        {
            IsProtocolActivated = true;
            Url = value[7..];
            return;
        }

        Url = value.Contains(' ') ? $"\"{value}\"" : value;
    }

    private static void TrimExecutablePath(List<string> values)
    {
        if (values.Count == 0)
        {
            return;
        }

        string currentExe = Environment.ProcessPath ?? string.Empty;
        if (string.Equals(values[0], currentExe, StringComparison.OrdinalIgnoreCase))
        {
            values.RemoveAt(0);
        }
    }

    public static CliArgs GatherInfo(AppActivationArguments activationArgs, bool isSecondInstance)
    {
        var rawCommandLine = GetActivationCommandLine(activationArgs);
        var parseResult = CliRoot.Parse(rawCommandLine);

#if DEBUG
        Task.Run(() =>
        {
            Directory.CreateDirectory(Constants.APP_SETTINGS_DIR);
            var argsStoreFile = Path.Combine(Constants.ROAMING, "Hurl", "args.txt");
            var activationKind = activationArgs.Kind.ToString();
            var rawArgs = parseResult.GetValue(ValuesArgument) ?? [];
            var strFormat = $"\n\n{isSecondInstance} --- {activationKind} --- {string.Join("__", rawArgs)}";
            File.AppendAllText(argsStoreFile, strFormat);
        });
#endif

        return new CliArgs(parseResult, isSecondInstance);
    }

    private static RootCommand CreateRootCommand()
    {
        RootCommand root = new()
        {
            TreatUnmatchedTokensAsErrors = false
        };

        root.Add(MinimizedOption);
        root.Add(UriOption);
        root.Add(ValuesArgument);

        return root;
    }

    private static string GetActivationCommandLine(AppActivationArguments activationArgs)
    {
        return activationArgs.Kind switch
        {
            ExtendedActivationKind.Protocol
                when activationArgs.Data is IProtocolActivatedEventArgs protocolArgs
                    => protocolArgs.Uri.AbsoluteUri,
            ExtendedActivationKind.CommandLineLaunch
                when activationArgs.Data is ICommandLineActivatedEventArgs commandLineArgs
                    => commandLineArgs.Operation.Arguments,
            ExtendedActivationKind.Launch
                when activationArgs.Data is ILaunchActivatedEventArgs launchArgs
                    => launchArgs.Arguments,
            _ => string.Empty
        };
    }
}
