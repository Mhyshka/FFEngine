using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Snapmesh : MonoBehaviour
{
	public Terrain terrain = null;
	public float threshold = 1f;
	public int offset = -1;
	public bool refresh = false;

	void Update ()
	{
		if (refresh)
		{
			TerrainData data = terrain.terrainData;
			refresh = false;
			float[,] heights = data.GetHeights(0, 0, data.alphamapWidth, data.alphamapHeight);

			Debug.Log(data.heightmapWidth + "x" + data.heightmapHeight);

			int y = 0, x = 0;
			try
			{
				for (y = 0; y < data.heightmapWidth+offset; y++)
				{

					for (x = 0; x < data.heightmapHeight+offset; x++)
				    {
						heights[x,y] -= heights[x,y]%(threshold / data.heightmapHeight); 
					}
				}
			}
			catch
			{
				Debug.LogWarning(x+","+y + " : Coords out of bounds, try to lower the offset.");
			}

			data.SetHeights(0,0,heights);
		}
	}
}
