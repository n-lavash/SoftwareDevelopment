using System;
using System.Collections.Generic;
using System.Text;

namespace DiContainer
{
    class Service
    {
        public Type ServiceType { get; }

        public Type ImplementType { get; }

        public object Implement { get; internal set; }

        public Service2 Now { get; }

        public Service(Type serviceType, Type implementType, Service2 now)
        {
            ServiceType = serviceType;
            ImplementType = implementType;
            Now = now;
        }
    }
}
