---
title: How to use PyJokes in Other Programming Languages?!
tags: [topublish]

---

# How to use PyJokes in Other Programming Languages?!

In this short article I will show you how to use PyJokes Python module directly in other programming languages. This simple will help you understand endless possibilities of combining multiple programming languages in your projects without needing web services!

[toc]

## PyJokes

First, let’s start with what PyJokes is. PyJokes is a Python library that generates programming-related jokes, making it a fun and lightweight tool for adding humor to your Python projects. It’s especially useful for quick laughs during development, or even for adding a playful touch to your software applications. Directly in Python you could use it like this:

```bash
pip install pyjokes
```

```python
import pyjokes

# Get a random programming joke
joke = pyjokes.get_joke()
print(joke)
```

This code imports the pyjokes module, retrieves a random programming joke, and prints it to the console. Pretty simple, right?

## Let the Magic Happen

Now that we've seen how easy it is to use PyJokes in Python, let's explore how we can make the magic happen by integrating it into other programming languages! The possibilities are endless, and you can add a bit of fun to your projects, no matter what language you're using. Today we will use some popular languages like Java, Ruby and Node.js, but remember to try other!


![Azure SDK](https://hackmd.io/_uploads/rky56Spb1l.jpg)





## Ruby - example
In your environment with Ruby just follow this:
```bash
gem install javonet-ruby-sdk
```

*Don’t forget to install Python and PyJokes in your environment*

```ruby
require 'javonet-ruby-sdk'
Javonet.activate("<your-javonet-key>")

called_runtime = Javonet.in_memory.python

joke = called_runtime.get_type('pyjokes.get_joke').create_instance().execute

result = joke.get_value
puts "pyjokes: " + result
```
```bash
$ ruby main.rb
"pyjokes: 3 Database Admins walked into a NoSQL bar. A little while later they walked out because they couldn't find a table."
```

## Java - example
In your environment with Java just follow this:

Frst add Javonet sdk with Maven or Gradle, here I will use Maven. In your **pom.xml** include:
```xml
<dependency>
    <groupId>com.javonet</groupId>
    <artifactId>javonet</artifactId>
    <version>2.5.2</version>  <!-- Use the latest version -->
</dependency>
```

And then in your **App.java** just use it :)
```java
import com.javonet.sdk.Javonet;
import com.javonet.sdk.InvocationContext;
import com.javonet.sdk.RuntimeContext;

public class App {
    public static void main(String[] args) {
        Javonet.activate("<your-javonet-key>");

        RuntimeContext pythonRuntime = Javonet.inMemory().python();

        InvocationContext jokeInstance = pythonRuntime.getType("pyjokes.get_joke").createInstance().execute();

        String result = (String) jokeInstance.getValue();

        System.out.println("Your PyJoke with Javonet: ");
        System.out.println(result);
    }
}
```
```bash
$ mvn compile
$ mvn exec:java -Dexec.mainClass="App"

[INFO] Scanning for projects...
[INFO] 
[INFO] -----------------------< com.example:my-project >-----------------------
[INFO] Building my-project 1.0-SNAPSHOT
[INFO]   from pom.xml
[INFO] --------------------------------[ jar ]---------------------------------
[INFO] 
[INFO] --- exec:3.5.0:java (default-cli) @ my-project ---
Your PyJoke with Javonet: 
In C we had to code our own bugs. In C++ we can inherit them.
[INFO] ------------------------------------------------------------------------
[INFO] BUILD SUCCESS
[INFO] ------------------------------------------------------------------------
[INFO] Total time:  3.274 s
[INFO] Finished at: 2024-11-20T17:38:01Z
[INFO] ------------------------------------------------------------------------
```

## Node.js - example
In your environment with Node.js just follow this:
```bash
npm i javonet-nodejs-sdk
```
```javascript
const {Javonet} = require('javonet-nodejs-sdk')
Javonet.activate("<your-javonet-key>")

let pythonRuntime = Javonet.inMemory().python()

let joke = pythonRuntime.getType('pyjokes.get_joke').createInstance().execute()

let result = joke.getValue()
console.log(result)
```
```bash
$ node app.js
"A QA engineer walks into a bar. Runs into a bar. Crawls into a bar. Dances into a bar. Tiptoes into a bar. Rams a bar. Jumps into a bar."
```

# Javonet
:::info
**Remember** to install Python and PyJokes in your environment (whether it's local, a VM, Docker, a web app, Codespace, or whatever setup you’re using). Also with your package manager install Javonet for your langauge.
```bash
pip install pyjokes
```
:::

So, as you might have guessed, we’re using Javonet here. Javonet is a cool tool that lets different programming languages talk to each other. Basically, it acts as a bridge, allowing us to run Python code, like PyJokes, in languages such as Java, Ruby, and Node.js without dealing with the mess of APIs or web services. It's a pretty slick way to integrate Python modules into your multi-language projects with minimal hassle.
</br>To use it, just head over to javonet.com and click on the free trial option. It’s completely free, super simple, and incredibly fast—took me just 3 minutes to get my key and start playing with it.

:::info
We are using some Javonet syntax (in different languages, naming conventions might be a little bit different, such as camelCase or snake_case) here like:
- **Javonet.activate("<your-javonet-key>")** - Javonet needs to be activated first. Activation must be called only once at the start-up of an application
- **Javonet.inMemory().python()** - As a second step, Runtime Context of the called technology needs to be created. RuntimeContext refers to single instance of the called runtime. Once it is created it is used to interact with called runtime.
- **pythonRuntime.getType('pyjokes.get_joke').createInstance().execute()** - Use find our module pyjokes and it's command get_joke. Next crete instance of it and execute.
- **joke.getValue()** - The returned value needs to be cast to calling technology type and can be used as any other variable
    
:::

# Conclusion
In this article, we’ve seen how to easily integrate the PyJokes module from Python into other programming languages using Javonet. By leveraging tools like Javonet, you can seamlessly combine Python with languages like Java, Ruby, and Node.js in your projects, without the need for web services or complicated setups. Whether you're working in a local environment, VM, Docker, or Codespace, it's a quick and efficient way to add some fun to your development process.

So, go ahead—experiment with different languages and enjoy the endless possibilities!
# Links

https://pyjok.es/
https://www.javonet.com/


