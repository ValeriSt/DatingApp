﻿using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
    {
        var users = await userRepository.GetMembersAsync();

        return Ok(users);
    }

    [HttpGet("{username}")] // api/users/username
    public async Task<ActionResult<MemberDTO>> GetUsers(string username)
    {
        var user = await userRepository.GetMemberAsync(username);
        
        if (user == null) return NotFound();

        return user;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO){
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (username == null) return BadRequest("Username not found in token");

        var user = await userRepository.GetUserByUsernameAsync(username);

        if (user == null) return BadRequest("User not found");

        mapper.Map(memberUpdateDTO, user);

        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update user");

    }
}
