using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonListButton : MonoBehaviour {

	[SerializeField] private Text myText;
	[SerializeField] private GameObject myCanvas;
	[SerializeField] private Text groupText;
	public static int groupActive = 0;

	public void SetText(string textString){
		myText.text = textString;
	}

	public void OnClick(){
		GameObject actualButton = EventSystem.current.currentSelectedGameObject;
		groupText.text = actualButton.GetComponentInChildren<Text> ().text;
		groupActive = int.Parse (groupText.text [6].ToString());
		myCanvas.SetActive (true);
	}
}