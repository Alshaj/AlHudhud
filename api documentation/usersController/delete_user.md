# Delete User Endpoint Documentation

This document describes the Delete User endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate user deletion.

---

## Delete User Endpoint
Soft-deletes a user from the system by marking their `IsActive` status to `false`. This prevents them from logging in or refreshing tokens, while preserving their historical references (e.g. inspections, proposals).

- **URL:** `/api/Users/{id}`
- **Method:** `DELETE`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Parameters
- **`id`** (Path Parameter, Required): The integer ID of the user to soft-delete.

### Response Body (`ApiResponse<string>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": "User deleted successfully.",
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
