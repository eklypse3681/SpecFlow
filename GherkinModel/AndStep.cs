using TechTalk.SpecFlow;

namespace NUnitHarness
{
    public class AndStep : Step
    {
        public AndStep(string text)
            : base(text)
        {

        }

        public AndStep(string text, string multiLineArg)
            : base(text, multiLineArg)
        {

        }

        public AndStep(string text, Table tableArg)
            : base(text, tableArg)
        {

        }

        public AndStep(string text, string multiLineTextArg, Table tableArg)
            : base(text, multiLineTextArg, tableArg)
        {

        }

        public override void Run(ITestRunner runner)
        {
            runner.And(Text, MultiLineTextArg, TableArg, "And");
        }

        protected override Step Clone()
        {
            return new AndStep(Text, MultiLineTextArg, Clone(TableArg));
        }
    }
}