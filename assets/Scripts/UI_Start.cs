using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Start : UI_Base {
	public Text mTitle;
	public Button mBtn_Start,mBtn_Quit;

	public override void node_Asset(string name,GameObject obj){
		if (name.Equals ("Title"))
			this.mTitle = obj.GetComponent<Text> ();
		if (name.Equals ("Btn_Start"))
			this.mBtn_Start = obj.GetComponent<Button> ();
		if (name.Equals ("Btn_Quit"))
			this.mBtn_Quit = obj.GetComponent<Button> ();
	}
	public override void button_Click(string name,GameObject obj){
		if (name.Equals ("Btn_Start")) {
			UI_Manager.Exit (this);
			UI_Manager.Enter<UI_Select> ();
		}
	}
}
