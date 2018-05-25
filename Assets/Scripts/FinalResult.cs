using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using System.Linq;

public class FinalResult : MonoBehaviour {

	private Firebase.Database.DatabaseReference mDatabase;
	private Firebase.Database.DataSnapshot mDataSnapshot;
	private string urlDatabase = "https://escaperoom-b425b.firebaseio.com/";
	[SerializeField] private GameObject puestos;

	private bool allTerminados = false;
	private bool allDone = false;
	private List<int> resultadosFinales;

	[SerializeField] private Text waitingText;
	[SerializeField] private List<Text> top;
	[SerializeField] private List<Text> puestoText;

	void Start () {
		resultadosFinales = new List<int> ();
		mDatabase = Firebase.Database.FirebaseDatabase.GetInstance (urlDatabase).GetReference("/EscapeRoom");
	}

	void Update () {
		Firebase.Database.FirebaseDatabase.GetInstance (urlDatabase).GetReference("/EscapeRoom").ValueChanged += HandleValueChanged;

		if (mDataSnapshot != null) {
			if (!allTerminados) {
				int terminados = 0;
				Debug.Log ("Final: " + SendData.grupoIDs.Count);
				for (int i = 0; i < SendData.grupoIDs.Count; i++) {
					if (mDataSnapshot.Child ("Sesion " + GroupManager.actualSesion).Child (SendData.grupoIDs [i]).Child ("Resultado").GetValue (true) != null)
						terminados++;
				}

				if (terminados == GroupManager.numGrupos)
					allTerminados = true;
			}
			else {
				Debug.Log ("Terminado");
				if(!allDone)
					CheckResults ();
			}
		}
	}

	void CheckResults(){
		allDone = true;
		waitingText.gameObject.SetActive (false);
		puestos.SetActive (true);
		for (int i = 0; i < SendData.grupoIDs.Count; i++) {
			resultadosFinales.Add (int.Parse(mDataSnapshot.Child ("Sesion " + GroupManager.actualSesion).Child (SendData.grupoIDs [i]).Child ("Resultado").GetValue (true).ToString()));
		}

		List<int> sortedResults = new List<int> ();

		for (int j = 0; j < resultadosFinales.Count; j++)
			sortedResults.Add (resultadosFinales [j]);

		sortedResults.Sort ();
		int antIndex = -1;
		int repetido = 0;
		for (int k = 0; k < resultadosFinales.Count; k++) {

			int highestValue = sortedResults [sortedResults.Count - 1];
			int index = resultadosFinales.FindIndex (a => a == highestValue);


			if (index == antIndex) {
				repetido++;
			}
			else {
				antIndex = index;
				repetido = 0;
			}

			mDatabase.Child ("Sesion " + GroupManager.actualSesion).Child (SendData.grupoIDs [index + repetido]).Child ("Puesto").SetValueAsync (k + 1 - repetido);
			Debug.Log ("Index: " + index + "; Index real: " + (index + repetido) 
				+ "; Highest: " + highestValue + "; K: " + k + "; Puesto: " + (k + 1 - repetido));
			sortedResults.Remove (highestValue);
			index++;
			if (k < 3) {
				puestoText [k].text = "Top " + (k + 1 - repetido);
				top [k].text = "Grupo " + (index + repetido);
			}
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


/*
mDataSnapshot.Child ("Sesion " + WaitingTeacher.actualSesion).Child (SendData.userID).Child ("Puesto").GetValue (true)
mDatabase.Child ("Sesion " + WaitingTeacher.actualSesion).Child (SendData.userID).Child ("Resultado").SetValueAsync (resultado);
*/