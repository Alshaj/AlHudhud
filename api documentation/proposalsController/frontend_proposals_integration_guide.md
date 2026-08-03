# Proposals Versioning & History Frontend Specification

This document details the updated `GET /api/proposals` endpoint response structure and the two new proposal versioning endpoints (`POST /api/proposals/{id}/version` and `GET /api/proposals/{id}/history`). Use this specification to integrate proposal versioning and history in the frontend application.

---

## 1. TypeScript Interfaces

```typescript
// Proposal Item Model (Returned by GET /api/proposals and GET /api/proposals/{id}/history)
export interface ProposalItem {
  id: number;
  proposalNumber: string;
  clientName: string;
  projectName: string;
  scopeOfWork: string;
  location: string;
  referedBy: string;
  price: number;
  vat: number;
  totalAmount: number;
  createdAt: string; // ISO 8601 Date String (e.g., "2026-08-03T17:50:00.000Z")
  status: 'Pending' | 'Approved' | 'Rejected';
  versionNumber: number;
  notes: string;
}

// Request Payload for Creating a New Proposal Version (POST /api/proposals/{id}/version)
export interface CreateProposalVersionRequest {
  price: number;
  notes?: string;
}

// Paginated Result Wrapper
export interface PaginatedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

// Generic API Response Wrapper
export interface ApiResponse<T> {
  statusCode: number;
  success: boolean;
  data: T;
  errors: string[];
}
```

---

## 2. Updated Endpoint: Get All Proposals (Latest Version Only & Paginated)

The `GET /api/proposals` endpoint filters and returns **only the latest version** (`highest versionNumber`) for each proposal number, paginated with default `pageSize = 20`.

- **URL:** `/api/proposals?pageNumber=1&pageSize=20`
- **Method:** `GET`
- **Authentication Required:** Yes (HttpOnly Cookie `access_token`, `withCredentials: true`)
- **Roles Allowed:** `Admin`, `Viewer`
- **Headers (Optional):** `X-Timezone-Offset: 3` (Number of offset hours, e.g. `3` for Yemen GMT+3. Defaults to `3` if omitted). Backend adjusts `createdAt` by adding this offset.

### Query Parameters (Optional):
- `pageNumber` (number, default: `1`): The 1-indexed page number.
- `pageSize` (number, default: `20`): Number of proposals per page (Max: 100).

### Response Payload (`ApiResponse<PaginatedResult<ProposalItem>>`)

```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "items": [
    {
      "id": 5,
      "proposalNumber": "AH-260001",
      "clientName": "Al Hudhud Consultancy LLC",
      "projectName": "Warehouse Inspection",
      "scopeOfWork": "Fire Alarm Inspection",
      "location": "Dubai Industrial City",
      "referedBy": "admin@example.com",
      "price": 12500.00,
      "vat": 625.00,
      "totalAmount": 13125.00,
      "createdAt": "2026-08-03T17:50:00.000Z",
      "status": "Pending",
      "versionNumber": 2,
      "notes": "Revised quote after scope meeting."
    }
    ],
    "pageNumber": 1,
    "pageSize": 20,
    "totalCount": 1,
    "totalPages": 1,
    "hasPreviousPage": false,
    "hasNextPage": false
  },
  "errors": []
}
```

---

## 3. New Endpoint: Create Proposal Version

Creates a new version of an existing proposal with an updated price. The backend automatically marks all previous version records for this proposal number as **`Rejected`**, sets the new version status to **`Pending`**, and increments `versionNumber`.

- **URL:** `/api/proposals/{id}/version`
- **Method:** `POST`
- **Authentication Required:** Yes (HttpOnly Cookie `access_token`, `withCredentials: true`)
- **Roles Allowed:** `Admin`

### Path Parameters:
- `id` (number, required): The ID of the existing proposal to base the new version on.

### Request Body (`CreateProposalVersionRequest`):
```json
{
  "price": 12500.00,
  "notes": "Revised price after secondary scope negotiation."
}
```

### Response Payload (`ApiResponse<{ message: string }>`)

#### Success (`201 Created`):
```json
{
  "statusCode": 201,
  "success": true,
  "data": {
    "message": "Proposal AH-260001 version 2 created successfully."
  },
  "errors": []
}
```

#### Bad Request (`400 Bad Request`):
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

#### Not Found (`404 Not Found`):
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

---

## 4. New Endpoint: Get Proposal History

Retrieves all historical versions associated with a proposal number, ordered by `versionNumber` descending.

- **URL:** `/api/proposals/{id}/history`
- **Method:** `GET`
- **Authentication Required:** Yes (HttpOnly Cookie `access_token`, `withCredentials: true`)
- **Roles Allowed:** `Admin`, `Viewer`

### Path Parameters:
- `id` (number, required): The ID of any proposal version belonging to the proposal group.

### Response Payload (`ApiResponse<ProposalItem[]>`)

#### Success (`200 OK`):
```json
{
  "statusCode": 200,
  "success": true,
  "data": [
    {
      "id": 5,
      "proposalNumber": "AH-260001",
      "clientName": "Al Hudhud Consultancy LLC",
      "projectName": "Warehouse Inspection",
      "scopeOfWork": "Fire Alarm Inspection",
      "location": "Dubai Industrial City",
      "referedBy": "admin@example.com",
      "price": 12500.00,
      "vat": 625.00,
      "totalAmount": 13125.00,
      "createdAt": "2026-08-03T17:50:00.000Z",
      "status": "Pending",
      "versionNumber": 2,
      "notes": "Revised quote after scope meeting."
    },
    {
      "id": 1,
      "proposalNumber": "AH-260001",
      "clientName": "Al Hudhud Consultancy LLC",
      "projectName": "Warehouse Inspection",
      "scopeOfWork": "Fire Alarm Inspection",
      "location": "Dubai Industrial City",
      "referedBy": "admin@example.com",
      "price": 10000.00,
      "vat": 500.00,
      "totalAmount": 10500.00,
      "createdAt": "2026-07-30T16:17:30.123Z",
      "status": "Rejected",
      "versionNumber": 1,
      "notes": "Original proposal quote."
    }
  ],
  "errors": []
}
```

#### Not Found (`404 Not Found`):
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
