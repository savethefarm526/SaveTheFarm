using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Start : UI_Base {
	public Text mTitle;
    // public Button mBtn_Defense, mBtn_Attack;
    public Button  mBtn_Quit;
    public Button mBtn_Start;
    public override void node_Asset(string name,GameObject obj){
		if (name.Equals ("Title"))
			this.mTitle = obj.GetComponent<Text> ();
        /*if (name.Equals ("Btn_Defense"))
			this.mBtn_Defense = obj.GetComponent<Button> ();
        
		if (name.Equals ("Btn_Attack"))
			this.mBtn_Attack = obj.GetComponent<Button> ();*/

        if (name.Equals("Btn_Start"))
            this.mBtn_Start = obj.GetComponent<Button>();
        if (name.Equals ("Btn_Quit"))
			this.mBtn_Quit = obj.GetComponent<Button> ();
	}
	public override void button_Click(string name,GameObject obj){
		/*if (name.Equals ("Btn_Defense")) {
			Map_Manager.defense = true;
			UI_Manager.Exit (this);
			UI_Manager.Enter<UI_Select>().init(true);
		}
		if (name.Equals ("Btn_Attack")) {
			Map_Manager.defense = false;
			UI_Manager.Exit (this);
			UI_Manager.Enter<UI_Select>().init(false);
		}*/

        if (name.Equals("Btn_Start"))
        {
            Map_Manager.defense = true;
            UI_Manager.Exit(this);
            UI_Manager.Enter<UI_Select>().init(true);
        }

            
		if (name.Equals ("Btn_Quit"))
			UI_Manager.Exit (this);
	}
}
