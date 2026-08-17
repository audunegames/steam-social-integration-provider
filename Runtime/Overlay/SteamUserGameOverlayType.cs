using System;

namespace Audune.Social.Steam
{
  /// <summary>
  /// Enum that defines the type of Steam user game overlay
  /// </summary>
  public enum SteamUserGameOverlayType
  {
    Profile,
    Chat,
    JoinTrade,
    Stats,
    Achievements,
    AddFriend,
    RemoveFriend,
    AcceptFriendRequest,
    IgnoreFriendRequest,
  }


  /// <summary>
  /// Class that defines extension methods for Steam user game overlay types
  /// </summary>
  internal static class SteamUserGameOverlayTypeExtensions
  {
    /// <summary>
    /// Returns the <c>SteamFriends.ActivateGameOverlayToUser</c> string for the specified type.
    /// </summary>
    /// <param name="type">The type to return the string for.</param>
    /// <returns>The <c>SteamFriends.ActivateGameOverlayToUser</c> string for the specified type.</returns>
    public static string ToActivateGameOverlayString(this SteamUserGameOverlayType type)
    {
      return type switch {
        SteamUserGameOverlayType.Profile => "steamid",
        SteamUserGameOverlayType.Chat => "chat",
        SteamUserGameOverlayType.JoinTrade => "jointrade",
        SteamUserGameOverlayType.Stats => "stats",
        SteamUserGameOverlayType.Achievements => "achievements",
        SteamUserGameOverlayType.AddFriend => "friendadd",
        SteamUserGameOverlayType.RemoveFriend => "friendremove",
        SteamUserGameOverlayType.AcceptFriendRequest => "friendrequestaccept",
        SteamUserGameOverlayType.IgnoreFriendRequest => "friendrequestignore",
        _ => throw new ArgumentOutOfRangeException(nameof(type)),
      };
    }
  }
}
