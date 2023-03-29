namespace Unity.Injection;


public delegate void InjectionInfoProvider<TInjectionInfo>(ref TInjectionInfo info)
    where TInjectionInfo : IInjectionInfo;
