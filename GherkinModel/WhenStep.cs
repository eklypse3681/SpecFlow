using TechTalk.SpecFlow;

namespace NUnitHarness
{
    public class WhenStep : Step
    {
        public WhenStep(string text)
            : base(text)
        {

        }

        public WhenStep(string text, string multiLineArg)
            : base(text, multiLineArg)
        {

        }

        public WhenStep(string text, Table tableArg)
            : base(text, tableArg)
        {

        }

        public WhenStep(string text, string multiLineTextArg, Table tableArg)
            : base(text, multiLineTextArg, tableArg)
        {

        }

        public override void Run(ITestRunner runner)
        {
            runner.When(Text, MultiLineTextArg, TableArg, "When");
        }

        protected override Step Clone()
        {
            return new WhenStep(Text, MultiLineTextArg, Clone(TableArg));
        }
    }
}