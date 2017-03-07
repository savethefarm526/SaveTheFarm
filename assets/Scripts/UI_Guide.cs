using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Guide : UI_Base {
	public string map;
	public List<Tower_Info> towers=new List<Tower_Info>();
	public List<Enemy_Info> enemies=new List<Enemy_Info>();
	public bool isDefenseMode=true;
    public int star = 0;
	public RawImage defense,attack;

	public void init(string map,List<Tower_Info> towers,List<Enemy_Info> enemies,bool isDefenseMode, int star){
		this.map = map;
		this.towers = towers;
		this.enemies = enemies;
		this.isDefenseMode = isDefenseMode;
        this.star = star;

		if (isDefenseMode) {
			defense.gameObject.SetActive (true);
			attack.gameObject.SetActive (false);
		} else {
			defense.gameObject.SetActive (false);
			attack.gameObject.SetActive (true);
		}
	}

	public override void node_Asset(string name,GameObject obj){
		if (name.Equals ("Defense"))
        {
            defense = obj.GetComponent<RawImage>();
            RectTransform rectTransform = defense.rectTransform;
            rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        }
			
		if (name.Equals ("Attack"))
        {
            attack = obj.GetComponent<RawImage>();
            RectTransform rectTransform = attack.rectTransform;
            rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        }
			
	}

	void Update(){
		if (Input.touchCount > 0 || Input.GetMouseButton (0)) {
			UI_Manager.Exit (this);
            
			UI_Manager.Enter<UI_Battle> ().init (map, towers, enemies, isDefenseMode, star);
		}
	}
}
