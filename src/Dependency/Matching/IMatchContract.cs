namespace Unity.Dependency;


/// <summary>
/// Calculates how members match against the import
/// </summary>
public interface IMatchContract
{
    /// <summary>
    /// Calculates how much member matches the contract
    /// </summary>
    MatchRank RankMatch(ref Contract contract);
}
