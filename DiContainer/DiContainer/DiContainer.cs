using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DiContainer
{
    public class DiContainer
    {
        private List<Service> relations;
        public DiContainer()
        {
            relations = new List<Service>();
        }

        public void AddTrans<TService, TRealiz>()
        {
            relations.Add(new Service(typeof(TService), typeof(TRealiz), Service2.Transient));
        }

        public void AddSingl<TService, TRealiz>()
        {
            relations.Add(new Service(typeof(TService), typeof(TRealiz), Service2.Singleton));
        }

        public T Get<T>() => (T)Get(typeof(T));

        public object Get(Type serviceType)
        {
            var desc = relations.SingleOrDefault(x => x.ServiceType == serviceType);

            if (desc == null)
            {
                throw new Exception("Не найден сервис");
            }

            if (desc.Implement != null)
            {
                return desc.Implement;
            }

            var actualType = desc.ImplementType;
            var construct = actualType.GetConstructors().First();

            if (construct.GetParameters().Any(x => CheckCycle(serviceType, x.ParameterType)))
            {
                throw new Exception("Зацикливание");
            }

            var paramet = construct.GetParameters().Select(x => Get(x.ParameterType)).ToArray();
            var implement = Activator.CreateInstance(actualType, paramet);

            if (desc.Now == Service2.Singleton)
            {
                desc.Implement = implement;
            }
            return implement;
        }

        public bool CheckCycle(Type serviceType, Type parametType)
        {
            var desc = relations.SingleOrDefault(x => x.ServiceType == parametType);
            var actType = desc.ImplementType;
            var constructType = actType.GetConstructors().First();
            return constructType.GetParameters().Any(x => Equals(serviceType, x.ParameterType));
        }
    }
}
