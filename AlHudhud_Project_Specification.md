# Al Hudhud Inspection & Consultancy System

## Project Overview

This system is a web application for **Al Hudhud**, a UAE-based company that provides:

- Inspections
- Consultancy services

The system manages the complete workflow from client registration through proposal management, inspections, report generation, certificate generation, and project completion.

---

# Business Workflow

## 1. Create Client

The Admin creates a client and enters:

- Client Name
- Tax Number
- Email
- Phone Number
- Company Type

A client can have multiple projects.

---

## 2. Create Project

The Admin creates a project for a specific client.

Project data:

- Project Name
- Project Location

A client may own multiple projects.

---

## 3. Assign Scope Of Work

Each project can contain multiple scopes of work.

Examples:

- Fire Alarm
- Fire Fighting
- Electrical Inspection
- Mechanical Inspection

A scope has:

- Name
- IsNeedInspection

Rules:

- The same project cannot contain the same scope more than once.
- A UNIQUE constraint exists on:

(ProjectId, ScopeId)

inside ProjectScopes.

---

## 4. Create Proposal (Quotation)

A Proposal belongs to a ProjectScope.

Proposal fields:

- Proposal Number
- Price
- VAT
- Refered By
- Status
- Version Number
- Notes

When a proposal is created:

Status = Pending

---

## 5. Proposal Revisions

The system must preserve all historical proposals.

Example:

V1 → Rejected

V2 → Rejected

V3 → Approved

Old proposals are never updated or overwritten.

A new revision creates a new Proposal record.

---

## 6. Proposal Approval Rules

Only ONE proposal can be Approved for the same ProjectScope.

Valid:

ProjectScope
 ├─ Proposal V1 → Rejected
 ├─ Proposal V2 → Rejected
 └─ Proposal V3 → Approved

Invalid:

ProjectScope
 ├─ Proposal V1 → Approved
 └─ Proposal V2 → Approved

---

## 7. Proposal Rejection Rules

Rejected proposals do NOT close the workflow.

The Admin can create a new proposal revision.

Example:

V1 → Rejected

V2 → Pending

V2 → Approved

---

## 8. Inspection Workflow

Approval does NOT automatically create inspections.

Approval only makes the ProjectScope ready for inspections.

The Admin manually creates inspections.

Inspection fields:

- Inspector
- Date
- Time
- Location
- Contact Number
- Inspection Order
- Notes

---

## 9. Inspection Order

A ProjectScope may have any number of inspections.

The number is NOT known beforehand.

Examples:

Inspection Order = 1

Inspection Order = 2

Inspection Order = 3

Displayed as:

- First Inspection
- Second Inspection
- Third Inspection

---

## 10. Inspectors

Inspectors are system users.

There is no separate Inspectors table.

Inspectors are users assigned to:

Role = Inspector

---

## 11. Report Generation

Each ProjectScope can have only ONE report.

The report currently stores:

- Report Number

A report is generated after inspections are completed.

A report belongs to ProjectScope, not to Proposal.

---

## 12. Certificate Generation

Each ProjectScope can have only ONE certificate.

The certificate currently stores:

- Certificate Number

A certificate belongs to ProjectScope.

---

## 13. Report and Certificate Rule

A certificate cannot be created unless a report already exists.

Valid:

Inspections
 → Report
 → Certificate

Invalid:

Inspections
 → Certificate

---

## 14. ProjectScope Completion

When a certificate is generated:

- The ProjectScope becomes Closed.
- No new proposals can be created.
- No new inspections can be created.
- No new reports can be created.
- No new certificates can be created.

---

# Entity Relationships

## Client

Client
 1 → N Projects

---

## Project

Project
 1 → N ProjectScopes

---

## Scope Of Work

ScopeOfWork
 1 → N ProjectScopes

---

## ProjectScope

ProjectScope
 1 → N Proposals

ProjectScope
 1 → 1 Report

ProjectScope
 1 → 1 Certificate

---

## Proposal

Proposal
 1 → N Inspections

---

## User

User
 N ↔ N Roles

through UserRoles

---

# Database Tables

## Clients

| Column | Type |
|----------|----------|
| Id | int |
| ClientName | nvarchar(50) |
| TaxNumber | nvarchar(50) |
| Email | nvarchar(50) |
| CompanyType | nvarchar |

---

## Projects

| Column | Type |
|----------|----------|
| Id | int |
| ClientId | int |
| Name | nvarchar(50) |
| Location | nvarchar(50) |

---

## ScopeOfWork

| Column | Type |
|----------|----------|
| Id | int |
| Name | nvarchar(50) |
| IsNeedInspection | bool |

---

## ProjectScopes

| Column | Type |
|----------|----------|
| Id | int |
| ProjectId | int |
| ScopeOfWorkId | int |
| Location | nvarchar(50) |
| ProjectScopeStatusId | int |

Constraint:

UNIQUE(ProjectId, ScopeOfWorkId)

---

## ProjectScopeStatuses

| Column | Type |
|----------|----------|
| Id | int |
| Name | nvarchar(50) |

Suggested statuses:

- ProposalPending
- ProposalApproved
- UnderInspection
- ReportGenerated
- CertificateGenerated
- Closed

---

## Proposals

| Column | Type |
|----------|----------|
| Id | int |
| ProposalNumber | nvarchar(50) |
| ProjectScopeId | int |
| StatusId | int |
| ReferedBy | int |
| Price | decimal |
| Vat | decimal |
| VersionNumber | int |
| Notes | nvarchar(max) |

---

## ProposalStatuses

| Column | Type |
|----------|----------|
| Id | int |
| Name | nvarchar(50) |

Examples:

- Pending
- Approved
- Rejected

---

## Inspections

| Column | Type |
|----------|----------|
| Id | int |
| ProposalId | int |
| InspectorId | int |
| Date | date |
| Time | date/time |
| Location | nvarchar(50) |
| ContactNumber | nvarchar(50) |
| InspectionOrder | int |
| Notes | nvarchar(max) |

---

## Reports

| Column | Type |
|----------|----------|
| Id | int |
| ReportNumber | nvarchar(50) |
| ProjectScopeId | int |

Rule:

One Report per ProjectScope

---

## Certificates

| Column | Type |
|----------|----------|
| Id | int |
| CertificateNumber | nvarchar(50) |
| ProjectScopeId | int |

Rule:

One Certificate per ProjectScope

---

## Users

| Column | Type |
|----------|----------|
| Id | int |
| UserName | nvarchar(50) |
| Email | nvarchar(50) |
| PhoneNumber | nvarchar(50) |
| PasswordHash | nvarchar(50) |
| IsActive | bool |

---

## Roles

| Column | Type |
|----------|----------|
| Id | int |
| Name | nvarchar(50) |

---

## UserRoles

| Column | Type |
|----------|----------|
| Id | int |
| UserId | int |
| RoleId | int |

---

# Key Business Rules

1. A Client can have multiple Projects.
2. A Project can have multiple Scopes.
3. The same Scope cannot be assigned twice to the same Project.
4. A ProjectScope can have multiple Proposal versions.
5. Only one Proposal can be Approved per ProjectScope.
6. Rejected Proposals do not end the workflow.
7. Inspections are created manually by the Admin.
8. The number of inspections is not known beforehand.
9. Inspectors are Users with the Inspector role.
10. Each ProjectScope has exactly one Report.
11. Each ProjectScope has exactly one Certificate.
12. A Certificate cannot exist without a Report.
13. Creating a Certificate closes the ProjectScope.
14. Closed ProjectScopes cannot receive new Proposals or Inspections.
15. Historical Proposal versions must always be preserved.
