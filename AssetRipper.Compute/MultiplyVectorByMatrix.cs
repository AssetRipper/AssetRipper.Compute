using ComputeSharp;
using System.Runtime.Versioning;

namespace AssetRipper.Compute;

[ThreadGroupSize(DefaultThreadGroupSizes.X)]
[GeneratedComputeShaderDescriptor]
[SupportedOSPlatform("windows6.2")]
internal readonly partial struct MultiplyVectorByMatrix(ReadWriteBuffer<Float3> buffer, Float4x4 matrix) : IComputeShader
{
	/// <inheritdoc/>
	public void Execute()
	{
		buffer[ThreadIds.X] = TransformNormal(buffer[ThreadIds.X]);
	}

	private Float3 TransformNormal(Float3 vector)
	{
		//0 is used for W so that translation is not applied.
		return Hlsl.Mul(new Float4(vector, 0), matrix).XYZ;
	}
}
