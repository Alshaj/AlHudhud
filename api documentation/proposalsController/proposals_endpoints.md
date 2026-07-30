# Proposals Endpoints Documentation

This document describes the proposals endpoints implemented for the Al Hudhud API. Use this documentation to integrate the proposals list and creation flows on the frontend.

---

## 1. Get All Proposals

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

---

## 2. Create Proposal

Allows the admin to create a new proposal. The backend automatically creates the associated project and project scope records, generates a sequential proposal number, calculates VAT (5%) and Total Amount, and sets the default status to `Pending` and version to `1`.

- **URL:** `/api/proposals`
- **Method:** `POST`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, roles: `Admin` only)

### Request Header
Same as GET.

### Request Body (`CreateProposalRequestDTO`)

| Field | Type | Required | Description |
|---|---|---|---|
| `clientId` | int | Yes | The ID of the existing client selected by the admin. |
| `projectName` | string | Yes | The name of the project to be created. |
| `location` | string | Yes | The location of the project and project scope. |
| `scopeOfWorkId` | int | Yes | The ID of the existing scope of work selected by the admin. |
| `referedById` | int | Yes | The ID of the system user who referred the client. |
| `price` | decimal | Yes | The base price of the proposal. Must be greater than 0. |
| `notes` | string | No | Optional notes for the proposal. |

#### Example Request Body:
```json
{
  "clientId": 1,
  "projectName": "Warehouse Inspection",
  "location": "Dubai Industrial City",
  "scopeOfWorkId": 2,
  "referedById": 1,
  "price": 10000.00,
  "notes": "Urgent inspection requested."
}
```

### Response Body (`ApiResponse<ConfirmationResponseDTO>`)

#### Success (`201 Created`)
```json
{
  "statusCode": 201,
  "success": true,
  "data": {
    "message": "Proposal AH-260001 created successfully."
  },
  "errors": []
}
```

#### Bad Request (`400 Bad Request`)
If validation checks fail (e.g. invalid request fields, price is 0, etc.):
```json
{
  "statusCode": 400,
  "success": false,
  "data": null,
  "errors": [
    "Price must be greater than zero."
  ]
}
```

#### Not Found (`404 Not Found`)
If the referenced `clientId`, `scopeOfWorkId`, or `referedById` user doesn't exist:
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

#### Unauthorized/Forbidden (`401` / `403`)
If the user is not authenticated or is not an `Admin`.

---

## 3. Update Proposal (In-Place Update)

Allows the admin to update all fields of an existing proposal in-place. The backend updates the underlying Project, ProjectScope, and Proposal records directly without creating a new revision row, and preserves the original creation date (`CreatedAt`).

- **URL:** `/api/proposals/{id}`
- **Method:** `PUT`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, roles: `Admin` only)

### Request Header
Same as GET.

### Request Body (`UpdateProposalRequestDTO`)

Same fields as `CreateProposalRequestDTO`:

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

#### Unauthorized/Forbidden (`401` / `403`)
If the user is not authenticated or is not an `Admin`.

