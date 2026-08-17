using Steamworks;

namespace Audune.Social.Steam
{
  /// <summary>
  /// Class that defines extension methods for relationship types.
  /// </summary>
  internal static class RelationshipTypeExtensions
  {
    /// <summary>
    /// Returns the relationship type for the specified <c>EFriendRelationship</c>.
    /// </summary>
    /// <param name="friendRelationship">The <c>EFriendRelationship</c> for which to return the relationship type.</param>
    /// <returns>The relationship type for the specified <c>EFriendRelationship</c>.</returns>
    public static RelationshipType ToRelationshipType(this EFriendRelationship friendRelationship)
    {
      return friendRelationship switch {
        EFriendRelationship.k_EFriendRelationshipNone => RelationshipType.None,
        EFriendRelationship.k_EFriendRelationshipIgnored => RelationshipType.None,
        EFriendRelationship.k_EFriendRelationshipFriend => RelationshipType.Friend,
        EFriendRelationship.k_EFriendRelationshipIgnoredFriend => RelationshipType.Friend,
        EFriendRelationship.k_EFriendRelationshipRequestRecipient => RelationshipType.IncomingFriendRequest,
        EFriendRelationship.k_EFriendRelationshipRequestInitiator => RelationshipType.OutgoingFriendRequest,
        EFriendRelationship.k_EFriendRelationshipBlocked => RelationshipType.Blocked,
        _ => RelationshipType.Unknown,
      };
    }
  }
}
