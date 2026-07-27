# Al Hudhud System - Authentication API Documentation

This document describes the authentication endpoints implemented for the Al Hudhud API. Use this documentation to instruct frontend models on how to integrate the API using secure HttpOnly cookies.

---

## Authentication Design Pattern: JWT HttpOnly Cookies

To mitigate Cross-Site Scripting (XSS) attacks, this API does **NOT** return JWT Access or Refresh tokens in the JSON response body. Instead, they are injected directly into the user's browser via secure `HttpOnly` cookies.

### Key Rules for the Frontend:
1. **With Credentials:** The frontend application must send requests with credentials enabled.
   - If using **Axios**, configure: `axios.defaults.withCredentials = true;` or pass `{ withCredentials: true }` in each request config.
   - If using **Fetch**, configure: `credentials: 'include'` in the options object.
2. **CORS:** Ensure the API's CORS policy is configured to allow your frontend's specific origin (e.g. `http://localhost:3000`). Wildcard (`*`) origins cannot be used when `withCredentials` is active.

---

## 1. Login Endpoint
Authenticates a user and issues access/refresh tokens in secure cookies.

- **URL:** `/api/auth/login`
- **Method:** `POST`
- **Authentication Required:** No (Public)
- **Content-Type:** `application/json`

### Request Body (`LoginRequestDTO`)
```json
{
  "email": "user@example.com",
  "password": "Password123!"
}
```

### Response Body (`ApiResponse<LoginResponseDTO>`)
Returns status `200 OK` on success. Tokens are injected as cookies and not included in this JSON response.

```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "id": 1,
    "email": "user@example.com",
    "userName": "user@example.com",
    "roles": ["Admin"]
  },
  "errors": []
}
```

### Response Cookies (Set-Cookie Headers)
- `access_token`: Stores the JWT access token. Expires in **15 minutes**.
- `refresh_token`: Stores the cryptographically secure rotation token. Expires in **1 day** (forcing user re-authentication daily).
- Both cookies are configured with:
  - `HttpOnly = true` (hidden from JavaScript to prevent XSS)
  - `Secure = true` (transmitted only over HTTPS)
  - `SameSite = None` (required for cross-site browser environments)
  - `Path = /`

---

## 2. Refresh Token Endpoint
Regenerates the access token only, leaving the original refresh token unchanged. This guarantees that the user will be forced to log in again after exactly 1 day since their original login, as the refresh token's expiry is not extended.

- **URL:** `/api/auth/refresh-token`
- **Method:** `POST`
- **Authentication Required:** No (Sends the `refresh_token` cookie automatically via credentials)

### Request
No request body is needed. The browser automatically sends the `refresh_token` cookie along with the request.

### Response Body (`ApiResponse<LoginResponseDTO>`)
Returns status `200 OK` on success, containing user details.

```json
{
  "statusCode": 200,
  "success": true,
  "data": {
    "id": 1,
    "email": "user@example.com",
    "userName": "user@example.com",
    "roles": ["Admin"]
  },
  "errors": []
}
```

### Response Cookies (Set-Cookie Headers)
Refreshes and replaces the access token cookie:
- `access_token`: Extended for another **15 minutes**.
- `refresh_token`: Stays unchanged (will expire exactly 1 day after the initial login).

---

## 3. Logout Endpoint
Revokes the user's refresh token in the database and clears the authentication cookies in the browser.

- **URL:** `/api/auth/logout`
- **Method:** `POST`
- **Authentication Required:** No (Sends the `refresh_token` cookie to identify and revoke it in the DB)

### Request
No request body is needed.

### Response Body (`ApiResponse<string>`)
```json
{
  "statusCode": 200,
  "success": true,
  "data": "Logged out successfully.",
  "errors": []
}
```

### Response Cookies (Set-Cookie Headers)
- Expired versions of both `access_token` and `refresh_token` cookies are sent to instruct the browser to delete them immediately.

---

## Integration Blueprint (Axios Example for Frontend AI Models)

Here is a recommended integration approach that frontend AI agents can implement to handle token rotation seamlessly.

```javascript
import axios from 'axios';

const api = axios.create({
  baseURL: 'https://api.yourdomain.com',
  withCredentials: true // ⚠️ CRITICAL: Sends and receives HttpOnly cookies
});

// Response interceptor to handle token expiry automatically
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    // If request fails with 401 Unauthorized and hasn't been retried yet
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        // Attempt to refresh the access token
        await api.post('/api/auth/refresh-token');
        
        // Retry the original request with the new access token cookie
        return api(originalRequest);
      } catch (refreshError) {
        // Refresh token is also expired or invalid; force user logout / redirect to login
        console.error('Session expired. Redirecting to login...');
        window.location.href = '/login';
        return Promise.reject(refreshError);
      }
    }
    return Promise.reject(error);
  }
);

export default api;
```
