using UnityEngine;
using System.Collections;

namespace FF.UI
{
	internal class FFRatingWidget : MonoBehaviour
	{
		public UISprite[] images = null;
		public string fullSprite = null;
		public string emptySprite = null;
		public int curValue = 0;
		
		internal int MaxValue
		{
			get
			{
				return images.Length;
			}
		}
		
		internal int Value
		{
			get
			{
				return curValue;
			}
			set
			{
				curValue = Mathf.Clamp(value,0,MaxValue);
				Compute();
			}
		}
	
		protected virtual void Awake()
		{
			Compute ();
		}
		
		protected virtual void Compute()
		{
			for(int i = 0 ; i < MaxValue; i++)
			{
				if(i < curValue)
					images[i].spriteName = fullSprite;
				else
					images[i].spriteName = emptySprite;
			}
		}
	}
}