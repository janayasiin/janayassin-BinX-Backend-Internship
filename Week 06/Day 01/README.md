# \# EventHub — Week 6 Day 1

# 

# \## Sprint 1 — Planning \& Database Design

# 

# \### Sprint Goal

# 

# Build the database foundation and core event and ticket booking functionality for EventHub.

# 

# \### Project Scope

# 

# EventHub is an ASP.NET Core Web API for managing events, venues, tickets, and user bookings.

# 

# \### Database Entities

# 

# \- Event

# \- Category

# \- Venue

# \- Ticket

# \- Booking

# \- BookingItem

# \- IdentityUser

# 

# \### Main Relationships

# 

# \- Category → Events (1:N)

# \- Venue → Events (1:N)

# \- Event → Tickets (1:N)

# \- User → Bookings (1:N)

# \- Booking → BookingItems (1:N)

# \- Ticket → BookingItems (1:N)

# 

# \### ERD

# 

# !\[EventHub ERD](./ERD.png)

# 

# \### Sprint 1 Backlog

# 

# \- \[ ] Create project structure

# \- \[ ] Implement database entities

# \- \[ ] Configure EF Core and relationships

# \- \[ ] Configure Fluent API

# \- \[ ] Create and apply migrations

# \- \[ ] Implement event read endpoints

# \- \[ ] Implement ticket endpoints

# \- \[ ] Implement booking creation

# \- \[ ] Add validation

# \- \[ ] Test core endpoints

