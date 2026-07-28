# Create User Endpoint Documentation

This document describes the Create User endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate user account creation.

---

## Create User Endpoint
Creates a new user account with `IsActive` set to `true` by default and assigns the requested role.

- **URL:** `/api/Users`
- **Method:** `POST`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Body (`CreateUserRequestDTO`)
- **`userName`** (string, Required): The unique login name.
- **`email`** (string, Required): The unique user email address.
- **`phoneNumber`** (string, Optional): The contact number.
- **`password`** (string, Required): The user login password (minimum 6 characters).
- **`roleId`** (int, Required): The ID of the role to assign (`1` for Admin, `2` for Inspector, `3` for Viewer).

```json
{
  "userName": "jane_doe",
  "email": "jane.d@alhudhud.ae",
  "phoneNumber": "0509988776",
  "password": "Password123!",
  "roleId": 2
}
```

### Response Body (`ApiResponse<UserResponseDTO>`)

#### Success (`201 Created`)
```json
{
  "statusCode": 201,
  "success": true,
  "data": {
    "id": 4,
    "userName": "jane_doe",
    "email": "jane.d@alhudhud.ae",
    "phoneNumber": "0509988776",
    "isActive": true,
    "role": "Inspector",
    "roleId": 2
  },
  "errors": []
}
```

#### Validation Error or Identity Failure (`400 Bad Request`)
If the role ID is invalid, input validation fails, or Identity rules (e.g. username already taken, weak password) are violated:
```json
{
  "statusCode": 400,
  "success": false,
  "data": null,
  "errors": [
    "Passwords must have at least one uppercase ('A'-'Z').",
    "Username 'jane_doe' is already taken."
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

#### Forbidden (`403 Forbidden`)
If the authenticated user is not an Admin:
```json
{
  "statusCode": 403,
  "success": false,
  "data": null,
  "errors": [
    "Forbidden."
  ]
}
```
