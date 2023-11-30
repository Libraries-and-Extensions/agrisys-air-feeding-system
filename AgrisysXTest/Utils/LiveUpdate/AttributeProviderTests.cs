using AgrisysAirFeedingSystem.Utils.LiveUpdate;
using AgrisysAirFeedingSystem.Utils.LiveUpdate.Handler;

namespace AgrisysXTest.Utils.LiveUpdate;

public class AttributeProviderTests
{
        public class AttributeProviderTest
    {
        [Fact]
        public void Add_AddsNewKeyValuePair()
        {
            // Arrange
            var provider = new AttributeProvider();

            // Act
            provider.Add("key", "value");

            // Assert
            Assert.Equal("value", provider.Get("key"));
        }
        
        [Theory]
        [InlineData("key","value", "key","value")]
        [InlineData("key", "value","otherkey",null!)]
        public void Get(string setKey,string setValue,string getKey,string? getValue)
        {
            // Arrange
            var provider = new AttributeProvider();
            provider.Add(setKey, setValue);

            // Act
            var result = provider.Get(getKey);

            // Assert
            Assert.Equal(getValue, result);
        }

        [Fact]
        public void Remove_RemovesExistingKeyValuePair()
        {
            // Arrange
            var provider = new AttributeProvider();
            provider.Add("key", "value");

            // Act
            provider.Remove("key");

            // Assert
            Assert.False(provider.Has("key"));
        }

        [Fact]
        public void TryGet_ReturnsFalseForNonExistentKey()
        {
            // Arrange
            var provider = new AttributeProvider();

            // Act
            bool result = provider.TryGet("key", out string? value);

            // Assert
            Assert.False(result);
            Assert.Null(value);
        }

        [Fact]
        public void TryGet_ReturnsTrueForExistentKey()
        {
            // Arrange
            var provider = new AttributeProvider();
            provider.Add("key", "value");

            // Act
            bool result = provider.TryGet("key", out string? value);

            // Assert
            Assert.True(result);
            Assert.Equal("value", value);
        }

        [Fact]
        public void Has_ReturnsFalseForNonExistentKey()
        {
            // Arrange
            var provider = new AttributeProvider();

            // Act
            bool result = provider.Has("key");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Has_ReturnsTrueForExistentKey()
        {
            // Arrange
            var provider = new AttributeProvider();
            provider.Add("key", "value");

            // Act
            bool result = provider.Has("key");

            // Assert
            Assert.True(result);
        }
    }
}