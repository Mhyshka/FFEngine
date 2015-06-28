using UnityEngine;
using System.Collections;

public class InteractableRenderer : MonoBehaviour
{
	#region Inspector Properties
	#endregion

	#region Properties
	Renderer _renderer;
	
	
	Material[] Materials
	{
		get
		{
			return _renderer.materials;
		}
	}
	
	float[] _defaultOutlineSizes;
	
	bool _isOutlined;
	
	internal bool IsOutlined
	{
		get
		{
			return _isOutlined;
		}
	}
	#endregion

	#region Methods
	protected virtual void Awake ()
	{
		_renderer = GetComponent<Renderer>();
		InitOutline();
	}
	#endregion
	
	#region Outline
	internal void InitOutline()
	{
		_defaultOutlineSizes = new float[Materials.Length];
		for(int i = 0 ; i < Materials.Length ; i++)
		{
			if(Materials[i].HasProperty("_OutlineWidth"))
			{
				_defaultOutlineSizes[i] = Materials[i].GetFloat("_OutlineWidth");
			}
		}
		DisableOutline();
	}
	
	internal void EnableOutline(Color a_color)
	{
		_isOutlined = true;
		for(int i = 0 ; i < Materials.Length ; i++)
		{
			if(Materials[i].HasProperty("_OutlineWidth"))
			{
				Materials[i].SetColor("_OutlineColor", a_color);
				Materials[i].SetFloat("_OutlineWidth", _defaultOutlineSizes[i]);
			}
		}
	}
	
	internal void DisableOutline()
	{
		_isOutlined = false;
		for(int i = 0 ; i < Materials.Length ; i++)
		{
			if(Materials[i].HasProperty("_OutlineWidth"))
			{
				Materials[i].SetFloat("_OutlineWidth", 0f);
			}
		}
	}
	#endregion

}
