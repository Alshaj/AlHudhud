# Get All Clients Endpoint Documentation

This document describes the Get All Clients endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate and list clients.

---

## Get All Clients Endpoint
Retrieves the list of all clients in the system.

- **URL:** `/api/clients`
- **Method:** `GET`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, roles: `Admin` or `Viewer`)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Body
None.

### Response Body (`ApiResponse<IEnumerable<ClientResponseDTO>>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": [
    {
      "id": 1,
      "clientName": "Al Hudhud Consultancy LLC",
      "taxNumber": "100234567800003",
      "email": "info@alhudhud.ae",
      "companyType": "Corporate"
    },
    {
      "id": 2,
      "clientName": "John Doe Enterprises",
      "taxNumber": null,
      "email": "johndoe@example.com",
      "companyType": "Individual"
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
