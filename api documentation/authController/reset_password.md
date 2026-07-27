# Reset Password Endpoint Documentation

This document describes the Reset Password endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to verify the OTP and apply the new password.

---

## Reset Password Endpoint
Validates the 6-digit OTP and applies the new password to the user account.

- **URL:** `/api/auth/reset-password`
- **Method:** `POST`
- **Authentication Required:** No (Public)

### Request Body (`ResetPasswordRequestDTO`)
- `otp` must be exactly 6 digits.
```json
{
  "email": "user@example.com",
  "otp": "123456",
  "newPassword": "NewSecurePassword123!"
}
```

### Response Body (`ApiResponse<string>`)

#### Success (`200 OK`)
If the OTP is correct, not expired, and the password matches all system validation rules:
```json
{
  "statusCode": 200,
  "success": true,
  "data": "Password has been reset successfully.",
  "errors": []
}
```

#### Validation Error or Expired OTP (`400 Bad Request`)
If the OTP is invalid, expired (longer than 5 minutes), or the password fails strength validation checks:
```json
{
  "statusCode": 400,
  "success": false,
  "data": null,
  "errors": [
    "Invalid or expired OTP.",
    "Passwords must have at least one non alphanumeric character."
  ]
}
```

#### User Not Found (`404 Not Found`)
```json
{
  "statusCode": 404,
  "success": false,
  "data": null,
  "errors": [
    "User not found."
  ]
}
```
