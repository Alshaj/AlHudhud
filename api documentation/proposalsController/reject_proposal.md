# Reject Proposal Endpoint Documentation

This document describes the Reject Proposal endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate proposal rejections.

---

## Reject Proposal Endpoint
Sets the status of the target proposal to `Rejected` (`StatusId = 3`).

- **URL:** `/api/Proposals/{id}/reject`
- **Method:** `PATCH`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Parameters
- **`id`** (Path Parameter, Required): The integer ID of the specific proposal version to reject.

### Request Body
None.

### Response Body (`ApiResponse<ProposalResponseDTO>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "id": 2,
    "proposalNumber": "AH-260001",
    "projectScopeId": 3,
    "projectName": "Al Masarak Tower",
    "clientName": "Al Masarak Real Estate",
    "statusId": 3,
    "statusName": "Rejected",
    "referedBy": 4,
    "referedByUserName": "john_inspector",
    "price": 10000.00,
    "vat": 500.00,
    "receivedFromClient": 2000.00,
    "pendingAmount": 8500.00,
    "versionNumber": 2,
    "notes": "Declined proposal."
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
If the authenticated user is not an Admin:
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
