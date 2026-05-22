namespace Core.classes;

public class NeuralNetwork
{
  private const int InputCount = 12;
  private const int OutputCount = 4;
  private readonly double[,] weights;
  private readonly double[] biases;
  private readonly Random random = new();

  public NeuralNetwork()
  {
    weights = new double[InputCount, OutputCount];
    biases = new double[OutputCount];
    Randomize();
  }

  public void Randomize()
  {
    for (int i = 0; i < InputCount; i++)
      for (int o = 0; o < OutputCount; o++)
        weights[i, o] = (random.NextDouble() * 2.0) - 1.0;
    for (int o = 0; o < OutputCount; o++)
      biases[o] = (random.NextDouble() * 2.0) - 1.0;
  }

  public int Activate(double[] inputs)
  {
    double[] outputs = new double[OutputCount];
    for (int o = 0; o < OutputCount; o++)
    {
      double sum = biases[o];
      for (int i = 0; i < InputCount; i++)
        sum += inputs[i] * weights[i, o];
      outputs[o] = Sigmoid(sum);
    }
    int winner = 0;
    for (int o = 1; o < OutputCount; o++)
      if (outputs[o] > outputs[winner]) winner = o;
    return winner;
  }

  private static double Sigmoid(double x) => 1.0 / (1.0 + Math.Exp(-x));

  public NeuralNetwork Clone()
  {
    NeuralNetwork clone = new();
    Array.Copy(weights, clone.weights, weights.Length);
    Array.Copy(biases, clone.biases, biases.Length);
    return clone;
  }

  public void Mutate(double mutationRate)
  {
    for (int i = 0; i < InputCount; i++)
      for (int o = 0; o < OutputCount; o++)
        if (random.NextDouble() < mutationRate)
          weights[i, o] += (random.NextDouble() * 0.4) - 0.2;
    for (int o = 0; o < OutputCount; o++)
      if (random.NextDouble() < mutationRate)
        biases[o] += (random.NextDouble() * 0.4) - 0.2;
  }
}