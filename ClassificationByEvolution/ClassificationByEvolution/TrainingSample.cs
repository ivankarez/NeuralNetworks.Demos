namespace Ivankarez.NeuralNetworks.Demos.ClassificationByEvolution
{
    public class TrainingSample
    {
        public float[] Input { get; }
        public float[] Output { get; }

        public TrainingSample(float[] input, float[] output)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));
        }
    }
}
