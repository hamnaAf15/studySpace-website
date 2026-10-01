using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace studySpaceWebApp.Models;

public partial class RoomMember
{
    [Key]
    public int MemberId { get; set; }

    public int? RoomId { get; set; }

    public int? UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? JoinedAt { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("RoomMembers")]
    public virtual StudyRoom? Room { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("RoomMembers")]
    public virtual User? User { get; set; }
   
}
