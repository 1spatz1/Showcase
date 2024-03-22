﻿namespace Showcase.Contracts.Authentication;

public record LoginRequest(string Email, string Password, string RecaptchaToken, string? Token = "");
