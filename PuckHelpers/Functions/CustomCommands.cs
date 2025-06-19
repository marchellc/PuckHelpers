using PuckHelpers.Core;
using PuckHelpers.Functions.Commands;

namespace PuckHelpers.Functions;

/// <summary>
/// Delegate used to handle custom commands.
/// </summary>
public delegate bool CommandHandler(Player sender, ArraySegment<string> args, out string response);

/// <summary>
/// Provides custom commands.
/// </summary>
public static class CustomCommands
{
    /// <summary>
    /// Gets all registered custom commands.
    /// </summary>
    public static Dictionary<string, CommandHandler> Commands { get; } = new();

    /// <summary>
    /// Registers a custom command.
    /// </summary>
    /// <param name="name">The command's name.</param>
    /// <param name="handler">The command's handler.</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static void RegisterCommand(string name, CommandHandler handler)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Command name cannot be empty", nameof(name));
        
        if (handler is null)
            throw new ArgumentNullException(nameof(handler));

        if (!Commands.ContainsKey(name))
        {
            Commands[name.ToLower()] = handler;
            
            HelpersPlugin.LogInfo("PuckHelpers / CustomCommands", $"Registered command: {name} ({handler.Method.DeclaringType.FullName}::{handler.Method})!");
        }
    }

    /// <summary>
    /// Attempts to invoke a command.
    /// </summary>
    /// <param name="command">The command name to invoke.</param>
    /// <param name="sender">The command's sender.</param>
    /// <param name="args">The command's arguments.</param>
    /// <param name="response">The command handler's response.</param>
    /// <returns>true if the command was executed</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static bool TryInvokeCommand(string command, Player sender, ArraySegment<string> args, out string response)
    {
        if (sender == null)
            throw new ArgumentNullException(nameof(sender));
        
        if (command?.Length < 1)
            throw new ArgumentException("Command cannot be empty", nameof(command));
        
        while (command.Length > 0 && command[0] == '/')
            command = command.Substring(1);

        if (command.Length == 0)
        {
            response = null;
            return false;
        }

        if (!Commands.TryGetValue(command.ToLower(), out var handler))
        {
            response = null;
            return false;
        }

        try
        {
            return handler(sender, args, out response);
        }
        catch (Exception ex)
        {
            response = ex.Message;
            return true;
        }
    }

    internal static void RegisterCommands()
    {
        GetPositionCommand.RegisterCommand();
        SetFaceOffPositionCommand.RegisterCommand();
        StartFaceOffCommand.RegisterCommand();
        RemovePositionCommand.RegisterCommand();
        ListPositionsCommand.RegisterCommand();
    }
}