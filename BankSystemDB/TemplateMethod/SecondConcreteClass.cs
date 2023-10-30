using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Homework_18.TemplateMethod
{
    class SecondConcreteClass : MyAbstractClass
    {
        protected override void RequiredOperations()
        {
            Debug.WriteLine("===> RequiredOperations реализация в конкретном классе: SecondConcreteClass");
        }

        protected override void Hook()
        {
            Debug.WriteLine("===> Hook реализация в конкретном классе: SecondConcreteClass");
        }
    }
}
