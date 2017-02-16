using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Result : UI_Base {
	public Text result;

	public void init(bool win){
		if (win) {
			result.text = "You win!";
		} else {
			result.text = "You lose!";
		}
	}
	public override void button_Click(string name,GameObject obj){
		if (name.Equals ("Btn_Back")) {
			UI_Manager.Exit_All ();
			UI_Manager.Enter<UI_Start> ();
			Battle_Manager.clear ();
		}
	}
	public override void node_Asset(string name,GameObject obj){
		if (name.Equals ("Result"))
			result = obj.GetComponent<Text> ();
	}
}