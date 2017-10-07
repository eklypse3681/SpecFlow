using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnitHarness;

namespace GherkinModel
{
    public interface ITestProvider
    {
        FeatureGroup GetRootFeatureGroup();
    }
}
