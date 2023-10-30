namespace Homework_18.Exception
{
    class СlientHasNoAccountException : System.Exception
    {
        public int ErrorCode { get; }
        public СlientHasNoAccountException() :
            base("У клиента отсутсвует счет!")
        {
            ErrorCode = 100;
        }
    }
}
