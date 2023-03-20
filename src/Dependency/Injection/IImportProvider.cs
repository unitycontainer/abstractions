using Unity.Resolution;

namespace Unity.Dependency
{
    //public delegate void ImportProvider<TDescriptor>(ref TDescriptor descriptor)
    //    where TDescriptor : IInjectionInfo;

    //public delegate void ImportProvider<TMemberInfo, TDescriptor>(ref TDescriptor descriptor)
    //    where TDescriptor : IInjectionInfo<TMemberInfo>;


    public interface IImportProvider
    {
        void ProvideImport<TDescriptor>(ref TDescriptor descriptor)
            where TDescriptor : IInjectionInfo;
    }


    public interface IImportProvider<TMemberInfo> : IImportProvider
    {
        new void ProvideImport<TDescriptor>(ref TDescriptor descriptor)
            where TDescriptor : IInjectionInfo<TMemberInfo>;
    }
}
