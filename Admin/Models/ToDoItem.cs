using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace studySpaceWebApp.Models;

public partial class ToDoItem
{
    [Key]
    public int ToDoId { get; set; }

    public int? RoomId { get; set; }

    [StringLength(255)]
    public string Task { get; set; } = null!;

    public bool? IsCompleted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("ToDoItems")]
    public virtual StudyRoom? Room { get; set; }
}
