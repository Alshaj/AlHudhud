# Update User Endpoint Documentation

This document describes the Update User endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate user modifications.

---

## Update User Endpoint
Modifies user account properties and reassigns roles based on the requested `RoleId`.

- **URL:** `/api/Users/{id}`
- **Method:** `PUT`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Parameters
- **`id`** (Path Parameter, Required): The integer ID of the user to update.

### Request Body (`UpdateUserRequestDTO`)
- **`userName`** (string, Required): The unique login name.
- **`email`** (string, Required): The unique user email address.
- **`phoneNumber`** (string, Optional): The contact number.
- **`roleId`** (int, Required): The ID of the role to assign. The list of roles and their IDs should be retrieved dynamically from the [Get Roles](file:///c:/Users/abdurahman/source/repos/AlHudhud/AlHudhud/api%20documentation/rolesController/get_roles.md) endpoint.

```json
{
  "userName": "jane_doe_updated",
  "email": "jane.u@alhudhud.ae",
  "phoneNumber": "0509988776",
  "roleId": 3
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
    "userName": "jane_doe_updated",
    "email": "jane.u@alhudhud.ae",
    "phoneNumber": "0509988776",
    "isActive": true,
    "role": "Viewer",
    "roleId": 3
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

#### Validation Error or Identity Failure (`400 Bad Request`)
If the role ID is invalid or the changes violate Identity constraints (e.g. email already used by another account):
```json
{
  "statusCode": 400,
  "success": false,
  "data": null,
  "errors": [
    "Email 'jane.u@alhudhud.ae' is already in use."
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
