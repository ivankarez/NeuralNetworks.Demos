namespace Ivankarez.NeuralNetworks.Demos.ClassificationByEvolution
{
    internal static class DnaTools
    {
        public static float[] Crossover(float[] dna1, float[] dna2, Random random)
        {
            var newDna = new float[dna1.Length];
            for (int i = 0; i < dna1.Length; i++)
            {
                newDna[i] = random.NextDouble() < 0.5 ? dna1[i] : dna2[i];
            }

            return newDna;
        }

        public static void Mutate(float[] dna, Random random, float mutationRate, float mutationAmount)
        {
            for (int i = 0; i < dna.Length; i++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    dna[i] += (float)random.NextDouble() * mutationAmount - (mutationAmount / 2f);
                }
            }
        }
    }
}
