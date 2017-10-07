using TechTalk.SpecFlow;

namespace NUnitHarness
{
    public class ButStep : Step
    {
        public ButStep(string text)
            : base(text)
        {

        }

        public ButStep(string text, string multiLineArg)
            : base(text, multiLineArg)
        {

        }

        public ButStep(string text, Table tableArg)
            : base(text, tableArg)
        {

        }

        public ButStep(string text, string multiLineTextArg, Table tableArg)
            : base(text, multiLineTextArg, tableArg)
        {

        }

        public override void Run(ITestRunner runner)
        {
            runner.But(Text, MultiLineTextArg, TableArg, "But");
        }

        protected override Step Clone()
        {
            return new ButStep(Text, MultiLineTextArg, Clone(TableArg));
        }
    }
}