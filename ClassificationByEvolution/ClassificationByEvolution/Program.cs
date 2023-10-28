using Ivankarez.NeuralNetworks;
using Ivankarez.NeuralNetworks.Api;
using Ivankarez.NeuralNetworks.Demos.ClassificationByEvolution;

namespace ClassificationByEvolution
{
    internal class Program
    {
        // CONFIG
        static readonly int POPULATION_SIZE = 100;
        static readonly int SURVIVOR_COUNT = 20;
        static readonly int REQUIRED_GENERATIONS = 500;
        static readonly float MUTATION_RATE = 0.1f;
        static readonly float MUTATION_AMOUNT = 0.1f;

        static readonly Random Random = new();
        static void Main(string[] args)
        {

            var samples = LoadSamples();
            var population = new List<Individual>(POPULATION_SIZE);
            for (int currentGeneration = 0; currentGeneration < REQUIRED_GENERATIONS; currentGeneration++)
            {
                population = CreateNextGeneration(population);
                var (Min, Avg, Max) = EvaluateGeneration(population, samples);
                Console.WriteLine($"Errors in gen {currentGeneration:d4}: (Min {Min:f4}, Avg: {Avg:f4}, Max: {Max:f4})");
            }

            Console.WriteLine("-------End Of Training-------");
            var best = population.OrderBy(i => i.Error).First();
            Console.WriteLine($"Best individual has error {best.Error:f2}");
            Console.WriteLine("Test file results:");
            foreach (var testImage in Directory.GetFiles(Path.Join("Training Data", "Test")))
            {
                var testInputs = ReadImageGrayscale(testImage);
                var testOutputs = best.NeuralNetwork.Feedforward(testInputs);
                var decision = testOutputs[0] > testOutputs[1] ? "A" : "B";
                Console.WriteLine($"\t{Path.GetFileName(testImage)}: {decision}");
            }
        }

        static ICollection<TrainingSample> LoadSamples()
        {
            var samples = new List<TrainingSample>();
            foreach (var file in Directory.GetFiles(Path.Join("Training Data", "A")))
            {
                samples.Add(new TrainingSample(ReadImageGrayscale(file), new[] { 1f, 0f }));
            }
            foreach (var file in Directory.GetFiles(Path.Join("Training Data", "B")))
            {
                samples.Add(new TrainingSample(ReadImageGrayscale(file), new[] { 0f, 1f }));
            }
            return samples;
        }

        static float[] ReadImageGrayscale(string path)
        {
            using (var image = Image.Load<Rgba32>(path))
            {
                var pixels = new float[image.Width * image.Height];
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        Rgba32 pixel = image[x, y];
                        pixels[y * image.Width + x] = (pixel.R + pixel.G + pixel.B) / (3f * 255f);
                    }
                }

                return pixels;
            }
        }

        static List<Individual> CreateFirstGeneration()
        {
            var individuals = new List<Individual>();
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                individuals.Add(new Individual(CreateNetworkModel()));
            }

            return individuals;
        }

        static LayeredNetworkModel CreateNetworkModel()
        {
            return NN.Models.Layered(125 * 125,
                NN.Layers.Conv2D((125, 125), (5, 5)),
                NN.Layers.Pooling2D((121, 121), (10, 10), (10, 10)),
                NN.Layers.Dense(10),
                NN.Layers.Dense(5),
                NN.Layers.Dense(2));
        }

        static (float Min, float Avg, float Max) EvaluateGeneration(IList<Individual> generation, ICollection<TrainingSample> samples)
        {
            Parallel.ForEach(generation, (individual, token) => {
                if (!individual.IsEvaluated)
                {
                    individual.Evaluate(samples);
                }                
            });

            var avgError = generation.Average(i => i.Error);
            var minError = generation.Min(i => i.Error);
            var maxError = generation.Max(i => i.Error);
            return (minError, avgError, maxError);
        }

        static List<Individual> CreateNextGeneration(IList<Individual> generation)
        {
            if (generation.Count == 0)
            {
                return CreateFirstGeneration();
            }

            var nextGeneration = generation.OrderBy(i => i.Error)
                .Take(SURVIVOR_COUNT)
                .ToList();
            while (nextGeneration.Count < POPULATION_SIZE)
            {
                var parent1 = nextGeneration[Random.Next(SURVIVOR_COUNT)];
                var parent2 = nextGeneration[Random.Next(SURVIVOR_COUNT)];
                var childDna = Crossover(parent1.NeuralNetwork.GetParametersFlat(),
                    parent2.NeuralNetwork.GetParametersFlat());
                Mutate(childDna);
                var childNetworkModel = CreateNetworkModel();
                childNetworkModel.SetParametersFlat(childDna);
                nextGeneration.Add(new Individual(childNetworkModel));
            }

            return nextGeneration;
        }

        static float[] Crossover(float[] dna1, float[] dna2)
        {
            var newDna = new float[dna1.Length];
            for (int i = 0; i < dna1.Length; i++)
            {
                newDna[i] = Random.NextDouble() < 0.5 ? dna1[i] : dna2[i];
            }

            return newDna;
        }

        static void Mutate(float[] dna)
        {
            for (int i = 0; i < dna.Length; i++)
            {
                if ( Random.NextDouble() < MUTATION_RATE )
                {
                    dna[i] += (float)Random.NextDouble() * MUTATION_AMOUNT - (MUTATION_AMOUNT / 2f);
                }
            }
        }
    }
}