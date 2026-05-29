using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

/// <summary>
/// Represents information about a course creator.
/// </summary>
[Table("creators")]
public sealed class CreatorEntity
{
    /// <summary>
    /// The unique creator identifier.
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// The biography or description of the creator.
    /// </summary>
    [Column("bio")]
    public string Bio { get; set; } = null!;

    /// <summary>
    /// The number of years of creator experience.
    /// </summary>
    [Column("experience_years")]
    public int Experience { get; set; }

    /// <summary>
    /// The list of areas of expertise of the creator.
    /// </summary>
    [Column("areas_of_expertise")]
    public string[] AreasOfExpertise { get; set; } = [];

    /// <summary>
    /// The list of languages in which the creator provides content.
    /// </summary>
    [Column("languages")]
    public string[] Languages { get; set; } = [];
}
