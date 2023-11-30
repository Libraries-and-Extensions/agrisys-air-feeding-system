using System.Globalization;
using AgrisysAirFeedingSystem.Models.DBModels;
using AgrisysAirFeedingSystem.Utils.LiveUpdate;
using AgrisysAirFeedingSystem.Utils.LiveUpdate.Handler;
using AgrisysXTest.testUtils;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AgrisysXTest.Utils.LiveUpdate.Handler;

public class CustomHandlerTests
{
    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { "value", null!},
            new object[] {"value", new SensorMeasurement(){Value = 1,SensorId = 1,TimeStamp = DateTime.Now.ToUniversalTime()}},
        };
    
    [Theory]
    [MemberData(nameof(Data))]
    public async void HandleInitialValue(string value, SensorMeasurement? measurement)
    {
        // Arrange
        var handler = new CustomHandler();
        
        var output = new TagHelperOutput("div", new TagHelperAttributeList(), (useCachedResult,
            htmlEncoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        handler.HandleInitialValue(value, output,measurement);

        // Assert
        if (measurement == null)
        {
            Assert.False(output.Attributes.TryGetAttribute("data-custom-initial", out _));
        }
        else
        {
            Assert.True(output.Attributes.TryGetAttribute("data-custom-initial", out _));
        }
    }
}
