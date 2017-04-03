using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_Select : UI_Base {
	private const int TITLE_SIZE = 90;
	private const int BUTTON_SIZE = 50;

	public Text[] levels=new Text[8];
    public Text mTitle;
    public Button mBtn_Back;
    public Image mLevel_Ground;
    public GameObject mStar_ground;

	public static bool fadeIn=true,fadeOut=false;
	public int choice;
	private string name;

	public static int lv = 0;

	public override void button_Click(string name,GameObject obj){
        Audio_Manager.PlaySound("btn");
        if (name.Equals ("Btn_Back")) {
			fadeOut = true;
			choice = 1;
		} else {
			if (obj.GetComponent<Image> ().sprite.name == name.Split (' ') [1] + "_0")
				return;
			this.name = name;
			fadeOut = true;
			choice = 2;
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
					UI_Manager.Exit (this);
					UI_Manager.Enter<UI_Start> ();
					UI_Start.fadeIn = true;
				} else if (choice == 2) {
					UI_Manager.Exit (this);
                    Audio_Manager.PlayMusic("music_battle");
                    init_Battle (name.Split (' ') [1]);
				}
			}
		}
	}

	public override void node_Asset(string name,GameObject obj){
        if (name.Equals("Title"))
        {
            this.mTitle = obj.GetComponent<Text>();
            /*  Text tx = mTitle.GetComponentInChildren<Text>();
              tx.fontSize = TITLE_SIZE;
              RectTransform rectTransform = mTitle.GetComponent<RectTransform>();
              rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 5.5f);
              rectTransform.localPosition = new Vector3(0, Screen.height / 3f, 0);
              */
          }
          if (name.Equals("Btn_Back"))
          {
              this.mBtn_Back = obj.GetComponent<Button>();
              /*Text tx = mBtn_Back.GetComponentInChildren<Text>();
              tx.fontSize = BUTTON_SIZE;
              RectTransform rectTransform = mBtn_Back.GetComponent<RectTransform>();
              rectTransform.sizeDelta = new Vector2(Screen.width / 7f, Screen.height / 8f);
              rectTransform.localPosition = new Vector3(-Screen.width / 3f, Screen.height / 3f, 0);*/
        }

        if (name.Equals("Levels"))
        {
            this.mLevel_Ground = obj.GetComponent<Image>();
            
            RectTransform rectTransform = mLevel_Ground.GetComponent<RectTransform>();
//            rectTransform.sizeDelta = new Vector2(2f*Screen.width / 2.5f, 4.5f*Screen.height / 7f);
//            rectTransform.localPosition = new Vector3(0, -Screen.height / 7f, 0);

            GridLayoutGroup gridLayoutGroup = mLevel_Ground.GetComponent<GridLayoutGroup>();
//            gridLayoutGroup.cellSize = new Vector2(rectTransform.sizeDelta.x/7f,3f*rectTransform.sizeDelta.y/13f);
//            gridLayoutGroup.spacing = new Vector2(rectTransform.sizeDelta.x / 21f, rectTransform.sizeDelta.y / 13f);
        }

        if (name.Split (' ')[0].Equals("Star")) {
			int level = int.Parse (name.Split (' ') [1]);
			levels [level-1] = obj.GetComponent<Text> ();
			levels [level-1].text = "Star: " + Battle_Manager.scores [level*2-1] + "/3";
		}
        if (name.Split(' ')[0].Equals("level"))
        {
            int level = int.Parse(name.Split(' ')[1]);
			if (Battle_Manager.scores [level * 2 - 1] == 1)
				obj.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Texture/ui/level/" + level + "_1");
			else if(Battle_Manager.scores [level * 2 - 1] == 2)
				obj.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Texture/ui/level/" + level + "_2");
			else if(Battle_Manager.scores [level * 2 - 1] == 3)
				obj.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Texture/ui/level/" + level + "_3");
            if (level > 1 && Battle_Manager.scores[level*2 - 3] == 0)
            {
				obj.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Texture/ui/level/" + level + "_0");

            }
                
        }
	}
	public void init_Battle(string level){
        string name = "level_" + level + "_env";
        GameObject env = GameObject.Find("env");
        for (int i = 0; i < env.transform.childCount; i++)
        {
            env.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < env.transform.childCount; i++)
        {
            if (env.transform.GetChild(i).name.Equals(name))
            {
                env.transform.GetChild(i).gameObject.SetActive(true);
                break;
            }
        }


        lv = int.Parse(level);
        Battle_Manager.cur_Level = lv;
        int star = lv > 1 ? Battle_Manager.scores[lv*2 - 3] : 0;

        List<Tower_Info> towers = new List<Tower_Info>();
        List<Enemy_Info> enemies = new List<Enemy_Info>();
        List<Enemy_Info> attackers = new List<Enemy_Info>();
        
        switch (level)
        {
		    case "1":
				setTowers(towers, Battle_Manager.locked_towers[0]?1:0, Battle_Manager.locked_towers[1]?1:0, Battle_Manager.locked_towers[2]?1:0, Battle_Manager.locked_towers[3]?1:0);
                setEnemies(lv, enemies, 1, 0, 0, 0);
                setAttackers(attackers, 1, 0, 0, 0);
                break;
            case "2":
				setTowers(towers, Battle_Manager.locked_towers[0]?1:0, Battle_Manager.locked_towers[1]?1:0, Battle_Manager.locked_towers[2]?1:0, Battle_Manager.locked_towers[3]?1:0);
                setEnemies(lv, enemies, 1, 0, 0, 0);
                setAttackers(attackers, 1, 0, 0, 0);
                break;
            case "3":
				setTowers(towers, Battle_Manager.locked_towers[0]?1:0, Battle_Manager.locked_towers[1]?1:0, Battle_Manager.locked_towers[2]?1:0, Battle_Manager.locked_towers[3]?1:0);
				setEnemies(lv, enemies, 1, 0, 0, 0);
				setAttackers(attackers, 1, 0, 0, 0);
                break;
            case "4":
				setTowers(towers, Battle_Manager.locked_towers[0]?1:0, Battle_Manager.locked_towers[1]?1:0, Battle_Manager.locked_towers[2]?1:0, Battle_Manager.locked_towers[3]?1:0);
                setEnemies(lv, enemies, 1, 1, 1, 1);
                setAttackers(attackers, 2, 1, 1, 0);
                break;
            case "5":
				setTowers(towers, Battle_Manager.locked_towers[0]?1:0, Battle_Manager.locked_towers[1]?1:0, Battle_Manager.locked_towers[2]?1:0, Battle_Manager.locked_towers[3]?1:0);
                setEnemies(lv, enemies, 1, 1, 1, 1);
                setAttackers(attackers, 2, 0, 1, 1);
                break;
            case "6":
				setTowers(towers, Battle_Manager.locked_towers[0]?1:0, Battle_Manager.locked_towers[1]?1:0, Battle_Manager.locked_towers[2]?1:0, Battle_Manager.locked_towers[3]?1:0);
                setEnemies(lv, enemies, 1, 1, 1, 1);
                setAttackers(attackers, 2, 1, 0, 1);
                break;
            case "7":
				setTowers(towers, Battle_Manager.locked_towers[0]?1:0, Battle_Manager.locked_towers[1]?1:0, Battle_Manager.locked_towers[2]?1:0, Battle_Manager.locked_towers[3]?1:0);
                setEnemies(lv, enemies, 1, 1, 1, 1);
                setAttackers(attackers, 2, 2, 1, 1);
                break;
            case "8":
				setTowers(towers, Battle_Manager.locked_towers[0]?1:0, Battle_Manager.locked_towers[1]?1:0, Battle_Manager.locked_towers[2]?1:0, Battle_Manager.locked_towers[3]?1:0);
                setEnemies(lv, enemies, 1, 1, 1, 1);
                setAttackers(attackers, 2, 1, 2, 1);
                break;
        }

		if (lv == 1)
            UI_Manager.Enter<UI_Battle>().init(level, towers, enemies, attackers, 3);
        else
            UI_Manager.Enter<UI_Battle>().init(level, towers, enemies, attackers, star);
		
		UI_Battle.fadeIn = true;
    }


    public void setTowers(List<Tower_Info> towers, int num1, int num2, int num3, int num4)
    {
        if (num1 > 0) towers.Add(new Tower_Info("tower1", "tower1", 1, 0.8f, 2, "", "bullet1", 1, 15));
        if (num2 > 0) towers.Add(new Tower_Info("tower2", "tower2", 2, 0.8f, 2, "", "bullet2", 1, 18));
        if (num3 > 0) towers.Add(new Tower_Info("tower3", "tower3", 3, 0.8f, 3, "", "bullet3", 5, 20));
        if (num4 > 0) towers.Add(new Tower_Info("tower4", "tower4", 3, 0.8f, 5, "", "bullet4", 1, 23));
    }

    public void setEnemies(int level, List<Enemy_Info> enemies, int num1, int num2, int num3, int num4)
    {
        for (int i = 0; i < num1; i++) enemies.Add(new Enemy_Info("zombie1", 2 * Mathf.Sqrt(level), 0.5f, 10, 5));
        for (int i = 0; i < num2; i++) enemies.Add(new Enemy_Info("zombie2", 5 * Mathf.Sqrt(level), 0.2f, 10, 6));
        for (int i = 0; i < num3; i++) enemies.Add(new Enemy_Info("zombie3", 10 * Mathf.Sqrt(level), 0.1f, 5, 7));
        for (int i = 0; i < num4; i++) enemies.Add(new Enemy_Info("zombie4", 20 * Mathf.Sqrt(level), 0.1f, 3, 10));
        
    }
    public void setAttackers( List<Enemy_Info> enemies, int num1, int num2, int num3, int num4)
    {
        for (int i = 0; i < num1; i++) enemies.Add(new Enemy_Info("tower1", 2, 0.5f, 10, 5));
        for (int i = 0; i < num2; i++) enemies.Add(new Enemy_Info("tower2", 5, 0.2f, 10, 6));
        for (int i = 0; i < num3; i++) enemies.Add(new Enemy_Info("tower3", 10, 0.1f, 5, 7));
        for (int i = 0; i < num4; i++) enemies.Add(new Enemy_Info("tower4", 20, 0.1f, 3, 10));
    }
}
