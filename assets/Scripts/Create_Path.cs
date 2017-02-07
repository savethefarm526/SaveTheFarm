using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;

public class Create_Path : MonoBehaviour {
	public List<Vector3> mMap = new List<Vector3> ();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (1)) {
			if (this.mMap.Count != 0)
				this.mMap.RemoveAt (this.mMap.Count - 1);
		}
		if (Input.GetMouseButtonUp (0)) {
			Vector3 pos = Input.mousePosition;
			pos.z = 10;

			pos = Camera.main.ScreenToWorldPoint (pos);

			pos.x = Mathf.RoundToInt (pos.x);
			pos.z = Mathf.RoundToInt (pos.z);
			this.mMap.Add (pos);
		}
		if (Input.GetKey (KeyCode.W)) {
			Vector3 pos = Camera.main.transform.localPosition;
			pos.z += 0.3f;
			Camera.main.transform.localPosition = pos;
		}
		if (Input.GetKey (KeyCode.A)) {
			Vector3 pos = Camera.main.transform.localPosition;
			pos.x -= 0.3f;
			Camera.main.transform.localPosition = pos;
		}
		if (Input.GetKey (KeyCode.S)) {
			Vector3 pos = Camera.main.transform.localPosition;
			pos.z -= 0.3f;
			Camera.main.transform.localPosition = pos;
		}
		if (Input.GetKey (KeyCode.D)) {
			Vector3 pos = Camera.main.transform.localPosition;
			pos.x += 0.3f;
			Camera.main.transform.localPosition = pos;
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0)
			Camera.main.orthographicSize *= 1.05f;
		if (Input.GetAxis ("Mouse ScrollWheel") > 0)
			Camera.main.orthographicSize *= 0.95f;
		if (Input.GetKeyUp (KeyCode.F1)) {
			StringBuilder sb = new StringBuilder ();
			for (int i = 0; i < this.mMap.Count; i++)
				sb.AppendLine (this.mMap [i].x + "," + this.mMap [i].z);
			string path = EditorUtility.SaveFilePanel ("Save path","/Assets/Resources/Map", "1", "txt");
			File.WriteAllText (path, sb.ToString ());
		}
	}
	public void OnDrawGizmos(){
		Gizmos.color = Color.white;
		if (mMap.Count > 0)
			Gizmos.DrawCube (mMap [0], new Vector3 (0.95f, 0.95f, 0.95f));
		Gizmos.color = Color.green;
		int x, z;
		for (int i = 1; i < mMap.Count; i++) {
			x = (int)mMap [i - 1].x;
			z = (int)mMap [i - 1].z;
			while (z != mMap [i].z) {
				if (z > mMap [i].z)
					z--;
				else
					z++;
				Gizmos.DrawCube (new Vector3 (x, 0, z), new Vector3 (0.95f, 0.95f, 0.95f));
			}
			while (x != mMap [i].x) {
				if (x > mMap [i].x)
					x--;
				else
					x++;
				Gizmos.DrawCube (new Vector3 (x, 0, z), new Vector3 (0.95f, 0.95f, 0.95f));
			}
		}
		Gizmos.color = Color.red;
		if (mMap.Count > 0)
			Gizmos.DrawCube (mMap [mMap.Count - 1], new Vector3 (0.95f, 0.95f, 0.95f));
	}
}
