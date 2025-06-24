const {Javonet} = require('javonet-nodejs-sdk')
const PythonWrapper = require('./pythonWrapper');

Javonet.activate("n9B5-Km7g-Pp69-j9FE-e9A5");

console.log("Hello, World from NODEJS!!");

const wrapper = new PythonWrapper();

console.log(wrapper.helloWorld());
console.log("add(5, 7):", wrapper.add(5, 7));
