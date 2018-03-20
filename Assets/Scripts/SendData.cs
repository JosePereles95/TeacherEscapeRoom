using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Firebase;

public class SendData : MonoBehaviour {
	
	private Firebase.Database.DatabaseReference mDatabase;
	private Firebase.Database.DataSnapshot mDataSnapshot;
	private string urlDatabase = "https://escaperoom-b425b.firebaseio.com/";

	private List<string> grupoIDs;

	[SerializeField] private List<Button> questionsButton;

	void Awake(){
		Application.targetFrameRate = 20;
	}

	void Start(){
		grupoIDs = new List<string> ();

		mDatabase = Firebase.Database.FirebaseDatabase.GetInstance (urlDatabase).GetReference("/EscapeRoom");
	}

	void Update(){
		Firebase.Database.FirebaseDatabase.GetInstance (urlDatabase).GetReference("/EscapeRoom").ValueChanged += HandleValueChanged;

		if (mDataSnapshot != null) {
			for (int i = 1; i <= GroupManager.numGrupos; i++) {
				if (mDataSnapshot.Child ("Grupos").Child ("Grupo " + i).Child ("userID").GetValue (true) != null) {
					grupoIDs.Add (mDataSnapshot.Child ("Grupos").Child ("Grupo " + i).Child ("userID").GetValue (true).ToString ());
				}
			}

			for (int j = 0; j < questionsButton.Count; j++) {
				if (ButtonListButton.groupActive != 0) {
					string questionValue = mDataSnapshot.Child (grupoIDs [ButtonListButton.groupActive - 1]).Child ("Questions").Child (questionsButton [j].name).GetValue (true).ToString ();
				
					if (questionValue == "True") {
						questionsButton [j].image.color = Color.green;
					} else {
						questionsButton [j].image.color = Color.red;
					}
				}
			}
		}
	}

	public void CheckQuestionPressed(){
		string actualQuestion = EventSystem.current.currentSelectedGameObject.name;

		string questionValue = mDataSnapshot.Child (grupoIDs [ButtonListButton.groupActive - 1]).Child ("Questions").Child (actualQuestion).GetValue (true).ToString();

		if(questionValue == "True")
			mDatabase.Child (grupoIDs[ButtonListButton.groupActive-1]).Child ("Questions").Child (actualQuestion).SetValueAsync (false);
		else
			mDatabase.Child (grupoIDs[ButtonListButton.groupActive-1]).Child ("Questions").Child (actualQuestion).SetValueAsync (true);
	}

	void HandleValueChanged(object sender, Firebase.Database.ValueChangedEventArgs args){
		if (args.DatabaseError != null) {
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
		mDataSnapshot = args.Snapshot;
	}
}