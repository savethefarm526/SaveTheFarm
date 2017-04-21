using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour{
	public const int STATUS_TIME = 1;
	public const float LOSE_HEALTH_CONTINUOUSLY = 0.02f;

	public List<Vector3> path=new List<Vector3>();
	public float health, max_Health;
	public float speed;
	public int Enemy_Money;
	public Transform health_Pos;
	public Health item_Health;

	public float total_Distance;
	public bool horizontal=true;

	public string enemyNo;

	public bool lose_health_skill_on = false;

	public bool stop = false, forward = false;
	public float period;
	public float time;
	public Tower target;

	public void init(List<Vector3> path,float health,float speed, int Enemy_Money,string model,float period){
		this.path.AddRange(path);
		this.transform.localPosition = this.path [0];
		if (path [1].x == path [0].x)
			horizontal = false;
		this.path.RemoveAt (0);
		max_Health = this.health = health;
		this.speed = speed;
		this.Enemy_Money = Enemy_Money;
		this.total_Distance = 0;
		//add animation
		health_Pos=this.transform.FindChild("hp_pos");

		this.period = period;
		time = period;

		enemyNo = "1";
		if (model.Equals ("tower4")||model.Equals("zombie3")||model.Equals("zombie4"))
			enemyNo = "2";

		Animator anim = this.GetComponent<Animator> ();
		anim.speed = 2f;
		if (Battle_Manager.ui_Battle.isDefenceMode) {
			anim.runtimeAnimatorController = Resources.Load ("Animation/enemyDefenseController" + enemyNo) as RuntimeAnimatorController;
		} else {
			anim.runtimeAnimatorController = Resources.Load ("Animation/towerAttackController" + enemyNo) as RuntimeAnimatorController;
		}
	}

	public void change_Health(float health, string name){
        Audio_Manager.PlaySound("hit");

        this.health += health;
		if(!name.Equals("skill_hurt"))
			this.GetComponent<Animator> ().SetTrigger ("hit");
		if (this.health <= 0) {
			if(!name.Equals("skill_hurt"))
				this.GetComponent<Animator> ().SetTrigger ("die");
			Destroy (this.gameObject, 1f);
			if (Battle_Manager.ui_Battle.isDefenceMode) {
				Battle_Manager.money += Enemy_Money;
			}else {
				Battle_Manager.money += (int)total_Distance;
				Battle_Manager.attacker_Left--;
				//Debug.Log(Battle_Manager.attacker_Left);
			}
			Battle_Manager.ui_Battle.money_Effect (0);
		} else {
			if(!name.Equals("skill_hurt"))
				this.GetComponent<Animator> ().SetTrigger ("walk");
		}
		item_Health.health_Bar.size = item_Health.enemy.health * 1f / item_Health.enemy.max_Health;

		StartCoroutine (show_Status (name));
	}

	public IEnumerator show_Status(string name){
		if (name.Equals ("bullet2")&&!this.item_Health.status[0].gameObject.activeSelf) {
			this.item_Health.status [0].gameObject.SetActive (true);
			this.speed /= 2;
			yield return new WaitForSeconds (STATUS_TIME);
			this.speed *= 2;
			this.item_Health.status [0].gameObject.SetActive (false);
		}
		if (name.Equals ("bullet4")&&!this.item_Health.status[1].gameObject.activeSelf) {
			this.item_Health.status [1].gameObject.SetActive (true);
			lose_health_skill_on = true;
			yield return new WaitForSeconds(STATUS_TIME);
			lose_health_skill_on = false;
			this.item_Health.status [1].gameObject.SetActive (false);
		}
		if (name.Equals ("power_up")&&!this.item_Health.status[2].gameObject.activeSelf) {
			this.item_Health.status [2].gameObject.SetActive (true);
			yield return new WaitForSeconds (STATUS_TIME);
			this.item_Health.status [2].gameObject.SetActive (false);
		}
		if (name.Equals ("speed_up")&&!this.item_Health.status[3].gameObject.activeSelf) {
			this.item_Health.status [3].gameObject.SetActive (true);
			yield return new WaitForSeconds (STATUS_TIME);
			this.item_Health.status [3].gameObject.SetActive (false);
		}
	}

	void OnTriggerEnter(Collider other){
//		other.gameObject.SetActive (false);
		change_Health (-3, "bullet3");
	}

	public Tower find_Tower_Target(){
		for (int i = 0; i < Battle_Manager.tower_List.Count; i++) {
			if (Battle_Manager.tower_List[i]!=null && Vector3.Distance (Battle_Manager.tower_List [i].transform.localPosition, this.transform.localPosition) < 1.1f)
				return Battle_Manager.tower_List [i];
		}
		return null;
	}

	// Update is called once per frame
	public void mUpdate () {
		if (lose_health_skill_on) {
			this.change_Health (-LOSE_HEALTH_CONTINUOUSLY, "skill_hurt");
		}

		if (target != null) {
			if (stop == true) {
				time += Time.deltaTime;
				if (time >= period) {
					time = 0;
					this.GetComponent<Animator> ().SetTrigger ("attack");
					Audio_Manager.PlaySound ("base_hit");
					target.change_Health ();
				}
				return;
			}
		} else
			stop = false;
		if(forward==false && stop==false){
			if ((target = find_Tower_Target ()) != null) {
				float rand = Random.value;
				if (rand >= 0.7f) {
					stop = true;
				} else
					forward = true;
			} else {
				stop = false;
				forward = false;
			}
		}

		if (path.Count > 0) {
			for (int i = 0; i < Battle_Manager.ui_Battle.blocks.Count; i++) {
				if (Battle_Manager.ui_Battle.blocks [i] != null && Vector3.Distance (this.transform.localPosition, Battle_Manager.ui_Battle.blocks [i].transform.localPosition) < 0.1f)
					return;
//				if (Battle_Manager.ui_Battle.blocks [i] != null) {
//					if (horizontal && Mathf.Abs (this.transform.localPosition.x - Battle_Manager.ui_Battle.blocks [i].transform.localPosition.x) < 0.1f)
//						return;
//					if (!horizontal && Mathf.Abs (this.transform.localPosition.z - Battle_Manager.ui_Battle.blocks [i].transform.localPosition.z) < 0.1f)
//						return;
//				}
			}
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
			if (Time.deltaTime < time) {
//				if (horizontal) {
//					pos.z += Random.value - 0.5f;
//					Debug.Log ("initial: " + this.transform.localPosition);
//					this.transform.localPosition = Vector3.Lerp (this.transform.localPosition, pos, Time.deltaTime / time);
//					Debug.Log ("position: " + this.transform.localPosition);
//					Debug.Log ("pos: " + pos);

//					float tmp_x = (Mathf.Lerp (this.transform.localPosition.x, pos.x, Time.deltaTime / time))*100;
//					float tmp_z = (this.transform.localPosition.z + (Random.value - 0.5f)/10f)*100;
//					Debug.Log ("tmp_x: " + tmp_x);
//					Debug.Log ("tmp_z: " + tmp_z);
//					Debug.Log("position: "+new Vector3(tmp_x,0,tmp_z)/100f);
//					this.transform.localPosition = new Vector3 (tmp_x, 0, tmp_z)/100f;
//				}else{
//					float tmp_z = Mathf.Lerp (this.transform.localPosition.z, pos.z, Time.deltaTime / time);
//					float tmp_x = this.transform.localPosition.x + (Random.value - 0.5f)/10f;
//					this.transform.localPosition = new Vector3 (tmp_x, 0, tmp_z);
//				}
//				Debug.Log ("position: " + this.transform.localPosition);
				this.transform.localPosition = Vector3.Lerp (this.transform.localPosition, pos, Time.deltaTime / time);
                
//				Debug.Log ("initial: " + this.transform.localPosition);
//				this.transform.localPosition = new Vector3 (this.transform.localPosition.x, 0, this.transform.localPosition.z + (Random.value - 0.5f)/10f);
//				Debug.Log ("final: " + this.transform.localPosition);
			} else {
				int i = 0;
				for (; i < Battle_Manager.ui_Battle.portals1.Count; i++) {
					if (Battle_Manager.ui_Battle.portals1 [i] != null && Vector3.Distance (pos, Battle_Manager.ui_Battle.portals1 [i].transform.localPosition) < 0.1f) {
						this.transform.localPosition = Battle_Manager.ui_Battle.portals2 [i].transform.localPosition;
						while (Vector3.Distance (path [0], Battle_Manager.ui_Battle.portals2 [i].transform.localPosition) > 0.1f)
							path.RemoveAt (0);
						break;
					}
				}
				if (i == Battle_Manager.ui_Battle.portals1.Count)
					this.transform.localPosition = pos;
				if (path.Count > 1) {
					if (path [0].x == path [1].x)
						horizontal = false;
					else
						horizontal = true;
				}
				//Debug.Log ("horizontal: " + horizontal);
				path.RemoveAt (0);

//				if (Battle_Manager.ui_Battle.portal_Finished && Vector3.Distance (pos, Battle_Manager.ui_Battle.Portal1.transform.localPosition) < 0.1f) {
//					this.transform.localPosition = Battle_Manager.ui_Battle.Portal2.transform.localPosition;
//					while (Vector3.Distance (path [0], Battle_Manager.ui_Battle.Portal2.transform.localPosition) > 0.1f)
//						path.RemoveAt (0);
//				} else {
//					this.transform.localPosition = pos;
//				}
//				path.RemoveAt (0);
			}
			total_Distance += Vector3.Distance (pre_Pos, this.transform.localPosition);
		} else {
			this.GetComponent<Animator> ().SetTrigger ("attack");
            Audio_Manager.PlaySound("base_hit");
            health = 0;
			Destroy (this.gameObject,0.6f);
			if (!Battle_Manager.ui_Battle.isDefenceMode) {
				Battle_Manager.money += (int)(total_Distance / Mathf.Sqrt(Battle_Manager.ui_Battle.level));
				Battle_Manager.ui_Battle.money_Effect (0);
			}
			Battle_Manager.enemy_Attack ();
		}
	}
}
