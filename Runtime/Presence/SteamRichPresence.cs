using System.Collections;
using System.Collections.Generic;

namespace Audune.Social.Steam
{
  /// <summary>
  /// Class that defines a dictionary of Steam rich presence keys and values.
  /// </summary>
  public sealed class SteamRichPresence : IReadOnlyDictionary<string, string>
  {
    // Constants
    private const string _statusKey = "status";
    private const string _connectKey = "connect";
    private const string _displayKey = "steam_display";
    private const string _playerGroupKey = "steam_player_group";
    private const string _playerGroupSizeKey = "steam_player_group_size";
    
    
    // Internal state
    private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>();
    
    
    
    /// <summary>
    /// Returns the amount of key-value pairs in the dictionary.
    /// </summary>
    public int Count => _dictionary.Count;
    
    /// <summary>
    /// Returns the keys in the dictionary.
    /// </summary>
    public IEnumerable<string> Keys => _dictionary.Keys;
    
    /// <summary>
    /// Returns the values in the dictionary.
    /// </summary>
    public IEnumerable<string> Values => _dictionary.Values;
    
    /// <summary>
    /// Returns of sets the value for the specified key in the dictionary.
    /// </summary>
    /// <param name="key">The key to set.</param>
    public string this[string key] {
      get => _dictionary[key];
      set => _dictionary[key] = value;
    }
    
    
    #region Managing the dictionary
    /// <summary>
    /// Returns if a value for the specified key exists in the dictionary.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>If a value for the specified key exists in the dictionary</returns>
    public bool ContainsKey(string key)
    {
      return _dictionary.ContainsKey(key);
    }
    
    /// <summary>
    /// Returns if a value for the specified key exists in the dictionary and stores it in <paramref name="value"/>.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <param name="value">The value for the specified key.</param>
    /// <returns>If a value for the specified key exists in the dictionary</returns>
    public bool TryGetValue(string key, out string value)
    {
      return _dictionary.TryGetValue(key, out value);
    }

    /// <summary>
    /// Add the specified key and value to the dictionary.
    /// </summary>
    /// <param name="key">The key to add.</param>
    /// <param name="value">The value to add.</param>
    public void Add(string key, string value)
    {
      _dictionary.Add(key, value);
    }

    /// <summary>
    /// Sets the value for the status key in the dictionary.
    /// </summary>
    /// <param name="statusValue">The status value to set.</param>
    public void SetStatus(string statusValue)
    {
      _dictionary[_statusKey] = statusValue;
    }

    /// <summary>
    /// Sets the value for the connect key in the dictionary.
    /// </summary>
    /// <param name="connectValue">The connect value to set.</param>
    public void SetConnect(string connectValue)
    {
      _dictionary[_connectKey] = connectValue;
    }
    
    /// <summary>
    /// Sets the value for the display key in the dictionary.
    /// </summary>
    /// <param name="displayValue">The display value to set.</param>
    public void SetDisplay(string displayValue)
    {
      _dictionary[_displayKey] = displayValue;
    }

    /// <summary>
    /// Sets the value for the player group key in the dictionary.
    /// </summary>
    /// <param name="playerGroupValue">The player group value to set.</param>
    public void SetPlayerGroup(string playerGroupValue)
    {
      _dictionary[_playerGroupKey] = playerGroupValue;
    }

    /// <summary>
    /// Sets the value for the player group size key in the dictionary.
    /// </summary>
    /// <param name="playerGroupSizeValue">The player group size value to set.</param>
    public void SetPlayerGroupSize(string playerGroupSizeValue)
    {
      _dictionary[_playerGroupSizeKey] = playerGroupSizeValue;
    }
    #endregion
    
    #region Returning enumerators
    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
      return _dictionary.GetEnumerator();
    }
    
    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return _dictionary.GetEnumerator();
    }
    #endregion
  }
}
