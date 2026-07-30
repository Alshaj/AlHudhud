# Create Proposal Endpoint Documentation

This document describes the Create Proposal endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate new proposal creation.

---

## Create Proposal Endpoint
Allows the admin to create a new proposal. The backend automatically creates the associated project and project scope records, generates a sequential proposal number, calculates VAT (5%) and Total Amount, and sets the default status to `Pending` and version to `1`.

- **URL:** `/api/proposals`
- **Method:** `POST`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, roles: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

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
