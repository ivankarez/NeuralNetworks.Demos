namespace Ivankarez.NeuralNetworks.Demos.ClassificationByEvolution
{
    public class Individual
    {
        public LayeredNetworkModel NeuralNetwork { get; }
        public float Error { get; private set; }
        public bool IsEvaluated { get; private set; }

        public Individual(LayeredNetworkModel neuralNetwork)
        {
            NeuralNetwork = neuralNetwork ?? throw new ArgumentNullException(nameof(neuralNetwork));
            Error = 0f;
            IsEvaluated = false;
        }

        public void Evaluate(ICollection<TrainingSample> samples)
        {
            var totalError = 0f;
            foreach (var sample in samples)
            {
                var output = NeuralNetwork.Feedforward(sample.Input);
                var error = sample.Output.Zip(output, (expected, actual) => Math.Abs(expected - actual)).Sum();
                totalError += error;
            }
            Error = totalError;
            IsEvaluated = true;
        }
    }
}
