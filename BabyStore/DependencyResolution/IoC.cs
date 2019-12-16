
using System.Security.Cryptography.X509Certificates;

namespace BabyStore.DependencyResolution
{
    using StructureMap;

    public static class IoC
    {
        public static IContainer Initialize()
        {

            return new Container(c => c.AddRegistry<DefaultRegistry>());
        }
    }
}