using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Start : UI_Base {
	public const float FADE_TIME = 0.02f;
	private const int TITLE_SIZE = 125;
	private const int BUTTON_SIZE = 50;

	public Text mTitle;
    public Button mBtn_Quit;
    public Button mBtn_Start;
	public Button mBtn_Func;
	public static bool fadeOut = false, fadeIn = true;
	public int choice;

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
        if (name.Equals("Btn_Start"))
        {
            this.mBtn_Start = obj.GetComponent<Button>();
            Text tx = mBtn_Start.GetComponentInChildren<Text>();
            tx.fontSize = BUTTON_SIZE;
            RectTransform rectTransform = mBtn_Start.GetComponent<RectTransform> ();
            rectTransform.sizeDelta = new Vector2(Screen.width / 5f, Screen.height / 6f);
            rectTransform.localPosition = new Vector3(-Screen.width/5f, -Screen.height/7f, 0);
        }
            
		if (name.Equals ("Btn_Func")) {
			this.mBtn_Func = obj.GetComponent<Button> ();
			Text tx = mBtn_Func.GetComponentInChildren<Text> ();
			tx.fontSize = BUTTON_SIZE;
			RectTransform rectTransform = mBtn_Func.GetComponent<RectTransform> ();
			rectTransform.sizeDelta = new Vector2 (Screen.width / 5f, Screen.height / 6f);
			rectTransform.localPosition = new Vector3 (0, -Screen.height/7f, 0);
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
        Audio_Manager.PlaySound("btn");
        if (name.Equals("Btn_Start"))
        {
			fadeOut = true;
			choice = 1;
        }

		if (name.Equals ("Btn_Func")) {
			fadeOut=true;
			choice = 2;
		}
            
		if (name.Equals ("Btn_Quit")) {
			fadeOut = true;
			choice = 3;
		}
	}
	void Update(){
		if (fadeIn) {
			if (this.GetComponent<CanvasGroup> ().alpha < 1) {
				this.GetComponent<CanvasGroup> ().alpha += 0.05f;
			} else {
				fadeIn = false;
			}
		}
		if (fadeOut) {
			if (this.GetComponent<CanvasGroup> ().alpha > 0) {
				this.GetComponent<CanvasGroup> ().alpha -= 0.05f;
			} else {
				fadeOut = false;
				if (choice == 1) {
					Map_Manager.defense = true;
					UI_Manager.Exit (this);
					UI_Manager.Enter<UI_Select> ().init();
					UI_Select.fadeIn = true;
				} else if (choice == 2) {
					GameObject start = this.gameObject;
					start.SetActive (false);
					GameObject map = GameObject.Find ("map_show");
					Vector3 tmp = map.transform.localPosition;
					tmp.y = 0.5f;
					map.transform.localPosition = tmp;
					map.GetComponent<Renderer> ().material.color = Color.black;
					UI_Manager.Enter<UI_Func> ().init(start);
					UI_Func.fadeIn = true;
//					UI_Manager.Enter<UI_Help> ().init (start);
//					UI_Help.fadeIn = true;
				} else if (choice == 3) {
					UI_Manager.Exit (this);
					Application.Quit ();
				}
			}
		}
	}
}
