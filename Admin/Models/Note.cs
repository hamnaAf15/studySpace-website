using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace studySpaceWebApp.Models;

public partial class Note
{
    [Key]
    public int NoteId { get; set; }

    public int? RoomId { get; set; }

    [StringLength(150)]
    public string? Title { get; set; }

    public string? Content { get; set; }

    public int? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("Notes")]
    public virtual User? CreatedByNavigation { get; set; }

    [InverseProperty("Note")]
    public virtual ICollection<NotesHistory> NotesHistories { get; set; } = new List<NotesHistory>();

    [ForeignKey("RoomId")]
    [InverseProperty("Notes")]
    public virtual StudyRoom? Room { get; set; }
}
