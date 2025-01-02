---
title: Why Limit Yourself? Unlock Python's AI Power in .NET with PyTorch Integration!
tags: [published]

---

# Why Limit Yourself? Unlock Python's AI Power in .NET with PyTorch Integration!

Discover how to integrate Python-based AI models, like PyTorch, directly into your .NET applications. Follow our step-by-step guide to create and test your own AI model seamlessly.

[toc]

## Introduction

Artificial Intelligence (AI) is transforming industries across the globe, and Python is at the forefront, thanks to its powerful libraries and an active community. However, if you're a .NET developer, you might wonder if you’re excluded from this innovation. The answer is: absolutely not!

You don’t need to settle for technology-specific solutions or struggle to connect disparate applications. Instead, you can seamlessly bridge the gap between .NET and Python, unlocking the full potential of AI.

In this article, we demonstrate how to use Python-based AI models within .NET applications. While the AI model we'll build is simple, the focus is on showcasing how easily these technologies can work together.

## Python AI code
>[!NOTE]
>Javonet currently requires you to set the desired Python version as the first entry in your system's environment variables and install the required pip packages.

Before we dive into coding, ensure you have [Python installed](https://www.python.org/downloads/). In our example, we use Python 3.12. You’ll also need [pip](https://pip.pypa.io/en/stable/installation/) to manage dependencies.

Once these prerequisites are ready, install the necessary Python packages using pip. Open your terminal and run:
```bash!
python3 -m pip install torch torchvision matplotlib
```

The complete code for this project is available on our GitHub repository. Here's the project structure we used:
```
PythonCode
│   app.py
└───scripts
│   │ train.py
│   │ test_model.py
│   └ test_custom_image.py
└───models
└───data
└───images
    └ test_image.jpg
```
By following the provided project structure, you'll have a well-organized Python project that’s easy to scale and maintain. Let’s now delve into the core of the project—training, testing, and customizing your AI model.

### Training the model
The training process involves preparing the data, defining a model, and optimizing it using a loss function and optimizer. Below is the `train.py` script:
```python=
# train.py
import os
import string
import torch
import torchvision
import torchvision.transforms as transforms
import torch.optim as optim
import torch.nn as nn
import torchvision.models as models

class Train:
    __type__ = "Train"
    
    def train(self, models_path: string):
        # Setup data loaders
        transform = transforms.Compose([
            transforms.ToTensor(),
            transforms.Normalize((0.5, 0.5, 0.5), (0.5, 0.5, 0.5))
        ])

        trainset = torchvision.datasets.CIFAR10(root='./data', train=True, download=True, transform=transform)
        trainloader = torch.utils.data.DataLoader(trainset, batch_size=4, shuffle=True)

        # Initialize model, loss, and optimizer
        model = models.resnet18(pretrained=True)
        num_features = model.fc.in_features
        model.fc = nn.Linear(num_features, 10)  # CIFAR-10 has 10 classes

        criterion = nn.CrossEntropyLoss()
        optimizer = optim.SGD(model.parameters(), lr=0.001, momentum=0.9)

        # Training loop
        for epoch in range(2):
            running_loss = 0.0
            for i, data in enumerate(trainloader, 0):
                inputs, labels = data
                optimizer.zero_grad()
                outputs = model(inputs)
                loss = criterion(outputs, labels)
                loss.backward()
                optimizer.step()

                running_loss += loss.item()
                if i % 2000 == 1999:
                    print(f"[{epoch + 1}, {i + 1}] loss: {running_loss / 2000:.3f}")
                    running_loss = 0.0

        # Save the trained model
        os.makedirs(models_path, exist_ok=True)
        torch.save(model.state_dict(), models_path + '/cifar10_model.pth')
        print(f"Model saved in '{models_path}/cifar10_model.pth'")
```
### Testing the Model
After training, it’s crucial to test the model's performance using a test dataset. Here’s the `test_model.py` script:
```Python=
# test_model.py
import string
import torch
import torchvision
import torchvision.transforms as transforms
import torchvision.models as models

class TestModel:
    __type__ = "TestModel"
    
    def test(self, models_path: string):
        # Setup data loader for testing
        transform = transforms.Compose([
            transforms.ToTensor(),
            transforms.Normalize((0.5, 0.5, 0.5), (0.5, 0.5, 0.5))
        ])

        testset = torchvision.datasets.CIFAR10(root='./data', train=False, download=True, transform=transform)
        testloader = torch.utils.data.DataLoader(testset, batch_size=4, shuffle=False)

        # Load the trained model
        model = models.resnet18()
        model.fc = torch.nn.Linear(model.fc.in_features, 10)
        model.load_state_dict(torch.load(models_path + '/cifar10_model.pth'))
        model.eval()

        # Test the model
        correct = 0
        total = 0
        with torch.no_grad():
            for data in testloader:
                images, labels = data
                outputs = model(images)
                _, predicted = torch.max(outputs, 1)
                total += labels.size(0)
                correct += (predicted == labels).sum().item()

        print(f"Accuracy of the network on the 10000 test images: {100 * correct / total}%")
```
### Customizing the Test
You can also test the model using your own custom images with the `test_custom_image.py` script:
```Python=
# test_custom_image.py
import string
import torch
import torchvision.transforms as transforms
from PIL import Image
import torchvision.models as models

class TestCustomImage:
    __type__ = "TestCustomImage"
    
    def test(self, models_path: string, image_path: string):
        # Load the trained model
        model = models.resnet18()
        model.fc = torch.nn.Linear(model.fc.in_features, 10)  # CIFAR-10 has 10 classes
        model.load_state_dict(torch.load(models_path + '/cifar10_model.pth'))
        model.eval()

        # Define the classes for CIFAR-10
        classes = ('plane', 'car', 'bird', 'cat', 'deer', 'dog', 'frog', 'horse', 'ship', 'truck')

        # Define the image transformation to match training setup
        transform = transforms.Compose([
            transforms.Resize((32, 32)),        # Resize image to 32x32
            transforms.ToTensor(),              # Convert image to PyTorch tensor
            transforms.Normalize((0.5, 0.5, 0.5), (0.5, 0.5, 0.5))  # Normalize
        ])

        # Load and preprocess your custom image
        image = Image.open(image_path)
        image = transform(image).unsqueeze(0)  # Add batch dimension

        # Run the image through the model
        output = model(image)
        _, predicted = torch.max(output, 1)

        # Print the prediction
        print(f"Predicted class: {classes[predicted.item()]}")

```
### Running the Application
Bring it all together in `app.py` class, here you can use a relative paths to your directories.
```Python=
# app.py
from scripts.test_model import TestModel
from scripts.test_custom_image import TestCustomImage
from scripts.train import Train

Train().train("./models")
TestModel().test("./models")
TestCustomImage().test("./models", "./images")
```
Run the application:
```bash!
python3 app.py
```
If everything is set up correctly, you should see output similar to:
```!
PS D:\PythonCode> python app.py
Downloading https://www.cs.toronto.edu/~kriz/cifar-10-python.tar.gz to ./data\cifar-10-python.tar.gz
100.0%
Extracting ./data\cifar-10-python.tar.gz to ./data
[1, 2000] loss: 2.719
[1, 4000] loss: 2.434
[1, 6000] loss: 2.387
[1, 8000] loss: 2.430
[1, 10000] loss: 2.348
[1, 12000] loss: 2.290
[2, 2000] loss: 2.341
[2, 4000] loss: 2.382
[2, 6000] loss: 2.280
[2, 8000] loss: 2.177
[2, 10000] loss: 2.103
[2, 12000] loss: 2.051
Model saved in './models/cifar10_model.pth'
Files already downloaded and verified
Accuracy of the network on the 10000 test images: 37.45%
Predicted class: horse
```
Even if the model predicts a deer instead of a horse, don’t worry :) The purpose here is to demonstrate how to integrate Python AI models into .NET applications.

## Javonet installation
If you haven't installed Javonet yet, [here are the exact instructions for .NET](https://www.javonet.com/guides/v2/csharp/python-package/getting-started/getting-started-dotnet).

Now we can proceed to the most interesting part. How to call our Python code from .NET application.

## Creating a .NET Application
Start by setting up a simple .NET console application. In this example, we’ll use .NET 8:
```
dotnet new console -n DotTorch
```
>[!NOTE]
>Due to some technical limitations of Javonet we have to use classic `Main()` structure od `Program.cs`

Here’s how the initial structure of your Program.cs file should look:
```csharp=
namespace DotnetTorch
{
    using Javonet.Netcore.Sdk;

    public class Program
    {
        public static void Main(string[] args)
        {
            
        }
    }
}

```
### Integrating Javonet with Python
Now, let’s activate Javonet and add support for the **Python runtime**. Start by activating Javonet using your license key:

```csharp!
Javonet.Activate(paste_your_license_key_here);

var calledRuntime = Javonet.InMemory().Python();
```

Next, provide the absolute path to your **Python script directory** and load the custom code:
```csharp!
// path to custom python code
var pythonResourcesPath = absolute_path_to_your_python_code_directory;

// load custom code
calledRuntime.LoadLibrary($"{pythonResourcesPath}/scripts");
```

### Training Your Model
To train the model, first identify the Python class responsible for training. In this case, it’s the `Train` class located in the `train` module. Define its name:
```csharp!
var trainClassName = "train.Train";
```
Create an instance of this class:
```csharp!
var calledTrainRuntime = calledRuntime.GetType(trainClassName).Execute();
```
Now we can call our first method:
```csharp!
var modelsPath = $"{pythonResourcesPath}/models";
calledTrainRuntime.InvokeInstanceMethod("train", calledTrainRuntime, modelsPath).Execute();
```
In this part we need to focus for a moment. We created an instance of our first class. We store it in `calledTrainRuntime` variable. So we can access any of its methods here. To invoke those methods we used `InvokeInstanceMethod()` with parameters.
As you probably remember definition of `train()` method in Python starts like this:
```python!
def train(self, models_path: string):
```
In our .NET `InvokeInstanceMethod` we used:
```csharp!
InvokeInstanceMethod("train", calledTrainRuntime, modelsPath)
```
Let's analyze the given parameters:
* `"train"` - this is the name of our **Python method** in `Train` class, we don't need to add `()` here, only the name. Next parameters will be passed as parameters for Python method,
* `calledTrainRuntime` - because Python method expects *`self`* parameter we need to pass our class itself,
* `modelsPath` - most obvious, a string with absolute path of **models** directory.

Here’s the updated code snippet for this step:
```csharp!
// train py model
var trainClassName = "train.Train";
var calledTrainRuntime = calledRuntime.GetType(trainClassName).Execute();
var modelsPath = $"{pythonResourcesPath}/models";
calledTrainRuntime.InvokeInstanceMethod("train", calledTrainRuntime, modelsPath).Execute();
```
At this point, your `Program.cs` file should look like this:
```csharp=
namespace DotnetTorch
{
    using Javonet.Netcore.Sdk;

    public class Program
    {
        public static void Main(string[] args)
        {
            Javonet.Activate("paste_your_license_key_here");

            var calledRuntime = Javonet.InMemory().Python();

            // path to custom python code
            var pythonResourcesPath = absolute_path_to_your_python_code_directory;

            // load custom code
            calledRuntime.LoadLibrary($"{pythonResourcesPath}/scripts");

            // train py model
            var trainClassName = "train.Train";
            var calledTrainRuntime = calledRuntime.GetType(trainClassName).Execute();
            var modelsPath = $"{pythonResourcesPath}/models";
            calledTrainRuntime.InvokeInstanceMethod("train", calledTrainRuntime, modelsPath).Execute();
        }
    }
}
```
Great! Now you can run this part of code! As a result you should see something similiar to:
```!
Verifying existing license file
Python Managed Runtime Info:
Python Version: 3.12.7
Python executable path: ----
Python Path: [----]
Python Implementation: CPython
OS Version: Windows 10.0.22631
Process Architecture: AMD64
Current Directory: ----

Python Native Launcher Runtime Info:
Library Location: python312.dll
Runtime Version: 3.12.7 (tags/v3.12.7:0b05ead, Oct  1 2024, 03:06:41) [MSC v.1941 64 bit (AMD64)]
Runtime Platform: win32

Files already downloaded and verified
```
Or similar to the log from **Python** section.

### Testing the Model
Basically, we need to follow the same steps as before. So we have to provide test class name:
```csharp!
var testClassName = "test_model.TestModel";
```
Now, create an instance of this class:
```csharp!
var calledTestRuntime = calledRuntime.GetType(testClassName).Execute();
```
At this point we can call `test()` method:
```csharp!
calledTestRuntime.InvokeInstanceMethod("test", calledTestRuntime, modelsPath).Execute();
```
And again, let's analyze these parameters:
* `"test"` - this is the name of our **Python method**,
* `calledTestRuntime` - instance of our class itself,
* `modelsPath` - absolute path to **models** directory.

`Program.cs` should look like below:
```csharp=
namespace DotnetTorch
{
    using Javonet.Netcore.Sdk;

    public class Program
    {
        public static void Main(string[] args)
        {
            Javonet.Activate("paste_your_license_key_here");

            var calledRuntime = Javonet.InMemory().Python();

            // path to custom python code
            var pythonResourcesPath = absolute_path_to_your_python_code_directory;

            // load custom code
            calledRuntime.LoadLibrary($"{pythonResourcesPath}/scripts");

            // train py model
            var trainClassName = "train.Train";
            var calledTrainRuntime = calledRuntime.GetType(trainClassName).Execute();
            var modelsPath = $"{pythonResourcesPath}/models";
            calledTrainRuntime.InvokeInstanceMethod("train", calledTrainRuntime, modelsPath).Execute();
            
            // test py model
            var testClassName = "test_model.TestModel";
            var calledTestRuntime = calledRuntime.GetType(testClassName).Execute();
            calledTestRuntime.InvokeInstanceMethod("test", calledTestRuntime, modelsPath).Execute();
        }
    }
}

```

After running of this code you should see:
```!
Verifying existing license file
Python Managed Runtime Info:
Python Version: 3.12.7
Python executable path: ----
Python Path: [----]
Python Implementation: CPython
OS Version: Windows 10.0.22631
Process Architecture: AMD64
Current Directory: ----

Python Native Launcher Runtime Info:
Library Location: python312.dll
Runtime Version: 3.12.7 (tags/v3.12.7:0b05ead, Oct  1 2024, 03:06:41) [MSC v.1941 64 bit (AMD64)]
Runtime Platform: win32

Files already downloaded and verified
Accuracy of the network on the 10000 test images: 37.45%
```

### Testing by custom image
But much more intresting is to test your model with custom image. We add some image in our repository, but you can download your own. In `TestCustomImage` Python class we have defined classes of objects that can be recognized by the model:
```python!
# Define the classes for CIFAR-10
classes = ('plane', 'car', 'bird', 'cat', 'deer', 'dog', 'frog', 'horse', 'ship', 'truck')
```
So you can find *jpg* file with one of these objects. The frame of the image should be square. It will be resized to 32x32 before passing it to the model:
```python!
# Define the image transformation to match training setup
transform = transforms.Compose([
    transforms.Resize((32, 32)),        # Resize image to 32x32
    transforms.ToTensor(),              # Convert image to PyTorch tensor
    transforms.Normalize((0.5, 0.5, 0.5), (0.5, 0.5, 0.5))  # Normalize
])
```
And this will be our .NET code:
```csharp!
// test by custom image
var imagePath = $"{pythonResourcesPath}/images/test_image.jpg";
var testImageClassName = "test_custom_image.TestCustomImage";
var calledImageTestRuntime = calledRuntime.GetType(testImageClassName).Execute();
calledImageTestRuntime.InvokeInstanceMethod("test", calledImageTestRuntime, modelsPath, imagePath).Execute();
```
Let's take another look at this code snippet:
* `"test"` - this is the name of our **Python method**, in `TestCustomImage` class we also called our method `test()`,
* `calledImageTestRuntime` - instance of our class itself,
* `modelsPath` - absolute path to **models** directory,
* `imagePath` - as we see, here we have additional parameter with absolute path to our image file. We defined it in `TestCustomImage.test()` method:

```python!
def test(self, models_path: string, image_path: string):
```
### Running the application
Here is the complete code of `Program.cs`:
```csharp=
namespace DotnetTorch
{
    using Javonet.Netcore.Sdk;

    public class Program
    {
        public static void Main(string[] args)
        {
            Javonet.Activate("paste_your_license_key_here");

            var calledRuntime = Javonet.InMemory().Python();

            // path to custom python code
            var pythonResourcesPath = absolute_path_to_your_python_code_directory;

            // load custom code
            calledRuntime.LoadLibrary($"{pythonResourcesPath}/scripts");

            // train py model
            var trainClassName = "train.Train";
            var calledTrainRuntime = calledRuntime.GetType(trainClassName).Execute();
            var modelsPath = $"{pythonResourcesPath}/models";
            calledTrainRuntime.InvokeInstanceMethod("train", calledTrainRuntime, modelsPath).Execute();
            
            // test py model
            var testClassName = "test_model.TestModel";
            var calledTestRuntime = calledRuntime.GetType(testClassName).Execute();
            calledTestRuntime.InvokeInstanceMethod("test", calledTestRuntime, modelsPath).Execute();
            
            // test by custom image
            var imagePath = $"{pythonResourcesPath}/images/test_image.jpg";
            var testImageClassName = "test_custom_image.TestCustomImage";
            var calledImageTestRuntime = calledRuntime.GetType(testImageClassName).Execute();
            calledImageTestRuntime.InvokeInstanceMethod("test", calledImageTestRuntime, modelsPath, imagePath).Execute();
        }
    }
}
```

At this point we've finished our code. Now you can run it. After a moment in the terminal you should see similar log:
```!
Verifying existing license file
Python Managed Runtime Info:
Python Version: 3.12.7
Python executable path: ----
Python Path: [----]
Python Implementation: CPython
OS Version: Windows 10.0.22631
Process Architecture: AMD64
Current Directory: ----

Python Native Launcher Runtime Info:
Library Location: python312.dll
Runtime Version: 3.12.7 (tags/v3.12.7:0b05ead, Oct  1 2024, 03:06:41) [MSC v.1941 64 bit (AMD64)]
Runtime Platform: win32

Files already downloaded and verified
Accuracy of the network on the 10000 test images: 37.45%
Predicted class: horse
```
### Getting value from Python code to .NET
One of the most powerful features of integrating Python with .NET is the ability to retrieve variable values from Python methods. Without this capability, the applicability of such integrations would be limited. Here's how to achieve this step efficiently.

#### Modifying Python code
First, update the `TestCustomImage` Python class to include a return type for the test method:
```python!
def test(self, models_path: string, image_path: string) -> string:
```
Next, store the predicted class in a variable and return it:
```python!
result = classes[predicted.item()]

# Print the prediction
print(f"Predicted class by Python: {result}")

return result
```
The updated `TestCustomImage` class should look like this:
```python=
# test_custom_image.py
import string
import torch
import torchvision.transforms as transforms
from PIL import Image
import torchvision.models as models

class TestCustomImage:
    __type__ = "TestCustomImage"
    
    def test(self, models_path: string, image_path: string) -> string:
        # Load the trained model
        model = models.resnet18()
        model.fc = torch.nn.Linear(model.fc.in_features, 10)  # CIFAR-10 has 10 classes
        model.load_state_dict(torch.load(models_path + '/cifar10_model.pth'))
        model.eval()

        # Define the classes for CIFAR-10
        classes = ('plane', 'car', 'bird', 'cat', 'deer', 'dog', 'frog', 'horse', 'ship', 'truck')

        # Define the image transformation to match training setup
        transform = transforms.Compose([
            transforms.Resize((32, 32)),        # Resize image to 32x32
            transforms.ToTensor(),              # Convert image to PyTorch tensor
            transforms.Normalize((0.5, 0.5, 0.5), (0.5, 0.5, 0.5))  # Normalize
        ])

        # Load and preprocess your custom image
        image = Image.open(image_path)
        image = transform(image).unsqueeze(0)  # Add batch dimension

        # Run the image through the model
        output = model(image)
        _, predicted = torch.max(output, 1)

        result = classes[predicted.item()]

        # Print the prediction
        print(f"Predicted class by Python: {result}")
        
        return result
```
#### Modifying .NET code
With the Python class updated to return a value, modify the .NET `Program.cs` to retrieve and process the predicted class. Update the method invocation for testing a custom image as follows:
```csharp!
// test by custom image
var imagePath = $"{pythonResourcesPath}/images/test_image.jpg";
var testImageClassName = "test_custom_image.TestCustomImage";
var calledImageTestRuntime = calledRuntime.GetType(testImageClassName).Execute();
var predictedClass = (string)calledImageTestRuntime // <- string casting
    .InvokeInstanceMethod("test", calledImageTestRuntime, modelsPath, imagePath)
    .Execute()
    .GetValue(); // <- new extension method

Console.WriteLine($"Predicted class by .NET: {predictedClass}");
```
**Key Changes:**
1. String Conversion - we explicitly cast the result to a string type.
2. `.GetValue()` - this extension method extracts the returned value from the invoked Python method.

By using `.GetValue()`, the value from the Python method can be accessed seamlessly in your .NET code. Ensuring type safety with explicit conversion further solidifies the integration.

**Expected Output**
When you run the updated code, the terminal should display predictions from both Python and .NET, confirming the seamless integration:
```bash!
Predicted class by Python: horse
Predicted class by .NET: horse
```

## Conclusion
Integrating .NET with Python using Javonet offers incredible flexibility for developers who want to combine the strengths of both ecosystems. In this example, we demonstrated how to train and test a machine learning model in Python, leveraging .NET's robust framework to orchestrate the process. This approach can be applied to countless scenarios, from automating complex workflows to integrating AI capabilities into enterprise applications.

By bridging the gap between .NET and Python, developers can focus on building innovative solutions without being constrained by technology silos.

What about you? How do you envision using such integrations in your projects? Share your ideas in the comments below! 🚀