namespace Application.Modules.Common;

/// <summary>
/// Represents the type of media used in a lesson.
/// </summary>
public enum MediaType
{
    /// <summary>
    /// No media is associated with the lesson.
    /// </summary>
    None,

    /// <summary>
    /// Represents an audio lesson.
    /// </summary>
    Audio,

    /// <summary>
    /// Represents a video lesson.
    /// </summary>
    Video,
}