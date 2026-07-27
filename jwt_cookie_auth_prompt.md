# JWT HttpOnly Cookie Authentication Implementation Template

This document provides a complete guide and template for implementing JWT (JSON Web Token) authentication using HttpOnly cookies in a .NET (C#) Web API project. 

By passing this template to an AI model, you can instruct it to apply this exact authentication pattern to any new project.

## Prompt for AI Model

**Goal:** Implement JWT Authentication using HttpOnly cookies instead of returning the token in the response body. This enhances security by preventing Cross-Site Scripting (XSS) attacks from accessing the token.

Please apply the following configurations and code snippets to my project:

### 1. Configure CORS to Allow Credentials

In `Program.cs`, when configuring CORS, ensure that the policy allows credentials. This is required for the browser to send cookies with cross-origin requests.

```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "https://yourdomain.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // REQUIRED: Allows cookies to be sent cross-origin
    });
});
```

### 2. Configure JWT Bearer to Read Token from Cookie

In `Program.cs`, within the `AddJwtBearer` configuration, you must intercept the authentication message and extract the token from the incoming request's cookies.

```csharp
// Program.cs
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        RoleClaimType = ClaimTypes.Role
    };

    // ✅ ADDITION: Read JWT from HttpOnly cookie
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // The cookie name must match the one set during login
            context.Token = context.Request.Cookies["access_token"];
            return Task.CompletedTask;
        }
    };
});
```

### 3. Inject Cookie in Authentication Handlers (Login/Register)

In your Authentication Controllers (e.g., `AccountController` or `AuthController`), when a user successfully logs in or verifies their email, generate the JWT token and append it to `Response.Cookies` instead of returning it in the JSON body.

```csharp
// Inside your Login or Auth Controller Method
var token = _accountService.GenerateJwtToken(user); // Replace with your token generation logic

// ✅ Inject cookie
Response.Cookies.Append("access_token", token, new CookieOptions
{
    HttpOnly = true, // Prevents JavaScript from accessing the cookie
    Secure = true, // Ensures the cookie is only sent over HTTPS
    SameSite = SameSiteMode.None, // Required for cross-site requests (adjust as needed for same-site)
    Expires = DateTime.UtcNow.AddDays(7), // Token expiration
    Path = "/",
    // Domain = ".yourdomain.com" // Uncomment and set if you need sub-domain access
});

return Ok(response); // Return success without the token in the body
```

### 4. Implement a Logout Endpoint

To log a user out, you need an endpoint that clears the cookie from the user's browser by expiring it.

```csharp
[HttpPost("Logout")]
[Authorize]
public IActionResult Logout()
{
    Response.Cookies.Delete("access_token", new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.None,
        Path = "/"
    });

    return Ok(new { message = "Logged out successfully" });
}
```

### 5. Application Cookie Settings (Optional but Recommended)

If you are also using ASP.NET Identity cookies (which is separate from JWT, but good practice), ensure its cookies are also strictly configured in `Program.cs`.

```csharp
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None; // REQUIRED for cross-site
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS only
});
```

### Implementation Rules for the AI:
- Ensure `AllowCredentials()` is set in the CORS policy.
- Ensure the `OnMessageReceived` event in `JwtBearerEvents` is properly configured to read from `context.Request.Cookies["access_token"]`.
- Ensure all login/registration endpoints that generate tokens append them to the response cookies rather than putting them in the response DTOs.
- Always use `HttpOnly = true` and `Secure = true` for the authentication cookie.
