# Get All Proposals Endpoint Documentation

This document describes the Get All Proposals endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate the proposals list display.

---

## Get All Proposals Endpoint
Retrieves the list of all proposals in the system.

- **URL:** `/api/proposals`
- **Method:** `GET`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, roles: `Admin` or `Viewer`)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Body
None.

### Response Body (`ApiResponse<List<ProposalResponseDTO>>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": [
    {
      "id": 1,
      "proposalNumber": "AH-260001",
      "clientName": "Al Hudhud Consultancy LLC",
      "projectName": "Warehouse Inspection",
      "scopeOfWork": "Fire Alarm Inspection",
      "location": "Dubai Industrial City",
      "referedBy": "admin@example.com",
      "price": 10000.00,
      "vat": 500.00,
      "totalAmount": 10500.00,
      "createdAt": "2026-07-30T16:17:30.123Z",
      "status": "Pending"
    }
  ],
  "errors": []
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
