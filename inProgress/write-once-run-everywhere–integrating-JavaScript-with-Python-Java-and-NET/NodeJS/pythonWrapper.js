const { Javonet } = require('javonet-nodejs-sdk');

class PythonWrapper {
  constructor() {
    const runtimeContext = Javonet.inMemory().python();

    runtimeContext.loadLibrary("C:/coding/articles/inProgress/write-once-run-everywhere–integrating-JavaScript-with-Python-Java-and-.NET/Python/MyClass.py");

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
