﻿using GameQueue.Core.Models;

namespace GameQueue.Core.Commands.Users;

public sealed record AddUserCommand
{
    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Level { get; set; }

    public UserRole Role { get; set; } = UserRole.Client;
}
