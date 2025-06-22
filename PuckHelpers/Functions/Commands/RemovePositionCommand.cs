using PuckHelpers.Extensions;
using PuckHelpers.Functions.FaceOffPositions;

namespace PuckHelpers.Functions.Commands;

/// <summary>
/// Command that removes custom face-off positions.
/// </summary>
public class RemovePositionCommand
{
    private static bool HandleCommand(Player sender, ArraySegment<string> args, out string response)
    {
        if (!sender.IsPermitted())
        {
            response = "You do not have permission to use this command.";
            return false;
        }

        var name = args.At(0);

        if (!FaceOffPositionManager.Positions.TryGetValue(name, out var position))
        {
            response = $"Position '{name}' does not exist.";
            return false;
        }

        if (args.Count > 1)
        {
            var teamName = args.At(1);

            if (!Enum.TryParse<PlayerTeam>(teamName, true, out var team))
            {
                if (string.Equals(teamName, "B", StringComparison.InvariantCultureIgnoreCase))
                {
                    team = PlayerTeam.Blue;
                }
                else if (string.Equals(teamName, "R", StringComparison.InvariantCultureIgnoreCase))
                {
                    team = PlayerTeam.Red;
                }
                else
                {
                    response = "Invalid 'Team Name' argument, must be 'Blue (or B)' or 'Red (or R)'";
                    return false;
                }
            }

            var positions = team is PlayerTeam.Blue
                ? position.BluePositions
                : position.RedPositions;

            if (args.Count > 2)
            {
                var roleName = args.At(2);

                if (!Enum.TryParse<FaceOffPositionName>(roleName, true, out var role))
                {
                    if (string.Equals(roleName, "RW", StringComparison.InvariantCultureIgnoreCase))
                    {
                        role = FaceOffPositionName.RightWing;
                    }
                    else if (string.Equals(roleName, "RD", StringComparison.InvariantCultureIgnoreCase))
                    {
                        role = FaceOffPositionName.RightDefense;
                    }
                    else if (string.Equals(roleName, "LW", StringComparison.InvariantCultureIgnoreCase))
                    {
                        role = FaceOffPositionName.LeftWing;
                    }
                    else if (string.Equals(roleName, "LD", StringComparison.InvariantCultureIgnoreCase))
                    {
                        role = FaceOffPositionName.LeftDefense;
                    }
                    else if (string.Equals(roleName, "C", StringComparison.InvariantCultureIgnoreCase))
                    {
                        role = FaceOffPositionName.Center;
                    }
                    else if (string.Equals(roleName, "G", StringComparison.InvariantCultureIgnoreCase))
                    {
                        role = FaceOffPositionName.Goalie;
                    }
                    else if (string.Equals(roleName, "P", StringComparison.InvariantCultureIgnoreCase))
                    {
                        role = FaceOffPositionName.Puck;
                    }
                    else
                    {
                        response =
                            "Invalid 'Role Name' argument! Supported are: RightWing / RW, RightDefense / RD, LeftWing / LW, LeftDefense / LD, Center / C, Goalie / G";
                        return false;
                    }
                }

                if (role is FaceOffPositionName.Puck)
                {
                    position.HasPuckPosition = false;

                    position.PuckPosition.X = 0f;
                    position.PuckPosition.Y = 0f;
                    position.PuckPosition.Z = 0f;

                    response = $"Removed puck position in '{position.Name}'";
                }
                else
                {
                    positions.Remove(role);

                    response = $"Removed role position '{role}' in '{position.Name}'";
                }

                FaceOffPositionManager.SavePositions();
                return true;
            }

            positions.Clear();
            
            FaceOffPositionManager.SavePositions();

            response = $"Removed all positions for team '{team}' in '{position.Name}'";
            return true;
        }

        FaceOffPositionManager.Positions.Remove(position.Name);
        FaceOffPositionManager.SavePositions();

        response = $"Removed position '{name}'";
        return true;
    }
    
    internal static void RegisterCommand()
    {
        CustomCommands.RegisterCommand("removepos", HandleCommand);
        CustomCommands.RegisterCommand("rpos", HandleCommand);
    }
}