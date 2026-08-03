# Global Pagination Integration Guide (Frontend Developer Specification)

This document details the standardized pagination query parameters, response structures, TypeScript interfaces, and list endpoints updated for the Al Hudhud API. Use this documentation to instruct frontend developers or AI models on how to integrate paginated tables and controls.

---

## 1. Overview & Default Settings

- **Default Page Number:** `1` (1-indexed).
- **Default Page Size:** `20` items per page.
- **Max Page Size:** `100` items per page.
- **Behavior:** Query parameters are optional. Omitting `pageNumber` or `pageSize` defaults to `pageNumber=1` and `pageSize=20`.

---

## 2. TypeScript Interfaces

```typescript
// Query Parameters
export interface PaginationParameters {
  pageNumber?: number; // Default: 1
  pageSize?: number;   // Default: 20, Max: 100
}

// Generic Paginated Result Container
export interface PaginatedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

// Standardized API Response Wrapper
export interface ApiResponse<T> {
  statusCode: number;
  success: boolean;
  data: T;
  errors: string[];
}
```

---

## 3. Endpoints Summary

### Paginated "Get All" List Endpoints (`ApiResponse<PaginatedResult<T>>`):
1. **Clients:** `GET /api/clients?pageNumber=1&pageSize=20`
2. **Proposals:** `GET /api/proposals?pageNumber=1&pageSize=20` (Returns latest proposal versions only)
3. **Scopes of Work:** `GET /api/ScopeOfWork?pageNumber=1&pageSize=20`
4. **Users:** `GET /api/Users?pageNumber=1&pageSize=20`

### Non-Paginated Endpoints (Unchanged):
- **Roles:** `GET /api/Roles` (Returns un-paginated array `ApiResponse<RoleResponseDTO[]>`)
- **Single Item Details:** `GET /api/clients/{id}`, `GET /api/ScopeOfWork/{id}`, `GET /api/Users/{id}`
- **Proposal History:** `GET /api/proposals/{id}/history` (Returns array of all version history for a proposal number)

---

## 4. Paginated Response Payloads & Examples

### 4.1. Clients List (`GET /api/clients`)

#### Example Request:
`GET /api/clients?pageNumber=1&pageSize=20`

#### Example Response (`200 OK`):
```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "items": [
      {
        "id": 1,
        "clientName": "Al Hudhud Consultancy LLC",
        "taxNumber": "100234567800003",
        "email": "info@alhudhud.ae",
        "companyType": "Corporate LLC"
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

### 4.2. Proposals List (`GET /api/proposals`)

#### Example Request:
`GET /api/proposals?pageNumber=1&pageSize=20`

#### Example Response (`200 OK`):
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

### 4.3. Scopes of Work List (`GET /api/ScopeOfWork`)

#### Example Request:
`GET /api/ScopeOfWork?pageNumber=1&pageSize=20`

#### Example Response (`200 OK`):
```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "items": [
      {
        "id": 1,
        "name": "Fire Safety Inspection",
        "isNeedInspection": true
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

### 4.4. Users List (`GET /api/Users`)

#### Example Request:
`GET /api/Users?pageNumber=1&pageSize=20`

#### Example Response (`200 OK`):
```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "items": [
      {
        "id": 1,
        "userName": "admin@example.com",
        "email": "admin@example.com",
        "phoneNumber": "+971501234567",
        "isActive": true,
        "role": "Admin",
        "roleId": 1
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

## 5. Frontend UI Integration Workflow

### 5.1. Managing State
For table components rendering paginated data, maintain the following state:
- `pageNumber`: current active page (default `1`).
- `pageSize`: items per page dropdown selection (default `20`).

### 5.2. Fetching Data
When `pageNumber` or `pageSize` changes:
```typescript
const fetchClients = async (page = 1, size = 20) => {
  const response = await axios.get<ApiResponse<PaginatedResult<Client>>>(
    `/api/clients?pageNumber=${page}&pageSize=${size}`,
    { withCredentials: true }
  );
  
  if (response.data.success) {
    setClients(response.data.data.items);
    setTotalPages(response.data.data.totalPages);
    setTotalCount(response.data.data.totalCount);
    setHasNextPage(response.data.data.hasNextPage);
    setHasPreviousPage(response.data.data.hasPreviousPage);
  }
};
```

### 5.3. Rendering Pagination Footer
- **Previous Button**: Disable when `hasPreviousPage === false` or `pageNumber === 1`.
- **Next Button**: Disable when `hasNextPage === false` or `pageNumber >= totalPages`.
- **Page Indicator**: Show `"Page {pageNumber} of {totalPages}"` or `"Showing {items.length} of {totalCount} entries"`.
