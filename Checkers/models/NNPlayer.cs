using System.Runtime.InteropServices;

namespace Checkers.models;

public class Model
{
    [DllImport("../../../../NeuralNetwork/build/libNeuralNetwork.so")]
    public static extern IntPtr Model_Create();

    [DllImport("../../../../NeuralNetwork/build/libNeuralNetwork.so")]
    public static extern void Model_Run(IntPtr model, float[] input, float[] output);
}
