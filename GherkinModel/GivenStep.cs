using TechTalk.SpecFlow;

namespace NUnitHarness
{
    public class GivenStep : Step
    {
        public GivenStep(string text)
            : base(text)
        {

        }

        public GivenStep(string text, string multiLineArg)
            : base(text, multiLineArg)
        {

        }

        public GivenStep(string text, Table tableArg)
            : base(text, tableArg)
        {

        }

        public GivenStep(string text, string multiLineTextArg, Table tableArg)
            : base(text, multiLineTextArg, tableArg)
        {
            
        }

        public override void Run(ITestRunner runner)
        {
            runner.Given(Text, MultiLineTextArg, TableArg, "Given");
        }

        protected override Step Clone()
        {
            return new GivenStep(Text, MultiLineTextArg, Clone(TableArg));
        }
    }
}