using System;

namespace DiContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            DiContainer di_container = new DiContainer();
            di_container.AddTrans<IA, A>();
            di_container.AddTrans<IB, B>();
            var instance = di_container.Get(typeof(IA));
        }
    }
}
