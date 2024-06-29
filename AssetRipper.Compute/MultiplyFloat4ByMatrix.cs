using ComputeSharp;
using System.Runtime.Versioning;

namespace AssetRipper.Compute;

[ThreadGroupSize(DefaultThreadGroupSizes.X)]
[GeneratedComputeShaderDescriptor]
[SupportedOSPlatform("windows6.2")]
internal readonly partial struct MultiplyFloat4ByMatrix(ReadWriteBuffer<Float4> buffer, Float4x4 matrix) : IComputeShader
{
	/// <inheritdoc/>
	public void Execute()
	{
		buffer[ThreadIds.X] = Hlsl.Mul(buffer[ThreadIds.X], matrix);
	}
}
