# Create Client Endpoint Documentation

This document describes the Create Client endpoint implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate client creation.

---

## Create Client Endpoint
Creates a new client in the system.

- **URL:** `/api/clients`
- **Method:** `POST`
- **Authentication Required:** Yes (Requires a valid `access_token` cookie, role: `Admin` only)

### Request Header
Since the API uses HttpOnly cookies, the browser will automatically include the `access_token` cookie in the request if credentials are enabled (`withCredentials = true` in Axios or `credentials: 'include'` in Fetch).

### Request Body (`CreateClientRequestDTO`)
Only `clientName` is required. The fields `taxNumber`, `email`, and `companyType` are optional and can be left out, set to null, or empty.

```json
{
  "clientName": "Gulf Enterprises LLC",
  "taxNumber": "100554433221109",
  "email": "contact@gulfent.com",
  "companyType": "Corporate"
}
```

*Example with only the required ClientName:*
```json
{
  "clientName": "Individual Contractor Name"
}
```

### Response Body (`ApiResponse<CreateClientResponseDTO>`)

#### Success (`201 Created`)
```json
{
  "statusCode": 201,
  "success": true,
  "data": {
    "id": 3,
    "message": "Client created successfully."
  },
  "errors": []
}
```

#### Validation Error (`400 Bad Request`)
If `clientName` is missing, or any field exceeds the length restrictions, or if the email format is invalid:
```json
{
  "statusCode": 400,
  "success": false,
  "data": null,
  "errors": [
    "The ClientName field is required.",
    "The Email field is not a valid e-mail address."
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
