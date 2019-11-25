using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Stopwatch = System.Diagnostics.Stopwatch;

public class STLTest : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		string fileName = @"C:\Users\Hayden\Downloads\Filament End Switch.stl";
		byte[] fileData = File.ReadAllBytes(fileName);
		STLFile testFile;
		Stopwatch sw = new Stopwatch();

		try
		{
			sw.Start();
			testFile = new STLFile(fileData);
			sw.Stop();
		}
		catch (Exception ex)
		{
			Debug.LogWarning($"Error loading STL file: {ex}");
			return;
		}

		Debug.Log($"Loaded solid \"{testFile.Name}\" with {testFile.Facets.Length} facets in {sw.ElapsedMilliseconds}ms");
	}

	// Update is called once per frame
	void Update()
	{

	}
}
