# Day 2 — JWT Login & Registration

## Overview

Day 2 focused on implementing a complete authentication and registration flow for the **Cardiac Patient Monitoring System** using **ASP.NET Core Identity and JWT**.

The implementation connects the application's domain entity (`Patient`) with the Identity user (`ApplicationUser`), creates both records during registration within a database transaction, assigns the `Patient` role, and issues a JWT containing the authenticated patient's domain-specific ID.

---

## Learning Objectives

By the end of Day 2, the following requirements were implemented:

* Link the `Patient` domain entity to `ApplicationUser`.
* Create the Identity user and Patient profile during registration.
* Keep registration operations consistent using a database transaction.
* Assign the `Patient` role to the newly registered user.
* Authenticate users using ASP.NET Core Identity.
* Generate JWT access tokens after successful login.
* Include the patient's domain ID (`PatientId`) as a JWT claim.
* Test the complete registration-to-login flow.
* Verify the generated JWT payload and domain-specific claim.

---

# 1. Linking Patient to IdentityUser

The `Patient` entity is connected to `ApplicationUser` through the `UserId` foreign key.

This keeps authentication information such as email, password hash, and roles inside ASP.NET Core Identity, while patient-specific information remains inside the domain model.

### Patient Entity

```csharp
public class Patient
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public Gender Gender { get; set; }

    public string MedicalHistory { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    public ICollection<VitalSign> VitalSigns { get; set; } = new List<VitalSign>();

    public ICollection<Medication> Medications { get; set; } = new List<Medication>();

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
```

The important relationship is:

```text
ApplicationUser
      │
      │ 1
      │
      │
      │ 0..1
      ▼
   Patient
```

`Patient.UserId` stores the Identity user's ID.

### Entity Relationship Configuration

```csharp
modelBuilder.Entity<ApplicationUser>()
    .HasOne(u => u.Patient)
    .WithOne(p => p.User)
    .HasForeignKey<Patient>(p => p.UserId)
    .OnDelete(DeleteBehavior.Cascade);
```

This creates a **one-to-zero/one relationship** between `ApplicationUser` and `Patient`.

### Evidence

![Patient-Identity relationship](images/identity-patient-relationship.png)

![Patient linked to Identity user](images/patient-identity-user-link.png)

---

# 2. Registration Creates Both Records

The registration process creates:

1. An `ApplicationUser` through ASP.NET Core Identity.
2. A `Patient` domain entity linked to that Identity user.
3. A `Patient` role assignment.

The Identity user is created first:

```csharp
var user = new ApplicationUser
{
    UserName = request.Email,
    Email = request.Email,
    PhoneNumber = request.PhoneNumber
};

var result = await _userManager.CreateAsync(
    user,
    request.Password);
```

After successful Identity creation, the patient's domain profile is created using the generated Identity user ID:

```csharp
var patient = new Patient
{
    UserId = user.Id,
    FullName = request.FullName,
    DateOfBirth = request.DateOfBirth,
    Gender = request.Gender,
    MedicalHistory = request.MedicalHistory
};

await _context.Patients.AddAsync(patient);
```

This ensures that the `Patient` record is directly associated with the correct Identity account.

### Registration Evidence

![Registration creates Identity user and Patient](images/registration-identity-patient-transaction.png)

---

# 3. Registration Transaction

The registration operation is wrapped inside an EF Core database transaction:

```csharp
await using var transaction =
    await _context.Database.BeginTransactionAsync();
```

If Identity user creation fails, the transaction is rolled back.

If role assignment fails, the transaction is also rolled back.

```csharp
if (!roleResult.Succeeded)
{
    await transaction.RollbackAsync();

    return (
        false,
        roleResult.Errors
            .Select(e => e.Description)
            .ToArray()
    );
}
```

Only after all required operations succeed is the transaction committed:

```csharp
await _context.SaveChangesAsync();

await transaction.CommitAsync();
```

This prevents an inconsistent state where an Identity account exists without its corresponding Patient profile.

### Transaction and Role Evidence

![Registration transaction](images/registration-identity-patient-transaction.png)

![Patient role and commit](images/registration-patient-role-commit.png)

---

# 4. Assigning the Patient Role

Every newly registered patient is assigned the `Patient` role:

```csharp
var roleResult = await _userManager.AddToRoleAsync(
    user,
    "Patient");
```

The role is later included in the JWT as a role claim.

This allows the API to distinguish patients from other possible roles and supports role-based authorization.

---

# 5. Testing Registration

The registration endpoint was tested using the following request:

```http
POST /api/Auth/register
```

Example request:

```json
{
  "fullName": "Day Two Test",
  "dateOfBirth": "2000-01-01",
  "gender": 0,
  "phoneNumber": "0599999999",
  "email": "day2test@example.com",
  "password": "Test@12345",
  "medicalHistory": "No known conditions"
}
```

The endpoint successfully returned:

```text
Patient registered successfully.
```

### Registration Result

![Registration success](images/registration-success.png)

---

# 6. Login Flow

After registration, the same credentials were used to authenticate the patient.

The login process first finds the Identity user by email:

```csharp
var user = await _userManager.FindByEmailAsync(
    request.Email);
```

Then the password is verified using:

```csharp
var result = await _signInManager.CheckPasswordSignInAsync(
    user,
    request.Password,
    false);
```

After successful authentication, the corresponding `Patient` is retrieved:

```csharp
var patient = await _context.Patients
    .FirstOrDefaultAsync(p => p.UserId == user.Id);
```

This connects the authenticated Identity user to the patient's domain record.

### Patient Lookup During Login

![Login patient lookup](images/login-patient-lookup.png)

---

# 7. JWT Token Generation

After successful login, a JWT is generated.

The token contains standard identity information such as:

* Identity user ID
* Email
* Roles

It also contains the application's domain-specific patient ID.

The JWT claims include:

```csharp
var claims = new List<Claim>
{
    new Claim(
        JwtRegisteredClaimNames.Sub,
        user.Id),

    new Claim(
        ClaimTypes.NameIdentifier,
        user.Id),

    new Claim(
        ClaimTypes.Email,
        user.Email!),

    new Claim(
        "PatientId",
        patient.Id.ToString())
};
```

The Patient ID is added as a custom claim:

```csharp
new Claim(
    "PatientId",
    patient.Id.ToString())
```

This allows later authenticated requests to identify the patient's domain record directly from the JWT.

### JWT Generation Evidence

![JWT token generation](images/jwt-token-generation.png)

![JWT domain claim](images/jwt-domain-claims.png)

---

# 8. JWT Role Claim

The user's Identity roles are also added to the JWT:

```csharp
foreach (var role in roles)
{
    claims.Add(
        new Claim(ClaimTypes.Role, role));
}
```

For the registered user, the generated token contains:

```text
Role: Patient
```

This allows the application to use role-based authorization.

---

# 9. JWT Configuration

The token is signed using the configured JWT key:

```csharp
var key = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(
        _configuration["Jwt:Key"]!));

var credentials = new SigningCredentials(
    key,
    SecurityAlgorithms.HmacSha256);
```

The token is configured with:

```csharp
var token = new JwtSecurityToken(
    issuer: _configuration["Jwt:Issuer"],
    audience: _configuration["Jwt:Audience"],
    claims: claims,
    expires: DateTime.UtcNow.AddMinutes(15),
    signingCredentials: credentials);
```

The token therefore contains:

* Issuer
* Audience
* Claims
* Expiration time
* Digital signature

---

# 10. JWT Payload Verification

After successful login, the generated JWT was decoded and verified.

Example payload:

```json
{
  "sub": "bd95d5fc-a7e0-4e2b-9a3d-199ca0219a9f",
  "nameidentifier": "bd95d5fc-a7e0-4e2b-9a3d-199ca0219a9f",
  "email": "day2.registration@example.com",
  "PatientId": "6",
  "role": "Patient",
  "exp": 1788203477,
  "iss": "CardiacPatientMonitoringSystem",
  "aud": "CardiacPatientMonitoringSystemUsers"
}
```

The important domain-specific claim is:

```text
PatientId = 6
```

This confirms that the JWT is associated with the corresponding Patient domain entity.

### JWT Payload Evidence

![JWT payload](images/jwt-payload.png)

![Login success](images/login-success.png)

---

# 11. Complete Registration → Login Flow

The complete end-to-end flow was tested as follows:

```text
Registration Request
        │
        ▼
Create ApplicationUser
        │
        ▼
Create Patient
        │
        ▼
Assign Patient Role
        │
        ▼
Commit Transaction
        │
        ▼
Registration Successful
        │
        ▼
Login with Same Credentials
        │
        ▼
Find Identity User
        │
        ▼
Validate Password
        │
        ▼
Find Linked Patient
        │
        ▼
Generate JWT
        │
        ▼
Add PatientId Claim
        │
        ▼
Return JWT
        │
        ▼
Decode and Verify JWT
```

This verifies the complete integration between **ASP.NET Core Identity**, the application's **Patient domain entity**, and **JWT authentication**.

---

# 12. Requirements Checklist

| Requirement                                | Status      |
| ------------------------------------------ | ----------- |
| Link Patient to IdentityUser               | ✅ Completed |
| Create Identity user during registration   | ✅ Completed |
| Create Patient profile during registration | ✅ Completed |
| Link Patient to Identity user              | ✅ Completed |
| Use transaction for registration           | ✅ Completed |
| Assign Patient role                        | ✅ Completed |
| Implement login                            | ✅ Completed |
| Generate JWT                               | ✅ Completed |
| Add domain-specific PatientId claim        | ✅ Completed |
| Test registration flow                     | ✅ Completed |
| Test login flow                            | ✅ Completed |
| Decode and verify JWT                      | ✅ Completed |
| Verify PatientId claim                     | ✅ Completed |

---

# 13. Key Concepts Learned

### Identity vs Domain Entity

`ApplicationUser` is responsible for authentication and identity-related information, while `Patient` contains patient-specific domain information.

### Transactions

A transaction ensures that the registration process succeeds as one consistent operation. If an important step fails, previous database changes can be rolled back.

### Domain Claims

The `PatientId` claim connects the authenticated user to the application's domain entity through the JWT.

### JWT

JWT provides a signed token that the API can use to identify and authorize the authenticated user on subsequent requests.

### End-to-End Testing

Testing registration, database relationships, login, and JWT contents together verifies that the different components work correctly as one complete authentication flow.

---

# 14. Tools Used

* ASP.NET Core
* ASP.NET Core Identity
* Entity Framework Core
* JWT
* SQL Server
* Swagger
* Postman
* C#
* .NET 9

---

# Day 2 Result

The Cardiac Patient Monitoring System now supports a complete **Identity-based registration and JWT login flow**.

Each registered patient has:

```text
ApplicationUser
      │
      │ UserId
      ▼
   Patient
```

and the generated JWT contains:

```text
User ID
Email
PatientId
Role
Issuer
Audience
Expiration
```

This completes the Day 2 requirements for **JWT Login & Registration for the Capstone Project**.
