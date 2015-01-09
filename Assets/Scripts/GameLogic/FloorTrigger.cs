﻿using UnityEngine;
using System.Collections;

public class FloorTrigger : ItemTrigger {

	public virtual float getScore()
	{
		float malus = 0;
		for (int i = 0; i < objectsInside.Count; i++) 
		{
			malus -= 1.0f;
		}
		return malus;
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter (other);

		if (other.gameObject.tag == "ShoppingItem")
		{
			other.gameObject.GetComponent<ItemStatus>().customer.onItemOnFloor(other.gameObject);
		}
	}
}
