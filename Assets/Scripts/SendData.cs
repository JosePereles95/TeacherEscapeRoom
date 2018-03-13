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

	private List<string> grupoIDs;

	//private string questionChecked = "false";
	//private string newDato;
	//private string newQ1;

	void Start(){
		grupoIDs = new List<string> ();

		mDatabase = Firebase.Database.FirebaseDatabase.GetInstance (urlDatabase).GetReference("/EscapeRoom");
	}

	void Update(){
		Firebase.Database.FirebaseDatabase.GetInstance (urlDatabase).GetReference("/EscapeRoom").ValueChanged += HandleValueChanged;

		if (mDataSnapshot != null) {
			for (int i = 1; i <= GroupManager.numGrupos; i++) {
				//grupoIDs.Add (mDataSnapshot.Child ("Grupos").Child ("Grupo " + i).Child ("userID").GetValue (true).ToString ());
			}
		}
	}

	public void CheckQuestionPressed(){
		string actualQuestion = EventSystem.current.currentSelectedGameObject.name;
		mDatabase.Child (grupoIDs[ButtonListButton.groupActive-1]).Child ("Questions").Child (actualQuestion).SetValueAsync (true);
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