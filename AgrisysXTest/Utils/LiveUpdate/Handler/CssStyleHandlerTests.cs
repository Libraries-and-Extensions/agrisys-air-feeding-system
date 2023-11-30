using AgrisysAirFeedingSystem.Utils.LiveUpdate;
using AgrisysAirFeedingSystem.Utils.LiveUpdate.Handler;
using AgrisysXTest.testUtils;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AgrisysXTest.Utils.LiveUpdate.Handler;

public class CssStyleHandlerTests
{
    
    private static CssStyleHandler HandlerTemp => new("height","px");
    
    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { HandlerTemp,  "30","height:30px" },
            new object[] { HandlerTemp,  "20","height:20px" },
        };

    
    [Theory]
    [MemberData(nameof(Data))]
    public async void HandleInitialValue_ReturnsCorrectHtml(CssStyleHandler handler, string value,string expected)
    {
        // Arrange
        var output = new TagHelperOutput("div", new TagHelperAttributeList(), (useCachedResult,
            htmlEncoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        handler.HandleInitialValue(value, output,null);

        // Assert
        if (output.Attributes.TryGetAttribute("style", out var attribute))
        {
            Assert.Equal(expected, attribute.Value);
        }
        else
        {
            Assert.True(false);
        }
    }
    
    public static IEnumerable<object[]> DataAttribute =>
        new List<object[]>
        {
            new object[] { "height","px"},
            new object[] { "width","px"},
            new object[] { "height","rem"},
        };

    
    [Theory]
    [MemberData(nameof(DataAttribute))]
    public void ShouldAddAttributes(string style, string unit)
    {
        //Arrange
        var handler = new CssStyleHandler(style, unit);
        
        AttributeProvider attributeProvider = new AttributeProvider();

        // Act
        handler.AddAttributes(attributeProvider);

        // Assert
        attributeProvider.Test("data-style-handler-property", style);
        attributeProvider.Test("data-style-handler-unit", unit);
    }
}
