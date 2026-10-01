using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace studySpaceWebApp.Models;

public partial class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [NotMapped]
    public string? Password { get; set; }

    [StringLength(50)]
    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<AuthProvider> AuthProviders { get; set; } = new List<AuthProvider>();
    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
    public virtual ICollection<NotesHistory> NotesHistories { get; set; } = new List<NotesHistory>();
    public virtual ICollection<Poll> Polls { get; set; } = new List<Poll>();
    public virtual ICollection<PollVote> PollVotes { get; set; } = new List<PollVote>();
    public virtual ICollection<Resource> Resources { get; set; } = new List<Resource>();
    public virtual ICollection<RoomMember> RoomMembers { get; set; } = new List<RoomMember>();
    public virtual ICollection<StudyRoom> StudyRooms { get; set; } = new List<StudyRoom>();
}
