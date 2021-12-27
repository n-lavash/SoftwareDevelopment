using System;
using System.Collections.Generic;
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

        public object Get(Type service, List<Type> list)
        {
            var descriptor = relations.SingleOrDefault(x => x.ServiceType == service);
            if (descriptor == null)
            {
                throw new Exception("Не найден сервис");
            }
            if (descriptor.Implement != null)
            {
                return descriptor.Implement;
            }



            var actual = descriptor.ImplementType;
            var construct = actual.GetConstructors().First();

            List<object> new_list = new List<object>();

            foreach (var p in construct.GetParameters())
            {
                if (list.Contains(service))
                {
                    throw new Exception("Зацикливание");
                }

                list.Add(service);

                var newParameter = Get(p.ParameterType, list);

                list.Remove(service);

                new_list.Add(newParameter);
            }


            var parameters = new_list.ToArray();
            var implementation = Activator.CreateInstance(actual, parameters);
            if (descriptor.Now == Service2.Singleton)
            {
                descriptor.Implement = implementation;
            }
            return implementation;
        }
    }
}
