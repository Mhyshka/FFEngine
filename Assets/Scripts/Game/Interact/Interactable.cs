using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
internal abstract class Interactable : MonoBehaviour
{
	internal virtual void OnInteraction()
	{

	}
}
