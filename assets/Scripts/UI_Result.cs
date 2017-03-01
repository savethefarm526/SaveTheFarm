using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Result : UI_Base {
	public Text result;
    private int TITLE_SIZE = 90;
    private int BUTTON_SIZE = 50;

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
        {
            result = obj.GetComponent<Text>();
            Text tx = result.GetComponentInChildren<Text>();
            tx.fontSize = TITLE_SIZE;
            RectTransform rectTransform = result.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 5.5f);
            rectTransform.localPosition = new Vector3(0, Screen.height / 4.5f, 0);
        }
        if (name.Equals("Btn_Back"))
        {
            Button mBtn_Back = obj.GetComponent<Button>();
            Text tx = mBtn_Back.GetComponentInChildren<Text>();
            tx.fontSize = BUTTON_SIZE;
            RectTransform rectTransform = mBtn_Back.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(Screen.width / 7f, Screen.height / 8f);
            rectTransform.localPosition = new Vector3(0, -Screen.height / 7f, 0);
        }


    }
}