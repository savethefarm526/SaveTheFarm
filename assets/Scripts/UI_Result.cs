using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Result : UI_Base {
	public Text result;
    private int TITLE_SIZE = 90;
    private int BUTTON_FONT_SIZE = 50;
    private int BUTTON_WIDTH = 150;
    private int BUTTON_HEIGHT = 70; 
    private bool isDefMode, win;
    GameObject mBtn_Back,  mBtn_Next;
	public Button[] skills = new Button[5];
	public Image[] attackers=new Image[5];
	public Image image_Win, image_Lose, star, extra, unlock, sorry;

	public static bool fadeIn=true,fadeOut=false;
//	public int choice;

	void Update(){
		if (fadeOut) {
			if (this.GetComponent<CanvasGroup> ().alpha > 0) {
				this.GetComponent<CanvasGroup> ().alpha -= 0.05f;
			} else {
				fadeOut = false;
			}
		}
	}

    public void init(bool isDefMode, bool win){
        this.isDefMode = isDefMode;
        this.win = win;
        
		for (int i = 0; i < 5; i++) {
			skills [i].gameObject.SetActive (false);
		}
		for (int i = 0; i < 4; i++) {
			attackers [i].gameObject.SetActive (false);
		}
		image_Win.gameObject.SetActive (false);
		image_Lose.gameObject.SetActive (false);
		star.gameObject.SetActive (false);
		extra.gameObject.SetActive (false);
		unlock.gameObject.SetActive (false);
		sorry.gameObject.SetActive (false);

        if (isDefMode && win)
        {
			image_Win.gameObject.SetActive (true);

			extra.gameObject.SetActive (true);
			if (Battle_Manager.scores [Battle_Manager.ui_Battle.level * 2 - 2] == 1) {
				extra.sprite = Resources.Load<Sprite> ("Texture/ui/extr1");
				attackers [0].gameObject.SetActive (true);
			}
			if (Battle_Manager.scores [Battle_Manager.ui_Battle.level * 2 - 2] == 2) {
				extra.sprite = Resources.Load<Sprite> ("Texture/ui/extr2");
				attackers [1].gameObject.SetActive (true);
			}
			if (Battle_Manager.scores [Battle_Manager.ui_Battle.level * 2 - 2] == 3) {
				extra.sprite = Resources.Load<Sprite> ("Texture/ui/extr3");
				attackers [2].gameObject.SetActive (true);
			}

			if (Battle_Manager.ui_Battle.level <= 3) {
				unlock.gameObject.SetActive (true);
				if (Battle_Manager.ui_Battle.level == 1) {
					unlock.sprite = Resources.Load<Sprite> ("Texture/ui/unlock_shield");
					skills [2].gameObject.SetActive (true);
				} else if (Battle_Manager.ui_Battle.level == 2) {
					unlock.sprite = Resources.Load<Sprite> ("Texture/ui/unlock_speedup");
					skills [3].gameObject.SetActive (true);
				} else if (Battle_Manager.ui_Battle.level == 3) {
					unlock.sprite = Resources.Load<Sprite> ("Texture/ui/unlock_teleport");
					skills [4].gameObject.SetActive (true);
				}
			} else {
				Vector3 tmp = unlock.transform.localPosition;
				unlock.transform.localPosition = new Vector3 (tmp.x, tmp.y - 100, tmp.z);
				tmp = attackers [0].GetComponentInParent<GridLayoutGroup> ().transform.localPosition;
				attackers [0].GetComponentInParent<GridLayoutGroup> ().transform.localPosition = new Vector3 (tmp.x, tmp.y - 120, tmp.z);
			}

            Audio_Manager.PlaySound("win");
            mBtn_Back.SetActive(false);
            mBtn_Next.SetActive(true);

            // Debug.Log(mBtn_Next.activeSelf);
            Battle_Manager.ui_Battle.def_item_list.SetActive(false);
        }
        else if (isDefMode && !win)
        {
			image_Lose.gameObject.SetActive (true);
			sorry.gameObject.SetActive (true);

            Audio_Manager.PlaySound("lose");
            mBtn_Back.SetActive(true);
            mBtn_Next.SetActive(false);
        }
        else if (!isDefMode && win)
        {
			image_Win.gameObject.SetActive (true);

			star.gameObject.SetActive (true);
			if (Battle_Manager.scores [Battle_Manager.ui_Battle.level * 2 - 1] == 1)
				star.sprite = Resources.Load<Sprite> ("Texture/ui/star1");
			else if (Battle_Manager.scores [Battle_Manager.ui_Battle.level * 2 - 1] == 2)
				star.sprite = Resources.Load<Sprite> ("Texture/ui/star2");
			else if (Battle_Manager.scores [Battle_Manager.ui_Battle.level * 2 - 1] == 3)
				star.sprite = Resources.Load<Sprite> ("Texture/ui/star3");

			if (Battle_Manager.ui_Battle.level <= 2) {
				unlock.gameObject.SetActive (true);
				if (Battle_Manager.ui_Battle.level == 1) {
					unlock.sprite = Resources.Load<Sprite> ("Texture/ui/unlock_block");
					skills [0].gameObject.SetActive (true);
				} else if (Battle_Manager.ui_Battle.level == 2) {
					unlock.sprite = Resources.Load<Sprite> ("Texture/ui/unlock_sun");
					skills [1].gameObject.SetActive (true);
				}
			} else {
				Vector3 tmp = star.transform.localPosition;
				star.transform.localPosition = new Vector3 (tmp.x, tmp.y - 60, tmp.z);
			}

            Audio_Manager.PlaySound("win");
            mBtn_Back.SetActive(true);
            mBtn_Next.SetActive(false);
        }
        else if (!isDefMode && !win)
        {
			image_Lose.gameObject.SetActive (true);
			sorry.gameObject.SetActive (true);
			Vector3 tmp = sorry.transform.localPosition;
			sorry.transform.localPosition = new Vector3 (tmp.x, tmp.y - 100, tmp.z);

			star.gameObject.SetActive (true);
			star.sprite = Resources.Load<Sprite> ("Texture/ui/star0");

            Audio_Manager.PlaySound("lose");
            mBtn_Back.SetActive(true);
            mBtn_Next.SetActive(false);
        }
    }
	public override void button_Click(string name,GameObject obj){
        Audio_Manager.PlaySound("btn");
        if (name.Equals ("back")) {
			UI_Battle.fadeOut = true;
			fadeOut = true;
			UI_Battle.choice = 2;
            Audio_Manager.PlayMusic("music_nobattle");
        }
        if (name.Equals("next"))
        {
            Battle_Manager.ui_Battle.start_attack_mode_s = true;
            UI_Manager.Exit(UI_Manager.mUI_List[UI_Manager.mUI_List.Count-1]);
        }
	}

	public override void node_Asset(string name,GameObject obj){
//		if (name.Equals ("Result"))
//        {
//            result = obj.GetComponent<Text>();
//            Text tx = result.GetComponentInChildren<Text>();
//            tx.fontSize = TITLE_SIZE;
//            RectTransform rectTransform = result.GetComponent<RectTransform>();
//            rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 5f);
//            rectTransform.localPosition = new Vector3(0, Screen.height / 4.5f, 0);
//        }
        if (name.Equals("BTN"))
        {
            GameObject mBtn = obj;
            mBtn.transform.localPosition = new Vector3(0, -Screen.height / 3f, 0);
        }
        if (name.Equals("back"))
        {
            mBtn_Back = obj;
//            Text tx = mBtn_Back.GetComponent<Button>().GetComponentInChildren<Text>();
//            tx.fontSize = BUTTON_FONT_SIZE;
            RectTransform rectTransform = mBtn_Back.GetComponent<Button>().GetComponent<RectTransform>();
//            rectTransform.sizeDelta = new Vector2(BUTTON_WIDTH, BUTTON_HEIGHT);
            rectTransform.localPosition = new Vector3(0, 0, 0);
        }
       /* if (name.Equals("restart"))
        {
            mBtn_Restart = obj;
            Text tx = mBtn_Restart.GetComponent<Button>().GetComponentInChildren<Text>();
            tx.text = "Restart";
            tx.fontSize = BUTTON_FONT_SIZE;
            RectTransform rectTransform = mBtn_Restart.GetComponent<Button>().GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(BUTTON_WIDTH, BUTTON_HEIGHT);
            rectTransform.localPosition = new Vector3(BUTTON_WIDTH * 0.6f, 0, 0);
        }*/
        if (name.Equals("next"))
        {
            mBtn_Next = obj;
//            Text tx = mBtn_Next.GetComponent<Button>().GetComponentInChildren<Text>();
//            tx.text = "Next";
//            tx.fontSize = BUTTON_FONT_SIZE;
            RectTransform rectTransform = mBtn_Next.GetComponent<Button>().GetComponent<RectTransform>();
//            rectTransform.sizeDelta = new Vector2(BUTTON_WIDTH, BUTTON_HEIGHT);
//            rectTransform.localPosition = new Vector3(0, 0, 0);
        }
		if (name.Equals ("block")) {
			skills [0] = obj.GetComponent<Button>();
		}
		if (name.Equals ("sun")) {
			skills [1] = obj.GetComponent<Button> ();
		}
		if (name.Equals ("shield")) {
			skills [2] = obj.GetComponent<Button> ();
		}
		if (name.Equals ("speed up")) {
			skills [3] = obj.GetComponent<Button> ();
		}
		if (name.Equals ("teleport")) {
			skills [4] = obj.GetComponent<Button> ();
		}
		if (name.Equals ("attacker1")) {
			attackers [0] = obj.GetComponent<Image> ();
		}
		if (name.Equals ("attacker2")) {
			attackers [1] = obj.GetComponent<Image> ();
		}
		if (name.Equals ("attacker3")) {
			attackers [2] = obj.GetComponent<Image> ();
		}
		if (name.Equals ("attacker4")) {
			attackers [3] = obj.GetComponent<Image> ();
		}
		if (name.Equals ("win")) {
			image_Win = obj.GetComponent<Image> ();
		}
		if (name.Equals ("lose")) {
			image_Lose = obj.GetComponent<Image> ();
		}
		if (name.Equals ("star")) {
			star = obj.GetComponent<Image> ();
		}
		if (name.Equals ("extra")) {
			extra = obj.GetComponent<Image> ();
		}
		if (name.Equals ("unlock")) {
			unlock = obj.GetComponent<Image> ();
		}
		if (name.Equals ("sorry")) {
			sorry = obj.GetComponent<Image> ();
		}
    }
}