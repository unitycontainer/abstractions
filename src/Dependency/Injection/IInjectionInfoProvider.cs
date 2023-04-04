namespace Unity.Injection;



public interface IInjectionInfoProvider
{
    void ProvideInfo<TInjectionInfo>(ref TInjectionInfo info)
        where TInjectionInfo : IInjectionInfo;
}
