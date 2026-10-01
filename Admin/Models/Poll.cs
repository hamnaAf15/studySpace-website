using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace studySpaceWebApp.Models;

public partial class Poll
{
    [Key]
    public int PollId { get; set; }

    [StringLength(255)]
    public string Question { get; set; } = null!;

    public int? RoomId { get; set; }

    public int? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("Polls")]
    public virtual User? CreatedByNavigation { get; set; }

    [InverseProperty("Poll")]
    public virtual ICollection<PollOption> PollOptions { get; set; } = new List<PollOption>();

    [ForeignKey("RoomId")]
    [InverseProperty("Polls")]
    public virtual StudyRoom? Room { get; set; }
}
