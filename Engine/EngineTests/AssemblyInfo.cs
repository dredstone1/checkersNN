using EngineTests;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(
    typeof(JsonXUnitSerializer<PieceTestCase>),
    typeof(PieceTestCase)
)]
