import com.javonet.sdk.Javonet;

public class Program {
	public static void main(String[] args) {
		Javonet.activate("your-license-key");
		
		System.out.println("Hello, World from JAVA code!!");
		
		Wrapper wrapper = new Wrapper();
		
		System.out.println(wrapper.helloWorld());
		System.out.println(wrapper.add(5, 9));
	}
}
