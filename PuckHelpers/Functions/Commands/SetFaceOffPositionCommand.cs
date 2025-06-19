using PuckHelpers.Configs;
using PuckHelpers.Extensions;
using PuckHelpers.Functions.FaceOffPositions;

namespace PuckHelpers.Functions.Commands;

/// <summary>
/// Commands to set face-off positions.
/// </summary>
public static class SetFaceOffPositionCommand
{
    private static bool HandleCommand(Player sender, ArraySegment<string> args, out string response)
    {
        if (!sender.IsPermitted())
        {
            response = "You do not have the required permissions to invoke this command.";
            return true;
        }

        if (args.Count < 2)
        {
            response =
                "Missing arguments! Usage: /setpos <Custom Position Name> <Team Name (Blue / Red)> <Role Position Name (RightWing, LeftWing etc)>";
            return true;
        }

        var positionName = args.At(0);
        var teamName = args.At(1);
        var roleName = args.At(2);

        if (!FaceOffPositionManager.Positions.TryGetValue(positionName, out var position))
            FaceOffPositionManager.Positions.Add(positionName, position = new() { Name = positionName });

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
                return true;
            }
        }

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
                return true;
            }
        }

        if (role is FaceOffPositionName.Puck)
        {
            var vectorPos = sender.Stick.BladeHandlePosition;
            
            position.HasPuckPosition = true;
            position.PuckPosition = new JsonVector3()
            {
                X = vectorPos.x,
                Y = vectorPos.y,
                Z = vectorPos.z
            };

            response =
                $"Saved puck position (X={position.PuckPosition.X}; Y={position.PuckPosition.Y}; Z={position.PuckPosition.Z})";
            return true;
        }
        else
        {
            var positions = team is PlayerTeam.Red
                ? position.RedPositions
                : position.BluePositions;

            var vectorPos = sender.Stick.BladeHandlePosition;
            var quaternionRot = sender.PlayerBody.Movement.MovementDirection.rotation;

            var pose = new JsonPose();

            pose.Position.X = vectorPos.x;
            pose.Position.Y = vectorPos.y;
            pose.Position.Z = vectorPos.z;

            pose.Rotation.X = quaternionRot.x;
            pose.Rotation.Y = quaternionRot.y;
            pose.Rotation.Z = quaternionRot.z;
            pose.Rotation.W = quaternionRot.w;

            positions[role] = pose;

            FaceOffPositionManager.SavePositions();

            response =
                $"Saved position '{position.Name}' ({team} / {role}): X={pose.Position.X}; Y={pose.Position.Y}; Z={pose.Position.Z} | X={pose.Rotation.X}; Y={pose.Rotation.Y}; Z={pose.Rotation.Z}; W={pose.Rotation.W}";
            return true;
        }
    }
    
    internal static void RegisterCommand()
    {
        CustomCommands.RegisterCommand("setpos", HandleCommand);
    }
}