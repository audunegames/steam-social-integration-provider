namespace Audune.Social.Steam
{
  /// <summary>
  /// Interface that defines an adapter for activity data.
  /// </summary>
  /// <typeparam name="TData">The type of the activity data.</typeparam>
  public interface ISteamRichPresenceAdapter<TData> where TData : IRichPresenceData
  {
    /// <summary>
    /// Converts the specified activity data to an activity.
    /// </summary>
    /// <param name="data">The activity data to convert.</param>
    public SteamRichPresence Convert(TData data);
  }
}
