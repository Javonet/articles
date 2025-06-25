const { Javonet } = require('javonet-nodejs-sdk');

class CsharpWrapper {
  constructor() {
    Javonet.activate("n9B5-Km7g-Pp69-j9FE-e9A5");
    const runtimeContext = Javonet.inMemory().netcore();

    runtimeContext.loadLibrary("./");

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
