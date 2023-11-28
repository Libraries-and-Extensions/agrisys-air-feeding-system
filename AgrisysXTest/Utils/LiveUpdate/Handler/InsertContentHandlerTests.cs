using AgrisysAirFeedingSystem.Utils.LiveUpdate;
using AgrisysAirFeedingSystem.Utils.LiveUpdate.Handler;
using AgrisysXTest.testUtils;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AgrisysXTest.Utils.LiveUpdate.Handler;

public class InsertContentHandlerTests
{
    
    private static ContentHandler HandlerTemp => new("height","px");
    
    public static IEnumerable<object[]> DataPrefix =>
        new List<object[]>
        {
            new object[] { CreateHandler("Hello", "World"), "Value", "HelloValueWorld" },
            new object[] { CreateHandler(null,"World"), "Value", "ValueWorld"},
            new object[] { CreateHandler("Hello", null), "Value", "HelloValue" },
            new object[] { CreateHandler("Hello{value}World"), "Value", "HelloValueWorld" },
        };
    
    private static ContentHandler CreateHandler(string? prefix,string? suffix)
    {
        return new(prefix,suffix);
    }
    
    private static ContentHandler CreateHandler(string format)
    {
        return new(format);
    }

    
    [Theory]
    [MemberData(nameof(DataPrefix))]
    public async void HandleInitialValue_ReturnsCorrectHtml(ContentHandler handler,string value,string expected)
    {
        // Arrange
        var output = new TagHelperOutput("div", new TagHelperAttributeList(), (useCachedResult,
            htmlEncoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        handler.HandleInitialValue(value, output,null);

        // Assert
        Assert.Equal(expected, output.Content.GetContent());
    }
    
    public static IEnumerable<object[]> DataAttribute =>
        new List<object[]>
        {
            new object[] { CreateHandler("Hello", "World"), null!, "Hello", "World" },
            new object[] { CreateHandler(null,"World"), null!, null!, "World"},
            new object[] { CreateHandler("Hello", null), null!, "Hello", null! },
            new object[] { CreateHandler("Hello{value}World"), "Hello{value}World", null!,null! },
        };

    
    [Theory]
    [MemberData(nameof(DataAttribute))]
    public void ShouldAddAttributes(ContentHandler handler,string? format,string? prefix,string? suffix)
    {
        //Arrange
        AttributeProvider attributeProvider = new AttributeProvider();

        // Act
        handler.AddAttributes(attributeProvider);

        // Assert
        if (format != null) attributeProvider.Test("data-content-format", format);
        if (prefix != null) attributeProvider.Test("data-content-prefix", prefix);
        if (suffix != null) attributeProvider.Test("data-content-suffix", suffix);
    }
}
