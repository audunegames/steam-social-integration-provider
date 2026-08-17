using Steamworks;

namespace Audune.Social.Steam
{
  /// <summary>
  /// Class that defines extension methods for user statuses.
  /// </summary>
  internal static class UserStatusExtensions
  {
    /// <summary>
    /// Returns the user status for the specified <c>EPersonaState</c>.
    /// </summary>
    /// <param name="personaState">The <c>EPersonaState</c> for which to return the user status.</param>
    /// <returns>The user status for the specified <c>EPersonaState</c>.</returns>
    public static UserStatus ToUserStatus(this EPersonaState personaState)
    {
      return personaState switch {
        EPersonaState.k_EPersonaStateOnline => UserStatus.Online,
        EPersonaState.k_EPersonaStateLookingToTrade => UserStatus.Online,
        EPersonaState.k_EPersonaStateLookingToPlay => UserStatus.Online,
        EPersonaState.k_EPersonaStateAway => UserStatus.Idle,
        EPersonaState.k_EPersonaStateSnooze => UserStatus.Idle,
        EPersonaState.k_EPersonaStateBusy => UserStatus.DoNotDisturb,
        _ => UserStatus.Offline,
      };
    }
  }
}
