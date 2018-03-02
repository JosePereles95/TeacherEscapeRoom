/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class FirebaseScript : MonoBehaviour {
	public InputField EmailAddress, Password;

	public void LogInButtonPressed(){
		FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync (EmailAddress.text, Password.text).ContinueWith ((obj) => {
			SceneManager.LoadSceneAsync ("Test");
		});
	}

	public void LogInAnonymousButtonPressed(){
		FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync().ContinueWith ((obj) => {
			SceneManager.LoadSceneAsync ("Test");
		});
	}

	public void CreateNewUserButtonPressed(){
		FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(EmailAddress.text, Password.text).ContinueWith ((obj) => {
			SceneManager.LoadSceneAsync ("Test");
		});
	}
}*/