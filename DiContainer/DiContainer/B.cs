using System;
using System.Collections.Generic;
using System.Text;

namespace DiContainer
{
    public interface IB
    {
        void writeB();
    }
    public class B : IB
    {
        public B(IA a) { }
        public void writeB()
        {
            Console.WriteLine("Класс B");
        }
    }
}
