﻿namespace BikeService.Sonic.Dtos.Bike;

public class BikeUpdateDto
{
    public int Id { get; set; }
    public string LicensePlate { get; set; } = null!;
    public string? Description { get; set; }
    public int? BikeStationId { get; set; }
}