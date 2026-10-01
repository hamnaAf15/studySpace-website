using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace studySpaceWebApp.Models;

[Table("NotesHistory")]
public partial class NotesHistory
{
    [Key]
    public int HistoryId { get; set; }

    public int? NoteId { get; set; }

    public int? EditedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EditTime { get; set; }

    public string? OldContent { get; set; }

    [ForeignKey("EditedBy")]
    [InverseProperty("NotesHistories")]
    public virtual User? EditedByNavigation { get; set; }

    [ForeignKey("NoteId")]
    [InverseProperty("NotesHistories")]
    public virtual Note? Note { get; set; }
}
