using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet_Manager {
	public static List<Bullet> list=new List<Bullet>();
	public static Bullet create(string name){
		for (int i = 0; i < list.Count; i++) {
			if (list [i].gameObject.activeSelf == false && list [i].name == name) {
				list [i].gameObject.SetActive (true);
				return list [i];
			}
		}
		GameObject obj;
		if (name.Equals(""))
			obj = new GameObject ();
		else
			obj = GameObject.Instantiate (Resources.Load<GameObject> ("Model/" + name));
		obj.name = name;
		Bullet bullet = obj.AddComponent<Bullet> ();
		list.Add (bullet);
		return bullet;
	}
}
