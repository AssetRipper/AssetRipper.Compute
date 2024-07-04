using ComputeSharp;
using System.Runtime.Versioning;

namespace AssetRipper.Compute;

[SupportedOSPlatform("windows6.2")]
internal static class GraphicsDeviceExtensions
{
	public static ReadWriteBuffer<float> AllocateReadWriteBuffer(this GraphicsDevice graphicsDevice, Span<float> span)
	{
		//float can't be trimmed
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
		return graphicsDevice.AllocateReadWriteBuffer<float>(span);
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
	}

	public static ReadWriteBuffer<Float2> AllocateReadWriteBuffer(this GraphicsDevice graphicsDevice, Span<Float2> span)
	{
		//Vector2 fields likely won't be trimmed
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
		return graphicsDevice.AllocateReadWriteBuffer<Float2>(span);
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
	}

	public static ReadWriteBuffer<Float3> AllocateReadWriteBuffer(this GraphicsDevice graphicsDevice, Span<Float3> span)
	{
		//Float3 fields likely won't be trimmed
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
		return graphicsDevice.AllocateReadWriteBuffer<Float3>(span);
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
	}

	public static ReadWriteBuffer<Float4> AllocateReadWriteBuffer(this GraphicsDevice graphicsDevice, Span<Float4> span)
	{
		//Float4 fields likely won't be trimmed
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
		return graphicsDevice.AllocateReadWriteBuffer<Float4>(span);
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
	}

	public static nuint GetMaxMemorySize(this GraphicsDevice graphicsDevice)
	{
		return nuint.Max(graphicsDevice.DedicatedMemorySize, graphicsDevice.SharedMemorySize);
	}

	public static nuint GetTotalMemorySize(this GraphicsDevice graphicsDevice)
	{
		return graphicsDevice.DedicatedMemorySize + graphicsDevice.SharedMemorySize;
	}

	public static int GetUsableMemorySize(this GraphicsDevice graphicsDevice, float maxProportion = .1f)
	{
		ArgumentOutOfRangeException.ThrowIfGreaterThan(maxProportion, 1);
		ArgumentOutOfRangeException.ThrowIfNegative(maxProportion);

		nuint maxMemorySize = graphicsDevice.GetMaxMemorySize();
		if (maxProportion == 1)
		{
			return maxMemorySize > int.MaxValue ? int.MaxValue : (int)maxMemorySize;
		}
		else
		{
			float multiplied = maxProportion * maxMemorySize;
			return multiplied > int.MaxValue ? int.MaxValue : (int)multiplied;
		}
	}
}
