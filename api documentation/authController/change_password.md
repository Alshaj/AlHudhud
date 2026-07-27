# Change Password Endpoint Documentation

This document describes the Change Password endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate the change password feature.

---

## Change Password Endpoint
Allows authenticated users to change their password.

- **URL:** `/api/auth/change-password`
- **Method:** `POST`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Body (`ChangePasswordRequestDTO`)
```json
{
  "email": "user@example.com",
  "oldPassword": "OldPassword123!",
  "newPassword": "NewPassword123!"
}
```

### Response Body (`ApiResponse<string>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": "Password changed successfully.",
  "errors": []
}
```

#### Validation Error or Identity Failure (`400 Bad Request`)
If the old password is incorrect, or the new password fails password strength requirements (e.g. missing uppercase, numbers, or special characters):
```json
{
  "statusCode": 400,
  "success": false,
  "data": null,
  "errors": [
    "Incorrect password.",
    "Passwords must have at least one non alphanumeric character.",
    "Passwords must have at least one uppercase ('A'-'Z')."
  ]
}
```

#### User Not Found (`404 Not Found`)
If the email specified does not exist in the system:
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

#### Unauthorized (`401 Unauthorized`)
If the user is not authenticated (missing or expired `access_token` cookie):
```json
{
  "statusCode": 401,
  "success": false,
  "data": null,
  "errors": [
    "Unauthorized."
  ]
}
```
