using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy_Manager {

    public static Enemy create(Enemy_Info info,List<Vector3> path){
		GameObject obj = Resources.Load<GameObject> ("Model/" + info.model);
		Enemy enemy = GameObject.Instantiate (obj).AddComponent<Enemy> ();
        
		enemy.init (path, info.health, info.speed, info.money, info.model, info.period);
		return enemy;
	}
}
