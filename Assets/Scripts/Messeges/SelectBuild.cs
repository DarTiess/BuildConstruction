using Data;

namespace Messeges
{
    public struct SelectBuild
    {
        private readonly BuildType _buildType;

        public BuildType BuildType => _buildType;

        public SelectBuild(BuildType buildType)
        {
            _buildType = buildType;
        }
    }
}