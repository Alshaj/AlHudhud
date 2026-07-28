# Get Users Endpoint Documentation

This document describes the Get Users endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate the list of users.

---

## Get Users Endpoint
Retrieves the list of all registered users in the system, including their active statuses and roles.

- **URL:** `/api/Users`
- **Method:** `GET`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, roles: `Admin` or `Viewer`)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Body
None.

### Response Body (`ApiResponse<IEnumerable<UserResponseDTO>>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": [
    {
      "id": 1,
      "userName": "admin",
      "email": "admin@alhudhud.ae",
      "phoneNumber": "0501234567",
      "isActive": true,
      "role": "Admin",
      "roleId": 1
    },
    {
      "id": 2,
      "userName": "john_inspector",
      "email": "john.i@alhudhud.ae",
      "phoneNumber": "0507654321",
      "isActive": true,
      "role": "Inspector",
      "roleId": 2
    },
    {
      "id": 3,
      "userName": "sarah_viewer",
      "email": "sarah.v@alhudhud.ae",
      "phoneNumber": null,
      "isActive": false,
      "role": "Viewer",
      "roleId": 3
    }
  ],
  "errors": []
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
