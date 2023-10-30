using System.Diagnostics;

namespace Homework_18.TemplateMethod
{
    abstract class MyAbstractClass
    {
        public void TemplateMethod()
        {
            BaseOperation();
            RequiredOperations();
            Hook();
        }

        protected void BaseOperation()
        {
            Debug.WriteLine("===> BaseOperation базовая pеализация в абстрактном классе");
        }

        protected abstract void RequiredOperations();


        protected virtual void Hook()
        {
            Debug.WriteLine("===> Hook Реализация по умолчанию");
        }
    }
}
