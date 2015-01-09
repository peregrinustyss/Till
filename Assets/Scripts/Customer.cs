﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Customer : MonoBehaviour {
	public ArrayList shoppingItems;
	public CustomerProfile profile;

	public float spawnDelay;

	private TillStateMachine till;

	public Text text;
	private float waitingTime;
	
//	public GameObject image;

	// Use this for initialization
	void OnEnable () {
		shoppingItems = new ArrayList ();

		till = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TillStateMachine> ();

		waitingTime = 0.0f;

		text = transform.GetComponentInChildren<Text> ();
		text.transform.parent.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
		waitingTime += Time.deltaTime;

		//TODO: show specific sentences when waitingTime reaches certain times. e.g. when waitingTime is bigger then 10 seconds say first boring sentence
		//how to get a child gameobject of customer
//		Transform textT = transform.FindChild ("texts");
//		GameObject texts = textT.gameObject;
//		
//		texts = transform.FindChild ("texts").gameObject;
	}

	public void showItems()
	{
		StartCoroutine (showingItems ());
	}

	//TODO: find best place to call this callback. maybe from TilLStateMachine or from floorTrigger<ItemTrigger>
	public void onItemOnFloor(GameObject item)
	{
		text.gameObject.transform.parent.gameObject.SetActive (true);
		text.text = "hey, you dropped my " + item.GetComponent<ItemStatus> ().name + " on the floor";
//		Debug.Log ("hey, you dropped my " + item.GetComponent<ItemStatus>().name + " on the floor");

		StartCoroutine (hideBubble ());
	}

	public void onMultipleScanned(GameObject item)
	{
		text.gameObject.transform.parent.gameObject.SetActive (true);
		text.text = "hey, you scanned my " + item.GetComponent<ItemStatus> ().name + " again, you fool!";
//		Debug.Log ("hey, you scanned my " + item.GetComponent<ItemStatus>().name + " again. WTF!");

		StartCoroutine (hideBubble ());

	}

	public void onNotMyItem(GameObject item)
	{
		text.gameObject.transform.parent.gameObject.SetActive (true);
		text.text = "hey, this is not my " + item.GetComponent<ItemStatus> ().name;
//		Debug.Log ("hey, this is not my " + item.GetComponent<ItemStatus>().name);

		StartCoroutine (hideBubble ());

	}

	IEnumerator showingItems()
	{
		
		//then spawn his/her items 
		till.isSpawningItems = true;
		for (int i = 0; i < shoppingItems.Count; i++) 
		{
			GameObject temp = (GameObject)shoppingItems[i];
			temp.SetActive(true);
			
			yield return new WaitForSeconds(spawnDelay);
		}
		till.isSpawningItems = false;
	}

	public void leave()
	{
		StartCoroutine (leaving ());
	}
	

	IEnumerator hideBubble()
	{
		yield return new WaitForSeconds (2.0f);

		//TODO: in an own Coroutine
//		Color colorText = text.color;
//		Color colorButton = text.transform.parent.gameObject.GetComponent<Image> ().color;
//		
//		float startTime = 0.0f;
//		float fadeTime = 0.5f;
//		while (startTime < fadeTime) 
//		{
//			startTime = startTime + Time.deltaTime;
//			float newAlpha = Mathf.Lerp(1, 0, startTime/fadeTime);
//			colorText.a = newAlpha;
//			colorButton.a = newAlpha;
//
//			
//			text.renderer.material.color = colorText;
//			text.transform.parent.gameObject.renderer.material.color = colorButton;
//
//			yield return null;
//		}

		text.gameObject.transform.parent.gameObject.SetActive (false);
	}

	IEnumerator leaving()
	{
		while (transform.position.x < 11.0f) 
		{
			transform.Translate (0.05f,0,0.05f, Space.World);
			
			yield return null;
		}
		
		transform.GetChild(0).gameObject.SetActive (false);
	}



	public void onBeltMoved(float offset)
	{
		if (transform.position.x < 3.5f)
			transform.Translate (offset, 0, 0);
	}
}