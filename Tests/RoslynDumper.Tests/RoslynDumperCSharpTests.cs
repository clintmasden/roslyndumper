using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using RoslynDumper.Tests.Data;
using RoslynDumper.Tests.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace RoslynDumper.Tests
{
    [Collection(TestCollections.CultureSpecific)]
    public class RoslynDumperCSharpCSharpTests
    {
        public RoslynDumperCSharpCSharpTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        private readonly ITestOutputHelper testOutputHelper;

        private static string GetUtcOffsetString()
        {
            var utcOffset = TimeZoneInfo.Local.BaseUtcOffset;
            return $"{(utcOffset >= TimeSpan.Zero ? "+" : "-")}{utcOffset:hh\\:mm}";
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpArray_OneDimensional()
        {
            // Arrange
            var array = new[] { "aaa", "bbb" };

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(array);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var stringArray = new String[]\r\n{\r\n  \"aaa\",\r\n  \"bbb\"\r\n};");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpArray_TwoDimensional()
        {
            // Arrange
            var array = new int[3, 2]
            {
                {1, 2},
                {3, 4},
                {5, 6}
            };

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(array);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var array = new int[3, 2]{\r\n                {1, 2},\r\n                {3, 4},\r\n                {5, 6}\r\n            };");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpCultureInfo()
        {
            // Arrange
            var cultureInfo = new CultureInfo("de-CH");

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(cultureInfo);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var cultureInfo = new CultureInfo(\"de-CH\");");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpDateTime_DateTimeKind_Local()
        {
            // Arrange
            var dateTime = new DateTime(2000, 01, 01, 23, 59, 59, DateTimeKind.Local);

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(dateTime);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be($"var dateTime = DateTime.ParseExact(\"2000-01-01T23:59:59.0000000{GetUtcOffsetString()}\", \"O\", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);");

            var returnedDateTime = DateTime.ParseExact($"2000-01-01T23:59:59.0000000{GetUtcOffsetString()}", "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            returnedDateTime.Should().Be(dateTime);
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpDateTime_DateTimeKind_Unspecified()
        {
            // Arrange
            var dateTime = new DateTime(2000, 01, 01, 23, 59, 59, DateTimeKind.Unspecified);

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(dateTime);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var dateTime = DateTime.ParseExact(\"2000-01-01T23:59:59.0000000\", \"O\", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);");

            var returnedDateTime = DateTime.ParseExact("2000-01-01T23:59:59.0000000", "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            returnedDateTime.Should().Be(dateTime);
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpDateTime_DateTimeKind_Utc()
        {
            // Arrange
            var dateTime = new DateTime(2000, 01, 01, 23, 59, 59, DateTimeKind.Utc);

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(dateTime);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var dateTime = DateTime.ParseExact(\"2000-01-01T23:59:59.0000000Z\", \"O\", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);");

            var returnedDateTime = DateTime.ParseExact("2000-01-01T23:59:59.0000000Z", "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            returnedDateTime.Should().Be(dateTime);
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpDateTimeMaxValue()
        {
            // Arrange
            var datetime = DateTime.MaxValue;

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(datetime);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var dateTime = DateTime.MaxValue;");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpDateTimeMinValue()
        {
            // Arrange
            var datetime = DateTime.MinValue;

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(datetime);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var dateTime = DateTime.MinValue;");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpDateTimeOffset()
        {
            // Arrange
            var offset = new DateTimeOffset(2000, 01, 01, 23, 59, 59, TimeSpan.Zero);

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(offset);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var dateTimeOffset = DateTimeOffset.ParseExact(\"2000-01-01T23:59:59.0000000+00:00\", \"O\", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpDateTimeOffsetMaxValue()
        {
            // Arrange
            var offset = DateTimeOffset.MaxValue;

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(offset);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var dateTimeOffset = DateTimeOffset.MaxValue;");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpDateTimeOffsetMinValue()
        {
            // Arrange
            var offset = DateTimeOffset.MinValue;

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(offset);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var dateTimeOffset = DateTimeOffset.MinValue;");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpDictionary()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>
            {
                {1, "Value1"},
                {2, "Value2"},
                {3, "Value3"}
            };

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(dictionary);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var dictionary = new Dictionary<Int32, String>\r\n{\r\n  { 1, \"Value1\" },\r\n  { 2, \"Value2\" },\r\n  { 3, \"Value3\" }\r\n};");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpEnum()
        {
            // Arrange
            var dateTimeKind = DateTimeKind.Utc;

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(dateTimeKind);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var dateTimeKind = System.DateTimeKind.Utc;");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpGenericClass_WithMultipleGenericTypeArguments()
        {
            // Arrange
            var person = PersonFactory.GeneratePersons(1).First();
            var genericClass = new GenericClass<string, float, Person> { Prop1 = "Test", Prop2 = 123.45f, Prop3 = person };

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(genericClass);

            // Assert
            testOutputHelper.WriteLine(dump);

            //my output isn't the same because
            //1. same issues as ShouldRoslynDumpNestedObjects
            //2. <String, Single, Person> is not being written correctly

            dump.Should().NotBeNull();
            dump.Should().Be("var genericClass = new GenericClass<String, Single, Person>\r\n{\r\n  Prop1 = \"Test\",\r\n  Prop2 = 123.45f,\r\n  Prop3 = new Person\r\n  {\r\n    Name = \"Person 1\",\r\n    Char = '',\r\n    Age = 2,\r\n    GetOnly = 11,\r\n    Bool = false,\r\n    Byte = 0,\r\n    ByteArray = new Byte[]\r\n    {\r\n      1,\r\n      2,\r\n      3,\r\n      4\r\n    },\r\n    SByte = 0,\r\n    Float = 0f,\r\n    Uint = 0,\r\n    Long = 0L,\r\n    ULong = 0L,\r\n    Short = 0,\r\n    UShort = 0,\r\n    Decimal = 0m,\r\n    Double = 0d,\r\n    DateTime = DateTime.MinValue,\r\n    NullableDateTime = null,\r\n    Enum = System.DateTimeKind.Unspecified\r\n  }\r\n};");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpGenericClass_WithNestedGenericTypeArguments()
        {
            // Arrange
            var complexDictionary = new Dictionary<string[,], List<int?[,][]>[,,]>[1]
            {
                new Dictionary<string[,], List<int?[,][]>[,,]>()
            };

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(complexDictionary);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var dictionary = new Dictionary<String[,], List<Nullable<Int32>[,][]>[,,]>[]\r\n{\r\n  new Dictionary<String[,], List<Nullable<Int32>[,][]>[,,]>\r\n  {\r\n  }\r\n};");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpGuid()
        {
            // Arrange
            var guid = new Guid("024CC229-DEA0-4D7A-9FC8-722E3A0C69A3");

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(guid);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var guid = new Guid(\"024cc229-dea0-4d7a-9fc8-722e3a0c69a3\");");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpNullableObject()
        {
            // Arrange
            DateTime? datetime = null;

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(datetime);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var x = null;");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpRecursiveTypes_RecursivePerson()
        {
            // Arrange
            var person = new RecursivePerson();
            person.Parent = person;

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(person);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var recursivePerson = new RecursivePerson\r\n{\r\n};");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpRuntimeType()
        {
            // Arrange
            var type = typeof(Person);

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(type);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var runtimeType = typeof(ObjectDumping.Tests.Testdata.Person);");
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpTimeSpan()
        {
            // Arrange
            var timeSpan = new TimeSpan(1, 2, 3, 4, 5);

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(timeSpan);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var timeSpan = TimeSpan.ParseExact(\"1.02:03:04.0050000\", \"c\", CultureInfo.InvariantCulture, TimeSpanStyles.None);");

            var returnedTimeSpan = TimeSpan.ParseExact("1.02:03:04.0050000", "c", CultureInfo.InvariantCulture, TimeSpanStyles.None);
            returnedTimeSpan.Should().Be(timeSpan);
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpTimeSpan_MaxValue()
        {
            // Arrange
            var timeSpan = TimeSpan.MaxValue;

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(timeSpan);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var timeSpan = TimeSpan.MaxValue;");

            var returnedTimeSpan = TimeSpan.MaxValue;
            returnedTimeSpan.Should().Be(timeSpan);
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpTimeSpan_MinValue()
        {
            // Arrange
            var timeSpan = TimeSpan.MinValue;

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(timeSpan);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var timeSpan = TimeSpan.MinValue;");

            var returnedTimeSpan = TimeSpan.MinValue;
            returnedTimeSpan.Should().Be(timeSpan);
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpTimeSpan_Negative()
        {
            // Arrange
            var timeSpan = new TimeSpan(1, 2, 3, 4, 5).Negate();

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(timeSpan);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var timeSpan = TimeSpan.ParseExact(\"-1.02:03:04.0050000\", \"c\", CultureInfo.InvariantCulture, TimeSpanStyles.None);");

            var returnedTimeSpan = TimeSpan.ParseExact("-1.02:03:04.0050000", "c", CultureInfo.InvariantCulture, TimeSpanStyles.None);
            returnedTimeSpan.Should().Be(timeSpan);
        }

        [Fact(Skip = "unsupported")]
        public void ShouldDumpTimeSpan_Zero()
        {
            // Arrange
            var timeSpan = TimeSpan.Zero;

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(timeSpan);

            // Assert
            testOutputHelper.WriteLine(dump);
            dump.Should().NotBeNull();
            dump.Should().Be("var timeSpan = TimeSpan.Zero;");

            var returnedTimeSpan = TimeSpan.Zero;
            returnedTimeSpan.Should().Be(timeSpan);
        }

        [Fact]
        public void ShouldRoslynDumpEnumerable()
        {
            // Arrange
            var persons = PersonFactory.GeneratePersons(2).ToList();

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(persons);

            // Assert
            testOutputHelper.WriteLine(dump);

            dump.Should().NotBeNullOrWhiteSpace();
            dump.Should().Be("var listOfPersons = new List<Person>{new Person{Name = \"Person 1\", Char = '\\0', Age = 3, GetOnly = 11, Bool = false, Byte = 0, ByteArray = new Byte[]{1, 2, 3, 4}, SByte = 0, Float = 0F, Uint = 0U, Long = 0L, ULong = 0UL, Short = 0, UShort = 0, Decimal = 0M, Double = 0, DateTime = DateTime.MinValue, NullableDateTime = null, Enum = System.DateTimeKind.Unspecified}, new Person{Name = \"Person 2\", Char = '\\0', Age = 3, GetOnly = 11, Bool = false, Byte = 0, ByteArray = new Byte[]{1, 2, 3, 4}, SByte = 0, Float = 0F, Uint = 0U, Long = 0L, ULong = 0UL, Short = 0, UShort = 0, Decimal = 0M, Double = 0, DateTime = DateTime.MinValue, NullableDateTime = null, Enum = System.DateTimeKind.Unspecified}};");
        }

        [Fact]
        public void ShouldRoslynDumpNestedObjects()
        {
            // Arrange
            var persons = PersonFactory.GeneratePersons(2).ToList();
            var organization = new Organization { Name = "name", Persons = persons };

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(organization);

            // Assert
            testOutputHelper.WriteLine(dump);

            dump.Should().NotBeNullOrWhiteSpace();
            dump.Should().Be("var organization = new Organization{Name = \"name\", Persons = new List{new Person{Name = \"Person 1\", Char = '\\0', Age = 3, GetOnly = 11, Bool = false, Byte = 0, ByteArray = new Byte[]{1, 2, 3, 4}, SByte = 0, Float = 0F, Uint = 0U, Long = 0L, ULong = 0UL, Short = 0, UShort = 0, Decimal = 0M, Double = 0, DateTime = DateTime.MinValue, NullableDateTime = null, Enum = System.DateTimeKind.Unspecified}, new Person{Name = \"Person 2\", Char = '\\0', Age = 3, GetOnly = 11, Bool = false, Byte = 0, ByteArray = new Byte[]{1, 2, 3, 4}, SByte = 0, Float = 0F, Uint = 0U, Long = 0L, ULong = 0UL, Short = 0, UShort = 0, Decimal = 0M, Double = 0, DateTime = DateTime.MinValue, NullableDateTime = null, Enum = System.DateTimeKind.Unspecified}}};");
        }

        [Fact]
        public void ShouldRoslynDumpObject()
        {
            // Arrange
            var person = PersonFactory.GetPersonThomas();

            // Act
            var roslynDumper = new RoslynDumper();
            var dump = roslynDumper.Dump(person);

            // Assert
            testOutputHelper.WriteLine(dump);

            dump.Should().NotBeNullOrWhiteSpace();
            dump.Should().Be("var person = new Person{Name = \"Thomas\", Char = '\\0', Age = 30, GetOnly = 11, Bool = false, Byte = 0, ByteArray = new Byte[]{1, 2, 3, 4}, SByte = 0, Float = 0F, Uint = 0U, Long = 0L, ULong = 0UL, Short = 0, UShort = 0, Decimal = 0M, Double = 0, DateTime = DateTime.MinValue, NullableDateTime = null, Enum = System.DateTimeKind.Unspecified};");
        }
    }
}