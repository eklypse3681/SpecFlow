using System.Collections.Generic;

namespace NUnitHarness
{
    public interface IScenarioProvider
    {
        IEnumerable<Scenario> Scenarios { get; }
    }
}