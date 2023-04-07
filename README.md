# FormattableSb
A mutable FormattableString:
```cs
List<DateTime> summerDates = GetSummerDates();

FormattableStringBuilder sqlBuilder = new FormattableStringBuilder()
    .AppendInterpolated($"INSERT INTO dbo.VacationDates (Date)")
    .AppendLine()
    .AppendInterpolated($"VALUES");

foreach (DateTime date in summerDates)
{
    sqlBuilder
        .AppendLine()
        .AppendInterpolated($"({date})");

    if (date != summerDates.Last())
    {
        sqlBuilder.AppendInterpolated($",");
    }
}

// sql.Format:
// INSERT INTO dbo.VacationDates (Date)
// VALUES
// ({0}),
// ({1}),
// ...

// sql.GetArguments():
// [
//   System.DateTime,
//   System.DateTime,
//   ...
// ]
FormattableString sql = sqlBuilder.ToFormattableString();
```
```cs
static List<DateTime> GetSummerDates()
{
    DateTime startDate = new(2040, 6, 20);
    DateTime endDate = new(2040, 9, 22);

    List<DateTime> dates = new();

    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
    {
        dates.Add(date);
    }

    return dates;
}
```
### With EF Core:
```cs
using VacationingContext context = new();
int rowsAffected = context.Database.ExecuteSql(sql);
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
public FormattableStringBuilder AppendInterpolated([InterpolatedStringHandlerArgument("")] ref AppendInterpolatedHandler handler)
```
### AppendLine:
```cs
/// <summary>
/// Appends the default line terminator to the end of the composite format string.
/// </summary>
/// <returns>A reference to this instance after the append operation has completed.</returns>
public FormattableStringBuilder AppendLine()
```
### ToFormattableString:
```cs
/// <summary>
/// Creates a <see cref="FormattableString"/> from this builder.
/// </summary>
/// <returns>The object that represents the composite format string and its arguments.</returns>
public FormattableString ToFormattableString()
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
