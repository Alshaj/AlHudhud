# Create Proposal Endpoint Documentation

This document describes the Create Proposal endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate proposal creation.

---

## Create Proposal Endpoint
Creates a new proposal, generating a sequential numbering string (`AH-YYxxxx`), computing 5% UAE VAT, and mapping dynamic calculations.

- **URL:** `/api/Proposals`
- **Method:** `POST`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Body (`CreateProposalRequestDTO`)
- **`projectScopeId`** (int, Required): The target project scope.
- **`referedBy`** (int, Required): The ID of the user referring the proposal. **Must have the `Inspector` role**.
- **`price`** (decimal, Required): The proposal cost before VAT (must be greater than 0).
- **`receivedFromClient`** (decimal, Required): The payment amount already received from the client.
- **`notes`** (string, Optional): Remarks or additional specifications.

```json
{
  "projectScopeId": 3,
  "referedBy": 4,
  "price": 10000.00,
  "receivedFromClient": 2000.00,
  "notes": "Initial quote for installation works."
}
```

### Response Body (`ApiResponse<ProposalResponseDTO>`)

#### Success (`201 Created`)
```json
{
  "statusCode": 201,
  "success": true,
  "data": {
    "id": 1,
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
    "versionNumber": 1,
    "notes": "Initial quote for installation works."
  },
  "errors": []
}
```

#### Validation Error or Inspector Check Fail (`400 Bad Request`)
If the specified `ReferedBy` user does not exist, does not have the `Inspector` role, or the `ProjectScopeId` is invalid:
```json
{
  "statusCode": 400,
  "success": false,
  "data": null,
  "errors": [
    "Referred user must exist and have the Inspector role."
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
