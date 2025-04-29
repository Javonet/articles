const {Javonet} = require('javonet-nodejs-sdk')
const Wrapper = require('./wrapper');

Javonet.activate("your-license-key");

console.log("Hello, World from NODEJS!!");

const wrapper = new Wrapper();

console.log(wrapper.helloWorld());
console.log("add(5, 7):", wrapper.add(5, 7));
