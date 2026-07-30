# Create Proposal Version Endpoint Documentation

This document describes the Create Proposal Version endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to create a new version of an existing proposal.

---

## Create Proposal Version Endpoint
Inserts a **new row** in the database representing a new version of the specified proposal. The new record inherits the `ProposalNumber` and `ProjectScopeId` of the base proposal, increments the `VersionNumber` (e.g. from `1` to `2`), and sets the status to `Pending` (`StatusId = 1`).

- **URL:** `/api/Proposals/{id}/version`
- **Method:** `POST`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Parameters
- **`id`** (Path Parameter, Required): The integer ID of the specific proposal version to use as a base.

### Request Body (`CreateProposalVersionRequestDTO`)
- **`referedBy`** (int, Required): The ID of the referring user (must have the `Inspector` role).
- **`price`** (decimal, Required): The cost of this new proposal version before VAT (must be greater than 0).
- **`receivedFromClient`** (decimal, Required): The payment received for this version.
- **`notes`** (string, Optional): Technical specifications or notes for this version.

```json
{
  "referedBy": 4,
  "price": 11000.00,
  "receivedFromClient": 4000.00,
  "notes": "Added optional thermal camera integration."
}
```

### Response Body (`ApiResponse<ProposalResponseDTO>`)

#### Success (`201 Created`)
```json
{
  "statusCode": 201,
  "success": true,
  "data": {
    "id": 4,
    "proposalNumber": "AH-260001",
    "projectScopeId": 3,
    "projectName": "Al Masarak Tower",
    "clientName": "Al Masarak Real Estate",
    "statusId": 1,
    "statusName": "Pending",
    "referedBy": 4,
    "referedByUserName": "john_inspector",
    "price": 11000.00,
    "vat": 550.00,
    "receivedFromClient": 4000.00,
    "pendingAmount": 7550.00,
    "versionNumber": 2,
    "notes": "Added optional thermal camera integration."
  },
  "errors": []
}
```

#### Proposal Not Found (`404 Not Found`)
If the specified base proposal ID does not exist in the database:
```json
{
  "statusCode": 404,
  "success": false,
  "data": null,
  "errors": [
    "Base proposal not found."
  ]
}
```

#### Inspector Check Fail (`400 Bad Request`)
If the `referedBy` user does not exist or does not possess the `Inspector` role:
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
