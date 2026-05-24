namespace Core.classes;

public class NeuralNetwork
{
  private const int InputCount = 12;
  private const int OutputCount = 4;
  private readonly double[,] _weights;
  private readonly double[] _biases;
  private readonly Random _random = new();

  public NeuralNetwork()
  {
    _weights = new double[InputCount, OutputCount];
    _biases = new double[OutputCount];
    Randomize();
  }

  private void Randomize()
  {
    for (var i = 0; i < InputCount; i++)
      for (var o = 0; o < OutputCount; o++)
        _weights[i, o] = (_random.NextDouble() * 2.0) - 1.0;
    for (var o = 0; o < OutputCount; o++)
      _biases[o] = (_random.NextDouble() * 2.0) - 1.0;
  }

  public int Activate(double[] inputs)
  {
    var outputs = new double[OutputCount];
    for (var o = 0; o < OutputCount; o++)
    {
      var sum = _biases[o];
      for (var i = 0; i < InputCount; i++)
        sum += inputs[i] * _weights[i, o];
      outputs[o] = Sigmoid(sum);
    }
    var winner = 0;
    for (var o = 1; o < OutputCount; o++)
      if (outputs[o] > outputs[winner]) winner = o;
    return winner;
  }

  private static double Sigmoid(double x) => 1.0 / (1.0 + Math.Exp(-x));

  public NeuralNetwork Clone()
  {
    NeuralNetwork clone = new();
    Array.Copy(_weights, clone._weights, _weights.Length);
    Array.Copy(_biases, clone._biases, _biases.Length);
    return clone;
  }

  public void Mutate(double mutationRate)
  {
    for (var i = 0; i < InputCount; i++)
      for (var o = 0; o < OutputCount; o++)
        if (_random.NextDouble() < mutationRate)
          _weights[i, o] += (_random.NextDouble() * 0.4) - 0.2;
    for (var o = 0; o < OutputCount; o++)
      if (_random.NextDouble() < mutationRate)
        _biases[o] += (_random.NextDouble() * 0.4) - 0.2;
  }
}