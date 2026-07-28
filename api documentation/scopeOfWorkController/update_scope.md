# Update Scope Endpoint Documentation

This document describes the Update Scope endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate scope of work modifications.

---

## Update Scope Endpoint
Modifies an existing active scope of work.

- **URL:** `/api/ScopeOfWork/{id}`
- **Method:** `PUT`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Parameters
- **`id`** (Path Parameter, Required): The integer ID of the scope of work to update.

### Request Body (`UpdateScopeOfWorkRequestDTO`)
Both `name` and `isNeedInspection` are required.

```json
{
  "name": "Fire Alarm System",
  "isNeedInspection": true
}
```

### Response Body (`ApiResponse<ScopeOfWorkResponseDTO>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "id": 1,
    "name": "Fire Alarm System",
    "isNeedInspection": true
  },
  "errors": []
}
```

#### Scope of Work Not Found (`404 Not Found`)
If the specified ID does not exist in the database or has been soft-deleted:
```json
{
  "statusCode": 404,
  "success": false,
  "data": null,
  "errors": [
    "Scope of work not found."
  ]
}
```

#### Validation Error (`400 Bad Request`)
If required request body validation fails:
```json
{
  "statusCode": 400,
  "success": false,
  "data": null,
  "errors": [
    "The Name field is required."
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
If the authenticated user is not an Admin (e.g., they have the `Viewer` or `Inspector` role):
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
