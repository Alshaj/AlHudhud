# Update Proposal Endpoint Documentation

This document describes the Update Proposal endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate proposal details modification.

---

## Update Proposal Endpoint
Updates all fields of an existing proposal in-place. The backend updates the underlying Project, ProjectScope, and Proposal records directly without creating a new revision row, preserving the original creation date (`CreatedAt`).

- **URL:** `/api/proposals/{id}`
- **Method:** `PUT`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Parameters
- **`id`** (Path Parameter, Required): The integer ID of the proposal to update.

### Request Body (`UpdateProposalRequestDTO`)

| Field | Type | Required | Description |
|---|---|---|---|
| `clientId` | int | Yes | The ID of the existing client selected by the admin. |
| `projectName` | string | Yes | The updated name of the project. |
| `location` | string | Yes | The updated location of the project. |
| `scopeOfWorkId` | int | Yes | The updated ID of the scope of work. |
| `referedById` | int | Yes | The ID of the system user who referred the client. |
| `price` | decimal | Yes | The updated base price. Must be greater than 0. |
| `notes` | string | No | Updated notes for the proposal. |

#### Example Request Body:
```json
{
  "clientId": 1,
  "projectName": "Warehouse Inspection - Phase 2",
  "location": "Dubai Industrial City - Block B",
  "scopeOfWorkId": 2,
  "referedById": 1,
  "price": 12000.00,
  "notes": "Price adjusted after scope update."
}
```

### Response Body (`ApiResponse<ConfirmationResponseDTO>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "message": "Proposal AH-260001 updated successfully."
  },
  "errors": []
}
```

#### Bad Request (`400 Bad Request`)
If validation checks fail, or if the project scope is closed/completed (has an existing Certificate):
```json
{
  "statusCode": 400,
  "success": false,
  "data": null,
  "errors": [
    "Cannot edit proposal because the project scope is closed or completed."
  ]
}
```

#### Not Found (`404 Not Found`)
If the referenced proposal ID, client, scope of work, or referring user does not exist:
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
