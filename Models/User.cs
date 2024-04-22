using System.ComponentModel.DataAnnotations;

namespace AtonAssignment.Models;

/// <summary>
/// Users model.
/// </summary>
public partial class User
{
    /// <summary>
    /// Users identifier.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Login
    /// </summary>
    public string Login { get; set; } = null!;

    /// <summary>
    /// Password.
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// Name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gender. 0 - woman, 1 - man, 2 - unknown.
    /// </summary>
    public int Gender { get; set; } = 2;

    /// <summary>
    /// Birthday.
    /// </summary>
    public DateTime? Birthday { get; set; } = null;

    /// <summary>
    /// Is user admin?
    /// </summary>
    public bool Admin { get; set; } = false;

    /// <summary>
    /// Date when user was created.
    /// </summary>
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User was created by.
    /// </summary>
    public string CreatedBy { get; set; } = null!;

    /// <summary>
    /// Date when user was modified.
    /// </summary>
    public DateTime ModifiedOn { get; set; } = DateTime.MaxValue;

    /// <summary>
    /// User was modified by.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;

    /// <summary>
    /// Date when user was softly deleted.
    /// </summary>
    public DateTime RevokedOn { get; set; } = DateTime.MaxValue;

    /// <summary>
    /// User was deleted by.
    /// </summary>
    public string RevokedBy { get; set; } = string.Empty!;
}
