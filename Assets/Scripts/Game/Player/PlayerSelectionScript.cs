using UnityEngine;
using System.Collections;

public class PlayerSelectionScript : APlayerScript
{
	#region Inspector Properties
	#endregion

	#region Properties
	protected AInteractable _hoveredObjet = null;
	internal AInteractable HoveredObject
	{
		get
		{
			return _hoveredObjet;
		}
	}
	
	protected AInteractable _selectedObject = null;
	internal AInteractable SelectedObject
	{
		get
		{
			return _selectedObject;
		}
	}
	
	protected bool _hasTerrainPosition;
	internal bool HasTerrainPosition
	{
		get
		{
			return _hasTerrainPosition;
		}
	}
	protected Vector3 _terrainPosition;
	internal Vector3 TerrainPosition
	{
		get
		{
			return _terrainPosition;
		}
	}
	#endregion

	#region Methods
	internal override void Init (Player a_player)
	{
		base.Init(a_player);
		RegisterForEvent();
	}
	
	protected virtual void OnDestroy ()
	{
		UnregisterForEvent ();
	}

	protected virtual void Update ()
	{
		if(!UpdateHover() && _hoveredObjet != null)
		{
			StopHover();
		}
	}
	#endregion


	#region Event Management
	protected virtual void RegisterForEvent ()
	{
		FFEngine.Inputs.RegisterOnEventKey(EInputEventKey.Select, OnTrySelection);
	}

	protected virtual void UnregisterForEvent ()
	{
		FFEngine.Inputs.UnregisterOnEventKey(EInputEventKey.Select, OnTrySelection);
	}
	#endregion

	#region Selection
	internal void OnTrySelection()
	{
		if(_selectedObject != null && _hoveredObjet != _selectedObject)
			StopSelection();
		
		_selectedObject = _hoveredObjet;
		
		if(_selectedObject != null)
			StartSelection();
	}
	
	internal void StartSelection()
	{
		_selectedObject.onSelection();
	}
	
	internal void StopSelection()
	{
		_selectedObject.onDeselection();
		_selectedObject = null;
	}
	#endregion
	
	#region Hover
	internal bool UpdateHover()
	{
		RaycastHit hit;
		Camera main = Camera.main;
		Ray ray = main.ScreenPointToRay(Input.mousePosition);		
		
		// Hover Terrain
		if(Physics.Raycast(ray,
		                   out hit,
		                   150f,
		                   1 << LayerMask.NameToLayer("Terrain")))
		{
			_hasTerrainPosition = true;
			_terrainPosition = hit.point;
		}
		else
		{
			_hasTerrainPosition = false;
		}
		
		//Hover Interactable
		if(Physics.Raycast(ray,
		                   out hit,
		                   150f,
		                   1 << LayerMask.NameToLayer("Interaction")))
		{
			InteractionTarget target = hit.collider.GetComponent<InteractionTarget>();
			AInteractable hovered = target.Interactable;
			if(hovered != null)
			{
				//Hovered success
				if(hovered != _hoveredObjet)
				{
					StopHover();
					_hoveredObjet = hovered;
					StartHover();
				}
				
				return true;
			}
		}
		return false;
	}
	
	internal void StartHover()
	{
		_hoveredObjet.onHoverStart();
	}
	
	internal void StopHover()
	{
		if(_hoveredObjet != null)
			_hoveredObjet.onHoverStop();
			
		_hoveredObjet = null;
	}
	#endregion
}
