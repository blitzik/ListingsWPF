using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Evidoo
{
    public interface IApplication
    {
        AppDomain AppDomain { get; }        
        string AppDomainName { get; }
        string AppName { get; }
        bool ShadowCopyFiles { get; }
        ApplicationState State { get; }

        void Start(Action<Thread> threadModifier);
    }


}
