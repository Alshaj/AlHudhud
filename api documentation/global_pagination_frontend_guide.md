# Global Pagination & Search Integration Guide (Frontend Developer Specification)

This document details the explicit query parameters (`page`, `pageSize`, `search`), response structures, TypeScript interfaces, and list endpoints updated for the Al Hudhud API.

---

## 1. Query Parameters Specification

All list endpoints accept the following optional query parameters:

| Parameter | Type | Default | Description | Example |
|---|---|---|---|---|
| `page` | int | `1` | The 1-indexed page number to retrieve. | `?page=1` |
| `pageSize` | int | `10` | Number of items per page (Min: 1, Max: 100). | `?pageSize=10` |
| `search` | string | `null` | Optional text search filter across names, emails, numbers. | `?search=Al+Hudhud` |

---

## 2. TypeScript Interfaces

```typescript
// Generic Paginated Result Container
export interface PaginatedResult<T> {
  items: T[];
  page: number;
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

### Paginated & Searchable List Endpoints (`ApiResponse<PaginatedResult<T>>`):
1. **Clients:** `GET /api/clients?page=1&pageSize=10&search=`
2. **Proposals:** `GET /api/proposals?page=1&pageSize=10&search=` (Returns latest proposal versions only)
3. **Scopes of Work:** `GET /api/ScopeOfWork?page=1&pageSize=10&search=`
4. **Users:** `GET /api/Users?page=1&pageSize=10&search=`

---

## 4. Response Payloads & Examples

### 4.1. Clients List (`GET /api/clients`)

#### Request:
`GET /api/clients?page=1&pageSize=10&search=Hudhud`

#### Response (`200 OK`):
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
    "page": 1,
    "pageSize": 10,
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

#### Request:
`GET /api/proposals?page=1&pageSize=10`

#### Response (`200 OK`):
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
        "createdAt": "2026-08-03T17:50:00",
        "status": "Pending",
        "versionNumber": 2,
        "notes": "Revised quote after scope meeting."
      }
    ],
    "page": 1,
    "pageSize": 10,
    "totalCount": 1,
    "totalPages": 1,
    "hasPreviousPage": false,
    "hasNextPage": false
  },
  "errors": []
}
```

---

## 5. Frontend Integration Example (Axios)

```typescript
const fetchProposals = async (page = 1, pageSize = 10, search = '') => {
  const response = await axios.get<ApiResponse<PaginatedResult<ProposalItem>>>(
    '/api/proposals',
    {
      params: { page, pageSize, search: search || undefined },
      withCredentials: true,
      headers: {
        'X-Timezone-Offset': -new Date().getTimezoneOffset() / 60 // Dynamically passes offset (e.g. 3)
      }
    }
  );
  return response.data;
};
```
