namespace MirGames.Specs.Steps
{
    using FluentAutomation;
    using FluentAutomation.Interfaces;

    using TechTalk.SpecFlow;

    public abstract class StepDefinitionBase : Steps
    {
        protected FluentTest CurrentTest
        {
            get
            {
                return (FluentTest)ScenarioContext.Current[ScenarioContext.Current.ScenarioInfo.Title];
            }
        }

        protected IActionSyntaxProvider I
        {
            get
            {
                return this.CurrentTest.I;
            }
        }

        protected object Provider
        {
            get
            {
                return this.CurrentTest.Provider;
            }
        }

        protected FluentSession Session
        {
            get
            {
                return this.CurrentTest.Session;
            }
        }
    }
}