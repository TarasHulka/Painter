using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BrushSettings {
	public BrushSettings (Color _color, int _size) {
		BColor = _color;
		BSize = _size;
	}
	public Color BColor { get; set; }
	public int BSize { get; set; }

}
public class Painter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IPointerDownHandler {

	Texture2D thisTxt2D;
	Sprite thisSprite;
	RectTransform thisRectTransform;
	Color[] TextureColours;
	Vector2 previos;
	bool InsideCanvas;
	Vector2 DrawCanvasSize;
	BrushSettings Brush;
	public Button btnClear;

	void Awake () {

		thisRectTransform = transform as RectTransform;
		Image thisImage = GetComponent<Image> ();
		thisSprite = thisImage.sprite;
		thisTxt2D = thisSprite.texture;
		TextureColours = new Color[(int) thisSprite.rect.width * (int) thisSprite.rect.height];
		TextureColours = thisTxt2D.GetPixels ();
		Clear ();
		DrawCanvasSize.x = thisSprite.rect.width;
		DrawCanvasSize.y = thisSprite.rect.height;

		Brush = new BrushSettings (Color.black, 2);

	}
	void OnEnable () {
		ColorSelector.event_ChangeColor += SetColor;
		SizeController.event_BrushSize += SetSize;
		btnClear.onClick.AddListener (Clear);
		SaveToPng.event_GetTexturesBytes += GetTexture;
	}
	void OnDisable () {
		ColorSelector.event_ChangeColor -= SetColor;
		SizeController.event_BrushSize -= SetSize;
		btnClear.onClick.RemoveAllListeners ();
		SaveToPng.event_GetTexturesBytes -= GetTexture;
	}

	void Clear () {
		for (int x = 0; x < TextureColours.Length; x++)
			TextureColours[x] = Color.white;
		thisTxt2D.SetPixels (TextureColours);
		thisTxt2D.Apply ();

	}

	void GetTexture (UnityAction<Texture2D> callback) {
		callback (thisTxt2D);
	}

	void SetColor (Color newColor) {
		Brush.BColor = newColor;
	}
	void SetSize (int newSize) {
		Brush.BSize = newSize;
	}
	public void OnPointerEnter (PointerEventData eventData) {
		InsideCanvas = true;

	}
	public void OnPointerExit (PointerEventData eventData) {
		InsideCanvas = false;

	}
	public void OnPointerDown (PointerEventData eventData) {
		if (InsideCanvas) {
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle (thisRectTransform, eventData.position, eventData.enterEventCamera, out previos)) {
				previos.x += DrawCanvasSize.x / 2;
				previos.y += DrawCanvasSize.y / 2;
				SetPixels (previos);
			}
		}
	}

	public void OnDrag (PointerEventData eventData) {
		if (InsideCanvas) {

			Vector2 pointer;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle (thisRectTransform, eventData.position, eventData.enterEventCamera, out pointer)) {

				pointer.x += DrawCanvasSize.x / 2;
				pointer.y += DrawCanvasSize.y / 2;

				float distance = Vector2.Distance (previos, pointer);

				if (distance > 1) {
					float steps = 1 / distance;
					Vector2 position;
					for (float lerp = 0; lerp <= 1; lerp += steps) {
						position = Vector2.Lerp (previos, pointer, lerp);
						SetPixels (position);;
					}
				} else {
					SetPixels (pointer);
				}
				previos = pointer;
			}
		}
	}

	public void SetPixels (Vector2 center) {

		int center_x = (int) center.x;
		int center_y = (int) center.y;

		int radius = Brush.BSize / 2;
		for (int x = center_x - radius; x <= center_x + radius; x++) {
			for (int y = center_y - radius; y <= center_y + radius; y++) {
				thisTxt2D.SetPixel (x, y, Brush.BColor);
			}
		}

		thisTxt2D.Apply ();
	}
}