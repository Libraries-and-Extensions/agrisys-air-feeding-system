using AgrisysAirFeedingSystem.Models.DBModels;
using AgrisysAirFeedingSystem.Utils.LiveUpdate;
using AgrisysAirFeedingSystem.Utils.LiveUpdate.Formatter;
using AgrisysXTest.testUtils;

namespace AgrisysXTest.Utils.LiveUpdate.Formatter;

using Xunit;


public enum TestEnum
{
    Fail,
    Success,
    Warning
}

public class EnumFormatterTests
{
    private static EnumFormatter FormatterEnum => new EnumFormatter(typeof(TestEnum));
    private static EnumFormatter FormatterStr => new EnumFormatter("Fail","Success","Warning");
    
    
    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { FormatterEnum,  0, "Fail" },
            new object[] { FormatterEnum,  1, "Success" },
            new object[] { FormatterEnum,  2, "Warning" },
            new object[] { FormatterEnum,  3, "Undefined" },
            new object[] { FormatterStr,  0, "Fail" },
            new object[] { FormatterStr,  1, "Success" },
            new object[] { FormatterStr,  2, "Warning" },
            new object[] { FormatterStr,  3, "Undefined" },
        };
    
    [Theory]
    [MemberData(nameof(Data))]
    public void FormatInitialValue_ReturnsCorrectStringRepresentation(EnumFormatter formatter, int value, string expected)
    {
        // Arrange
        var measurement = new SensorMeasurement()
        {
            Value = value
        };

        // Act
        string result = formatter.FormatInitialValue(measurement);

        // Assert
        Assert.Equal(expected, result);
    }
    
    public static IEnumerable<object[]> DataAttribute =>
        new List<object[]>
        {
            new object[] { FormatterEnum,  "Fail,Success,Warning" },
            new object[] { FormatterStr,  "Fail,Success,Warning" },
        };

    
    [Theory]
    [MemberData(nameof(DataAttribute))]
    public void ShouldAddAttributes(EnumFormatter formatter, string value)
    {
        //Arrange
        AttributeProvider attributeProvider = new AttributeProvider();

        // Act
        formatter.AddAttributes(attributeProvider);

        // Assert
        attributeProvider.Test("data-sensor-options",value);
    }
}