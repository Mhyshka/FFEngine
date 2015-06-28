using UnityEngine;
using System.Collections;

internal class PlayerSelectionScript : MonoBehaviour
{
	#region Inspector Properties
	public Player player = null;
	#endregion

	#region Properties
	AInteractable _hoveredObjet = null;
	AInteractable _selectedObject = null;
	#endregion

	#region Methods
	protected virtual void Awake ()
	{
		RegisterForEvent();
	}
	
	protected virtual void OnDestroy ()
	{
		UnregisterForEvent ();
	}

	protected virtual void Update ()
	{
		if(!CheckForHover() && _hoveredObjet != null)
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
		if(_selectedObject != null && _hoveredObjet == null)
			StopSelection();
		
		_selectedObject = _hoveredObjet;
		
		if(_selectedObject != null)
			StartSelection();
	}
	
	internal void StartSelection()
	{
		_selectedObject.OnSelection();
	}
	
	internal void StopSelection()
	{
		_selectedObject.OnDeselection();
		_selectedObject = null;
	}
	#endregion
	
	#region Hover
	internal bool CheckForHover()
	{
		RaycastHit hit;
		Camera main = Camera.main;
		Ray ray = main.ScreenPointToRay(Input.mousePosition);			
		if(Physics.Raycast(ray,
		                   out hit,
		                   150f,
		                   1 << LayerMask.NameToLayer("Interaction")))
		{
			AInteractable hovered = hit.collider.GetComponent<AInteractable>();
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
		_hoveredObjet.OnHoverStart();
	}
	
	internal void StopHover()
	{
		if(_hoveredObjet != null)
			_hoveredObjet.OnHoverStop();
			
		_hoveredObjet = null;
	}
	#endregion
}
