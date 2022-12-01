using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FormattableSb.Tests
{
    public class FormattableStringBuilderTest
    {
        [Fact]
        public void AppendInterpolated_Empty()
        {
            var fsb = new FormattableStringBuilder();

            var fs = fsb.ToFormattableString();

            Assert.Equal(string.Empty, fs.Format);
            Assert.Equal(0, fs.ArgumentCount);
        }

        [Theory]
        [MemberData(nameof(AppendInterpolated_Single_Data))]
        public void AppendInterpolated_Single(object?[] args)
        {
            var fsb = new FormattableStringBuilder();

            var fs = fsb
                .AppendInterpolated($"{args[0]}")
                .ToFormattableString();

            Assert.Equal("{0}", fs.Format);
            Assert.Equal(args.AsEnumerable(), fs.GetArguments());
        }

        public static IEnumerable<object[]> AppendInterpolated_Single_Data =>
            new List<object[]>
            {
                new object[] { new object?[] { "one" } },
                new object[] { new object?[] { new FormattableStringBuilder() } }
            };

        [Theory]
        [MemberData(nameof(AppendInterpolated_Multiple_Data))]
        public void AppendInterpolated_Multiple(object?[] args)
        {
            var fsb = new FormattableStringBuilder();

            var fs = fsb
                .AppendInterpolated($"{args[0]}")
                .AppendInterpolated($"{args[1]}")
                .ToFormattableString();

            Assert.Equal("{0}{1}", fs.Format);
            Assert.Equal(args.AsEnumerable(), fs.GetArguments());
        }

        public static IEnumerable<object[]> AppendInterpolated_Multiple_Data =>
            new List<object[]>
            {
                new object[] { new object?[] { "one", "two" } },
                new object[] { new object?[] { new FormattableStringBuilder(), new FormattableStringBuilder() } }
            };

        [Fact]
        public void AppendInterpolated_Alignment()
        {
            var fsb = new FormattableStringBuilder();
            var args = new[] { "one" };

            var fs = fsb
                .AppendInterpolated($"{args[0],-1}")
                .ToFormattableString();

            Assert.Equal("{0,-1}", fs.Format);
            Assert.Equal(args.Cast<object?>(), fs.GetArguments());
        }

        [Fact]
        public void AppendInterpolated_FormatString()
        {
            var fsb = new FormattableStringBuilder();
            var args = new[] { "one" };

            var fs = fsb
                .AppendInterpolated($"{args[0]:o}")
                .ToFormattableString();

            Assert.Equal("{0:o}", fs.Format);
            Assert.Equal(args.Cast<object?>(), fs.GetArguments());
        }

        [Fact]
        public void AppendInterpolated_AlignmentAndFormatString()
        {
            var fsb = new FormattableStringBuilder();
            var args = new[] { "one" };

            var fs = fsb
                .AppendInterpolated($"{args[0],-1:o}")
                .ToFormattableString();

            Assert.Equal("{0,-1:o}", fs.Format);
            Assert.Equal(args.Cast<object?>(), fs.GetArguments());
        }

        [Fact]
        public void AppendInterpolated_Braces()
        {
            var fsb = new FormattableStringBuilder();
            var args = new[] { "two", "four" };

            var fs = fsb
                .AppendInterpolated($"{{one}} {{{args[0]}}} {{{{three}}}} {{{{{args[1]}}}}}")
                .ToFormattableString();

            Assert.Equal("{{one}} {{{0}}} {{{{three}}}} {{{{{1}}}}}", fs.Format);
            Assert.Equal(args.Cast<object?>(), fs.GetArguments());
        }

        [Fact]
        public void AppendInterpolated_Verbatim()
        {
            var fsb = new FormattableStringBuilder();
            var args = new[] { "one" };

            var fs = fsb
                .AppendInterpolated($@"{args[0]}")
                .ToFormattableString();

            Assert.Equal("{0}", fs.Format);
            Assert.Equal(args.Cast<object?>(), fs.GetArguments());
        }

        [Fact]
        public void AppendInterpolated_Verbatim_CSharp8()
        {
            var fsb = new FormattableStringBuilder();
            var args = new[] { "one" };

            var fs = fsb
                .AppendInterpolated(@$"{args[0]}")
                .ToFormattableString();

            Assert.Equal("{0}", fs.Format);
            Assert.Equal(args.Cast<object?>(), fs.GetArguments());
        }

        [Fact]
        public void AppendInterpolated_Readme()
        {
            var fsb = new FormattableStringBuilder();
            var args = new[] {
                DateTime.Today,
                DateTime.Today.AddDays(1),
                DateTime.Today.AddDays(2)
            };

            fsb
                .AppendInterpolated($"INSERT INTO dbo.VacationDates (Date)")
                .AppendLine()
                .AppendInterpolated($"VALUES");

            for (var date = args.First(); date <= args.Last(); date = date.AddDays(1))
            {
                fsb
                    .AppendLine()
                    .AppendInterpolated($"({date})");

                if (date != args.Last())
                {
                    fsb.AppendInterpolated($",");
                }
            }

            var fs = fsb.ToFormattableString();

            Assert.Equal("INSERT INTO dbo.VacationDates (Date)" + Environment.NewLine +
                "VALUES" + Environment.NewLine +
                "({0})," + Environment.NewLine +
                "({1})," + Environment.NewLine +
                "({2})", fs.Format);
            Assert.Equal(args.Cast<object?>(), fs.GetArguments());
        }

#if NET7_0_OR_GREATER
        [Fact]
        public void AppendInterpolated_RawStringLiteral()
        {
            var fsb = new FormattableStringBuilder();
            var args = new[] { "one" };

            var fs = fsb
                .AppendInterpolated($"""
{args[0]}
""")
                .ToFormattableString();

            Assert.Equal("{0}", fs.Format);
            Assert.Equal(args.Cast<object?>(), fs.GetArguments());
        }
#endif
    }
}
