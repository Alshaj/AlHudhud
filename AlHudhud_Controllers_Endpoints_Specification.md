# Al Hudhud System - Controllers & Endpoints Specification

## Purpose
This document defines all Controllers, Endpoints, and Authorization Rules for the Al Hudhud Inspection & Consultancy System.

## Roles

| Role | Description |
|------|-------------|
| Admin | Full system management |
| Inspector | Execute and follow up assigned inspections |
| Viewer | Read-only access to business data and reports |

---

## AuthController

| Endpoint | Method | Admin | Inspector | Viewer |
|----------|--------|--------|-----------|--------|
| Login | POST | ✅ | ✅ | ✅ |
| Change Password | POST | ✅ | ✅ | ✅ |
| My Profile | GET | ✅ | ✅ | ✅ |

---

## UsersController

| Endpoint | Method | Admin | Inspector | Viewer |
|----------|--------|--------|-----------|--------|
| Get Users | GET | ✅ | ❌ | ✅ |
| Get User Details | GET | ✅ | ❌ | ✅ |
| Create User | POST | ✅ | ❌ | ❌ |
| Update User | PUT | ✅ | ❌ | ❌ |
| Activate/Deactivate User | PATCH | ✅ | ❌ | ❌ |
| Delete User | DELETE | ✅ | ❌ | ❌ |

---

## ClientsController

| Endpoint | Method | Admin | Inspector | Viewer |
|----------|--------|--------|-----------|--------|
| Get All Clients | GET | ✅ | ❌ | ✅ |
| Get Client Details | GET | ✅ | ❌ | ✅ |
| Create Client | POST | ✅ | ❌ | ❌ |
| Update Client | PUT | ✅ | ❌ | ❌ |
| Delete Client | DELETE | ✅ | ❌ | ❌ |

---

## ProjectsController

| Endpoint | Method | Admin | Inspector | Viewer |
|----------|--------|--------|-----------|--------|
| Get Projects | GET | ✅ | ❌ | ✅ |
| Get Project Details | GET | ✅ | ❌ | ✅ |
| Create Project | POST | ✅ | ❌ | ❌ |
| Update Project | PUT | ✅ | ❌ | ❌ |
| Delete Project | DELETE | ✅ | ❌ | ❌ |

---

## ScopeOfWorkController

| Endpoint | Method | Admin | Inspector | Viewer |
|----------|--------|--------|-----------|--------|
| Get Scopes | GET | ✅ | ❌ | ✅ |
| Create Scope | POST | ✅ | ❌ | ❌ |
| Update Scope | PUT | ✅ | ❌ | ❌ |
| Delete Scope | DELETE | ✅ | ❌ | ❌ |

---

## ProjectScopesController

| Endpoint | Method | Admin | Inspector | Viewer |
|----------|--------|--------|-----------|--------|
| Get Project Scopes | GET | ✅ | ❌ | ✅ |
| Get Project Scope Details | GET | ✅ | ✅ | ✅ |
| Assign Scope To Project | POST | ✅ | ❌ | ❌ |
| Update Project Scope | PUT | ✅ | ❌ | ❌ |
| Change Project Scope Status | PATCH | ✅ | ❌ | ❌ |

---

## ProposalsController

| Endpoint | Method | Admin | Inspector | Viewer |
|----------|--------|--------|-----------|--------|
| Get All Proposals | GET | ✅ | ❌ | ✅ |
| Get Proposal Details | GET | ✅ | ❌ | ✅ |
| Create Proposal | POST | ✅ | ❌ | ❌ |
| Update Proposal | PUT | ✅ | ❌ | ❌ |
| Approve Proposal | PATCH | ✅ | ❌ | ❌ |
| Reject Proposal | PATCH | ✅ | ❌ | ❌ |
| Proposal History | GET | ✅ | ❌ | ✅ |

---

## InspectionsController

| Endpoint | Method | Admin | Inspector | Viewer |
|----------|--------|--------|-----------|--------|
| Get All Inspections | GET | ✅ | ❌ | ✅ |
| Get Inspection Details | GET | ✅ | ✅ | ✅ |
| Create Inspection | POST | ✅ | ❌ | ❌ |
| Update Inspection | PUT | ✅ | ❌ | ❌ |
| Delete Inspection | DELETE | ✅ | ❌ | ❌ |
| Get My Inspections | GET | ❌ | ✅ | ❌ |
| Add Notes | PATCH | ❌ | ✅ | ❌ |

---

## ReportsController

| Endpoint | Method | Admin | Inspector | Viewer |
|----------|--------|--------|-----------|--------|
| Get Reports | GET | ✅ | ❌ | ✅ |
| Get Report Details | GET | ✅ | ❌ | ✅ |
| Generate Report Number | POST | ✅ | ❌ | ❌ |
| Delete Report | DELETE | ✅ | ❌ | ❌ |

---

## CertificatesController

| Endpoint | Method | Admin | Inspector | Viewer |
|----------|--------|--------|-----------|--------|
| Get Certificates | GET | ✅ | ❌ | ✅ |
| Get Certificate Details | GET | ✅ | ❌ | ✅ |
| Generate Certificate Number | POST | ✅ | ❌ | ❌ |
| Delete Certificate | DELETE | ✅ | ❌ | ❌ |

---

## DashboardController

### Admin Dashboard

| Endpoint | Method | Admin | Inspector | Viewer |
|----------|--------|--------|-----------|--------|
| System Statistics | GET | ✅ | ❌ | ❌ |

### Inspector Dashboard

| Endpoint | Method | Admin | Inspector | Viewer |
|----------|--------|--------|-----------|--------|
| My Assigned Inspections | GET | ❌ | ✅ | ❌ |

### Viewer Dashboard

| Endpoint | Method | Admin | Inspector | Viewer |
|----------|--------|--------|-----------|--------|
| Read Only Statistics | GET | ❌ | ❌ | ✅ |

---

## Recommended Controller-Level Authorization

| Controller | Roles |
|------------|-------|
| UsersController | Admin |
| ClientsController | Admin, Viewer |
| ProjectsController | Admin, Viewer |
| ScopeOfWorkController | Admin, Viewer |
| ProjectScopesController | Admin, Viewer, Inspector |
| ProposalsController | Admin, Viewer |
| InspectionsController | Admin, Viewer, Inspector |
| ReportsController | Admin, Viewer |
| CertificatesController | Admin, Viewer |
| DashboardController | Admin, Viewer, Inspector |

---

## Important Business Rules

1. Proposal history must be preserved.
2. Multiple proposal versions are allowed.
3. Only one approved proposal can exist per ProjectScope.
4. Inspections are manually created by Admin.
5. Reports are generated after inspections.
6. Certificates require a report.
7. Creating a certificate closes the ProjectScope.
8. Viewer is read-only.
9. Admin has full workflow control.
