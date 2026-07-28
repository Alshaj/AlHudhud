# Resend OTP Endpoint Documentation

This document describes the Resend OTP endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate the OTP resending features.

---

## Resend OTP Endpoint
Regenerates a fresh 6-digit OTP (valid for 5 minutes) and resends it to the user's email address.

- **URL:** `/api/auth/resend-otp`
- **Method:** `POST`
- **Authentication Required:** No
- **Rate Limited:** Yes (Uses the `AuthLimiter` policy)

### Request Header
Standard headers.

### Request Body (`ResendOtpRequestDTO`)
```json
{
  "email": "user@example.com"
}
```

### Response Body (`ApiResponse<string>`)

#### Success (`200 OK`)
For security, if the email is not registered in the system, it will still return a success message.
```json
{
  "statusCode": 200,
  "success": true,
  "data": "If the email is registered, a new OTP has been sent.",
  "errors": []
}
```

#### Validation Error (`400 Bad Request`)
If the email field is missing or is not a valid email address:
```json
{
  "statusCode": 400,
  "success": false,
  "data": null,
  "errors": [
    "The Email field is required.",
    "The Email field is not a valid e-mail address."
  ]
}
```

#### Too Many Requests (`429 Too Many Requests`)
If the client IP exceeds the limit of the `AuthLimiter` policy (max 5 requests per minute):
```
Too many login attempts. Please try again later.
```
