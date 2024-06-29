using ComputeSharp;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace AssetRipper.Compute;

public static class Gpu
{
	[SupportedOSPlatformGuard("windows6.2")]
	public static bool Supported => OperatingSystem.IsOSPlatformVersionAtLeast("windows", 6, 2);

	private static bool Float3NotTrimmed => Unsafe.SizeOf<Float3>() == sizeof(float) * 3;
	private static bool Float4NotTrimmed => Unsafe.SizeOf<Float4>() == sizeof(float) * 4;
	private static bool Vector3NotTrimmed => Unsafe.SizeOf<Vector3>() == sizeof(float) * 3;
	private static bool Vector4NotTrimmed => Unsafe.SizeOf<Vector4>() == sizeof(float) * 4;

	public static void Transform(Span<Vector3> buffer, Matrix4x4 matrix, bool applyTranslation = true)
	{
		if (buffer.Length == 0)
		{
		}
		else if (Supported && Float3NotTrimmed && Vector3NotTrimmed)
		{
			GraphicsDevice graphicsDevice = GraphicsDevice.GetDefault();

			if (buffer.Length * Unsafe.SizeOf<Vector3>() > graphicsDevice.GetUsableMemorySize())
			{
				SlowPath(buffer, matrix, applyTranslation);
			}
			else
			{
				Span<Float3> span = MemoryMarshal.Cast<Vector3, Float3>(buffer);

				// Create the graphics buffer
				using ReadWriteBuffer<Float3> gpuBuffer = graphicsDevice.AllocateReadWriteBuffer(span);

				// Run the shader
				if (applyTranslation)
				{
					graphicsDevice.For(span.Length, new MultiplyPointByMatrix(gpuBuffer, matrix));
				}
				else
				{
					graphicsDevice.For(span.Length, new MultiplyVectorByMatrix(gpuBuffer, matrix));
				}

				// Get the data back
				gpuBuffer.CopyTo(span);
			}
		}
		else
		{
			SlowPath(buffer, matrix, applyTranslation);
		}

		static void SlowPath(Span<Vector3> buffer, Matrix4x4 matrix, bool applyTranslation)
		{
			if (applyTranslation)
			{
				SlowPathPoint(buffer, matrix);
			}
			else
			{
				SlowPathVector(buffer, matrix);
			}
		}

		static void SlowPathPoint(Span<Vector3> buffer, Matrix4x4 matrix)
		{
			for (int i = buffer.Length - 1; i >= 0; i--)
			{
				buffer[i] = Vector3.Transform(buffer[i], matrix);
			}
		}

		static void SlowPathVector(Span<Vector3> buffer, Matrix4x4 matrix)
		{
			for (int i = buffer.Length - 1; i >= 0; i--)
			{
				buffer[i] = Vector3.TransformNormal(buffer[i], matrix);
			}
		}
	}

	public static void Transform(Span<Vector4> buffer, Matrix4x4 matrix)
	{
		if (buffer.Length == 0)
		{
		}
		else if (Supported && Float4NotTrimmed && Vector4NotTrimmed)
		{
			GraphicsDevice graphicsDevice = GraphicsDevice.GetDefault();

			if (buffer.Length * Unsafe.SizeOf<Vector4>() > graphicsDevice.GetUsableMemorySize())
			{
				SlowPath(buffer, matrix);
			}
			else
			{
				Span<Float4> span = MemoryMarshal.Cast<Vector4, Float4>(buffer);

				// Create the graphics buffer
				using ReadWriteBuffer<Float4> gpuBuffer = graphicsDevice.AllocateReadWriteBuffer(span);

				// Run the shader
				graphicsDevice.For(span.Length, new MultiplyFloat4ByMatrix(gpuBuffer, matrix));

				// Get the data back
				gpuBuffer.CopyTo(span);
			}
		}
		else
		{
			SlowPath(buffer, matrix);
		}

		static void SlowPath(Span<Vector4> buffer, Matrix4x4 matrix)
		{
			for (int i = buffer.Length - 1; i >= 0; i--)
			{
				buffer[i] = Vector4.Transform(buffer[i], matrix);
			}
		}
	}
}
