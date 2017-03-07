using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour{
	public List<Vector3> path=new List<Vector3>();
	public float health, max_Health;
	public float speed;
	public int Enemy_Money;
	public Transform health_Pos;
	public Health item_Health;

	public float total_Distance;

	public void init(List<Vector3> path,float health,float speed, int Enemy_Money){
		this.path.AddRange(path);
		this.transform.localPosition = this.path [0];
		this.path.RemoveAt (0);
		max_Health = this.health = health;
		this.speed = speed;
		this.Enemy_Money = Enemy_Money;
		this.total_Distance = 0;
		//add animation
		health_Pos=this.transform.FindChild("hp_pos");
	}
	public void change_Health(int health){
		this.health += health;
		if (this.health <= 0) {
			Destroy (this.gameObject);
			if (Battle_Manager.ui_Battle.isDefenceMode)
				Battle_Manager.money += Enemy_Money;
			else {
				Battle_Manager.money += (int)total_Distance;
				Battle_Manager.enemy_Left--;
                //Debug.Log(Battle_Manager.enemy_Left);
			}
		}
		item_Health.health_Bar.size = item_Health.enemy.health * 1f / item_Health.enemy.max_Health;
	}
	
	// Update is called once per frame
	public void mUpdate () {
		if (path.Count > 0) {
			Vector3 pos = path [0];
			float distance;
			if (this.transform.localPosition.z == pos.z) {
				distance = Mathf.Abs (this.transform.localPosition.x - pos.x);
				if (this.transform.localPosition.x < pos.x)
					this.transform.localEulerAngles = new Vector3 (0, 90, 0);
				else
					this.transform.localEulerAngles = new Vector3 (0, 270, 0);
			} else {
				distance = Mathf.Abs (this.transform.localPosition.z - pos.z);
				if (this.transform.localPosition.z < pos.z)
					this.transform.localEulerAngles = new Vector3 (0, 0, 0);
				else
					this.transform.localEulerAngles = new Vector3 (0, 180, 0);
			}
			float time = distance / speed;
			Vector3 pre_Pos = this.transform.localPosition;
			if (Time.deltaTime < time)
				this.transform.localPosition = Vector3.Lerp (this.transform.localPosition, pos, Time.deltaTime / time);
			else {
				if (Battle_Manager.ui_Battle.portal_Finished && Vector3.Distance (pos, Battle_Manager.ui_Battle.Portal1.transform.localPosition) < 0.1f) {
					this.transform.localPosition = Battle_Manager.ui_Battle.Portal2.transform.localPosition;
					while (Vector3.Distance (path [0], Battle_Manager.ui_Battle.Portal2.transform.localPosition) > 0.1f)
						path.RemoveAt (0);
				} else {
					this.transform.localPosition = pos;
				}
				path.RemoveAt (0);
			}
			total_Distance += Vector3.Distance (pre_Pos, this.transform.localPosition);
		} else {
			health = 0;
			Destroy (this.gameObject);
			Battle_Manager.enemy_Attack ();
		}
	}
}
