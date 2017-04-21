using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Health : UI_Base {
	public Scrollbar health_Bar;
	public Enemy enemy = null;
	public Tower tower = null;

	public List<Image> status=new List<Image>();
	public Image pointing_Down;

	public void init(Enemy enemy){
		this.enemy = enemy;
		enemy.item_Health = this;
		this.health_Bar = this.transform.FindChild ("bar_hp").GetComponent<Scrollbar> ();
		health_Bar.size = this.enemy.health * 1f / this.enemy.max_Health;

		GridLayoutGroup grid = this.transform.FindChild ("imgs").GetComponent<GridLayoutGroup> ();
		status.Add(grid.transform.FindChild("speed_down").GetComponent<Image>());
		status.Add(grid.transform.FindChild("lose_health").GetComponent<Image>());
		status.Add(grid.transform.FindChild("power_up").GetComponent<Image>());
		status.Add(grid.transform.FindChild("speed_up").GetComponent<Image>());
		for (int i = 0; i < status.Count; i++)
			status [i].gameObject.SetActive (false);
		
		pointing_Down = this.transform.FindChild ("locked").GetComponent<Image> ();
		pointing_Down.gameObject.SetActive (false);

		update_Pos ();
	}
	public void init(Tower tower){
		this.tower = tower;
		tower.item_Health = this;
		this.health_Bar = this.transform.FindChild ("bar_hp").GetComponent<Scrollbar> ();
		health_Bar.size = this.tower.health * 1f / this.tower.max_Health;

		this.transform.FindChild ("imgs").GetComponent<GridLayoutGroup> ().gameObject.SetActive (false);
		this.transform.FindChild ("locked").GetComponent<Image> ().gameObject.SetActive (false);

		update_Pos ();
	}
	public void update_Pos(){
		Vector3 pos;
		if (enemy != null)
			pos = enemy.health_Pos.position;
		else
			pos = tower.health_Pos.position;
		pos = Camera.main.WorldToScreenPoint (pos);
		pos.x -= Screen.width / 2;
		pos.y -= Screen.height / 2;
//		pos = pos * 598 / Screen.height;
		this.transform.localPosition = pos;
	}

	// Update is called once per frame
	void LateUpdate () {
		if (enemy != null || tower != null)
			update_Pos ();
		else
			Destroy (this.gameObject);
	}
}
