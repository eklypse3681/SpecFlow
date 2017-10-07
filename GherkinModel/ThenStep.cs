using TechTalk.SpecFlow;

namespace NUnitHarness
{
    public class ThenStep : Step
    {
        public ThenStep(string text)
            : base(text)
        {

        }

        public ThenStep(string text, string multiLineArg)
            : base(text, multiLineArg)
        {

        }

        public ThenStep(string text, Table tableArg)
            : base(text, tableArg)
        {

        }

        public ThenStep(string text, string multiLineTextArg, Table tableArg)
            : base(text, multiLineTextArg, tableArg)
        {

        }

        public override void Run(ITestRunner runner)
        {
            runner.Then(Text, MultiLineTextArg, TableArg, "Then");
        }

        protected override Step Clone()
        {
            return new ThenStep(Text, MultiLineTextArg, Clone(TableArg));
        }
    }
}