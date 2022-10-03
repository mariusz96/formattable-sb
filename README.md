# FormattableSb
A mutable FormattableString:
```cs
var firstDayOfSummer = new DateTime(2022, 6, 21);
var lastDayOfSummer = new DateTime(2022, 9, 23);

var sqlBuilder = new FormattableStringBuilder()
    .AppendInterpolated($"INSERT INTO dbo.VacationDates (Date)")
    .AppendLine()
    .AppendInterpolated($"VALUES");

for (var date = firstDayOfSummer; date <= lastDayOfSummer; date = date.AddDays(1))
{
    sqlBuilder
        .AppendLine()
        .AppendInterpolated($"({date})");

    if (date != lastDayOfSummer)
    {
        sqlBuilder.AppendInterpolated($",");
    }
}

// sql.GetArguments():
// [
//   System.DateTime,
//   System.DateTime,
//   ...
// ]

// sql.Format:
// INSERT INTO dbo.VacationDates (Date)
// VALUES
// ({0}),
// ({1}),
// ...
var sql = sqlBuilder.ToFormattableString();
```
### With EF Core:
```cs
using var context = new VacationingContext();
context.Database.ExecuteSqlInterpolated(sql);
```
## Features:
- Adheres to the C# language specification
- Can be used when you want to modify a FormattableString
- Preserves alignment and format strings
## API:
### AppendInterpolated:
```cs
/// <summary>
/// Appends the specified interpolated string to the end of the composite format string,
/// replacing its arguments with placeholders and adding them as objects.
/// </summary>
/// <param name="handler">The interpolated string to append, along with the arguments.</param>
/// <returns>A reference to this instance after the append operation has completed.</returns>
public FormattableStringBuilder AppendInterpolated([InterpolatedStringHandlerArgument("")] AppendInterpolatedHandler handler)
```
### AppendLine:
```cs
/// <summary>
/// Appends the default line terminator to the end of the composite format string.
/// </summary>
/// <returns>A reference to this instance after the append operation has completed.</returns>
public FormattableStringBuilder AppendLine()
```
## Setup:
- Install FormattableSb via NuGet Package Manager, Package Manager Console or dotnet CLI:
```
Install-Package FormattableSb
```
```
dotnet add package FormattableSb
```
## Credits:
- Thanks to Stephen Toub for the implementation
