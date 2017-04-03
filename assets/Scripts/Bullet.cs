using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public Enemy enemy;
	public float speed;
	public int power;
	public string name;
	public Vector3 enemy_Pos;
	public Vector3 init_Pos;
	private float time;

	public void init(Enemy enemy,float speed,int power, string name){
		this.enemy = enemy;
		this.speed = speed;
		this.power = power;
		this.enemy_Pos = enemy.transform.localPosition;
		this.name = name;
		this.init_Pos = this.transform.localPosition;
        
        if (name.StartsWith("bullet1"))
            Audio_Manager.PlaySound("bullet1");
        else if (name.StartsWith("bullet2"))
            Audio_Manager.PlaySound("bullet2");
        else if (name.StartsWith("bullet3"))
            Audio_Manager.PlaySound("bullet3");
        else if (name.StartsWith("bullet4"))
            Audio_Manager.PlaySound("bullet4");
        else
            Audio_Manager.PlaySound("enemy_attack");
        if (name.Equals ("bullet3"))
			StartCoroutine (disappear_Bullet());
	}
	public void init(Vector3 pos,float speed,int power, string name){
		this.enemy = null;
		this.speed = speed;
		this.power = power;
		this.enemy_Pos = pos;
		this.name = name;
		this.init_Pos = this.transform.localPosition;
		if (name.Equals ("bullet3"))
			StartCoroutine (disappear_Bullet ());
	}

	public IEnumerator disappear_Bullet(){
		yield return new WaitForSeconds (5);
		this.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (UI_Func.pause)
			return;

		if (name.Equals ("bullet3")) {
			Vector3 dir = enemy_Pos - init_Pos;
			dir.y = 0;
			this.transform.LookAt (this.transform.localPosition + dir);
			time = Vector3.Distance (init_Pos, enemy_Pos) / speed;
			this.transform.localPosition = Vector3.Lerp (this.transform.localPosition, this.transform.localPosition + dir, Time.deltaTime / time);
			return;
		}

		this.transform.LookAt (enemy_Pos);
		if (enemy != null) {
			enemy_Pos = enemy.transform.localPosition;
			enemy_Pos.y += 0.5f;
			if (speed > 0)
				time = Vector3.Distance (this.transform.localPosition, enemy_Pos) / speed;
			else
				time = 0;
			if (time > Time.deltaTime)
				this.transform.localPosition = Vector3.Lerp (this.transform.localPosition, enemy_Pos, Time.deltaTime / time);
			else {
				enemy.change_Health (-power,name);
				this.gameObject.SetActive (false);
			}
		} else {
			if (speed > 0)
				time = Vector3.Distance (this.transform.localPosition, enemy_Pos) / speed;
			else
				time = 0;
			if (time > Time.deltaTime)
				this.transform.localPosition = Vector3.Lerp (this.transform.localPosition, enemy_Pos, Time.deltaTime / time);
			else
				this.gameObject.SetActive (false);
		}
	}
}
