using System.Globalization;
using AgrisysAirFeedingSystem.Models.DBModels;
using AgrisysAirFeedingSystem.Utils.LiveUpdate;
using AgrisysAirFeedingSystem.Utils.LiveUpdate.Formatter;
using AgrisysXTest.testUtils;

namespace AgrisysXTest.Utils.LiveUpdate.Formatter;

using Xunit;
using Moq;

public class FormatInitialValueTests
{
    [Theory]
    [InlineData(1000,2, 10543, "10.54")]
    [InlineData(1000,2, 10546, "10.55")]
    [InlineData(1000,1, 10543, "10.5")]
    [InlineData(1000,1, 10553, "10.6")]
    public void Should_ReturnFormattedNumber(int scale, int? digit, int value, string expected)
    {
        // Arrange
        var formatter = new NumberFormatter(scale, digit);
        var measurement = new SensorMeasurement()
        {
            Value = value
        };

        // Act
        var result = formatter.FormatInitialValue(measurement);

        // Assert
        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData(1000, 10543, "10.5")]
    [InlineData(10000, 10543, "1.05")]
    [InlineData(100, 10543, "105")]
    public void ShouldAdjustDigitsIfNotSpecified(int scale, int value, string expected)
    {
        // Arrange
        var formatter = new NumberFormatter(scale);
        var measurement = new SensorMeasurement()
        {
            Value = value
        };

        formatter.SensorCheck(new Sensor()
        {
            min = 0,
            max = 2000
        });

        // Act
        var result = formatter.FormatInitialValue(measurement);

        // Assert
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(1000,2, null)]
    [InlineData(1000,2, 2)]
    public void ShouldAddAttributes(int value,int scale, int? digit)
    {
        //Arrange
        var formatter = new NumberFormatter(scale, digit);

        AttributeProvider attributeProvider = new AttributeProvider();

        // Act
        formatter.AddAttributes(attributeProvider);

        // Assert
        attributeProvider.Test("data-sensor-scale-factor",scale.ToString());
        if (digit.HasValue) attributeProvider.Test("data-sensor-scale-digit",digit.ToString());
    }
}