using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower_Manager {
	public static GameObject create(string name){
		GameObject obj = Resources.Load<GameObject> ("Model/" + name);
		GameObject tower = GameObject.Instantiate (obj);
		return tower;
	}
}
