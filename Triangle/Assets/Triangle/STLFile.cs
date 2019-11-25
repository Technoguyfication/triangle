using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class STLFile
{
	private const string AsciiStartString = "solid";

	public string Name { get; }
	public Facet[] Facets { get; }

	public STLFile(byte[] file)
	{
		// check if file is ASCII or Binary
		string asciiFileStart = Encoding.ASCII.GetString(file.Take(AsciiStartString.Length).ToArray());
		if (AsciiStartString.Equals(asciiFileStart, StringComparison.InvariantCultureIgnoreCase))
		{
			// file is ascii
			var facets = new List<Facet>();
			var fileLines = Encoding.ASCII.GetString(file).Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

			// loop through and load file
			for (int i = 0; i < fileLines.Length; i++)
			{
				string[] lineSplit = fileLines[i].Split(' ');

				// switch on line type
				switch (lineSplit[0])
				{
					case "solid":
						Name = string.Join(" ", lineSplit.Skip(1));
						break;

					case "facet":
						Facet newFacet = new Facet
						{
							Normal = ParseVector(lineSplit.Skip(2)),    // skip "facet normal"
							Vertices = new Vector3[3]
						};

						// skip "outer loop" line and move to next line
						i += 2;

						// get vertices from next three lines
						int startIndex = i;
						for (; i < startIndex + 3; i++)
						{
							newFacet.Vertices[i - startIndex] = ParseVector(fileLines[i].Split(' ').Skip(1));
						}

						facets.Add(newFacet);
						break;
					case "endloop":
					case "endfacet":
					case "endsolid":
						break;
					default:
						throw new Exception($"Invalid STL: Line {i}: {fileLines[i]}");

				}
			}

			Facets = facets.ToArray();
		}
		else
		{
			// file is binary
			throw new NotImplementedException("Binary STL files are not yet supported");
		}
	}

	private Vector3 ParseVector(IEnumerable<string> input)
	{
		if (input.Count() != 3)
			throw new ArgumentException("Invalid vector length");

		return new Vector3(
			float.Parse(input.ElementAt(0)),
			float.Parse(input.ElementAt(1)),
			float.Parse(input.ElementAt(2)));
	}

	public struct Facet
	{
		public Vector3 Normal;
		public Vector3[] Vertices;
	}
}
