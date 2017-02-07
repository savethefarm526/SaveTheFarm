using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Select : UI_Base {
	public override void button_Click(string name,GameObject obj){
		if (name.Equals ("Btn_Back")) {
			UI_Manager.Exit (this);
			UI_Manager.Enter<UI_Start> ();
		} else {
			UI_Manager.Exit (this);
			init_Battle (name.Split (' ') [1]);
		}
	}
	public void init_Battle(string level){
		List<Tower_Info> towers = new List<Tower_Info> ();
		Debug.Log (level);
		//...
		UI_Manager.Enter<UI_Battle>().init(level,towers);
	}
}
