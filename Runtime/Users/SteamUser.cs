using System.Threading;
using Cysharp.Threading.Tasks;
using Steamworks;
using UnityEngine;

namespace Audune.Social.Steam
{
  /// <summary>
  /// Class that defines a user in the Steam social provider.
  /// </summary>
  public sealed class SteamUser : IUser
  {
    // Internal state
    private readonly SteamSocialProvider _socialProvider;
    private readonly CSteamID _userId;


    /// <inheritdoc/>
    public SocialProvider socialProvider => _socialProvider;
    
    /// <summary>
    /// Returns the Steam user ID of the user.
    /// </summary>
    public CSteamID userId => _userId;

    /// <inheritdoc/>
    public string name {
      get {
        if (!_socialProvider.isInitialized)
          return null;

        return SteamFriends.GetFriendPersonaName(_userId);
      }
    }
    
    /// <inheritdoc/>
    public string displayName {
      get {
        if (!_socialProvider.isInitialized)
          return null;

        var nickname = SteamFriends.GetPlayerNickname(_userId);
        return !string.IsNullOrEmpty(nickname) ? nickname : SteamFriends.GetFriendPersonaName(_userId);
      }
    }

    /// <inheritdoc/>
    public UserStatus status {
      get {
        if (!_socialProvider.isInitialized)
          return UserStatus.Unknown;
  
        var state = SteamFriends.GetFriendPersonaState(_userId);
        return state switch
        {
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


    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="socialProvider">The source social provider of the user.</param>
    /// <param name="userId">The Steam user ID of the user.</param>
    internal SteamUser(SteamSocialProvider socialProvider, CSteamID userId)
    {
      _socialProvider = socialProvider;
      _userId = userId;
    }
    
    
    #region User implementation
    /// <inheritdoc/>
    public async UniTask<Texture2D> GetAvatar(int size = 1024, CancellationToken cancellationToken = default)
    {
      if (!_socialProvider.isInitialized)
        return null;

      return await SteamImageUtils.CreateAvatarTextureFromImage(_userId, cancellationToken);
    }
    #endregion
  }
}
