using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour{
	
	private Firebase.Database.DataSnapshot mDataSnapshot;
	private string urlDatabase = "https://escaperoom-b425b.firebaseio.com/";

	[SerializeField] private float tiempo = 0.0f;
	private int tiempoTotal = 0;
	private bool asignado = false;
	[SerializeField] private Text textTiempo;

	void Start(){
		
	}

	void Update(){
		Firebase.Database.FirebaseDatabase.GetInstance (urlDatabase).GetReference("/EscapeRoom").ValueChanged += HandleValueChanged;

		if (mDataSnapshot != null) {
			tiempoTotal = int.Parse (mDataSnapshot.Child ("Tiempo").GetValue (true).ToString ());
		}

		if (tiempoTotal != 0 && !asignado) {
			tiempo = tiempoTotal * 60;
			asignado = true;
		}

		tiempo -= Time.deltaTime;

		int horas = ((int) tiempo / 3600);
		int minutos = (((int) tiempo - horas * 3600) / 60);
		int segundos = (int) tiempo - (horas * 3600 + minutos * 60);

		textTiempo.text = horas.ToString () + ":" + minutos.ToString ("D2") + ":" + segundos.ToString ("D2");

		if (horas == 0 && minutos == 0 && segundos == 0)
			TiempoAgotado ();
			
	}

	void HandleValueChanged(object sender, Firebase.Database.ValueChangedEventArgs args){

		if (args.DatabaseError != null) {
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
		mDataSnapshot = args.Snapshot;
	}

	void TiempoAgotado(){
		Debug.Log ("Se acabó el tiempo");
	}
}