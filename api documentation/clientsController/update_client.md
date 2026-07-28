# Update Client Endpoint Documentation

This document describes the Update Client endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate client details modification.

---

## Update Client Endpoint
Updates an existing client's details.

- **URL:** `/api/clients/{id}`
- **Method:** `PUT`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Parameters
- **`id`** (Path Parameter, Required): The integer ID of the client to update.

### Request Body (`UpdateClientRequestDTO`)
Only `clientName` is required. Optional properties (`taxNumber`, `email`, `companyType`) can be null or omitted to overwrite them with empty/null values.

```json
{
  "clientName": "Al Hudhud Consultancy LLC (Updated)",
  "taxNumber": "100234567800003",
  "email": "operations@alhudhud.ae",
  "companyType": "Corporate LLC"
}
```

### Response Body (`ApiResponse<ClientResponseDTO>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "id": 1,
    "clientName": "Al Hudhud Consultancy LLC (Updated)",
    "taxNumber": "100234567800003",
    "email": "operations@alhudhud.ae",
    "companyType": "Corporate LLC"
  },
  "errors": []
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

#### Validation Error (`400 Bad Request`)
If `clientName` is missing, or formatting validation fails:
```json
{
  "statusCode": 400,
  "success": false,
  "data": null,
  "errors": [
    "The ClientName field is required."
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
