using System.Runtime.InteropServices;

namespace Checkers.models;

public class Model
{
    [DllImport("../../../../NeuralNetwork/build/libNeuralNetwork.so")]
    public static extern IntPtr Model_Create();

    [DllImport("../../../../NeuralNetwork/build/libNeuralNetwork.so")]
    public static extern float[] Model_Run(float[] input, Model model);
}
