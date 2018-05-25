using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Firebase;

public class SendData : MonoBehaviour {

	public static List<string> grupoIDs;

	private Firebase.Database.DatabaseReference mDatabase;
	private Firebase.Database.DataSnapshot mDataSnapshot;
	private string urlDatabase = "https://escaperoom-b425b.firebaseio.com/";

	[SerializeField] private List<Button> questionsButton;

	private bool groupsAdded = false;

	void Awake(){
		Application.targetFrameRate = 20;
	}

	void Start(){
		grupoIDs = new List<string> ();

		mDatabase = Firebase.Database.FirebaseDatabase.GetInstance (urlDatabase).GetReference("/EscapeRoom");
	}

	void Update(){
		Firebase.Database.FirebaseDatabase.GetInstance (urlDatabase).GetReference("/EscapeRoom").ValueChanged += HandleValueChanged;
		Debug.Log ("Normal: " + grupoIDs.Count);
		if (mDataSnapshot != null) {

			if (!groupsAdded) {
				for (int i = 1; i <= GroupManager.numGrupos; i++) {
					if (mDataSnapshot.Child ("Sesion " + GroupManager.actualSesion).Child ("Grupos").Child ("Grupo " + i).Child ("userID").GetValue (true) != null) {
						grupoIDs.Add (mDataSnapshot.Child ("Sesion " + GroupManager.actualSesion).Child ("Grupos").Child ("Grupo " + i).Child ("userID").GetValue (true).ToString ());
					}
				}
				groupsAdded = true;
			}

			for (int j = 0; j < questionsButton.Count; j++) {
				if (ButtonListButton.groupActive != 0) {
					string questionValue = mDataSnapshot.Child("Sesion " + GroupManager.actualSesion).Child (grupoIDs [ButtonListButton.groupActive - 1]).Child ("Questions").Child (questionsButton [j].name).GetValue (true).ToString ();

					if (mDataSnapshot.Child ("Sesion " + GroupManager.actualSesion).Child (grupoIDs [ButtonListButton.groupActive - 1]).Child ("Detection").Child (questionsButton [j].name).GetValue (true).ToString () == "True") {
						if (questionValue == "True") {
							questionsButton [j].image.color = Color.green;
						} else {
							questionsButton [j].image.color = Color.red;
						}
					}
				}
			}
		}
	}

	public void CheckQuestionPressed(){
		string actualQuestion = EventSystem.current.currentSelectedGameObject.name;

		string questionValue = mDataSnapshot.Child("Sesion " + GroupManager.actualSesion).Child (grupoIDs [ButtonListButton.groupActive - 1]).Child ("Questions").Child (actualQuestion).GetValue (true).ToString();

		if (mDataSnapshot.Child ("Sesion " + GroupManager.actualSesion).Child (grupoIDs [ButtonListButton.groupActive - 1]).Child ("Detection").Child (actualQuestion).GetValue (true).ToString () == "True") {
			if (questionValue == "True")
				mDatabase.Child ("Sesion " + GroupManager.actualSesion).Child (grupoIDs [ButtonListButton.groupActive - 1]).Child ("Questions").Child (actualQuestion).SetValueAsync (false);
			else
				mDatabase.Child ("Sesion " + GroupManager.actualSesion).Child (grupoIDs [ButtonListButton.groupActive - 1]).Child ("Questions").Child (actualQuestion).SetValueAsync (true);
		}
	}

	void HandleValueChanged(object sender, Firebase.Database.ValueChangedEventArgs args){
		if (args.DatabaseError != null) {
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
		mDataSnapshot = args.Snapshot;
	}
}