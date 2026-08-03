# Timezone Adjustment Integration Guide (Frontend Developer Specification)

This document describes how the Al Hudhud API handles timezone conversion for timestamp fields (such as `createdAt`) using the `X-Timezone-Offset` HTTP request header. Use this guide to instruct frontend developers or AI agents on how to send user timezone preferences.

---

## 1. Overview

The backend database stores all timestamps in **UTC**. When returning timestamps to the frontend (e.g. `createdAt` in proposals), the backend automatically adjusts the timestamp to local time based on the `X-Timezone-Offset` header provided in the HTTP request.

---

## 2. Request Header Specification

| Header Name | Type | Required? | Default | Description | Example |
|---|---|---|---|---|---|
| `X-Timezone-Offset` | Number / String | Optional | `3` | Timezone offset from UTC in hours. | `3` (for Yemen / GMT+3), `3.5`, `-5` |

### Default Fallback:
If the `X-Timezone-Offset` header is omitted from a request, the backend automatically defaults to **`3`** (+3 hours for Yemen / Arab Standard Time).

---

## 3. Frontend Integration Code Examples

### 3.1. Calculating Browser Timezone Offset in JavaScript / TypeScript
You can dynamically calculate the user's local timezone offset in hours:

```typescript
// Calculates current browser offset in hours (e.g., 3 for GMT+3)
export const getLocalTimezoneOffset = (): number => {
  return -new Date().getTimezoneOffset() / 60;
};
```

### 3.2. Configuring Axios Globally
Set the header globally in your API client so all requests automatically transmit the timezone offset:

```typescript
import axios from 'axios';

const api = axios.create({
  baseURL: '/api',
  withCredentials: true, // Required for HttpOnly cookies
  headers: {
    'X-Timezone-Offset': -new Date().getTimezoneOffset() / 60 // Dynamically passes 3 for GMT+3
  }
});

export default api;
```

### 3.3. Fetch API Example
```typescript
const fetchProposals = async () => {
  const response = await fetch('/api/proposals?pageNumber=1&pageSize=20', {
    method: 'GET',
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json',
      'X-Timezone-Offset': '3' // Pass 3 for Yemen (+3 hours)
    }
  });
  
  const data = await response.json();
  return data;
};
```

---

## 4. Affected Endpoints & Example Output

### Endpoints Converting `createdAt`:
- `GET /api/proposals` (List proposals)
- `GET /api/proposals/{id}/history` (Proposal version history)

### Example Output Comparison

#### Server Database (Stored UTC Time):
`2026-08-03 14:00:00.000`

#### API Response when sending `X-Timezone-Offset: 3` (Yemen Time):
```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "items": [
      {
        "id": 5,
        "proposalNumber": "AH-260001",
        "createdAt": "2026-08-03T17:00:00",
        "status": "Pending",
        "versionNumber": 2
      }
    ],
    "pageNumber": 1,
    "pageSize": 20,
    "totalCount": 1
  },
  "errors": []
}
```
*(Notice: 14:00 UTC + 3 hours = 17:00 Yemen Local Time)*
