# Wrap Once, Run Everywhere: Integrating Python with .NET, Java, and Node.js using Javonet

## Introduction

In today’s diverse software landscape, integrating code across different technologies has become not just useful, but often essential. Imagine writing your core business logic once in Python and then instantly making it available to a .NET, Java, or Node.js application — without rewriting or rearchitecting your solution. With Javonet, this vision becomes a reality.

Javonet allows you to create native wrappers around your Python code, providing a natural, idiomatic interface for foreign technologies. This approach brings the best of both worlds: you maintain the simplicity and productivity of Python while exposing your functionality seamlessly to external platforms. No complex API layers, no messy middleware — just clean, native interoperability.

In this article, we'll build a simple Python module and demonstrate how elegantly it can be wrapped for use in .NET, Java, and Node.js applications. Whether you’re modernizing an existing system, creating cross-platform libraries, or simply looking to maximize code reuse, Javonet offers a powerful and developer-friendly solution.

Let’s dive in and see how easy it can be to bridge the gap between languages — the elegant way!

[toc]

## Sample Python code

Let’s start by creating a simple Python class that we will later wrap and expose to other platforms. Our class, `MyClass`, contains two basic methods: one returns a greeting message, and the other performs a simple addition.

Here is the Python code:
```python=
class MyClass:
    __type__ = "MyClass"

    def hello_world(self) -> str:
        return "Hello, World from Python Class!!"
    
    def add(self, a: int, b: int) -> int:
        return a + b

```

* The `hello_world` method simply returns a friendly greeting. 
* The `add` method takes two integers and returns their sum.

Notice the `__type__` attribute defined at the top of the class. This attribute can be used to explicitly set the class type name when working with **Javonet**, making it easier to reference this class from external platforms like .NET, Java, or Node.js.

Now that we have our Python logic ready, let’s see how we can elegantly wrap and access it from different technologies using Javonet.

## Wrapping the Python Class for .NET
To expose our Python class `MyClass` to a .NET application using **Javonet**, we will create a native .NET wrapper that feels completely natural for C# developers.
Our wrapper will consist of three elements:

* An interface to define the contract.
* A class implementation that internally delegates calls to the Python object.
* A `Program.cs` file to demonstrate how to use the wrapper.

Let's break it down step-by-step:

### Install nuget package
First, you need to install `nuget` package for your **.NET** application:
```bash!
dotnet add package Javonet.Netcore.Sdk -s https://api.nuget.org/v3/index.json
```
If you want to use **.NET Framework**:
```bash!
dotnet add package Javonet.Clr.Sdk -s https://api.nuget.org/v3/index.json
```
>[!NOTE] 
>You also need to change class path in your code from: `Javonet.Netcore.Sdk` to `Javonet.Clr.Sdk.`


### Create the Interface
First, define a simple interface that mirrors the methods of the Python class:
```csharp=
namespace Sample.Core
{
    public interface IWrapper
    {
        string HelloWorld();

        int Add(int a, int b);
    }
}
```
### Implement the Wrapper Class
Now, create a class that implements the interface. This class will use Javonet to invoke the Python methods under the hood.
Let's create a class with a simple constructor. We need to create a new runtime context for Python by:
```csharp!
Javonet.Netcore.Sdk.Javonet.InMemory().Python();
```
and load the path to our Python files:
```csharp!
runtimeContext.LoadLibrary("your-python-files-path");
```
Then we can create an invocation context for our Python class:
```csharp!
_invocationContext = runtimeContext.GetType("MyClass.MyClass").Execute();
```
This will be the constructor for the `Wrapper` class:
```csharp=
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
    }
}
```
Now we can add the implementation for each method. In the final step, this class should look like the example below:
```csharp=
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
```
### Build the Program Entry Point
The final step is to add a `Program.cs` class that will allow us to run and test our code. Within this class, we also need to handle the activation of Javonet:
```csharp!
Javonet.Netcore.Sdk.Javonet.Activate("your-license-key");
```
>[!NOTE]
>Of course, when implementing such a solution in a real project, you should initialize Javonet in a location that best suits the architecture and lifecycle of your application (for example, during application startup or within a dedicated initialization module).

For now, the complete Program.cs class should look like the example below:
```csharp=
using Sample.Core;

Console.WriteLine("Hello, World from .NET code!");

Javonet.Netcore.Sdk.Javonet.Activate("your-license-key");

var wrapper = new Wrapper();

Console.WriteLine(wrapper.HelloWorld());
Console.WriteLine($"Adding result from Python code: {wrapper.Add(5, 9)}");
```
## Wrapping the Python Class for Java
To expose our Python class `MyClass` to a Java application using **Javonet**, we will follow a very similar structure to the .NET approach.
Our Java wrapper will consist of three elements:

* An interface defining the contract.
* A class implementation that internally delegates calls to the Python object.
* A `Program` class to demonstrate how to run and test the solution.

Let's go through each part step-by-step:
### Edit pom.xml file
You need to add Javonet to your `pom.xml` file:
```xml=
<project xmlns="http://maven.apache.org/POM/4.0.0" 
         xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" 
         xsi:schemaLocation="http://maven.apache.org/POM/4.0.0 https://maven.apache.org/xsd/maven-4.0.0.xsd">
  
  <modelVersion>4.0.0</modelVersion>

  <groupId>com.chatbot</groupId>
  <artifactId>chatbot</artifactId>
  <version>0.0.1-SNAPSHOT</version>
  <name>chatbot.client</name>
  <description>test</description>

  <dependencies>
    <dependency>
      <groupId>com.javonet</groupId>
      <artifactId>javonet-java-sdk</artifactId>
      <version>2.5.16</version>
    </dependency>
  </dependencies>

</project>

```
### Create the Interface
First, define a Java interface that mirrors the methods of the Python class:
```java=
public interface IWrapper {
    String helloWorld();

    int add(int a, int b);
}
```
### Implement the Wrapper Class
Now, create a class that implements the interface. This class will use Javonet to invoke the Python methods behind the scenes:
```java=
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
```
### Build the Program Entry Point
Finally, create a `Program` class that will initialize Javonet, create an instance of your wrapper, and invoke its methods to demonstrate everything works:
```java=
import com.javonet.sdk.Javonet;

public class Program {
  public static void main(String[] args) {
    Javonet.activate("your-license-key");
		
    System.out.println("Hello, World from JAVA code!!");

    Wrapper wrapper = new Wrapper();

    System.out.println(wrapper.helloWorld());
    System.out.println("Adding result from Python code: " + wrapper.add(5, 9));
	}
}
```
>[!NOTE]
> As with any real-world project, you might want to place the Javonet activation and runtime initialization in a more centralized location depending on your application's architecture.
## Wrapping the Python Class for Node.js
For Node.js, the structure is slightly simpler compared to .NET and Java.
We will organize our solution into two files:

* A `wrapper.js` file containing the wrapper class and all necessary logic.
* A `client.js` file serving as the entry point to run and test the solution.

Let's walk through the structure:
### Install packages
The first step is to add Javonet to your project by installing the required npm packages.
Simply run the following command to include Javonet in your project dependencies:
```bash!
npm install javonet-nodejs-sdk
```
and:
```bash!
npm install javonet-binaries
```
### Create the Wrapper File
First, create a `wrapper.js` file that defines a class wrapping the Python functionality.
This class will:
* Initialize the Javonet runtime.
* Load the Python library.
* Create an instance of the Python class.
* Implement methods that internally delegate calls to the Python object.

```javascript=
const { Javonet } = require('javonet-nodejs-sdk');

class Wrapper {
  constructor() {
    const runtimeContext = Javonet.inMemory().python();

    runtimeContext.loadLibrary("your-python-files-path");

    this.invocationContext = runtimeContext
      .getType("MyClass.MyClass")
      .execute();
  }

  helloWorld() {
    const result = this.invocationContext
      .invokeInstanceMethod("hello_world", this.invocationContext)
      .execute();

    return result.getValue();
  }

  add(a, b) {
    const result = this.invocationContext
      .invokeInstanceMethod("add", this.invocationContext, a, b)
      .execute();

    return result.getValue();
  }
}

module.exports = Wrapper;

```
### Create the Client File
Next, create a `client.js` file that will serve as the main entry point for your application.
This file will:
* Require the `wrapper.js` module.
* Activate **Javonet**
* Instantiate the **wrapper** class.
* Call its methods and display the results.

```javascript=
const {Javonet} = require('javonet-nodejs-sdk')
const Wrapper = require('./wrapper');

Javonet.activate("your-license-key");

console.log("Hello, World from NODEJS!!");

const wrapper = new Wrapper();

console.log(wrapper.helloWorld());
console.log("add(5, 7): ", wrapper.add(5, 7));
```

>[!NOTE] 
>In a production-ready application, you might want to structure the initialization and runtime activation more cleanly, possibly separating configuration and runtime setup into a dedicated module.
## Conclusion
As you can see, **Javonet** makes it incredibly easy to bridge your code with other technology stacks. By creating simple, native wrappers, you can seamlessly integrate your logic into external applications, without rewriting or reimplementing your core business functionality.

This approach not only boosts code reuse and consistency across platforms, but also allows teams to innovate faster by combining the strengths of different technologies in a clean, maintainable way.

Whether you are modernizing legacy systems, developing cross-platform libraries, or building scalable multi-language applications, Javonet offers a powerful and elegant solution to make interoperability effortless.
Now it’s your turn — take your code and connect it to the world!🚀