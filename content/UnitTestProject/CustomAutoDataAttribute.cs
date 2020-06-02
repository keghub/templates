using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace Tests
{
    public class CustomAutoDataAttribute : AutoDataAttribute
    {
        public CustomAutoDataAttribute() : base (FixtureHelper.CreateFixture) { }
    }

    public class InlineCustomAutoDataAttribute : InlineAutoDataAttribute
    {
        public InlineCustomAutoDataAttribute(params object[] arguments) : base(FixtureHelper.CreateFixture, arguments) { }
    }

    public static class FixtureHelper
    {
        public static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            fixture.Customize(new AutoMoqCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });

            return fixture;
        }
    }
}