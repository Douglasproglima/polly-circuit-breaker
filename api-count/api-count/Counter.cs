using System;
using System.Reflection;
using System.Runtime.Versioning;

namespace api_count
{
    public class Counter
    {
        private static readonly string _LOCALE;
        private static readonly string _KERNEL;
        private static readonly string _TARGET_FWK;

        static Counter()
        {
            _LOCALE = Environment.MachineName;
            _KERNEL = Environment.OSVersion.VersionString;
            _TARGET_FWK = Assembly
                .GetEntryAssembly()?
                .GetCustomAttribute<TargetFrameworkAttribute>()?
                .FrameworkName;
        }

        private int _valorAtual = 0;

        public int ValorAtual { get => _valorAtual; }
        public string Local { get => _LOCALE; }
        public string Kernel { get => _KERNEL; }
        public string TargetFramework { get => _TARGET_FWK; }

        public void Incrementar() =>  _valorAtual++;
    }
}
