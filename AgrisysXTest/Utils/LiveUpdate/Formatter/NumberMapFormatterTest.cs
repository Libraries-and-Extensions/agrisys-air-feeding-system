using System.Globalization;
using AgrisysAirFeedingSystem.Models.DBModels;
using AgrisysAirFeedingSystem.Utils.LiveUpdate;
using AgrisysAirFeedingSystem.Utils.LiveUpdate.Formatter;
using AgrisysXTest.testUtils;

namespace AgrisysXTest.Utils.LiveUpdate.Formatter;

public class NumberMapFormatterTest
{
    //generate test cases for NumberMapFormatter using xunit snippets
// Path: AgrisysAirFeedingSystem/Utils/LiveUpdate/Formatter/NumberMapFormatter.cs

    [Fact]
    public void TestSensorFallbackError()
    {
        //Arrange
        var outputMax = 200;
        var numberMapFormatter = new NumberMapFormatter(outputMax);
        
        var measurement = new SensorMeasurement()
        {
            Value = 30,
        };
        
        //Act
        Assert.ThrowsAny<NullReferenceException>(() => numberMapFormatter.FormatInitialValue(measurement));
    }
    
    [Fact]
    public void TestSensorFallback()
    {
        //Arrange
        var outputMax = 200;
        var numberMapFormatter = new NumberMapFormatter(outputMax);
        
        var measurement = new SensorMeasurement()
        {
            Value = 30,
        };
        
        numberMapFormatter.SensorCheck(new Sensor()
        {
            min = 0,
            max = 100
        });
        
        //Act
        var result = numberMapFormatter.FormatInitialValue(measurement);
        
        //Assert
        Assert.Equal("60", result);
    }
    
        
    [Fact]
    public void TestInputOverride()
    {
        //Arrange
        var outputMax = 200;
        var numberMapFormatter = new NumberMapFormatter(outputMax, inputMax: 100);
        
        var measurement = new SensorMeasurement()
        {
            Value = 30,
        };
        
        numberMapFormatter.SensorCheck(new Sensor()
        {
            min = 0,
            max = 200
        });
        
        //Act
        var result = numberMapFormatter.FormatInitialValue(measurement);
        
        //Assert
        Assert.Equal("60", result);
    }
    
    [Fact]
    public void TestDigits()
    {
        //Arrange
        var outputMax = 200;
        var value = 30;
        var inputMax = 70;
        var digits = 2;
        var numberMapFormatter = new NumberMapFormatter(outputMax, inputMax: inputMax, digit: digits);
        
        var measurement = new SensorMeasurement()
        {
            Value = value,
        };
        
        //Act
        var result = numberMapFormatter.FormatInitialValue(measurement);
        
        //Assert
        Assert.Equal(Math.Round((double)value*outputMax/inputMax,digits).ToString(CultureInfo.CurrentCulture), result);
    }
    
    [Fact]
    public void TestFull()
    {
        //Arrange
        var value = 5;
        var digits = 2;
        var numberMapFormatter = new NumberMapFormatter(5, inputMax: 10,outputMin:-5, inputMin: 0, digit: digits);
        
        var measurement = new SensorMeasurement()
        {
            Value = value,
        };
        
        //Act
        var result = numberMapFormatter.FormatInitialValue(measurement);
        
        //Assert
        Assert.Equal("0", result);
    }
    
    //test attributes
    
        [Fact]
    public void TestSensor()
    {
        //Arrange
        var outputMax = 200;
        var numberMapFormatter = new NumberMapFormatter(outputMax);
        
        var attributes = new AttributeProvider();
        
        //Act
        Assert.ThrowsAny<NullReferenceException>(() => numberMapFormatter.AddAttributes(attributes));
    }
    
    [Fact]
    public void TestAttributeFallback()
    {
        //Arrange
        var outputMax = 200;
        var inputMax = 200;
        var numberMapFormatter = new NumberMapFormatter(outputMax,inputMax:inputMax);
        
        var attributes = new AttributeProvider();
        
        //Act
        numberMapFormatter.AddAttributes(attributes);
        
        //Assert
        attributes.Test("data-sensor-map-in-max",inputMax.ToString());
        attributes.Test("data-sensor-map-out-max",outputMax.ToString());
    }
    
    
        
    [Fact]
    public void TestInput()
    {
        //Arrange
        var outputMax = 200;
        var inputMax = 100;
        var outputMin = 30;
        var inputMin = 40;
        var digits = 2;
        var numberMapFormatter = new NumberMapFormatter(outputMax,inputMax:inputMax,outputMin:outputMin, inputMin: inputMin,digit:digits);
        
        var attributes = new AttributeProvider();
        
        //Act
        numberMapFormatter.AddAttributes(attributes);
        
        //Assert
        attributes.Test("data-sensor-map-in-max",inputMax.ToString());
        attributes.Test("data-sensor-map-out-max",outputMax.ToString());
        attributes.Test("data-sensor-map-in-min",inputMin.ToString());
        attributes.Test("data-sensor-map-out-min",outputMin.ToString());
        attributes.Test("data-sensor-scale-digit",digits.ToString());
    }
}