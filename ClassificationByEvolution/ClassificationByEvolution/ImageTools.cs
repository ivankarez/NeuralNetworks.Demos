namespace Ivankarez.NeuralNetworks.Demos.ClassificationByEvolution
{
    public static class ImageTools
    {
        public static float[] ReadImageGrayscale(string path)
        {
            using var image = Image.Load<Rgba32>(path);
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
}
