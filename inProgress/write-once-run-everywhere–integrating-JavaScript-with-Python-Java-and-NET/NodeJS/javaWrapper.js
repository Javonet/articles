const { Javonet } = require('javonet-nodejs-sdk');

class JavaWrapper {
  constructor() {
    Javonet.activate("n9B5-Km7g-Pp69-j9FE-e9A5");
    const runtimeContext = Javonet.inMemory().jvm();

    runtimeContext.loadLibrary("./../MyClass.jar");

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
