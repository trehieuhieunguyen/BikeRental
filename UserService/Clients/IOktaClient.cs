﻿using UserService.Dtos;
using UserService.Dtos.OktaClient;

namespace UserService.Clients;

public interface IOktaClient
{
    public Task<string?> AddUserToOkta(OktaUserInsertDto oktaUserInsertDto);
    public Task<List<OktaUserResponse>> GetOktaUserByGroup(string groupId);
    public Task<List<OktaRoleResponse>> GetOktaGroups();
    public Task DeleteOktaUser(string? oktaUserId);
}