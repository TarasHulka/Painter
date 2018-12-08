using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PredefinedBrushes : MonoBehaviour {

	public int BruseSize;
	public static UnityAction<int> event_BrushSize;

	Button thisButton;
	void Awake () {
		thisButton = GetComponent<Button> ();
	}
	private void OnEnable () {
		thisButton.onClick.AddListener (() => {
			if (event_BrushSize != null) event_BrushSize (BruseSize);
		});
	}

	private void OnDisable () {
		thisButton.onClick.RemoveAllListeners ();
	}

}