using UnityEngine;
using System.Collections;

internal class RatingWidget : MonoBehaviour
{
	public ThreeStatesWidget[] threeStatesSprites = null;

	internal void SetRating(int a_rating, int a_newCount)
	{
		a_rating = Mathf.Clamp (a_rating, 0, threeStatesSprites.Length);
		for (int i = 0; i < threeStatesSprites.Length; i++)
		{
			if(i < a_newCount)
			{
				threeStatesSprites[i].SetNewly();
			}
			else if(i < a_rating)
			{
				threeStatesSprites[i].SetActive();
			}
			else
			{
				threeStatesSprites[i].SetInactive();
			}
		}
	}
}
