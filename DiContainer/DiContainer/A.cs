using System;
using System.Collections.Generic;
using System.Text;

namespace DiContainer
{
    public interface IA
    {
        void writeA();
    }
    public class A : IA
    {
        public A(IB b) { }
        public void writeA()
        {
            Console.WriteLine("Класс A");
        }
    }
}
