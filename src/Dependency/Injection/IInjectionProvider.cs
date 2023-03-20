namespace Unity.Injection;



public interface IInjectionProvider
{
    void ProvideInfo<TInjectionInfo>(ref TInjectionInfo info)
        where TInjectionInfo : IInjectionInfo;
}


public interface IInjectionProvider<TMemberInfo> : IInjectionProvider
{
    new void ProvideInfo<TInjectionInfo>(ref TInjectionInfo info)
        where TInjectionInfo : IInjectionInfo<TMemberInfo>;
}
