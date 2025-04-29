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
