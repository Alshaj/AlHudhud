# Delete Scope Endpoint Documentation

This document describes the Delete Scope endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate scope of work deletion.

---

## Delete Scope Endpoint
Soft-deletes a scope of work by marking its `IsDeleted` property to `true`. This preserves relational history in the system while hiding the scope from active lists.

- **URL:** `/api/ScopeOfWork/{id}`
- **Method:** `DELETE`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Parameters
- **`id`** (Path Parameter, Required): The integer ID of the scope of work to delete.

### Response Body (`ApiResponse<string>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": "Scope of work deleted successfully.",
  "errors": []
}
```

#### Scope of Work Not Found (`404 Not Found`)
If the specified ID does not exist, or has already been soft-deleted:
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
