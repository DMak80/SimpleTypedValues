# SimpleTypedValue

The goal of the project is to simplify moving from code like this

```csharp
    AnyFunc(long organizationId, long userId)
```

to code like this

```csharp
    AnyFunc(OrganizationId organizationId, UserId userId)
```

where `OrganizationId` and `UserId` may be like this

```csharp
    [TypedValue]
    public partial struct OrganizationId : ITypedValue<long>
    {
        public long Value { get; }
    }

    [TypedValue]
    public partial struct UserId : ITypedValue<long>
    {
        public long Value { get; }
    }
```

or like this

```csharp
    AnyFunc(Organization.ID organizationId, User.ID userId)
```

or `Organization.ID` and `User.ID` may be like this

```csharp
    public class BaseEntity<TID>
        where TID : struct
    {
        public TID Value { get; }
    }

    public partial class Organization : BaseEntity<Organization.ID>
    {
        [TypedValue]
        public partial struct ID : ITypedValue<long>
        {
            public long Value { get; }
        }
    }

    public partial class User : BaseEntity<User.ID>
    {
        [TypedValue]
        public partial struct ID : ITypedValue<long>
        {
            public long Value { get; }
        }
    }
```

The main interface for simple typed value is
```csharp
    public interface ITypedValue
    {
        object? Value { get; }
    }

    public interface ITypedValue<out TValue> : ITypedValue
        where TValue : notnull, IComparable<TValue>, IEquatable<TValue>
    {
        new TValue Value { get; }
        object? ITypedValue.Value => Value;
    }
```
The main interface `ITypedValue<out TValue>` where TValue can be any CLR value or reference type. The inteface is used mainly as generic methods constraint.

`ITypedValue` is interface for internal usage in very common tasks.

Use `SimpleTypedValue` NuGet package for .Net 6.0.

Attribute and SourceGenerator are in `SimpleTypedValue.Attributes` and `SimpleTypedValue.Generator` NuGet packages.

# Use with Dapper

Use `SimpleTypedValue.Dapper` NuGet package.

There are two ways to register handler

The first is to use attribute argument:
```csharp
    [TypedValue(TypedValueExtensionFlag.Dapper)]
    public partial struct TestEntityId : ITypedValue<long>
    {
        public long Value { get; }
    }
```

The second is direct register
```csharp
    typeof(TestEntityId).AddDapperTypedValueHandler();
```

# Use with Entity Framework Core 6.0

Use `SimpleTypedValue.EF` NuGet package.

Configure `DbContext` and use `UseTypedValues` on `modelBuilder`

```csharp
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        ...

        modelBuilder.UseTypedValues(Database.IsInMemory());
    }
```
