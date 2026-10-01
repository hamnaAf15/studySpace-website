using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace studySpaceWebApp.Models;

public partial class Resource
{
    [Key]
    public int ResourceId { get; set; }

    [StringLength(150)]
    public string Title { get; set; } = null!;

    [StringLength(50)]
    public string? FileType { get; set; }

    [StringLength(255)]
    public string? FileUrl { get; set; }

    public int? UploadedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UploadedAt { get; set; }

    [ForeignKey("UploadedBy")]
    [InverseProperty("Resources")]
    public virtual User? UploadedByNavigation { get; set; }
}
