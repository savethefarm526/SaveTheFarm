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

	public void init (Tower_Info info)
	{
		this.power = info.power;
		this.period = info.period;
		this.range = info.range;
		this.bullet = info.bullet;
        this.name = info.name;
		this.bullet_Spd = info.bullet_spd;
		this.animation = info.animation;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		time += Time.deltaTime;
		if (time >= period) {
			time = 0;
			if (enemy == null) {
				enemy = get_Enemy ();
				if (enemy != null) {
					this.transform.LookAt (enemy.transform);
					attack_Enemy (enemy);
				}
			} else if (Vector3.Distance (this.transform.localPosition, enemy.transform.localPosition) <= range) {
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
	public void attack_Enemy(Enemy enemy){
		//something about animation
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
		if (enemy == null)
			bullet_Obj.init (pos, bullet_Spd, power);
		else
			bullet_Obj.init (enemy, bullet_Spd, power);
		bullet_Obj.transform.localPosition = this.transform.localPosition;
	}
	public Enemy get_Enemy(){
		for (int i = 0; i < Battle_Manager.enemy_List.Count; i++) {
            if (Battle_Manager.enemy_List[i] == null) return null;
			if (Vector3.Distance (this.transform.localPosition, Battle_Manager.enemy_List [i].transform.localPosition) <= range)
				return Battle_Manager.enemy_List [i];
		}
		return null;
	}
}
