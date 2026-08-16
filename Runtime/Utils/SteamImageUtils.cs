using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Steamworks;
using UnityEngine;

namespace Audune.Social.Steam
{
  // Class that defines utility methods for Steam image handles
  public static class SteamImageUtils
  {
    // Dictionary that maps image handles to loaded textures
    private static readonly Dictionary<int, Texture2D> _cachedTextures = new Dictionary<int, Texture2D>();
    

    #region Creating textures from image handles
    /// <summary>
    /// Creates a texture from the specified image handle.
    /// </summary>
    /// <param name="imageHandle">The image handle to create a texture from.</param>
    /// <returns>A texture from the specified image handle.</returns>
    public static Texture2D CreateTextureFromImage(int imageHandle)
    {
      if (_cachedTextures.TryGetValue(imageHandle, out var texture))
        return texture;

      if (!SteamUtils.GetImageSize(imageHandle, out var width, out var height))
        return null;

      var bufferSize = width * height * 4;
      var buffer = new byte[bufferSize];
      if (!SteamUtils.GetImageRGBA(imageHandle, buffer, (int)bufferSize))
        return null;

      var mirroredBuffer = new byte[bufferSize];
      for (var i = 0; i < bufferSize; i += (int)width * 4)
        Array.Copy(buffer, i, mirroredBuffer, bufferSize - width * 4 - i, width * 4);

      texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
      texture.LoadRawTextureData(mirroredBuffer);
      texture.Apply();
      texture.filterMode = FilterMode.Bilinear;

      _cachedTextures[imageHandle] = texture;
      return texture;
    }

    /// <summary>
    /// Creates a texture containing the avatar of the specified user.
    /// </summary>
    /// <param name="userId">The user ID of the use whose avatar to get.</param>
    /// <param name="cancellationToken">The cancellation token for the request.</param>
    /// <returns>A texture containing the avatar of the specified user.</returns>
    public static async UniTask<Texture2D> CreateAvatarTextureFromImage(CSteamID userId, CancellationToken cancellationToken = default)
    {
      var avatarImageHandle = SteamFriends.GetLargeFriendAvatar(userId);
      if (avatarImageHandle == 0)
        return null;
        
      while (avatarImageHandle == -1)
      {
        await UniTask.Delay(50, true, cancellationToken: cancellationToken);
        avatarImageHandle = SteamFriends.GetLargeFriendAvatar(userId);
      }

      if (cancellationToken.IsCancellationRequested)
        return null;

      var avatarTexture = CreateTextureFromImage(avatarImageHandle);
      return avatarTexture;
    }
    #endregion
  }
}
