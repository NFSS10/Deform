﻿using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Deform
{
	[Deformer (Name = "UV Offset", Description = "Offsets the mesh's UVs", Type = typeof (UVOffsetDeformer), Category = Category.Normal)]
	public class UVOffsetDeformer : Deformer
	{
		public Vector2 Offset
		{
			get => offset;
			set => offset = value;
		}
		[SerializeField, HideInInspector] private Vector2 offset;

		public override DataFlags DataFlags => DataFlags.UVs;

		public override JobHandle Process (MeshData data, JobHandle dependency = default)
		{
			return new UVOffsetDeformJob
			{
				offset = offset,
				uvs = data.DynamicNative.UVBuffer
			}.Schedule (data.length, BatchCount, dependency);
		}

		[BurstCompile (CompileSynchronously = COMPILE_SYNCHRONOUSLY)]
		private struct UVOffsetDeformJob : IJobParallelFor
		{
			public float2 offset;
			public NativeArray<float2> uvs;

			public void Execute (int index)
			{
				uvs[index] += offset;
			}
		}
	}
}