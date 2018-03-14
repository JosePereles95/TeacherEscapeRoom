using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListControl : MonoBehaviour {

	[SerializeField] private GameObject buttonTemplate;

	void Start(){
		for (int i = 1; i <= GroupManager.numGrupos; i++) {
			GameObject button = Instantiate (buttonTemplate) as GameObject;
			button.SetActive (true);

			button.GetComponent<ButtonListButton> ().SetText ("Grupo " + i);

			button.transform.SetParent (buttonTemplate.transform.parent, false);

		}
	}
}