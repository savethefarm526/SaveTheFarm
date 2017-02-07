using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Manager : MonoBehaviour {
	public static GameObject canvas;
	public static List<MonoBehaviour> mUI_List = new List<MonoBehaviour> ();

	public static T Enter<T>() where T:UI_Base{
		if (canvas == null)
			canvas = GameObject.Find ("Canvas");

		GameObject ui = GameObject.Instantiate(Resources.Load<GameObject> ("Prefabs/" + typeof(T).ToString ()));
		ui.transform.SetParent (canvas.transform);

		RectTransform rect_transform = ui.GetComponent<RectTransform> ();
		rect_transform.offsetMin = Vector2.zero;
		rect_transform.offsetMax = Vector2.zero;

		T t = ui.AddComponent<T> ();
		t.init_Node (t.transform);
		mUI_List.Add (t);
		return t;
	}
	public static void Exit(MonoBehaviour mono){
		mUI_List.Remove (mono);
		GameObject.Destroy (mono.gameObject);
	}
}
