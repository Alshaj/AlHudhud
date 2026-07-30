# Get Proposals Endpoint Documentation

This document describes the Get Proposals endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate the list of latest proposals.

---

## Get Proposals Endpoint
Retrieves the list of all proposals in the system. The results are grouped by `ProposalNumber`, displaying only the latest version of each proposal.

- **URL:** `/api/Proposals`
- **Method:** `GET`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, roles: `Admin` or `Viewer`)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Body
None.

### Response Body (`ApiResponse<IEnumerable<ProposalResponseDTO>>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": [
    {
      "id": 2,
      "proposalNumber": "AH-260001",
      "projectScopeId": 3,
      "projectName": "Al Masarak Tower",
      "clientName": "Al Masarak Real Estate",
      "statusId": 1,
      "statusName": "Pending",
      "referedBy": 4,
      "referedByUserName": "john_inspector",
      "price": 10000.00,
      "vat": 500.00,
      "receivedFromClient": 2000.00,
      "pendingAmount": 8500.00,
      "versionNumber": 2,
      "notes": "Updated version 2 with revised specifications."
    },
    {
      "id": 3,
      "proposalNumber": "AH-260002",
      "projectScopeId": 4,
      "projectName": "Hudhud Villa",
      "clientName": "Abdurahman Client",
      "statusId": 2,
      "statusName": "Approved",
      "referedBy": 4,
      "referedByUserName": "john_inspector",
      "price": 15000.00,
      "vat": 750.00,
      "receivedFromClient": 15750.00,
      "pendingAmount": 0.00,
      "versionNumber": 1,
      "notes": "Approved proposal."
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
If the authenticated user does not have the necessary permissions:
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
