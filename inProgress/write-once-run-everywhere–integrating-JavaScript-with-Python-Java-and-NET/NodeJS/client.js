const PythonWrapper = require('./pythonWrapper');
const JavaWrapper = require('./javaWrapper');
const CsharpWrapper = require('./csharpWrapper')

console.log("Hello, World from NODEJS!!");

const pythonWrapper = new PythonWrapper();

console.log(pythonWrapper.helloWorld());
console.log("add(5, 7):", pythonWrapper.add(5, 7));

const javaWrapper = new JavaWrapper();

console.log(javaWrapper.helloWorld());
console.log("add(5, 7):", javaWrapper.add(5, 7));

const csharpWrapper = new CsharpWrapper();

console.log(csharpWrapper.helloWorld());
console.log("add(5, 7):", csharpWrapper.add(5, 7));