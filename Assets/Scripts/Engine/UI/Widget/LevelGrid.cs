using UnityEngine;
using System.Collections;
using System.Collections.Generic;

internal class LevelGrid : MonoBehaviour
{
	public UIGrid grid = null;
	public GameObject rowPrefab = null;

	private int[] selectedIndex = {0,0};
	internal int SelectedIndex
	{
		get
		{
			return selectedIndex[0] + selectedIndex[1] * grid.maxPerLine;
		}
	}
	private List<LevelSelectionWidget> widgets = new List<LevelSelectionWidget>();

	// Use this for initialization
	void Start ()
	{
		InitLevelGrid ();
		UpdateSelectedIndex ();
	}
	
	internal void NextIndex()
	{
		widgets [SelectedIndex].selectedTweeners.PlayReverse ();
		selectedIndex[0] = (int)Mathf.Repeat (selectedIndex[0] + 1, grid.maxPerLine);
		UpdateSelectedIndex ();
	}

	internal void PreviousIndex()
	{
		widgets[SelectedIndex].selectedTweeners.PlayReverse ();
		selectedIndex[0] = (int)Mathf.Repeat (selectedIndex[0] - 1, grid.maxPerLine);
		UpdateSelectedIndex ();
	}

	internal void NextRow()
	{
		widgets[SelectedIndex].selectedTweeners.PlayReverse ();
		selectedIndex[1] = (int)Mathf.Repeat (selectedIndex [1] + 1, grid.maxPerLine);
		UpdateSelectedIndex ();
	}

	internal void PreviousRow()
	{
		widgets[SelectedIndex].selectedTweeners.PlayReverse ();
		selectedIndex[1] = (int)Mathf.Repeat (selectedIndex [1] - 1, grid.maxPerLine);
		UpdateSelectedIndex ();
	}

	internal void UpdateSelectedIndex()
	{
		widgets [SelectedIndex].selectedTweeners.PlayForward ();
	}

	/*private int CellCountAtRow(int a_row)
	{
		int count = grid.GetChildList().Count / grid.maxPerLine;
		if( > )
		count =

		return count;
	}

	private int RowCount
	{
		get
		{
			int count = grid.GetChildList().Count / grid.maxPerLine;
			if(grid.GetChildList().Count % grid.maxPerLine > 0)
			{
				count++;
			}

			return count;
		}
	}*/

	void InitLevelGrid()
	{
		int i = 0;
		
		foreach(GameLevelData each in FFEngine.Game.levels)
		{
			GameObject newRow = (GameObject)GameObject.Instantiate(rowPrefab);
			grid.AddChild(newRow.transform);
			
			newRow.transform.localPosition = Vector3.zero;
			newRow.transform.localRotation = Quaternion.identity;
			newRow.transform.localScale = rowPrefab.transform.localScale;
			
			LevelSelectionWidget widget = newRow.GetComponent<LevelSelectionWidget>();
			widget.label.text = each.levelName;
			widget.button.levelIndex = i;

			widgets.Add(widget);
			
			i++;
		}
		
		grid.Reposition ();
	}
}
