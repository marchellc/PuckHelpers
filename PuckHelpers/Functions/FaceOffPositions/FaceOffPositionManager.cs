using PuckHelpers.Configs; 

namespace PuckHelpers.Functions.FaceOffPositions;

/// <summary>
/// Allows custom face-off positions to be used.
/// </summary>
public static class FaceOffPositionManager
{
    /// <summary>
    /// Gets or sets a list of face-off positions.
    /// </summary>
    public static Dictionary<string, FaceOffPosition> Positions { get; private set; } = new();

    /// <summary>
    /// Saves all current positions.
    /// </summary>
    public static void SavePositions()
        => ConfigLoader.SaveConfig("faceOffPositions", Positions);

    internal static void LoadPositions()
        => Positions = ConfigLoader.LoadConfig("faceOffPositions", Positions);
}