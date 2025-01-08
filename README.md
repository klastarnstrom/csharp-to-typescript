# CSharp to TypeScript

This is a simple, opinionated tool used to convert C# classes, interfaces and enums to TypeScript interfaces.

The intended use case is to convert .NET API request and response classes to TypeScript interfaces for use in a front-end application.

## Usage

Use the `TsGenerate` attribute to mark classes, interfaces and enums that you want to convert to TypeScript.

For example, the following C# code:

```csharp

public class Address
{
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
}

public class PersonRequest
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Address Address { get; set; }
}

[TsGenerate]
public class EmployeeRequest : PersonRequest
{   
    public List<string> Roles { get; set; }
}
```

Will generate the following TypeScript interfaces:

```typescript
export interface Address {
    streetAddress: string;
    city: string;
    state: string;
    zip: string;
}

export interface PersonRequest {
    name: string;
    age: number;
    address: Address;
}

export interface EmployeeRequest extends PersonRequest {
    roles: string[];
}
```