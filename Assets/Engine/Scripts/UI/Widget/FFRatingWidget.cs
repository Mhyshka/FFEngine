using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FF.UI
{
	internal class FFRatingWidget : MonoBehaviour
	{
		public Image[] images = null;
		public Sprite fullSprite = null;
		public Sprite emptySprite = null;
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
					images[i].sprite = fullSprite;
				else
					images[i].sprite = emptySprite;
			}
		}
	}
}