using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;

public class Create_Path : MonoBehaviour {
	public List<Vector3> mMap = new List<Vector3> ();
    public List<Vector3> mMap2 = new List<Vector3>();
    public List<Vector3> tower1List = new List<Vector3> ();
	public List<Vector3> tower2List = new List<Vector3> ();
	public List<Vector3> tower3List = new List<Vector3> ();
	public List<Vector3> tower4List = new List<Vector3> ();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (1)) {
			Vector3 pos = Input.mousePosition;
			pos.z = 10;

			pos = Camera.main.ScreenToWorldPoint (pos);

			pos.x = Mathf.RoundToInt (pos.x);
			pos.z = Mathf.RoundToInt (pos.z);

			if (Input.GetKey (KeyCode.Alpha1) && tower1List.Count != 0) {
				for (int i = 0; i < tower1List.Count; i++) {
					if (pos.Equals (tower1List [i])) {
						tower1List.RemoveAt (i);
						break;
					}
				}
			}else if (Input.GetKey (KeyCode.Alpha2) && tower2List.Count != 0) {
				for (int i = 0; i < tower2List.Count; i++) {
					if (pos.Equals (tower2List [i])) {
						tower2List.RemoveAt (i);
						break;
					}
				}
			}else if (Input.GetKey (KeyCode.Alpha3) && tower3List.Count != 0) {
				for (int i = 0; i < tower3List.Count; i++) {
					if (pos.Equals (tower3List [i])) {
						tower3List.RemoveAt (i);
						break;
					}
				}
			}else if (Input.GetKey (KeyCode.Alpha4) && tower4List.Count != 0) {
				for (int i = 0; i < tower4List.Count; i++) {
					if (pos.Equals (tower4List [i])) {
						tower4List.RemoveAt (i);
						break;
					}
				}
                
			}
            else if (Input.GetKey(KeyCode.Z) && this.mMap2.Count != 0)
                this.mMap2.RemoveAt(this.mMap2.Count - 1);
            else if (this.mMap.Count != 0)
				this.mMap.RemoveAt (this.mMap.Count - 1);
		}
		if (Input.GetMouseButtonUp (0)) {
			Vector3 pos = Input.mousePosition;
			pos.z = 10;

			pos = Camera.main.ScreenToWorldPoint (pos);

			pos.x = Mathf.RoundToInt (pos.x);
			pos.z = Mathf.RoundToInt (pos.z);

			if (Input.GetKey (KeyCode.Alpha1))
				this.tower1List.Add (pos);
			else if (Input.GetKey (KeyCode.Alpha2))
				this.tower2List.Add(pos);
			else if (Input.GetKey (KeyCode.Alpha3))
				this.tower3List.Add(pos);
			else if (Input.GetKey (KeyCode.Alpha4))
				this.tower4List.Add(pos);
            else if (Input.GetKey(KeyCode.Z))
                this.mMap2.Add(pos);
			else 
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
			Camera.main.fieldOfView *= 1.05f;
		if (Input.GetAxis ("Mouse ScrollWheel") > 0)
			Camera.main.fieldOfView *= 0.95f;
		if (Input.GetKeyUp (KeyCode.F1)) {
			StringBuilder sb = new StringBuilder ();
			for (int i = 0; i < this.mMap.Count; i++)
				sb.AppendLine (this.mMap [i].x + "," + this.mMap [i].z);
			sb.AppendLine ("#");
			for (int i = 0; i < this.tower1List.Count; i++)
				sb.AppendLine (this.tower1List [i].x + "," + this.tower1List [i].z);
			sb.AppendLine ("#");
			for (int i = 0; i < this.tower2List.Count; i++)
				sb.AppendLine (this.tower2List [i].x + "," + this.tower2List [i].z);
			sb.AppendLine ("#");
			for (int i = 0; i < this.tower3List.Count; i++)
				sb.AppendLine (this.tower3List [i].x + "," + this.tower3List [i].z);
			sb.AppendLine ("#");
			for (int i = 0; i < this.tower4List.Count; i++)
				sb.AppendLine (this.tower4List [i].x + "," + this.tower4List [i].z);
            sb.AppendLine("#");
            for (int i = 0; i < this.mMap2.Count; i++)
                sb.AppendLine(this.mMap2[i].x + "," + this.mMap2[i].z);
            
            string path = EditorUtility.SaveFilePanel ("Save path","Assets/Resources/Map", "1", "txt");
			File.WriteAllText (path, sb.ToString ());
		}
	}
	public void OnDrawGizmos(){
		Gizmos.color = Color.white;
		if (mMap.Count > 0)
			Gizmos.DrawCube (mMap [0], new Vector3 (0.95f, 0.95f, 0.95f));
        if (mMap2.Count > 0)
            Gizmos.DrawCube(mMap2[0], new Vector3(0.95f, 0.95f, 0.95f));
        Gizmos.color = Color.gray;
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
        for (int i = 1; i < mMap2.Count; i++)
        {
            x = (int)mMap2[i - 1].x;
            z = (int)mMap2[i - 1].z;
            while (z != mMap2[i].z)
            {
                if (z > mMap2[i].z)
                    z--;
                else
                    z++;
                Gizmos.DrawCube(new Vector3(x, 0, z), new Vector3(0.95f, 0.95f, 0.95f));
            }
            while (x != mMap2[i].x)
            {
                if (x > mMap2[i].x)
                    x--;
                else
                    x++;
                Gizmos.DrawCube(new Vector3(x, 0, z), new Vector3(0.95f, 0.95f, 0.95f));
            }
        }
        Gizmos.color = Color.black;
		if (mMap.Count > 0)
			Gizmos.DrawCube (mMap [mMap.Count - 1], new Vector3 (0.95f, 0.95f, 0.95f));
        if (mMap2.Count > 0)
            Gizmos.DrawCube(mMap2[mMap2.Count - 1], new Vector3(0.95f, 0.95f, 0.95f));
        if (tower1List.Count > 0) {
			Gizmos.color = Color.red;
			for (int i = 0; i < tower1List.Count; i++)
				Gizmos.DrawSphere (tower1List [i], 0.3f);
		}
		if (tower2List.Count > 0) {
			Gizmos.color = Color.yellow;
			for (int i = 0; i < tower2List.Count; i++)
				Gizmos.DrawSphere (tower2List [i], 0.3f);
		}
		if (tower3List.Count > 0) {
			Gizmos.color = Color.blue;
			for (int i = 0; i < tower3List.Count; i++)
				Gizmos.DrawSphere (tower3List [i], 0.3f);
		}
		if (tower4List.Count > 0) {
			Gizmos.color = Color.green;
			for (int i = 0; i < tower4List.Count; i++)
				Gizmos.DrawSphere (tower4List [i], 0.3f);
		}
	}
}
