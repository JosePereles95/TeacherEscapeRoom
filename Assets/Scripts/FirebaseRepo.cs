/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using System;
using System.Threading.Tasks;

[CreateAssetMenu(menuName = "Firebase/Repository")]
public class FirebaseRepo : ScriptableObject {

	public FirebasePath Path;

	public Task Push(object item){
		
		//if (Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser == null) {		
			return Path.GetReferenceFromRoot (Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference)
			.Push ().SetRawJsonValueAsync (JsonUtility.ToJson (item));

			return Task.FromResult (new object ());
		//}
	}
}*/