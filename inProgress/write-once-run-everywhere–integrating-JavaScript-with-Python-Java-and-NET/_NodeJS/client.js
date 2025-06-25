const PythonWrapper = require('./pythonWrapper');

console.log("Hello, World from NODEJS!!");

const wrapper = new PythonWrapper();

console.log(wrapper.helloWorld());
console.log("add(5, 7):", wrapper.add(5, 7));
