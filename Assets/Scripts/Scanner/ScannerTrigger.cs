﻿using UnityEngine;
using System.Collections;

public class ScannerTrigger : ItemTrigger 
{

	private TillStateMachine machine;

	private GameObject currentItem;
	private Pin pin;

	private float lerpStartTime;
	private Vector3 lerpStartPosition;
	public float moveToPinThreshold = 0.1f;
	public float moveToPinDuration = 1.0f;


	public GameObject scannerTriggerPublic;


	// Use this for initialization
	void Awake () 
	{
		machine = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();
		
		pin = GameObject.FindGameObjectWithTag ("Pin").GetComponent<Pin>();
	}
	
	public override void OnTriggerEnter (Collider other) 
	{
		base.OnTriggerEnter (other);

		if (other.gameObject.tag == "ShoppingItem") 
		{
			GameObject toPin = other.gameObject;
			ItemStatus status = toPin.GetComponent<ItemStatus> ();
			if (pin.pinning || currentItem != null || (status !=null && status.autoDragged))
				return;
			
			currentItem = toPin;

			startPinning();
		}
	}
	
//	public void OnTriggerStay (Collider other) 
//	{
//		if (!pin.GetComponent<Pin>().pinning && currentItem == null && other.gameObject.tag == "ShoppingItem" && other.gameObject.GetComponent<ItemStatus>().scanned == 0) 
//		{
//			startPinning(other.gameObject);
//		}
//	}


	public void startPinning()
	{
		currentItem.transform.Find ("Dragger").gameObject.SetActive (false);
		currentItem.rigidbody.isKinematic = true;
		lerpStartTime = Time.time;
		lerpStartPosition = currentItem.transform.position;
		StartCoroutine ("moveToPin");
	}
	

	IEnumerator moveToPin()
	{
		while(Vector3.Distance(currentItem.transform.position, pin.transform.position) > moveToPinThreshold)
		{
			float t = (Time.time - lerpStartTime)/moveToPinDuration;
			currentItem.transform.position = Vector3.Lerp(lerpStartPosition, pin.transform.position, t);
			yield return null;
		}
		
		currentItem.transform.position = pin.transform.position;
		pin.pinItem(currentItem);

		currentItem = null;
		
	}
}
