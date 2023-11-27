using AgrisysAirFeedingSystem.Utils.LiveUpdate;

namespace AgrisysXTest.testUtils;

public static class AttributeProviderUtilTest
{
    public static void Test(this AttributeProvider attributeProvider, string key,string value)
    {
        if (attributeProvider.tryGet(key,out var inMax))
        {
            Assert.Equal(value, inMax);
        }
        else
        {
            Assert.Fail("data-sensor-map-in-max not found");
        }
    }
}