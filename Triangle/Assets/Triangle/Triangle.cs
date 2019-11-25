using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility for creating <see cref="Mesh"/>es from STL files
/// </summary>
public static class Triangle
{
	public static Mesh CreateMeshFromSTL(STLFile stlFile)
	{
		Vector3[] vertices = new Vector3[stlFile.Facets.Length * 3];
		Vector3[] normals = new Vector3[stlFile.Facets.Length * 3];
		int[] triangles = new int[stlFile.Facets.Length * 3];

		for (int i = 0; i < stlFile.Facets.Length; i++)
		{
			// copy vertices, triangles, and normals for each facet
			for (int j = 0; j < 3; j++)
			{
				int meshIndex = (i * 3) + j;

				vertices[meshIndex] = stlFile.Facets[i].Vertices[j];
				// unity normals are per-vertex and not triangle. copy normal 3x for each vertex
				normals[meshIndex] = stlFile.Facets[i].Normal;
				triangles[meshIndex] = meshIndex;
			}
		}

		return new Mesh()
		{
			vertices = vertices,
			normals = normals,
			triangles = triangles
		};
	}
}
