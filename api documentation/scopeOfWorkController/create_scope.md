# Create Scope Endpoint Documentation

This document describes the Create Scope endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate scope of work creation.

---

## Create Scope Endpoint
Creates a new scope of work in the system.

- **URL:** `/api/ScopeOfWork`
- **Method:** `POST`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Body (`CreateScopeOfWorkRequestDTO`)
Both `name` and `isNeedInspection` are required.

```json
{
  "name": "Mechanical Inspection",
  "isNeedInspection": true
}
```

### Response Body (`ApiResponse<ScopeOfWorkResponseDTO>`)

#### Success (`201 Created`)
```json
{
  "statusCode": 201,
  "success": true,
  "data": {
    "id": 4,
    "name": "Mechanical Inspection",
    "isNeedInspection": true
  },
  "errors": []
}
```

#### Validation Error (`400 Bad Request`)
If the required fields are missing or validation rules fail (e.g., name is too long):
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
