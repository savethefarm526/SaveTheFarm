using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : UI_Base {
	public Scrollbar health_Bar;
	public Enemy enemy;

	public void init(Enemy enemy){
		this.enemy = enemy;
		enemy.item_Health = this;
		this.health_Bar = this.transform.FindChild ("bar_hp").GetComponent<Scrollbar> ();
		health_Bar.size = this.enemy.health * 1f / this.enemy.max_Health;
		update_Pos ();
	}
	public void update_Pos(){
		Vector3 pos = enemy.health_Pos.position;
		pos = Camera.main.WorldToScreenPoint (pos);
		pos.x -= Screen.width / 2;
		pos.y -= Screen.height / 2;
//		pos = pos * 598 / Screen.height;
		this.transform.localPosition = pos;
	}

	// Update is called once per frame
	void LateUpdate () {
		if (enemy != null)
			update_Pos ();
		else
			Destroy (this.gameObject);
	}
}
