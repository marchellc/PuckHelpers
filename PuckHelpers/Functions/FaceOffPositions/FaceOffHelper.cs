using PuckHelpers.Core;
using PuckHelpers.Patches;

using UnityEngine;

namespace PuckHelpers.Functions.FaceOffPositions;

public static class FaceOffHelper
{
    /// <summary>
    /// Converts a string position ID into a position name.
    /// </summary>
    /// <param name="positionName">The ID to convert.</param>
    /// <returns>The converted name.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    public static FaceOffPositionName ToName(string positionName)
    {
        if (string.IsNullOrEmpty(positionName))
            throw new ArgumentNullException(nameof(positionName));

        if (positionName is "C")
            return FaceOffPositionName.Center;

        if (positionName is "RW")
            return FaceOffPositionName.RightWing;

        if (positionName is "RD")
            return FaceOffPositionName.RightDefense;
        
        if (positionName is "LW")
            return FaceOffPositionName.LeftWing;

        if (positionName is "LD")
            return FaceOffPositionName.LeftDefense;

        if (positionName is "G")
            return FaceOffPositionName.Goalie;

        if (positionName is "P")
            return FaceOffPositionName.Puck;

        throw new Exception($"Unknown position: {positionName}");
    }
    
    /// <summary>
    /// Starts a custom face-off.
    /// </summary>
    /// <param name="position">The custom position.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void StartCustom(FaceOffPosition position)
    {
        if (position is null)
            throw new ArgumentNullException(nameof(position));

        try
        {
            var gameManager = NetworkBehaviourSingleton<GameManager>.Instance;
            var eventManager = MonoBehaviourSingleton<EventManager>.Instance;
            var playerManager = NetworkBehaviourSingleton<PlayerManager>.Instance;
            var puckManager = NetworkBehaviourSingleton<PuckManager>.Instance;

            var phaseTime = gameManager.PhaseDurationMap.TryGetValue(GamePhase.FaceOff, out var time) ? time : 3;
            var roundTime = gameManager.GameState.Value.Time;

            var eventCallback = default(Action<Dictionary<string, object>>);
            
            var puckPos = Vector3.zero;
            var puckRot = Quaternion.identity;

            if (position.HasPuckPosition)
            {
                puckPos = position.PuckPosition.Vector;
                puckPos.y += 1f;
            }

            eventCallback = _ =>
            {
                eventManager.RemoveEventListener("Event_OnGamePhaseChanged", eventCallback);

                var curState = gameManager.GameState.Value;

                curState.Time = roundTime;

                gameManager.GameState.Value = curState;
                
                SpawnPuck(puckPos, puckRot, puckManager.puckPrefab);
                
                DisablePuckSpawnPatch.IsDisabled = false;
                DisableRespawnPatch.IsDisabled = false;
            };

            DisablePuckSpawnPatch.IsDisabled = true;
            DisableRespawnPatch.IsDisabled = true;

            gameManager.Server_SetPhase(GamePhase.FaceOff, phaseTime);

            eventManager.AddEventListener("Event_OnGamePhaseChanged", eventCallback);

            puckManager.Server_DespawnPucks(true);

            foreach (var player in playerManager.players)
            {
                if (player == null)
                    continue;

                if (player.PlayerPosition == null)
                    continue;

                if (player.IsReplay.Value)
                    continue;

                if (player.PlayerBody == null)
                    continue;

                var name = ToName(player.PlayerPosition.Name);
                var positions = player.PlayerPosition.Team is PlayerTeam.Red
                    ? position.RedPositions
                    : position.BluePositions;

                if (!positions.TryGetValue(name, out var positionValue))
                    continue;

                player.Server_DespawnCharacter();
                player.Server_SpawnCharacter(positionValue.Position.Vector, positionValue.Rotation.Quaternion, player.Role.Value);
            }
        }
        catch (Exception ex)
        {
            HelpersPlugin.LogError("PuckHelpers / FaceOff", ex);
        }
    }

    private static void SpawnPuck(Vector3 position, Quaternion rotation, Puck prefab)
    {
        var puck = UnityEngine.Object.Instantiate(prefab, position, rotation);
        
        puck.IsReplay.Value = false;
        puck.Rigidbody.AddForce(Vector3.zero, ForceMode.VelocityChange);
        puck.NetworkObject.Spawn();
        
        Debug.Log(string.Format("[PuckManager] Spawned CUSTOM puck !{0}!", puck.NetworkObjectId));
    }
}