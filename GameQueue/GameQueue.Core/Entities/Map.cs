﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameQueue.Core.Models;

namespace GameQueue.Core.Models;

[Table("maps")]
public sealed record Map
{
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [Range(0, int.MaxValue)]
    public int Width { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Height { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int MaxPlayersCount { get; set; }

    [Required]
    public string CoverImageUrl { get; set; } = null!;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    public MapStatus Status { get; set; } = MapStatus.Pending;

    public List<RequestToMap> RequestsToMap { get; } = new List<RequestToMap>();

    public override string ToString()
        => string.Format("{0} {1}x{2} ({3}p)", Name, Width, Height, MaxPlayersCount);
}
