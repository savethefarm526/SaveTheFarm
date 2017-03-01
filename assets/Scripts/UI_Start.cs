using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Start : UI_Base {
	public Text mTitle;
    // public Button mBtn_Defense, mBtn_Attack;
    public Button  mBtn_Quit;
    public Button mBtn_Start;
    private int TITLE_SIZE = 70;
    private int BUTTON_SIZE = 50;
    public override void node_Asset(string name,GameObject obj){
		if (name.Equals ("Title"))
        {
            this.mTitle = obj.GetComponent<Text>();
            Text tx = mTitle.GetComponentInChildren<Text>();
            tx.fontSize = TITLE_SIZE;
            RectTransform rectTransform  = mTitle.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 5.5f);
            rectTransform.localPosition = new Vector3(0, Screen.height / 4f, 0);
        }
			
        /*if (name.Equals ("Btn_Defense"))
			this.mBtn_Defense = obj.GetComponent<Button> ();
        
		if (name.Equals ("Btn_Attack"))
			this.mBtn_Attack = obj.GetComponent<Button> ();*/

        if (name.Equals("Btn_Start"))
        {
            this.mBtn_Start = obj.GetComponent<Button>();
            Text tx = mBtn_Start.GetComponentInChildren<Text>();
            tx.fontSize = BUTTON_SIZE;
            RectTransform rectTransform = mBtn_Start.GetComponent<RectTransform> ();
            rectTransform.sizeDelta = new Vector2(Screen.width / 5f, Screen.height / 6f);
            rectTransform.localPosition = new Vector3(-Screen.width/5f, -Screen.height/7f, 0);
        }
            
        if (name.Equals("Btn_Quit"))
        {
            this.mBtn_Quit = obj.GetComponent<Button>();
            Text tx = mBtn_Quit.GetComponentInChildren<Text>();
            tx.fontSize = BUTTON_SIZE;
            RectTransform rectTransform = mBtn_Quit.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(Screen.width / 5f, Screen.height/ 6f);
            rectTransform.localPosition = new Vector3(Screen.width / 5f, -Screen.height / 7f, 0);
        }
			
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
        {
            //UI_Manager.Exit(this);
            Application.Quit();
        }
			
	}
}
