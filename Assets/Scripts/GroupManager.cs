using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using UnityEngine.SceneManagement;

public class GroupManager : MonoBehaviour {

	public static int numGrupos = 0;
	public static float minsJuego = 0;

	private Firebase.Database.DatabaseReference mDatabase;
	private Firebase.Database.DataSnapshot mDataSnapshot;
	private string urlDatabase = "https://escaperoom-b425b.firebaseio.com/";

	[SerializeField] private InputField inputNumGrupos;
	[SerializeField] private InputField inputTiempo;
	[SerializeField] private Button buttonTiempo;
	[SerializeField] private Text waitingGroups;

	private bool gruposOk = false;
	private bool tiempoOk = false;

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

		if (numGrupos != 0 && numGrupos == groupsConfirmed && tiempoOk) {
			mDatabase.Child ("All Confirmed").SetValueAsync (true);
			SceneManager.LoadScene ("Questions");
		}

		if (gruposOk) {
			inputTiempo.gameObject.SetActive (true);
			buttonTiempo.gameObject.SetActive (true);
		}

		if(tiempoOk)
			waitingGroups.gameObject.SetActive (true);
	}

	public void ConfirmarGrupos(){
		if (inputNumGrupos.text != "0" && inputNumGrupos.text != "" && numGrupos == 0) {
			numGrupos = int.Parse (inputNumGrupos.text);
			mDatabase.Child ("Num Grupos").SetValueAsync (numGrupos);
			gruposOk = true;
			inputNumGrupos.GetComponent<InputField> ().enabled = false;
		}
	}

	public void ConfirmarTiempo(){
		if (inputTiempo.text != "0" && inputTiempo.text != "" && minsJuego == 0) {
			minsJuego = float.Parse (inputTiempo.text);
			mDatabase.Child ("Tiempo").SetValueAsync (minsJuego);
			tiempoOk = true;
			inputTiempo.GetComponent<InputField> ().enabled = false;
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