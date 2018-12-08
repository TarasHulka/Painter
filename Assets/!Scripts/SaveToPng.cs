using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
public class SaveToPng : MonoBehaviour {

	public static UnityAction<UnityAction<Texture2D>> event_GetTexturesBytes;
	Button thisButton;
	void Awake () {
		thisButton = GetComponent<Button> ();
	}
	private void OnEnable () {
		thisButton.onClick.AddListener (Export);
	}
	private void OnDisable () {
		thisButton.onClick.RemoveAllListeners ();
	}

	void Export(){
		if (event_GetTexturesBytes!=null) event_GetTexturesBytes(OnTexureRecive);
	}

	void OnTexureRecive(Texture2D texture){
		if (texture!=null){
		 	byte[] bytes = texture.EncodeToPNG();
			File.WriteAllBytes("./export/painter-"+Random.Range(0,9999)+".png", bytes);
		}

	}
}
