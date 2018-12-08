using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SizeController : MonoBehaviour {
	const int MinSize = 1;
	const int MaxSize = 20;

	[Range (1, 20)]
	public int BrushSize;
	public Slider mainSlider;
	public RectTransform BrushExample;
	Image Example;
	int BrushSizeDelta = 3; // для кращої видимості кісточки. Маленькі розміри не видимі

	public static UnityAction<int> event_BrushSize;

	void Start () {

		Example = BrushExample.GetComponent<Image> ();
		mainSlider.minValue = MinSize;
		mainSlider.maxValue = MaxSize;
		mainSlider.value = 3; // init values;

	}
	void OnEnable () {
		mainSlider.onValueChanged.AddListener (delegate { ValueChangeCheck (); });
		ColorSelector.event_ChangeColor += SetColor;
		PredefinedBrushes.event_BrushSize += PredefinedBushSize;
	}
	private void OnDisable () {
		mainSlider.onValueChanged.RemoveAllListeners ();
		ColorSelector.event_ChangeColor -= SetColor;
		PredefinedBrushes.event_BrushSize -= PredefinedBushSize;
	}

	void PredefinedBushSize (int size) {
		mainSlider.value = size;
	}

	public void ValueChangeCheck () {
		SetBrushSize (mainSlider.value);
	}

	void SetColor (Color newColor) {
		Example.color = newColor;
	}
	void SetBrushSize (float value) {
		BrushSize = Mathf.RoundToInt (value);
		BrushExample.sizeDelta = new Vector2 (BrushSize + BrushSizeDelta, BrushSize + BrushSizeDelta);
		if (event_BrushSize != null) event_BrushSize (BrushSize);
	}
}