const { Javonet } = require('javonet-nodejs-sdk');

class PythonWrapper {
  constructor() {
    Javonet.activate("n9B5-Km7g-Pp69-j9FE-e9A5");
    const runtimeContext = Javonet.inMemory().python();

    runtimeContext.loadLibrary("./../Python/MyClass.py");

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
