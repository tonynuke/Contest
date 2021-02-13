using System.Text.Json;
using FluentAssertions;
using Services;
using Xunit;

namespace UnitTests
{
    public class VkEventsSerializationTests
    {
        [Fact]
        public void DeserializeConfirmationEvent_ShouldBe_ConfirmationEventType()
        {
            string json =
                "{" +
                "\"type\":\"confirmation\", " +
                "\"object\":{ \"user_id\":1, \"join_type\":\"approved\" }," +
                "\"group_id\":1 " +
                "}";

            var vkEvent = JsonSerializer.Deserialize<VkEvent>(json);
            vkEvent.Type.Should().Be(VkEventType.Confirmation);
        }
    }
}