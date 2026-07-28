# Get User Details Endpoint Documentation

This document describes the Get User Details endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to retrieve a single user's profile information.

---

## Get User Details Endpoint
Retrieves the profile details of a specific user by their ID.

- **URL:** `/api/Users/{id}`
- **Method:** `GET`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, roles: `Admin` or `Viewer`)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Parameters
- **`id`** (Path Parameter, Required): The integer ID of the user.

### Response Body (`ApiResponse<UserResponseDTO>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "id": 2,
    "userName": "john_inspector",
    "email": "john.i@alhudhud.ae",
    "phoneNumber": "0507654321",
    "isActive": true,
    "role": "Inspector",
    "roleId": 2
  },
  "errors": []
}
```

#### User Not Found (`404 Not Found`)
If the specified user ID does not exist in the system:
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

#### Forbidden (`403 Forbidden`)
If the authenticated user does not have the necessary permissions (e.g., has the role `Inspector`):
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
