using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace studySpaceWebApp.Models;

public partial class AuthProvider
{
    [Key]
    public int ProviderId { get; set; }

    [StringLength(100)]
    public string ProviderName { get; set; } = null!;

    [StringLength(255)]
    public string ProviderKey { get; set; } = null!;

    public int? UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("AuthProviders")]
    public virtual User? User { get; set; }
}
