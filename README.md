# Ockham.Convert
Succinct but flexible data conversions for extended primitive data types. Part of the [Ockham.NET](https://github.com/ockham-net/ockham.net) project.

## The Problem
> Every Ockham component should solve a clear problem that is not solved in the .NET BCL, or in the particular libraries it is meant to augment. 

The utilities in this library provide robust, configurable data inspection and conversion utilities. These are particularly useful when deserializing from databases or other external sources, and when working with dynamic data. For more detail, see [Purpose](#purpose).

# The API

The most basic operation is to convert from a value of an unknown type (`object`) to a specific type, using either generic syntax (when you know the target type at compile time) or non-generic sytax (when you need to specify the target type at runtime):

```C#
using Ockham.Data;

     T Convert.To<T>(object);
object Convert.To(object, Type);
```

This intuitively works for `DBNull`, `Guid`, `Timespan`, `bool`, `Nullable<T>`, and all enums (even nullables of enums!), and will automatically use `IConvertible` and/or user defined conversion operators where applicable. 

Subtleties of these conversions can be controlled with the `ConvertOptions` class, described in more detail below:

```C#
object Convert.To(object, Type, ConvertOptions);
     T Convert.To<T>(object, ConvertOptions);
```

Several type-specific overloads are also defined for convenience, which use the `ConvertOptions.Default` settings:

```C#
  bool Convert.ToBool(object)
  Guid Convert.ToGuid(object)
   int Convert.ToInt(object)
  long Convert.ToLong(object)
string Convert.ToString(object)
... etc
```

If desired, conversion failures can be ignored (it is left to the user to determine when this is wise), with or without a default fallback value:

```C#
object Force.To(object value, Type targetType);
object Force.To(object value, Type targetType, object defaultValue);
     T Force.To<T>(object value);
     T Force.To<T>(object value, T defaultValue);
```
 
Finally, methods are provided to convert to and from `DBNull`, or detect a "null" value (with the user specifying which things should be considered null):

```C#
object Convert.ToNull(object)  // Converts DBNull to null, leaves other values as-is
object Convert.ToDBNull(object)  // Converts null to DBNull, leaves other values as-is

// More control over what is considered null:
object Convert.ToNull(object value, ConvertOptions options) 
object Convert.ToNull(object value, bool emptyStringAsNull) 
object Convert.ToDBNull(object value, ConvertOptions options) 
object Convert.ToDBNull(object value, bool emptyStringAsNull) 

// Or to simply inspect:
bool Value.IsNull(object value)     // True if null or DBNull
bool Value.IsNull(object value, bool emptyStringAsNull)
bool Value.IsNull(object value, ConvertOptions options)
```

Similar functions are provided on the `Converter` class, where they use whatever options have been configured on the converter instance:

```C#
// Where 'other empty value' is determined by the converter's ConvertOptions 
converter.ToNull(value)    // Converts DBNull or other empty value to null
converter.ToDBNull(value)  // Converts null or other empty value to DBNull
converter.IsNull(value)    // Detects null, DBNull, or other empty value
```

## Customization
 
A specific set of conversion options can be reused more easily with an instance of the `Converter` class

```C#
var options = ConvertOptionsBuilder.Create()
  .WithEnumOptions(undefinedNames: UndefinedValueOption.Ignore)
  .WithStringOptions(emptyStringOptions: EmptyStringConvertOptions.WhitespaceAsNull, allowHex: true)
  .WithTrueStrings(bool.TrueString, 't', 'x', 'y')
  .WithFalseStrings(bool.FalseString, 'f', '', 'x')
  .Build();

var converter = new Converter(options);

// All of the following use the ConvertOptions used to construct the converter:
converter.ToInt(value)  
converter.To<int?>(value)
converter.To(value, type)
converter.Force<T>(value, T @default)
... etc
```

`Converter`, `ConvertOptions`, and `ConvertOptionsBuilder` can all be subclassed. In addition, the base `Converter` class can be extended by registering converters for specific types by providing a method that can match `ConverterDelegate` or `ConverterDelegate<T>`:

```C#
var extendedConverter = converter
  .WithConverter<bool>((value, options) => /* custom logic, replacing default */ );
```

# Purpose

The Base Class Library and .NET languages themselves provide multiple ways to convert between primitive data types. So why is a separate library like this needed?

The existing basic data conversions available from the [`System.Convert`](https://docs.microsoft.com/en-us/dotnet/api/system.convert) and [`Microsoft.VisualBasic.CompilerServices.Conversions`](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.VisualBasic.CompilerServices.Conversions) classes fail in several cases that arise frequently when working with serialization and deserialization:

 - Converting to and from [`DBNull`](https://docs.microsoft.com/en-us/dotnet/api/system.dbnull)
 - Converting to and from [`Nullable<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1) values
 - Converting to and from [enumeration](https://docs.microsoft.com/en-us/dotnet/standard/base-types/common-type-system#Enumerations) types
 - Converting to and from [`Guid`](https://docs.microsoft.com/en-us/dotnet/api/system.guid)s
 - Converting to and from [`TimeSpan`](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)
 - Converting between [`Boolean`](https://docs.microsoft.com/en-us/dotnet/api/system.boolean) and the many string representations used in databases in the wild

They also lack a universal conversion API with generic syntax. I.e. there is no `T Convert.To<T>(object value)` in the BCL. The two BCL universal converters are non-generic methods which require a runtime `Type` object and return an `object`:
 - [`System.Convert.ChangeType`](https://docs.microsoft.com/en-us/dotnet/api/system.convert.changetype) 
 - [`Conversions.ChangeType`](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.VisualBasic.CompilerServices.Conversions.ChangeType) , which is used by the [VB.NET](https://docs.microsoft.com/en-us/dotnet/visual-basic/) compiler to implement the [`CType` language function](https://docs.microsoft.com/en-us/dotnet/visual-basic/language-reference/functions/ctype-function).
 
## Differences in `System.Convert`, `Microsoft.VisualBasic.CompilerServices.Conversions`

The two BCL converters also have important differences and notable behaviors: 

- `System.Convert.ChangeType(object, Type)` will automatically use an applicable `IConvertible` interface method, but will **not** use [user-defined conversion operators](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators) (`operator implicit` in C# / [`Widening Operator` in VB.NET](https://docs.microsoft.com/en-us/dotnet/visual-basic/language-reference/statements/operator-statement))
- `Microsoft.VisualBasic.CompilerServices.Conversions.ChangeType(object, Type)` **will** use user-defined conversion operators but will **not** use `IConvertible`.
- `System.Convert.ToInt32(null)` returns `0`, while `System.Convert.ChangeType(null, typeof(int))` throws an exception
- `Microsoft.VisualBasic.CompilerServices.Conversions.Integer(null)` returns `0`, and `Microsoft.VisualBasic.CompilerServices.Conversions.ChangeType(null, typeof(int))` also returns `0`

## Controlling subtleties of conversion

Beyond the awkward `IFormatProvider` option which can be provided to `System.Convert`, the BCL conversions do not provide means of explicitly controlling conversion behaviors which must be considered when deserializing or converting from an external data source. This library exposes the following options as explicit choices:

  - Treating an empty string as null (or not)
  - Recognizing hexadecimal strings as numbers (or not)
  - Converting null values to the default value of a value type (or not)
  - Suppressing errors when attempting to convert, with or without an explicit fallback value (or not)
 
## Dynamic Value Inspection

The BCL also lacks succinct methods for checking if a value can be treated as numeric, if it is the default value of a value type (e.g `0` for any numeric type), or if it is null / empty / DBNull. These basic inspection actions are often required when validating or reasoning about input from external services or data sources.

# Architecture

The core conversion logic is implemented in the **internal** method `Ockham.Data.Convert.To`:

```c#
internal static object To(
  object value, 
  Type targetType, 
  ConvertOptions options, 
  bool ignoreError, 
  object defaultValue,
  bool customConverters,
  Dictionary<Type, ConverterDelegate> converters
)
```

The various overloads of `To*` and `Force*` on the static `Convert` class and configurable `Converter` class instances internally resolve to this single conversion logic, just with different variations of the input arguments, and sometimes with a direct cast to the required output type.

The conversion logic does not replace built-in BCL conversion tools, but rather arranges and combines them into an efficient whole. It falls back to `System.Convert.ChangeType(object, Type)` if it detects an `IConvertible` input with a valid candidate output type, and falls back to `Microsoft.VisualBasic.CompilerServices.Conversions.ChangeType(object, Type)` for other basic conversions, which takes advantage of that implementation's advanced features such as automatic detection and use of user-defined conversion operators.
