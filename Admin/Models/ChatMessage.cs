using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace studySpaceWebApp.Models;

public partial class ChatMessage
{
    [Key]
    public int MessageId { get; set; }

    public int? RoomId { get; set; }

    public int? UserId { get; set; }

    public string? MessageText { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SentAt { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("ChatMessages")]
    public virtual StudyRoom? Room { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("ChatMessages")]
    public virtual User? User { get; set; }
}
