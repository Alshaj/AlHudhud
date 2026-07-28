# Get Scope Details Endpoint Documentation

This document describes the Get Scope Details endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to retrieve a single scope of work.

---

## Get Scope Details Endpoint
Retrieves the details of a specific active scope of work by its ID.

- **URL:** `/api/ScopeOfWork/{id}`
- **Method:** `GET`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, roles: `Admin` or `Viewer`)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Parameters
- **`id`** (Path Parameter, Required): The integer ID of the scope of work.

### Response Body (`ApiResponse<ScopeOfWorkResponseDTO>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "id": 1,
    "name": "Fire Alarm",
    "isNeedInspection": true
  },
  "errors": []
}
```

#### Scope of Work Not Found (`404 Not Found`)
If the specified ID does not exist, or if the scope has been soft-deleted (`IsDeleted = true`):
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
