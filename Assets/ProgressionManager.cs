﻿using UnityEngine;
using System.Collections;

public class ProgressionManager : MonoBehaviour {

	TillStateMachine till;
	BonusManager bonus;

	string[] unlockables;

	bool unlockAll = true;

	// Use this for initialization
	void Awake () {
		till = GetComponent<TillStateMachine> ();
		bonus = GetComponent<BonusManager> ();

		string[] temp = { "Hippie", "RichLady", "PremiumSet" };
		unlockables = temp;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Debug.isDebugBuild && Input.GetKey (KeyCode.R) && Input.GetKey (KeyCode.LeftCommand)) 
		{
			resetProgress();
		}
	}

	public bool isCustomerUnlocked(CustomerProfile profile)
	{
		if (unlockAll)
			return true;

		if (profile.name == "Family" || profile.name == "Proletarian") 
		{
			return true;
		}

		return isUnlocked(profile.name);
	}

	public bool isItemUnlocked(ItemInfo item)
	{
		if (unlockAll)
			return true;

		if (item.tags.IndexOf ("starterset") > -1)
			return true;

		foreach (string key in unlockables) 
		{			
			if (item.tags.IndexOf (key) > -1)
				return isUnlocked (key);
		}

//		if (item.tags.IndexOf ("HippieSet") > -1)
//			return isUnlocked ("Hippie");
//
//		if (item.tags.IndexOf ("RichLadySet") > -1)
//			return isUnlocked ("RichLady");

//		if(item.tags.IndexOf("PremiumSet"

		return false;
	}

	public void progress(float wage, float profit)
	{
		if (!isUnlocked ("Hippie")) 
			unlock ("Hippie");

		if (!isUnlocked ("RichLady") && wage > 1000)
			unlock ("RichLady");
		
		if (!isUnlocked ("PremiumSet") && bonus.maxBonus >= 10)
			unlock ("PremiumSet");


		PlayerPrefs.Save ();
	}

	bool isUnlocked(string key)
	{
		return unlockAll || PlayerPrefs.GetInt (key) == 1;
	}

	void unlock(string key)
	{
		PlayerPrefs.SetInt (key, 1);
	}

	void resetProgress()
	{
		PlayerPrefs.DeleteAll ();
	}
}
