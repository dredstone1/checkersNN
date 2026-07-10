using System.Runtime.InteropServices;

namespace Checkers.models;

public class Model
{
    [DllImport("../../../../NeuralNetwork/build/libNeuralNetwork.so")]
    public static extern IntPtr Model_Create();

    [DllImport("../../../../NeuralNetwork/build/libNeuralNetwork.so")]
    public static extern void Model_Delete(out IntPtr modelPtr);

    [DllImport("../../../../NeuralNetwork/build/libNeuralNetwork.so")]
    public static extern void Model_Run(IntPtr model, float[] input, float[] output);

    [DllImport("../../../../NeuralNetwork/build/libNeuralNetwork.so")]
    public static extern void Model_Save(IntPtr model, char[] path, int l);

    [DllImport("../../../../NeuralNetwork/build/libNeuralNetwork.so")]
    public static extern void Model_Load(IntPtr model, char[] path, int i);

    [DllImport("../../../../NeuralNetwork/build/libNeuralNetwork.so")]
    public static extern void Model_train(IntPtr model, char[] PT, char[] PE); // Path Training, Path Evaluating
}
