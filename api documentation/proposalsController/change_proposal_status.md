# Change Proposal Status Endpoint Documentation

This document describes the Change Proposal Status endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate approving or rejecting proposals.

---

## Change Proposal Status Endpoint
Updates the status of an existing proposal directly (e.g., Pending, Approved, or Rejected). 

- **URL:** `/api/proposals/{id}/status`
- **Method:** `PATCH`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Parameters
- **`id`** (Path Parameter, Required): The integer ID of the proposal to update.

### Request Body (`ChangeProposalStatusDTO`)

| Field | Type | Required | Description |
|---|---|---|---|
| `statusId` | int | Yes | The target status ID. Must be 1 (Pending), 2 (Approved), or 3 (Rejected). |

#### Example Request Body:
```json
{
  "statusId": 2
}
```

### Response Body (`ApiResponse<ConfirmationResponseDTO>`)

#### Success (`200 OK`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "message": "Proposal status updated to Approved successfully."
  },
  "errors": []
}
```

#### Bad Request (`400 Bad Request`)
If validation checks fail, or if attempting to approve the proposal (status ID 2) when another proposal for the same project scope is already approved:
```json
{
  "statusCode": 400,
  "success": false,
  "data": null,
  "errors": [
    "Cannot approve this proposal because another proposal is already approved for this project scope."
  ]
}
```

#### Not Found (`404 Not Found`)
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
