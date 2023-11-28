using AgrisysAirFeedingSystem.Utils.LiveUpdate;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AgrisysXTest.testUtils;

public static class AttributeProviderUtilTest
{
    public static void Test(this AttributeProvider attributeProvider, string key,string value)
    {
        if (attributeProvider.TryGet(key,out var inMax))
        {
            Assert.Equal(value, inMax);
        }
        else
        {
            Assert.Fail(key+" not found");
        }
    }
}

public static class TagHelperAttributeListTestUtil
{
    public static void Test(this TagHelperAttributeList attributeProvider, string key,string value)
    {
        if (attributeProvider.TryGetAttribute(key,out var attribute))
        {
            Assert.Equal(value, attribute.Value);
        }
        else
        {
            Assert.Fail(key + "not found");
        }
    }
}