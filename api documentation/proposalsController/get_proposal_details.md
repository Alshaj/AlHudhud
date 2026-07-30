# Get Proposal Details Endpoint Documentation

This document describes the Get Proposal Details endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to retrieve a single proposal's details combined with its version history.

---

## Get Proposal Details Endpoint
Retrieves the details of a specific proposal version by its ID and simultaneously lists all historical versions of that proposal.

- **URL:** `/api/Proposals/{id}`
- **Method:** `GET`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, roles: `Admin` or `Viewer`)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Parameters
- **`id`** (Path Parameter, Required): The integer ID of the specific proposal version.

### Response Body (`ApiResponse<ProposalDetailsWithHistoryResponseDTO>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "details": {
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
    "history": [
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
        "id": 1,
        "proposalNumber": "AH-260001",
        "projectScopeId": 3,
        "projectName": "Al Masarak Tower",
        "clientName": "Al Masarak Real Estate",
        "statusId": 3,
        "statusName": "Rejected",
        "referedBy": 4,
        "referedByUserName": "john_inspector",
        "price": 9500.00,
        "vat": 475.00,
        "receivedFromClient": 0.00,
        "pendingAmount": 9975.00,
        "versionNumber": 1,
        "notes": "Initial quote."
      }
    ]
  },
  "errors": []
}
```

#### Proposal Not Found (`404 Not Found`)
If the specified proposal ID does not exist in the database:
```json
{
  "statusCode": 404,
  "success": false,
  "data": null,
  "errors": [
    "Proposal not found."
  ]
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
If the authenticated user does not have access:
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
