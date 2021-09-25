using Notifications.Common.Fields;
using Xunit;

namespace Notifications.Tests
{
    public class EventTitleTests
    {
        [Fact]
        public void ValidationSuccess()
        {

        }

        [Fact]
        public void ValidationFailure()
        {
            const string input = null;
            var result = EventTitle.Validate(input);

            Assert.True(result.IsFailure);
            Assert.Equal("Value should not be empty", result.Error);
        }
    }
}
