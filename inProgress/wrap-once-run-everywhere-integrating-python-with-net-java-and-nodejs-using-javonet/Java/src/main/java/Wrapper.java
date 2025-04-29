
import com.javonet.sdk.InvocationContext;
import com.javonet.sdk.Javonet;
import com.javonet.sdk.RuntimeContext;

public class Wrapper implements IWrapper {
	private InvocationContext invocationContext;

    public Wrapper() {
        RuntimeContext runtimeContext = Javonet.inMemory().python();

        runtimeContext.loadLibrary("your-python-files-path");

        invocationContext = runtimeContext.getType("MyClass.MyClass").execute();
    }

	@Override
	public String helloWorld() {
		InvocationContext helloWorld = invocationContext
                .invokeInstanceMethod("hello_world", invocationContext)
                .execute();
        return (String) helloWorld.getValue();
	}

	@Override
	public int add(int a, int b) {
		InvocationContext addResult = invocationContext
                .invokeInstanceMethod("add", invocationContext, a, b)
                .execute();
        return (Integer) addResult.getValue();
	}

}
