using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace studySpaceWebApp.Models;

public partial class PollVote
{
    [Key]
    public int VoteId { get; set; }

    public int? OptionId { get; set; }

    public int? UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? VotedAt { get; set; }

    [ForeignKey("OptionId")]
    [InverseProperty("PollVotes")]
    public virtual PollOption? Option { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("PollVotes")]
    public virtual User? User { get; set; }
}
