using ComputeSharp;
using System.Runtime.Versioning;

namespace AssetRipper.Compute;

[ThreadGroupSize(DefaultThreadGroupSizes.X)]
[GeneratedComputeShaderDescriptor]
[SupportedOSPlatform("windows6.2")]
internal readonly partial struct MultiplyPointByMatrix(ReadWriteBuffer<Float3> buffer, Float4x4 matrix) : IComputeShader
{
	/// <inheritdoc/>
	public void Execute()
	{
		buffer[ThreadIds.X] = Transform(buffer[ThreadIds.X]);
	}

	private Float3 Transform(Float3 vector)
	{
		//1 is used for W so that translation gets applied.
		return Hlsl.Mul(new Float4(vector, 1), matrix).XYZ;
	}
}
