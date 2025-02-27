﻿using System.Security.Claims;
using BikeService.Sonic.Const;
using BikeService.Sonic.Dtos;
using BikeService.Sonic.Dtos.BikeOperation;
using BikeService.Sonic.Models;
using BikeService.Sonic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Okta.AspNetCore;

namespace BikeService.Sonic.Services.Implementation;

[Authorize(AuthenticationSchemes = OktaDefaults.ApiAuthenticationScheme)]
public class BikeLocationHub : Hub, IBikeLocationHub
{
    private readonly IHubContext<BikeLocationHub> _hubContext;

    public BikeLocationHub(IHubContext<BikeLocationHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public async Task SendBikeLocationsData(string? email, BikeLocationDto bike)
    {
        if (string.IsNullOrEmpty(email)) return;
        await _hubContext.Clients.Group(email).SendAsync(SignalRChannel.BikeLocationChannel, bike);
    }
    
    public override async Task OnConnectedAsync()
    {
        var email = Context.GetHttpContext()!.User.Claims.FirstOrDefault(x => 
            x.Type == ClaimTypes.NameIdentifier)!.Value;

        await Groups.AddToGroupAsync(Context.ConnectionId, email);
    }
}