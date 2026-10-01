using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace studySpaceWebApp.Models;

public partial class PollOption
{
    [Key]
    public int OptionId { get; set; }

    public int? PollId { get; set; }

    [StringLength(255)]
    public string OptionText { get; set; } = null!;

    [ForeignKey("PollId")]
    [InverseProperty("PollOptions")]
    public virtual Poll? Poll { get; set; }

    [InverseProperty("Option")]
    public virtual ICollection<PollVote> PollVotes { get; set; } = new List<PollVote>();
}
