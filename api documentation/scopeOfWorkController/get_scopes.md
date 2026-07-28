# Get Scopes Endpoint Documentation

This document describes the Get Scopes endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to list active scopes of work.

---

## Get Scopes Endpoint
Retrieves the list of all active (non-deleted) scopes of work in the system.

- **URL:** `/api/ScopeOfWork`
- **Method:** `GET`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, roles: `Admin` or `Viewer`)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Body
None.

### Response Body (`ApiResponse<IEnumerable<ScopeOfWorkResponseDTO>>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "Fire Alarm",
      "isNeedInspection": true
    },
    {
      "id": 2,
      "name": "Fire Fighting",
      "isNeedInspection": true
    },
    {
      "id": 3,
      "name": "Electrical Inspection",
      "isNeedInspection": false
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
