# Update Proposal Endpoint Documentation

This document describes the Update Proposal endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to edit proposal data in-place.

---

## Update Proposal Endpoint
Modifies the selected proposal record **in-place** (overwrites the record in the database instead of generating a new version row).

- **URL:** `/api/Proposals/{id}`
- **Method:** `PUT`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Parameters
- **`id`** (Path Parameter, Required): The integer ID of the specific proposal version to modify.

### Request Body (`UpdateProposalRequestDTO`)
- **`referedBy`** (int, Required): The ID of the referring user (must have the `Inspector` role).
- **`price`** (decimal, Required): The updated cost before VAT (must be greater than 0).
- **`receivedFromClient`** (decimal, Required): The payment amount received.
- **`notes`** (string, Optional): General notes or specs.

```json
{
  "referedBy": 4,
  "price": 10500.00,
  "receivedFromClient": 3000.00,
  "notes": "Updated scope description and pricing."
}
```

### Response Body (`ApiResponse<ProposalResponseDTO>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
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
    "price": 10500.00,
    "vat": 525.00,
    "receivedFromClient": 3000.00,
    "pendingAmount": 8025.00,
    "versionNumber": 1,
    "notes": "Updated scope description and pricing."
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
