using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_Select : UI_Base {
	public Text[] levels=new Text[15];
    public Text mTitle;
    public Button mBtn_Back;
    public Image mLevel_Ground;
    public GameObject mStar_ground;
    private int TITLE_SIZE = 90;
    private int BUTTON_SIZE = 50;
    private bool isDefence = true;
    public void init(bool isDefence)
    {
        this.isDefence = isDefence;
    }


	public override void button_Click(string name,GameObject obj){
        
		if (name.Equals ("Btn_Back")) {
            
            UI_Manager.Exit (this);
			UI_Manager.Enter<UI_Start> ();
		} else {
			UI_Manager.Exit (this);
			init_Battle (name.Split (' ') [1]);
		}
	}
	public override void node_Asset(string name,GameObject obj){
        if (name.Equals("Title"))
        {
            this.mTitle = obj.GetComponent<Text>();
            Text tx = mTitle.GetComponentInChildren<Text>();
            tx.fontSize = TITLE_SIZE;
            RectTransform rectTransform = mTitle.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height / 5.5f);
            rectTransform.localPosition = new Vector3(0, Screen.height / 3f, 0);
        }
        if (name.Equals("Btn_Back"))
        {
            this.mBtn_Back = obj.GetComponent<Button>();
            Text tx = mBtn_Back.GetComponentInChildren<Text>();
            tx.fontSize = BUTTON_SIZE;
            RectTransform rectTransform = mBtn_Back.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(Screen.width / 7f, Screen.height / 8f);
            rectTransform.localPosition = new Vector3(-Screen.width / 3f, Screen.height / 3f, 0);
        }

        if (name.Equals("Levels"))
        {
            this.mLevel_Ground = obj.GetComponent<Image>();
            
            RectTransform rectTransform = mLevel_Ground.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(2f*Screen.width / 2.5f, 4.5f*Screen.height / 7f);
            rectTransform.localPosition = new Vector3(0, -Screen.height / 7f, 0);

            GridLayoutGroup gridLayoutGroup = mLevel_Ground.GetComponent<GridLayoutGroup>();
            gridLayoutGroup.cellSize = new Vector2(rectTransform.sizeDelta.x/7f,3f*rectTransform.sizeDelta.y/13f);
            gridLayoutGroup.spacing = new Vector2(rectTransform.sizeDelta.x / 21f, rectTransform.sizeDelta.y / 13f);
        }
        /*if (name.Equals("Stars"))
        {
            this.mStar_ground = obj.GetComponent<GameObject>();
            RectTransform rectTransform = mStar_ground.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(2f * Screen.width / 2.7f, 3f * Screen.height / 4f);
            rectTransform.localPosition = new Vector3(0, -Screen.height / 7f, 0);
        }*/

        if (name.Split (' ')[0].Equals("Star")) {
			int level = int.Parse (name.Split (' ') [1]);
			levels [level-1] = obj.GetComponent<Text> ();
			levels [level-1].text = "Star: " + Battle_Manager.scores [level-1] + "/3";
		}
        if (name.Split(' ')[0].Equals("level"))
        {
            int level = int.Parse(name.Split(' ')[1]);
            if (level > 1 && Battle_Manager.scores[level - 2] == 0)
            {
                obj.GetComponent<Button>().interactable = false;
            }
                
        }
	}
	public void init_Battle(string level){
        int lv = int.Parse(level);
        Battle_Manager.cur_Level = lv;
        
        List<Tower_Info> towers = new List<Tower_Info>();
        List<Enemy_Info> enemies = new List<Enemy_Info>();

        /* 
        // power, period, range, bullet_spd
        towers.Add(new Tower_Info("tower1", "tower1", 1, 0.8f, 2, "", "bullet", 10, 15));
        towers.Add(new Tower_Info("tower2", "tower2", 2, 0.9f, 2, "", "bullet", 10, 18));
        towers.Add(new Tower_Info("tower3", "tower3", 1, 0.5f, 2, "", "bullet", 10, 18));
        towers.Add(new Tower_Info("tower4", "tower4", 2, 0.8f, 5, "", "bullet", 10, 23));

        towers.Add(new Tower_Info("tower1_2", "tower1_2", 2, 0.8f, 2, "", "bullet", 10, 18));
        towers.Add(new Tower_Info("tower2_2", "tower2_2", 3, 0.9f, 2, "", "bullet", 10, 23));
        towers.Add(new Tower_Info("tower3_2", "tower3_2", 2, 0.5f, 3, "", "bullet", 10, 23));
        towers.Add(new Tower_Info("tower4_2", "tower4_2", 3, 0.8f, 5, "", "bullet", 10, 28));

        towers.Add(new Tower_Info("tower1_3", "tower1_3", 3, 0.8f, 2, "", "bullet", 10, 20));
        towers.Add(new Tower_Info("tower2_3", "tower2_3", 4, 0.9f, 2, "", "bullet", 10, 28));
        towers.Add(new Tower_Info("tower3_3", "tower3_3", 3, 0.5f, 3, "", "bullet", 10, 28));
        towers.Add(new Tower_Info("tower4_3", "tower4_3", 4, 0.5f, 5, "", "bullet", 10, 40));

        enemies.Add(new Enemy_Info("enemy1", 2, 5, 10, 5));
        enemies.Add(new Enemy_Info("enemy2", 5, 2, 10, 6));
        enemies.Add(new Enemy_Info("enemy3", 10, 1, 5, 7));
        enemies.Add(new Enemy_Info("enemy4", 20, 1, 3, 10));
        */


        int star1 = lv > 1 ? Battle_Manager.scores[lv - 2] : 0;
        int star2 = star1 / 2;
        int star3 = star1 / 3;
        switch (level)
        {
		    case "1":
                setTowers(towers, 1, 0, 0, 0);
                break;
            case "2":
                setEnemies(lv, enemies, 2 + star3, 0, 0, 0);
                break;
            case "3":
                setTowers(towers, 1, 1, star3, 0);
                break;
            case "4":
                setEnemies(lv, enemies, 2 + star3, 1, 0, 0);
                break;
            case "5":
                setTowers(towers, 1, 1, 1, star3);
                break;
            case "6":
                setEnemies(lv, enemies, 2, 1 + star3, 1, 0);
                break;
            case "7":
                setTowers(towers, star3, 1, 1, star3);
                break;
            case "8":
                setEnemies(lv, enemies, 1 + star3, 1, 1, 1);
                break;
            case "9":
                setTowers(towers, 1, star3, star3, 1);
                break;
            case "10":
                setEnemies(lv, enemies, 1 + star3, 0, 0, 1);
                break;
            case "11":
                setTowers(towers, 1, star3, 1, star3);
                break;
            case "12":
                setEnemies(lv, enemies, 2, star3, 1, 2);
                break;
            case "13":
                setTowers(towers, star3, 1, star3, star3);
                break;
            case "14":
                setEnemies(lv, enemies, 2 + star3, star2, 2, 1);
                break;
            case "15":
                setTowers(towers, star3, star3, star3, star3);
                break;
        }

        

        

        if (lv % 2 == 1)
        {
            isDefence = true;
            setEnemies(lv, enemies, 1, 1,1,1);
        }
            
        else isDefence = false;

        
        UI_Manager.Enter<UI_Battle>().init(level, towers, enemies, isDefence);
        
		
	}


    public void setTowers(List<Tower_Info> towers, int num1, int num2, int num3, int num4)
    {
        if (num1 > 0) towers.Add(new Tower_Info("tower1", "tower1", 1, 0.8f, 2, "", "bullet", 10, 15));
        if (num2 > 0) towers.Add(new Tower_Info("tower2", "tower2", 2, 0.8f, 2, "", "bullet", 10, 18));
        if (num3 > 0) towers.Add(new Tower_Info("tower3", "tower3", 3, 0.8f, 3, "", "bullet", 10, 20));
        if (num4 > 0) towers.Add(new Tower_Info("tower4", "tower4", 3, 0.8f, 5, "", "bullet", 10, 23));
    }

    public void setEnemies(int level, List<Enemy_Info> enemies, int num1, int num2, int num3, int num4)
    {   if (level%2 == 1)
        {
            for (int i = 0; i < num1; i++) enemies.Add(new Enemy_Info("enemy1", 2, 5, 10, 5));
            for (int i = 0; i < num2; i++) enemies.Add(new Enemy_Info("enemy2", 5, 2, 10, 6));
            for (int i = 0; i < num3; i++) enemies.Add(new Enemy_Info("enemy3", 10, 1, 5, 7));
            for (int i = 0; i < num4; i++) enemies.Add(new Enemy_Info("enemy4", 20, 1, 3, 10));
        }
        else
        {
            for (int i = 0; i < num1; i++) enemies.Add(new Enemy_Info("tower1", 2, 5, 10, 5));
            for (int i = 0; i < num2; i++) enemies.Add(new Enemy_Info("tower2", 5, 2, 10, 6));
            for (int i = 0; i < num3; i++) enemies.Add(new Enemy_Info("tower3", 10, 1, 5, 7));
            for (int i = 0; i < num4; i++) enemies.Add(new Enemy_Info("tower4", 20, 1, 3, 10));
        }
    }
}
