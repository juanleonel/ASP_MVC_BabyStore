
using StructureMap;

namespace BabyStore.DependencyResolution
{
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                    scan.AssembliesFromApplicationBaseDirectory();
                });
            //For<IExample>().Use<Example>();
        }

        #endregion
    }
}