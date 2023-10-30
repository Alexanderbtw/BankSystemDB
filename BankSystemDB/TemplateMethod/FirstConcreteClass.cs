using System.Diagnostics;

namespace Homework_18.TemplateMethod
{
    class FirstConcreteClass : MyAbstractClass
    {
        protected override void RequiredOperations()
        {
            Debug.WriteLine("===> RequiredOperations реализация в конкретном классе: FirstConcreteClass");
        }
    }
}
