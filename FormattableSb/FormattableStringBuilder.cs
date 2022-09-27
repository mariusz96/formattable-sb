using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace FormattableSb
{
    /// <summary>
    /// Represents a mutable FormattableString. This class cannot be inherited.
    /// </summary>
    public sealed class FormattableStringBuilder
    {
        private readonly StringBuilder _format = new();
        private readonly List<object?> _arguments = new();

        /// <summary>
        /// Appends the specified interpolated string to the end of the composite format string,
        /// replacing its arguments with placeholders and adding them as objects.
        /// </summary>
        /// <param name="handler">The interpolated string to append, along with the arguments.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public FormattableStringBuilder AppendInterpolated([InterpolatedStringHandlerArgument("")] AppendInterpolatedHandler handler) => this;

        /// <summary>
        /// Appends the default line terminator to the end of the composite format string.
        /// </summary>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public FormattableStringBuilder AppendLine()
        {
            _format.AppendLine();
            return this;
        }

        /// <summary>
        /// Create a <see cref="FormattableString"/> from this builder.
        /// </summary>
        /// <returns>The object that represents the composite format string and its arguments.</returns>
        public FormattableString ToFormattableString() => FormattableStringFactory.Create(_format.ToString(), _arguments.ToArray());

        [InterpolatedStringHandler]
        public struct AppendInterpolatedHandler
        {
            private readonly FormattableStringBuilder _builder;

            public AppendInterpolatedHandler(int literalLength, int formattedCount, FormattableStringBuilder builder)
            {
                _builder = builder;
            }

            public void AppendLiteral(string value) => _builder._format.Append(value.Replace("{", "{{").Replace("}", "}}"));

            public void AppendFormatted(object? value, int alignment = 0, string? format = null)
            {
                _builder._format.Append('{').Append(_builder._arguments.Count);
                if (alignment != 0) _builder._format.Append(CultureInfo.InvariantCulture, $",{alignment}");
                if (format is not null) _builder._format.Append(':').Append(format);
                _builder._format.Append('}');

                _builder._arguments.Add(value);
            }
        }
    }
}
