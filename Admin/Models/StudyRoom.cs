using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace studySpaceWebApp.Models;

public partial class StudyRoom
{
    [Key]
    public int RoomId { get; set; }

    [StringLength(100)]
    public string RoomName { get; set; } = null!;

    public bool? IsPrivate { get; set; }

    [StringLength(50)]
    public string? AccessCode { get; set; }

    public int? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Room")]
    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("StudyRooms")]
    public virtual User? CreatedByNavigation { get; set; }

    [InverseProperty("Room")]
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    [InverseProperty("Room")]
    public virtual ICollection<Poll> Polls { get; set; } = new List<Poll>();

    [InverseProperty("Room")]
    public virtual ICollection<RoomMember> RoomMembers { get; set; } = new List<RoomMember>();

    [InverseProperty("Room")]
    public virtual ICollection<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();
}
