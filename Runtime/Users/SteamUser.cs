using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Steamworks;
using UnityEngine;

namespace Audune.Social.Steam
{
  /// <summary>
  /// Class that defines a user in the Steam social provider.
  /// </summary>
  public sealed class SteamUser : IUser, IEquatable<SteamUser>
  {
    // Internal state
    private readonly SteamSocialProvider _socialProvider;
    private readonly CSteamID _userId;

    
    /// <summary>
    /// Returns the Steam user ID of the user.
    /// </summary>
    public CSteamID userId => _userId;
    
    
    /// <inheritdoc/>
    public SocialProvider socialProvider => _socialProvider;

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

        return SteamFriends.GetFriendPersonaState(_userId).ToUserStatus();
      }
    }
    
    /// <inheritdoc/>
    public bool isPlaying {
      get {
        if (!_socialProvider.isInitialized)
          return false;

        return SteamFriends.GetFriendGamePlayed(_userId, out _);
      } 
    }
    
    /// <inheritdoc/>
    public bool isPlayingThisGame {
      get {
        if (!_socialProvider.isInitialized)
          return false;

        return SteamFriends.GetFriendGamePlayed(_userId, out var friendGameInfo)
          && friendGameInfo.m_gameID == new CGameID(_socialProvider.steamApplicationId);
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
    
    
    #region Steam-specific user methods
    /// <summary>
    /// Opens the Steam game overlay to the specified user page.
    /// </summary>
    /// <param name="type">The type of overlay to open.</param>
    public void OpenGameOverlayToUser(SteamUserGameOverlayType type)
    {
      if (!_socialProvider.isInitialized)
        return;

      _socialProvider.OpenGameOverlayToUser(type, this);
    }

    /// <summary>
    /// Sends a Remote Play Together invite to the user.
    /// </summary>
    /// <returns>If the Remote Play Together invite was sent successfully.</returns>
    public bool SendRemotePlayTogetherInvite()
    {
      if (!_socialProvider.isInitialized)
        return false;

      return SteamRemotePlay.BSendRemotePlayTogetherInvite(_userId);
    }
    #endregion
    
    #region User implementation
    /// <inheritdoc/>
    public async UniTask<Texture2D> GetAvatar(int size = 1024, CancellationToken cancellationToken = default)
    {
      if (!_socialProvider.isInitialized)
        return null;

      return await SteamImageUtils.CreateAvatarTextureFromImage(_userId, cancellationToken);
    }
    #endregion
    
    #region Equatable implementation
    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
      return ReferenceEquals(this, obj) 
        || obj is IUser other && Equals(other);
    }
    
    /// <inheritdoc/>
    public bool Equals(IUser other)
    {
      return ReferenceEquals(this, other) 
        || other is SteamUser steamUser && Equals(steamUser);
    }
    
    /// <inheritdoc/>
    public bool Equals(SteamUser other)
    {
      if (other is null)
        return false;
      if (ReferenceEquals(this, other))
        return true;

      return Equals(_socialProvider, other._socialProvider)
        && Equals(_userId, other._userId);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
      return HashCode.Combine(_socialProvider, _userId);
    }

    
    /// <summary>
    /// Returns if the specified Steam users equal each other.
    /// </summary>
    /// <param name="left">The left Steam user to check.</param>
    /// <param name="right">The right Steam user to check.</param>
    /// <returns>If the specified Steam users equal each other.</returns>
    public static bool operator ==(SteamUser left, SteamUser right)
    {
      return Equals(left, right);
    }

    /// <summary>
    /// Returns if the specified Steam users do not equal each other.
    /// </summary>
    /// <param name="left">The left Steam user to check.</param>
    /// <param name="right">The right Steam user to check.</param>
    /// <returns>If the specified Steam users do not equal each other.</returns>
    public static bool operator !=(SteamUser left, SteamUser right)
    {
      return !(left == right);
    }
    #endregion
  }
}
