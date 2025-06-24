class MyClass:
    __type__ = "MyClass"

    def hello_world(self) -> str:
        return "Hello, World from Python Class!!"
    
    def add(self, a: int, b: int) -> int:
        return a + b