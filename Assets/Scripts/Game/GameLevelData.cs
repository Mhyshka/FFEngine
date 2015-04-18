using UnityEngine;
using System.Collections;

[System.Serializable]
internal class LevelRating
{
	public float threeStarsTime = 3f;
	public float twoStarsTime = 2f;
	public float oneStarTime = 1f;
}

internal class GameLevelData : MonoBehaviour
{
	#region Inspector Properties
	public float targetTime = 30f;
	public bool useAdditionalTime = true;
	public string sceneName = "";
	public string levelName = "";
	public LevelRating rating = null;
	#endregion

	#region Properties
	internal int currentRating = 0;

	private float _bestTime;
	internal float BestTime
	{
		get
		{
			return _bestTime;
		}
	}

	internal float NewScore(float a_score)
	{
		float value = -1f;
		if (a_score > _bestTime)
		{
			value -= a_score - _bestTime;
			_bestTime = a_score;
			return value;
		}
		return value;
	}

	internal int GetRating(float a_timeRemaining)
	{
		int lRating = 0;

		if (a_timeRemaining < rating.threeStarsTime)
		{
			lRating = 3;
		}
		else if (a_timeRemaining < rating.twoStarsTime)
		{
			lRating = 2;
		}
		else if (a_timeRemaining < rating.oneStarTime)
		{
			lRating = 1;
		}

		if (lRating > currentRating)
			currentRating = lRating;

		return lRating;
	}
	#endregion
}