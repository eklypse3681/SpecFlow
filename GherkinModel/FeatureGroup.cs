using System.Collections.Generic;

namespace NUnitHarness
{
    public class FeatureGroup
    {
        public string Name { get; }
        private readonly List<Feature> _features = new List<Feature>();
        private readonly List<FeatureGroup> _groups = new List<FeatureGroup>();

        public IEnumerable<Feature> Features => _features;
        public IEnumerable<FeatureGroup> Groups => _groups;

        public void AddFeature(Feature feature) => _features.Add(feature);
        public void AddGroup(FeatureGroup group) => _groups.Add(group);
        public FeatureGroup AddGroup(string name)
        {
            var group = new FeatureGroup(name);
            _groups.Add(group);
            return group;
        }

        public FeatureGroup(string name)
        {
            Name = name;
        }
    }
}

