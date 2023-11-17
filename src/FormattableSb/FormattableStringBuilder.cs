using System.ComponentModel;
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
        public FormattableStringBuilder AppendInterpolated([InterpolatedStringHandlerArgument("")] ref AppendInterpolatedHandler handler) => this;

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
        /// Creates a <see cref="FormattableString"/> from this builder.
        /// </summary>
        /// <returns>The object that represents the composite format string and its arguments.</returns>
        public FormattableString ToFormattableString() => FormattableStringFactory.Create(_format.ToString(), _arguments.ToArray());

        /// <summary>
        /// Provides a handler used by the language compiler to append interpolated strings into <see cref="FormattableStringBuilder"/> instances.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [InterpolatedStringHandler]
        public struct AppendInterpolatedHandler
        {
            private readonly FormattableStringBuilder _builder;

            /// <summary>Creates a handler used to append an interpolated string into a <see cref="FormattableStringBuilder"/>.</summary>
            /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
            /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
            /// <param name="builder">The associated FormattableStringBuilder to which to append.</param>
            /// <remarks>This is intended to be called only by compiler-generated code. Arguments are not validated as they'd otherwise be for members intended to be used directly.</remarks>
            public AppendInterpolatedHandler(int literalLength, int formattedCount, FormattableStringBuilder builder)
            {
                _builder = builder;
            }

            /// <summary>Writes the specified string to the handler.</summary>
            /// <param name="value">The string to write.</param>
            public void AppendLiteral(string value) => _builder._format.Append(value.Replace("{", "{{").Replace("}", "}}"));

            /// <summary>Writes the specified value to the handler.</summary>
            /// <param name="value">The value to write.</param>
            /// <param name="alignment">Minimum number of characters that should be written for this value. If the value is negative, it indicates left-aligned and the required minimum is the absolute value.</param>
            /// <param name="format">The format string.</param>
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
