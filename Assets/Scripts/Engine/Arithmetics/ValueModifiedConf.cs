using UnityEngine;
using System.Collections;

public enum EReducPerStack
{
	Additive,
	ReduceWhatsLeft,
	WorstOfAll
}

public class ModifiedConf
{
	public bool bonus = false;
	public bool malus = true;
	public EReducPerStack stack = EReducPerStack.ReduceWhatsLeft;
	
	public ModifiedConf()
	{
		bonus = false;
		malus = true;
		stack = EReducPerStack.ReduceWhatsLeft;
	}
	
	public ModifiedConf(bool _bonus, bool _malus, EReducPerStack _stack)
	{
		bonus = _bonus;
		malus = _malus;
		stack = _stack;
	}
}