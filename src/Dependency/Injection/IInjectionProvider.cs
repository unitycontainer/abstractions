namespace Unity.Injection;



public interface IInjectionProvider
{
    void GetInjectionInfo<TInjectionInfo>(ref TInjectionInfo info)
        where TInjectionInfo : IInjectionInfo;
}


public interface IInjectionProvider<TMemberInfo> : IInjectionProvider
{
    new void GetInjectionInfo<TInjectionInfo>(ref TInjectionInfo info)
        where TInjectionInfo : IInjectionInfo<TMemberInfo>;
}
