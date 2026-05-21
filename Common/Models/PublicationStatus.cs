namespace Common.Models;

/// <summary>
/// The publication status.
/// </summary>
public enum PublicationStatus
{
    /// <summary>
    /// The item is in draft mode and not accessible to users.
    /// </summary>
    Draft,

    /// <summary>
    /// The item is published and accessible to users.
    /// </summary>
    Published,

    /// <summary>
    /// The item was previously published but is now unpublished
    /// and not accessible to users.
    /// </summary>
    Unpublished,
}