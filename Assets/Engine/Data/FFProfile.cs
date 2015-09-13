using UnityEngine;
using System.Collections;

namespace FF.Data
{
	internal class FFProfile
	{
		#region Properties
		protected string _name;
		internal string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
		
		
		#endregion
	}
}