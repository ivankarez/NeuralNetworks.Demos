# NeuralNetworks.Demos
A repository to store demo projects for my [Neural Networks](https://github.com/ivankarez/NeuralNetworks) C# library.

## Classification By Evolution
This C# console application is a demo project that utilizes neuroevolution to teach networks to distinguish between characters 'A' and 'B'. The code evolves a population of neural network individuals over multiple generations to minimize classification errors for these characters.

### Dataset
This demo uses a dummy dataset of 8 128*128 black-white image of a 'A' and 'B' characters. After 500 generations it uses the best network of the latest generation to classify the images in the `Training Data/Test` folder.
![Training Data Set](https://raw.githubusercontent.com/ivankarez/NeuralNetworks.Demos/main/Images/Training%20Data%20Set.png)
![Testing Data Set](https://raw.githubusercontent.com/ivankarez/NeuralNetworks.Demos/main/Images/Testing%20Data%20Set.png)

### Example Output
```csharp
Errors in gen 0000: (Min 7,9971, Avg: 8,0002, Max: 8,0046)
Errors in gen 0001: (Min 7,9918, Avg: 7,9997, Max: 8,0053)
Errors in gen 0002: (Min 7,9901, Avg: 7,9988, Max: 8,0081)
...
Errors in gen 0497: (Min 0,0068, Avg: 0,0071, Max: 0,0074)
Errors in gen 0498: (Min 0,0067, Avg: 0,0069, Max: 0,0075)
Errors in gen 0499: (Min 0,0066, Avg: 0,0068, Max: 0,0075)
-------End Of Training-------
Best individual has error 0,01
Test file results:
        test_a_1.png: A (99,95% confidence)
        test_a_2.png: A (99,94% confidence)
        test_b_1.png: B (99,87% confidence)
        test_b_2.png: B (99,87% confidence)
```