using AgrisysAirFeedingSystem.Utils.LiveUpdate;
using AgrisysAirFeedingSystem.Utils.LiveUpdate.Handler;
using AgrisysXTest.testUtils;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AgrisysXTest.Utils.LiveUpdate.Handler;

public class CssClassHandlerTests
{
    
    private static CssClassHandler HandlerTemp => new("My{value}Class");
    private static CssClassHandler HandlerRaw => new();
    
    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { HandlerTemp,  "Css","MyCssClass" },
            new object[] { HandlerTemp,  "Hello","MyHelloClass" },
            new object[] { HandlerRaw,  "Css","Css"},
            new object[] { HandlerRaw,  "Hello","Hello" },
        };

    
    [Theory]
    [MemberData(nameof(Data))]
    public void HandleInitialValue(CssClassHandler handler, string value,string expected)
    {
        // Arrange
        var output = new TagHelperOutput("div", new TagHelperAttributeList(), (useCachedResult,
            htmlEncoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        handler.HandleInitialValue(value, output,null);
        
        // Assert
        output.Attributes.Test("class", expected);
        output.Attributes.Test("data-old-css-class", expected);
    }
    
    public static IEnumerable<object[]> DataAttribute =>
        new List<object[]>
        {
            new object[] { HandlerTemp,  "My{value}Class" },
            new object[] { HandlerRaw,  null! },
        };

    
    [Theory]
    [MemberData(nameof(DataAttribute))]
    public void ShouldAddAttributes(CssClassHandler handler, string? value)
    {
        //Arrange
        AttributeProvider attributeProvider = new AttributeProvider();

        // Act
        handler.AddAttributes(attributeProvider);

        // Assert
        if (value != null) attributeProvider.Test("data-css-class",value);
    }
    
    [Fact]
    public void ShouldThrowExceptionIfNoValue()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<Exception>(() => new CssClassHandler("MyClass"));
    }
}
