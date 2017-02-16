using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public Enemy enemy;
	public float speed;
	public int power;
	public Vector3 enemy_Pos;
	private float time;

	public void init(Enemy enemy,float speed,int power){
		this.enemy = enemy;
		this.speed = speed;
		this.power = power;
		this.enemy_Pos = enemy.transform.localPosition;
	}
	public void init(Vector3 pos,float speed,int power){
		this.enemy = null;
		this.speed = speed;
		this.power = power;
		this.enemy_Pos = pos;
	}

	// Update is called once per frame
	void Update () {
		if (enemy != null) {
			enemy_Pos = enemy.transform.localPosition;
			if (speed > 0)
				time = Vector3.Distance (this.transform.localPosition, enemy_Pos) / speed;
			else
				time = 0;
			if (time > Time.deltaTime)
				this.transform.localPosition = Vector3.Lerp (this.transform.localPosition, enemy_Pos, Time.deltaTime / time);
			else {
				enemy.change_Health (-power);
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
