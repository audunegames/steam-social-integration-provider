using System;

namespace Audune.Social.Steam
{
  /// <summary>
  /// Enum that defines the type of Steam game overlay
  /// </summary>
  public enum SteamGameOverlayType
  {
    Friends,
    Community,
    Players,
    Settings,
    OfficialGameGroup,
    Stats,
    Achievements,
  }


  /// <summary>
  /// Class that defines extension methods for Steam game overlay types
  /// </summary>
  internal static class SteamGameOverlayTypeExtensions
  {
    /// <summary>
    /// Returns the <c>SteamFriends.ActivateGameOverlay</c> string for the specified type.
    /// </summary>
    /// <param name="type">The type to return the string for.</param>
    /// <returns>The <c>SteamFriends.ActivateGameOverlay</c> string for the specified type.</returns>
    public static string ToActivateGameOverlayString(this SteamGameOverlayType type)
    {
      return type switch {
        SteamGameOverlayType.Friends => "friends",
        SteamGameOverlayType.Community => "community",
        SteamGameOverlayType.Players => "players",
        SteamGameOverlayType.Settings => "settings",
        SteamGameOverlayType.OfficialGameGroup => "officialgamegroup",
        SteamGameOverlayType.Stats => "stats",
        SteamGameOverlayType.Achievements => "achievements",
        _ => throw new ArgumentOutOfRangeException(nameof(type)),
      };
    }
  }
}
