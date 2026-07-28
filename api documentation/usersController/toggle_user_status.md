# Toggle User Status Endpoint Documentation

This document describes the Toggle User Status endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to change user status (active/inactive).

---

## Toggle User Status Endpoint
Allows an Admin to activate or deactivate a user account. Deactivating a user immediately blocks them from logging in or refreshing their JWT.

- **URL:** `/api/Users/{id}/status`
- **Method:** `PATCH`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Parameters
- **`id`** (Path Parameter, Required): The integer ID of the user.

### Request Body (`ChangeUserStatusDTO`)
- **`isActive`** (bool, Required): The new activation state.

```json
{
  "isActive": false
}
```

### Response Body (`ApiResponse<UserResponseDTO>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "id": 4,
    "userName": "jane_doe",
    "email": "jane.d@alhudhud.ae",
    "phoneNumber": "0509988776",
    "isActive": false,
    "role": "Inspector",
    "roleId": 2
  },
  "errors": []
}
```

#### User Not Found (`404 Not Found`)
If the specified user ID does not exist in the database:
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
If the user is not authenticated:
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
