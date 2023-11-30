using AgrisysAirFeedingSystem.Models.DBModels;
using AgrisysAirFeedingSystem.Utils.LiveUpdate;
using AgrisysAirFeedingSystem.Utils.LiveUpdate.Formatter;
using AgrisysXTest.testUtils;

namespace AgrisysXTest.Utils.LiveUpdate.Formatter;

public class LevelFormatterTest
{
    private static LevelFormatter Formatter => new LevelFormatter(new Dictionary<int, string>()
    {
        {0, "Fail"},
        {13, "Success"},
        {21, "Warning"}
    });
    
    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { Formatter,  -2, "Unknown" },
            new object[] { Formatter,  0, "Fail" },
            new object[] { Formatter,  13, "Success" },
            new object[] { Formatter,  21, "Warning" },
        };
    
    [Theory]
    [MemberData(nameof(Data))]
    public void FormatInitialValue_ReturnsCorrectStringRepresentation(LevelFormatter formatter, int value, string expected)
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
            new object[] { Formatter, "0:Fail,13:Success,21:Warning" }
        };

    
    [Theory]
    [MemberData(nameof(DataAttribute))]
    public void ShouldAddAttributes(LevelFormatter formatter, string value)
    {
        //Arrange
        AttributeProvider attributeProvider = new AttributeProvider();

        // Act
        formatter.AddAttributes(attributeProvider);

        // Assert
        attributeProvider.Test("data-sensor-levels",value);
    }
}
    