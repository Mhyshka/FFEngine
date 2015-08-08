using UnityEngine;
using System.Collections;
using FullInspector;

[CreateAssetMenu()]
public class UnitConf : InteractableConf
{
	public UnitAttackConf attack = null;
	public UnitDefenseConf defense = null;
	public UnitAttributesConf attributes = null;
}
