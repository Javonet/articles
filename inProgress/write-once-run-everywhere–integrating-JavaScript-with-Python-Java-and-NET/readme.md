# Write Once, Run Everywhere – Integrating JavaScript with Python, Java and .NET

## Introduction

In the ever-expanding ecosystem of modern software development, the need to reuse logic across platforms has evolved from a convenience to a strategic advantage. Imagine writing your business logic once in JavaScript — the language of the web — and immediately reusing it inside desktop, backend, or mobile applications built with Python, Java, or .NET. No translation layers, no rewrites — just direct, efficient reuse.

This is precisely the power Javonet brings to the table. By enabling native interoperability between JavaScript and other major platforms, Javonet allows you to wrap your existing Node.js modules and expose them as if they were native libraries in any target environment. You retain the speed and agility of JavaScript while delivering robust integrations across the stack.

In this article, we’ll take a simple JavaScript module and show how it can be embedded directly into applications written in Python, Java, and .NET. Whether you’re a frontend developer expanding into backend systems, a team leader maintaining multi-language projects, or an architect looking to maximize code reuse, Javonet offers a clean and seamless path toward true cross-platform integration.

Let’s explore how easy and elegant it can be to connect your JavaScript logic to the rest of your tech stack — without compromise.

[toc]

## Code samples

### Sample Python code

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

### Sample Java code
We can now create an equivalent class in Java, preserving the same logic and structure to ensure seamless integration from other platforms.

Here is the Java code:
```java=
package com.mydomain.myjavaproject;

public class MyClass {
    public String helloWorld() {
        return "Hello, World from Java Class!!";
    }
    
    public int add(int a, int b) {
        return a + b;
    }
}
```
### Sample .NET code
We can now create an equivalent class in .NET, following the same logic and naming conventions used in the Python and Java versions.

Here is the C# code:
```csharp=
namespace MyLibrary
{
    public class MyClass
    {
        public string HelloWorld()
        {
            return "Hello, World from C# Class!!";
        }

        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}
```
## Wrapping the code for Node.js
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

**Python:**

```javascript=
const { Javonet } = require('javonet-nodejs-sdk');

class PythonWrapper {
  constructor() {
    Javonet.activate("your-API-key");
    const runtimeContext = Javonet.inMemory().python();

    runtimeContext.loadLibrary("your-python-py-file");

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

module.exports = PythonWrapper;
```
**Java:**

```javascript=
const { Javonet } = require('javonet-nodejs-sdk');

class JavaWrapper {
  constructor() {
    Javonet.activate("your-API-key");
    const runtimeContext = Javonet.inMemory().jvm();

    runtimeContext.loadLibrary("your-jar-path");

    this.invocationContext = runtimeContext
      .getType("MyClass.MyClass")
      .execute();
  }

  helloWorld() {
    const result = this.invocationContext
      .invokeInstanceMethod("helloWorld", this.invocationContext)
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

module.exports = JavaWrapper;
```
**C#:**

```javascript=
const { Javonet } = require('javonet-nodejs-sdk');

class CsharpWrapper {
  constructor() {
    Javonet.activate("your-API-key");
    
    // 'netcore' for .NET Core, 'clr' for .NET Framework
    const runtimeContext = Javonet.inMemory().netcore();

    runtimeContext.loadLibrary("your-DLL-path");

    this.invocationContext = runtimeContext
      .getType("MyClass.MyClass")
      .execute();
  }

  helloWorld() {
    const result = this.invocationContext
      .invokeInstanceMethod("HelloWorld", this.invocationContext)
      .execute();

    return result.getValue();
  }

  add(a, b) {
    const result = this.invocationContext
      .invokeInstanceMethod("Add", this.invocationContext, a, b)
      .execute();

    return result.getValue();
  }
}

module.exports = CsharpWrapper;
```
### Create the Client File
Next, create a `client.js` file that will serve as the main entry point for your application.
This file will:
* Require the `wrapper.js` module.
* Activate **Javonet**
* Instantiate the **wrapper** class.
* Call its methods and display the results.

```javascript=
const PythonWrapper = require('./pythonWrapper');
const JavaWrapper = require('./javaWrapper');
const CsharpWrapper = require('./csharpWrapper')

console.log("Hello, World from NODEJS!!");

const pythonWrapper = new PythonWrapper();

console.log(pythonWrapper.helloWorld());
console.log("add(5, 7):", pythonWrapper.add(5, 7));

const javaWrapper = new JavaWrapper();

console.log(javaWrapper.helloWorld());
console.log("add(6, 4):", javaWrapper.add(6, 4));

const csharpWrapper = new CsharpWrapper();

console.log(csharpWrapper.helloWorld());
console.log("add(2, 1):", csharpWrapper.add(2, 1));
```

>[!NOTE] 
>In a production-ready application, you might want to structure the initialization and runtime activation more cleanly, possibly separating configuration and runtime setup into a dedicated module.
## Conclusion
As you've seen, Javonet empowers you to break the boundaries between platforms by making your JavaScript code instantly accessible to Python, Java, and .NET applications. With just a lightweight wrapper, you can expose your logic natively — without duplicating effort, introducing fragile glue code, or compromising maintainability.

This seamless integration not only accelerates development and ensures consistency, but also unlocks the full potential of a polyglot architecture — where each team can work in their preferred language, while sharing a common codebase underneath.

Whether you're migrating functionality from the frontend to backend, building SDKs for multiple platforms, or simply aiming to get more value out of your JavaScript modules, Javonet offers a clean, efficient, and developer-friendly path forward.

Now it’s your move — write once in JavaScript, and let it run everywhere! 🚀