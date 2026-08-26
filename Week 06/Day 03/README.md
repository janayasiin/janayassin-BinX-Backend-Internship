## Day 3 — Core Routes I: Catalog & Read Operations

### Implemented
- Added paginated GET endpoint for Events using `page` and `pageSize`.
- Added optional filtering by `categoryId` and `location`.
- Added sorting by event start date (ascending/descending).
- Created `EventResponse` DTO and projected queries using `Select`.
- Implemented an `EventService` to keep business/query logic outside the controller.
- Used `AsNoTracking()` for read-only queries.
- Added seed data for Events to test the endpoint.

### Endpoint
GET `/api/events`

### Query Parameters
- `page`
- `pageSize`
- `categoryId`
- `location`
- `sort`

### Example
`GET /api/events?page=1&pageSize=2&categoryId=2&location=Nablus&sort=startdate_desc`

### Testing
Tested pagination, filtering, sorting, DTO projection, and combined query parameters using Postman.
