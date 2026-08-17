using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using Steamworks;
using UnityEngine;

namespace Audune.Social.Steam
{
  /// <summary>
  /// Class that defines a social provider for the Steamworks API.
  /// </summary>
  [AddComponentMenu("Audune/Social/Steam Social Provider")]
  public sealed class SteamSocialProvider : SocialProvider,
    IRelationshipProvider,
    IIRichPresenceProvider,
    IGameOverlayProvider
  {
    // Static variables
    private static SteamSocialProvider _current;

    /// <summary>
    /// Returns the static instance of the Discord social provider.
    /// </summary>
    public static SteamSocialProvider current => _current;
    
    
    // Variables
    [SerializeField, Tooltip("The Steam Application ID")]
    private uint _steamApplicationId = 480;
    [SerializeField, Tooltip("When the game will attempt to restart itself through the Steam client using RestartAppIfNecessary")]
    private ExecutionMode _steamClientRequired = ExecutionMode.BuildOnly;

    // Internal state
    private bool _initialized = false;
    private SteamAPIWarningMessageHook_t _warningMessageHook;
    private Callback<GameOverlayActivated_t> _gameOverlayActivatedCallback;

    private readonly Dictionary<Type, object> _richPresenceAdapters = new Dictionary<Type, object>();

    /// <summary>
    /// Returns the Steam Application ID.
    /// </summary>
    public ulong steamApplicationId => _steamApplicationId;
    
    /// <inheritdoc/>
    public override bool isInitialized => _initialized;
    
    
    /// <inheritdoc/>
    public event GameOverlayActivatedEvent onGameOverlayActivated;
    
    
    /// <inheritdoc/>
    protected override void Awake()
    {
      base.Awake();
      
      // Set the static instance
      if (_current == null)
        _current = this;
      else
        Destroy(gameObject);
    }

    /// <inheritdoc/>
    public override void OnEnableSocialProvider()
    {
      base.OnEnableSocialProvider();
      
      try
      {
        // Execute the pack size test
        if (!Packsize.Test())
          Debug.LogError("[Steam] Could not initialize the Steam client: the wrong version of Steamworks is being run in this platform", this);

        // Execute the DLL check test
        if (!DllCheck.Test())
          Debug.LogError("[Steam] Could not initialize the Steam client: one or more of the Steamworks binaries seems to be the wrong version", this);

        // Check if the Steam client is running
        if (_steamClientRequired.ShouldExecute() && SteamAPI.RestartAppIfNecessary((AppId_t)_steamApplicationId))
        {
          Debug.Log("[Steam] Restarting application from the Steam client...", this);
          Application.Quit();
          return;
        }

        // Initialize the Steam API
        _initialized = SteamAPI.Init();
        if (!_initialized)
        {
          Debug.LogError("[Steam] Could not initialize the Steam client", this);
          return;
        }

        // Set the warning message hook
        if (_warningMessageHook == null)
        {
          _warningMessageHook = OnWarningMessage;
          SteamClient.SetWarningMessageHook(_warningMessageHook);
        }

        // Add callbacks
        _gameOverlayActivatedCallback = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);

        // Log the successful initialization
        Debug.Log("[Steam] Successfully initialized the Steam client", this);
      }
      catch (DllNotFoundException ex)
      {
        Debug.LogError($"[Steam] Could not load the Steamworks binaries: {ex.Message}", this);
      }
    }

    /// <inheritdoc/>
    public override void OnDisableSocialProvider()
    {
      base.OnDisableSocialProvider();
      
      // Check if the client is initialized
      if (!isInitialized)
        return;

      // Dispose of the callbacks
      _gameOverlayActivatedCallback.Dispose();

      // Shut down the Steam client
      SteamAPI.Shutdown();

      // Log the successful disposal
      Debug.Log("[Steam] Successfully disposed of the Steam client", this);
    }
    
    /// <inheritdoc/>
    public override void OnUpdateSocialProvider()
    {
      base.OnUpdateSocialProvider();
      
      // Run the callbacks of the Steam client
      SteamAPI.RunCallbacks();
    }
    
    
    #region Managing rich presence adapters
    /// <summary>
    /// Registers the specified rich presence adapter for the specified data type.
    /// </summary>
    /// <param name="adapter">The rich presence adapter to register.</param>
    /// <typeparam name="TData">The type of the rich presence data to register the adapter for.</typeparam>
    public void RegisterRichPresenceAdapter<TData>(ISteamRichPresenceAdapter<TData> adapter) where TData : IRichPresenceData
    {
      _richPresenceAdapters.Add(typeof(TData), adapter);
    }

    /// <summary>
    /// Unregisters the rich presence adapter for the specified data type.
    /// </summary>
    /// <typeparam name="TData">The type of the rich presence data to unregister the adapter for.</typeparam>
    public void UnregisterRichPresenceAdapter<TData>() where TData : IRichPresenceData
    {
      _richPresenceAdapters.Remove(typeof(TData));
    }
    #endregion
    
    #region Opening game overlays
    /// <summary>
    /// Open the specified Steam game overlay.
    /// </summary>
    /// <param name="type">The type of overlay to open.</param>
    public void OpenGameOverlay(SteamGameOverlayType type)
    {
      if (!isInitialized)
        return;
      
      SteamFriends.ActivateGameOverlay(type.ToActivateGameOverlayString());
    }

    /// <summary>
    /// Open the Steam game overlay to the specified user page.
    /// </summary>
    /// <param name="type">The type of overlay to open.</param>
    /// <param name="user">The user whose user page to open.</param>
    public void OpenGameOverlayToUser(SteamUserGameOverlayType type, SteamUser user)
    {
      if (!isInitialized)
        return;
      
      SteamFriends.ActivateGameOverlayToUser(type.ToActivateGameOverlayString(), user.userId);
    }

    /// <summary>
    /// Open the Steam game overlay to the web page with the specified URL
    /// </summary>
    /// <param name="url">The URL of the web page to open.</param>
    public void OpenGameOverlayToWebPage(string url)
    {
      if (!isInitialized)
        return;
      
      SteamFriends.ActivateGameOverlayToWebPage(url);
    }
    #endregion
    
    #region Relationship provider implementation
    /// <inheritdoc/>
    public UniTask<IUser> GetCurrentUser()
    {
      // Check if the client is initialized
      if (!isInitialized)
        return UniTask.FromResult<IUser>(null);

      // Get the current user
      var currentUserId = Steamworks.SteamUser.GetSteamID();
      var currentUser = new SteamUser(this, currentUserId);
      return UniTask.FromResult<IUser>(currentUser);
    }

    /// <inheritdoc/>
    public async UniTask<IReadOnlyCollection<Relationship>> GetCurrentUserRelationships()
    {
      // Create a list to store the relationships
      var relationships = new List<Relationship>();
      
      // Check if the client is initialized
      if (!isInitialized)
        return relationships;

      // Iterate over the Steam friends
      var friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagAll);
      for (var i = 0; i < friendCount; i++)
      {
        // Get the friend user
        var friendUserId = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagAll);
        var friendUser = new SteamUser(this, friendUserId);
        
        // Get the relationship type
        var relationshipType = await GetCurrentUserRelationshipType(friendUser);
        
        // Add a new relationship to the list
        relationships.Add(new Relationship(friendUser, relationshipType));
      }
      
      // Return the relationships
      return relationships;
    }

    /// <inheritdoc/>
    public UniTask<RelationshipType> GetCurrentUserRelationshipType(IUser otherUser)
    {
      // Check if the other user is sourced from this social provider
      if (otherUser is not SteamUser otherSteamUser || otherSteamUser.socialProvider != this)
        return UniTask.FromResult(RelationshipType.None);
      
      // Return the relationship type
      return UniTask.FromResult(SteamFriends.GetFriendRelationship(otherSteamUser.userId).ToRelationshipType());
    }
    #endregion

    #region Rich presence provider implementation
    /// <inheritdoc/>
    public void UpdateRichPresence(IRichPresenceData data)
    {
      // Check if the client is initialized
      if (!isInitialized)
        return;
      
      // Clear the rich presence values
      SteamFriends.ClearRichPresence();
      
      // Check if the data is set
      if (data == null)
        return;
      
      // Get the adapter for the data
      if (!_richPresenceAdapters.TryGetValue(data.GetType(), out var adapterObject))
        throw new ArgumentException($"No adapter found for {data.GetType()}", nameof(data));

      try
      {
        // Create the rich presence
        var adapterType = typeof(ISteamRichPresenceAdapter<>).MakeGenericType(data.GetType());
        var convertMethod = adapterType.GetMethod("Convert", new[] { data.GetType() });
        if (convertMethod == null)
          throw new ArgumentException($"Wrong adapter type found for {data.GetType()}", nameof(data));

        var richPresence = (SteamRichPresence)convertMethod.Invoke(adapterObject, new object[] { data });

        // Iterate over the rich presence and set the rich presence values
        foreach (var e in richPresence)
          SteamFriends.SetRichPresence(e.Key, e.Value);
      }
      catch (Exception)
      {
        throw new ArgumentException($"Wrong adapter type found for {data.GetType()}", nameof(data));
      }
    }

    /// <inheritdoc/>
    public void ClearRichPresence()
    {
      // Check if the client is initialized
      if (!isInitialized)
        return;
      
      // Clear the rich presence values
      SteamFriends.ClearRichPresence();
    }
    #endregion

    #region Event handlers
    // Warning message handler
    [AOT.MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
    private void OnWarningMessage(int severity, StringBuilder message)
    {
      // Log the message
      if (severity == 1)
        Debug.LogWarning($"[{gameObject.name}] {message}", this);
      else
        Debug.Log($"[{gameObject.name}] {message}", this);
    }
    
    // Game overlay activated handler
    private void OnGameOverlayActivated(GameOverlayActivated_t callback)
    {
      // Invoke the game overlay activated event
      onGameOverlayActivated?.Invoke(this, callback.m_bActive > 0);
    }
    #endregion
  }
}
