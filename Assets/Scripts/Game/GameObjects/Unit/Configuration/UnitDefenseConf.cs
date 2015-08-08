using UnityEngine;
using System.Collections;
using FullInspector;

[System.Serializable]
public class UnitDefenseConf
{
	public ArmorConf general = null;
	
	public ArmorConf physical = null;
	public ArmorConf slashing = null;
	public ArmorConf crushing = null;
	public ArmorConf piercing = null;
	
	public ArmorConf magic = null;
	public ArmorConf fire = null;
	public ArmorConf frost = null;
	public ArmorConf lightning = null;
	
	public ArmorConf bleed = null;
	public ArmorConf poison = null;
	public ArmorConf spirit = null;
}