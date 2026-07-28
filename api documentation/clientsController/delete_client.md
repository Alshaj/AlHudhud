# Delete Client Endpoint Documentation

This document describes the Delete Client endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate client deletion.

---

## Delete Client Endpoint
Deletes a client from the system. 

- **URL:** `/api/clients/{id}`
- **Method:** `DELETE`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Parameters
- **`id`** (Path Parameter, Required): The integer ID of the client to delete.

### Response Body (`ApiResponse<string>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": "Client deleted successfully.",
  "errors": []
}
```

#### Client Has Associated Projects (`400 Bad Request`)
If the client has projects registered in the system, deletion will be blocked:
```json
{
  "statusCode": 400,
  "success": false,
  "data": null,
  "errors": [
    "Cannot delete client because they have associated projects."
  ]
}
```

#### Client Not Found (`404 Not Found`)
If the specified ID does not exist in the database:
```json
{
  "statusCode": 404,
  "success": false,
  "data": null,
  "errors": [
    "Client not found."
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
