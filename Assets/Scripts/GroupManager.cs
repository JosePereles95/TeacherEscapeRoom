using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using UnityEngine.SceneManagement;

public class GroupManager : MonoBehaviour {

	public static int numGrupos = 0;

	private Firebase.Database.DatabaseReference mDatabase;
	private Firebase.Database.DataSnapshot mDataSnapshot;
	private string urlDatabase = "https://escaperoom-b425b.firebaseio.com/";

	[SerializeField] private InputField inputNumGrupos;
	[SerializeField] private Text waitingGroups;
	private int groupsConfirmed = 0;
	private List<int> numsConfirmed;

	void Start () {
		mDatabase = Firebase.Database.FirebaseDatabase.GetInstance (urlDatabase).GetReference("/EscapeRoom");
		mDatabase.RemoveValueAsync ();
		numsConfirmed = new List<int> ();

		mDatabase.Child ("All Confirmed").SetValueAsync (false);
	}

	void Update () {
		Firebase.Database.FirebaseDatabase.GetInstance (urlDatabase).GetReference("/EscapeRoom").ValueChanged += HandleValueChanged;

		if (mDataSnapshot != null) {
			if (numGrupos != 0 && groupsConfirmed < numGrupos) {
				for (int i = 1; i <= numGrupos; i++) {
					if (mDataSnapshot.Child ("Grupos").Child ("Grupo " + i).GetValue(true) != null && !numsConfirmed.Contains(i)){
						groupsConfirmed++;
						numsConfirmed.Add (i);
					}
				}
			}
		}

		if (numGrupos != 0 && numGrupos == groupsConfirmed) {
			mDatabase.Child ("All Confirmed").SetValueAsync (true);
			SceneManager.LoadScene ("Questions");
		}
		
	}

	public void ConfirmarGrupos(){
		if (inputNumGrupos.text != "0" && inputNumGrupos.text != "") {
			numGrupos = int.Parse (inputNumGrupos.text);
			mDatabase.Child ("Num Grupos").SetValueAsync (numGrupos);
			waitingGroups.gameObject.SetActive (true);
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