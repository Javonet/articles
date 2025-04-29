namespace Sample.Core
{
    public class Wrapper : IWrapper
    {
        Javonet.Netcore.Sdk.InvocationContext _invocationContext;

        public Wrapper()
        {
            var runtimeContext = Javonet.Netcore.Sdk.Javonet.InMemory().Python();

            runtimeContext.LoadLibrary("your-python-files-path");

            _invocationContext = runtimeContext.GetType("MyClass.MyClass").Execute();
        }

        public string HelloWorld()
        {
            var helloWorld = _invocationContext
                .InvokeInstanceMethod("hello_world", _invocationContext).Execute();
            return (string)helloWorld.GetValue();
        }

        public int Add(int a, int b)
        {
            var addResult = _invocationContext
                .InvokeInstanceMethod("add", _invocationContext, a, b).Execute();
            return (int)addResult.GetValue();
        }
    }
}
