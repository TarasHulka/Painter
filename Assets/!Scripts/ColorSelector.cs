using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ColorSelector : MonoBehaviour {

	Button thisButton;
	Color CurrentColor;
	public static UnityAction<Color> event_ChangeColor;
	void Awake () {
		thisButton = GetComponent<Button> ();
		CurrentColor = thisButton.image.color;

	}
	private void OnEnable () {
		thisButton.onClick.AddListener (() => {
			if (event_ChangeColor != null) {
				event_ChangeColor (CurrentColor);
			}
		});
	}
	private void OnDisable () {
		thisButton.onClick.RemoveAllListeners ();
	}
}