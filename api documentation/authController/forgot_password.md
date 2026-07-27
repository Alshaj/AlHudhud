# Forgot Password Endpoint Documentation

This document describes the Forgot Password endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate the password recovery flow.

---

## Forgot Password Endpoint
Initiates the password reset workflow by sending a 6-digit OTP (One-Time Password) to the registered email address.

- **URL:** `/api/auth/forgot-password`
- **Method:** `POST`
- **Authentication Required:** No (Public)

### Request Body (`ForgotPasswordRequestDTO`)
```json
{
  "email": "user@example.com"
}
```

### Response Body (`ApiResponse<string>`)

#### Success (`200 OK`)
To prevent email enumeration attacks, the API returns `200 OK` regardless of whether the email is registered in the database.
```json
{
  "statusCode": 200,
  "success": true,
  "data": "If the email is registered, we have sent a 6-digit OTP.",
  "errors": []
}
```

#### Server SMTP Error (`500 Internal Server Error`)
If there was a problem with the SMTP mail server dispatching the email:
```json
{
  "statusCode": 500,
  "success": false,
  "data": null,
  "errors": [
    "Failed to send reset email. Please try again later."
  ]
}
```
