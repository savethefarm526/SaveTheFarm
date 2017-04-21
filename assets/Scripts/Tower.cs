using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
	public Enemy enemy;
	public int power;
	public float period;
	public float range;
	public string bullet;
	public float bullet_Spd;
	public string animation;
    public string name;
	private float time=0;

	public string model;
	public string towerNo;

	public float health, max_Health;
	public Transform health_Pos;
	public Health item_Health;

	public void init (Tower_Info info)
	{
		this.power = info.power;
		this.period = info.period;
		this.range = info.range;
		this.bullet = info.bullet;
        this.name = info.name;
		this.bullet_Spd = info.bullet_spd;
		this.animation = info.animation;

		this.model = info.model;

		this.health = this.max_Health = info.health;
		health_Pos=this.transform.FindChild("hp_pos");

		towerNo = "1";
		if (info.model.Equals ("tower4") || info.model.Equals ("tower4_2") || info.model.Equals ("tower4_3"))
			towerNo = "2";

		Animator anim = this.GetComponent<Animator> ();
		anim.speed = 2f;
		if (Battle_Manager.ui_Battle.isDefenceMode) {
			anim.runtimeAnimatorController = Resources.Load ("Animation/towerDefenseController" + towerNo) as RuntimeAnimatorController;
		} else {
			anim.runtimeAnimatorController = Resources.Load ("Animation/enemyAttackController" + towerNo) as RuntimeAnimatorController;
		}
	}

	public void change_Health(){
		this.health--;
		if (this.health <= 0) {
			Destroy (this.gameObject);
            if(Battle_Manager.ui_Battle.isDefenceMode)
            {
                Battle_Manager.ui_Battle.def_wave_count++;
                Battle_Manager.ui_Battle.defwave.text = Battle_Manager.ui_Battle.def_wave_count + "/" + Battle_Manager.ui_Battle.def_total_wave_count;
            }
		}
		item_Health.health_Bar.size = item_Health.tower.health * 1f / item_Health.tower.max_Health;
	}

	public void changeAnimator(){
		towerNo = "1";
		if (model.Equals ("zombie3")||model.Equals("zombie4"))
			towerNo = "2";
		this.GetComponent<Animator> ().runtimeAnimatorController = Resources.Load ("Animation/enemyAttackController" + towerNo) as RuntimeAnimatorController;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (UI_Func.pause)
			return;

        time += Time.deltaTime;
        if (Battle_Manager.ui_Battle.isDefenceMode)
        {
            if (time >= period)
            {
                time = 0;

				Enemy target;
				if ((target = get_Enemy_Target ()) != null && Vector3.Distance(this.transform.localPosition,target.transform.localPosition)<=range) {
					enemy = target;
					this.transform.LookAt (enemy.transform);
					attack_Enemy (enemy);
				} else {
					if (enemy == null) {
						enemy = get_Enemy ();
						if (enemy != null) {
							this.transform.LookAt (enemy.transform);
							attack_Enemy (enemy);
						}
					} else if (Vector3.Distance (this.transform.localPosition, enemy.transform.localPosition) <= range) {
						this.transform.LookAt (enemy.transform);
						attack_Enemy (enemy);
					} else {
						enemy = get_Enemy ();
						if (enemy != null) {
							this.transform.LookAt (enemy.transform);
							attack_Enemy (enemy);
						}
					}
				}
            }
        } else
        {
            if (time >= period)
            {
                time = 0;
                if (enemy == null)
                {
                    enemy = get_Attacker();
                    if (enemy != null)
                    {
                        this.transform.LookAt(enemy.transform);
                        attack_Enemy(enemy);
                    }
                }
                else if (Vector3.Distance(this.transform.localPosition, enemy.transform.localPosition) <= range)
                {
                    attack_Enemy(enemy);
                }
                else
                {
                    enemy = get_Attacker();
                    if (enemy != null)
                    {
                        this.transform.LookAt(enemy.transform);
                        attack_Enemy(enemy);
                    }
                }
            }
        }
		
		
	}
	public void attack_Enemy(Enemy enemy){
		this.GetComponent<Animator> ().speed = 1f;
		this.GetComponent<Animator> ().SetTrigger ("attack");
		this.GetComponent<Animator> ().speed = 2f;
		float time;
		if (period <= 1)
			time = 0.5f;
		else
			time = 0.5f / period;
		StartCoroutine(bullet_Emit(enemy,enemy.transform.localPosition,time));
	}

	public IEnumerator bullet_Emit(Enemy enemy,Vector3 pos,float time){
		yield return new WaitForSeconds (time);
		Bullet bullet_Obj = Bullet_Manager.create (bullet);
		Vector3 tmp = this.transform.localPosition;
		bullet_Obj.transform.localPosition = new Vector3 (tmp.x - 0.1f, 0.5f, tmp.z);
		if (enemy == null)
			bullet_Obj.init (pos, bullet_Spd, power, bullet);
		else
			bullet_Obj.init (enemy, bullet_Spd, power, bullet);
        GameObject parent_node = GameObject.Find("map_parent_node");
        bullet_Obj.transform.SetParent(parent_node.transform);


    }
	public Enemy get_Enemy(){
		for (int i = 0; i < Battle_Manager.enemy_List.Count; i++) {
            if (Battle_Manager.enemy_List[i] == null) return null;
			if (Vector3.Distance (this.transform.localPosition, Battle_Manager.enemy_List [i].transform.localPosition) <= range)
				return Battle_Manager.enemy_List [i];
		}
		return null;
	}
	
	public Enemy get_Enemy_Target(){
		for (int i = 0; i < Battle_Manager.enemy_List.Count; i++) {
			if (Battle_Manager.enemy_List[i]!=null && Battle_Manager.enemy_List [i].item_Health.pointing_Down.gameObject.activeSelf)
				return Battle_Manager.enemy_List [i];
		}
		return null;
	}

    public Enemy get_Attacker()
    {
        for (int i = 0; i < Battle_Manager.attacker_List.Count; i++)
        {
            if (Battle_Manager.attacker_List[i] == null) return null;
            if (Vector3.Distance(this.transform.localPosition, Battle_Manager.attacker_List[i].transform.localPosition) <= range)
                return Battle_Manager.attacker_List[i];
        }
        return null;
    }

}
