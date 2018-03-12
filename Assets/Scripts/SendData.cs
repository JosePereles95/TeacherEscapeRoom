using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Firebase;

public class SendData : MonoBehaviour {

	//public InputField data;
	//public InputField readData;
	//public InputField q1Data;

	//public InputField consolaAndroid;
	//public int check = 0;

	private Firebase.Database.DatabaseReference mDatabase;
	private Firebase.Database.DataSnapshot mDataSnapshot;
	private string urlDatabase = "https://escaperoom-b425b.firebaseio.com/";

	private string grupo1ID = "";
	private string grupo2ID = "";
	private string grupo3ID = "";

	//private string questionChecked = "false";
	//private string newDato;
	//private string newQ1;

	void Start(){
		mDatabase = Firebase.Database.FirebaseDatabase.GetInstance (urlDatabase).GetReference("/EscapeRoom");
		mDatabase.RemoveValueAsync ();
	}

	void Update(){
		Firebase.Database.FirebaseDatabase.GetInstance (urlDatabase).GetReference("/EscapeRoom").ValueChanged += HandleValueChanged;

		Debug.Log (grupo1ID);

		if (mDataSnapshot != null) {
			if (mDataSnapshot.Child ("Grupos").Child ("Grupo 1").Child ("userID").GetValue (true) != null)
				grupo1ID = mDataSnapshot.Child ("Grupos").Child ("Grupo 1").Child ("userID").GetValue (true).ToString ();
			
			if (mDataSnapshot.Child ("Grupos").Child ("Grupo 2").Child ("userID").GetValue (true) != null)
				grupo2ID = mDataSnapshot.Child ("Grupos").Child ("Grupo 2").Child ("userID").GetValue (true).ToString ();
			
			if (mDataSnapshot.Child ("Grupos").Child ("Grupo 3").Child ("userID").GetValue (true) != null)
				grupo3ID = mDataSnapshot.Child ("Grupos").Child ("Grupo 3").Child ("userID").GetValue (true).ToString ();
		}
	}

	public void CheckG1Pressed(){
		if (grupo1ID != "")
			mDatabase.Child (grupo1ID).Child ("Questions").Child ("question1").SetValueAsync ("true");
	}

	public void CheckG2Pressed(){
		if (grupo2ID != "")
			mDatabase.Child (grupo2ID).Child ("Questions").Child ("question1").SetValueAsync ("true");
	}

	public void CheckG3Pressed(){
		if (grupo3ID != "")
			mDatabase.Child (grupo3ID).Child ("Questions").Child ("question1").SetValueAsync ("true");
	}

	void HandleValueChanged(object sender, Firebase.Database.ValueChangedEventArgs args){
		//check++;

		if (args.DatabaseError != null) {
			Debug.LogError(args.DatabaseError.Message);
			//consolaAndroid.text = "ERROR";
			return;
		}
		mDataSnapshot = args.Snapshot;
	}
		
}