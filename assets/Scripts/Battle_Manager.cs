using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Battle_Manager {
	public const int BASE_LIFE=10;

	public static UI_Battle ui_Battle;
	public static List<Vector3> path;
	public static List<Enemy_Info> enemies;
	public static List<Enemy> enemy_List=new List<Enemy>();
	public static List<Tower> tower_List=new List<Tower>();
	public static int cur_Wave, wave_Time, base_Life;
	private static float time;
	public static bool stop;

	public static int[] scores = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
	public static int cur_Level;

	public static int total_Enemy;
	public static int enemy_Left;

	public static int money;

    public static bool beforeCountDown = true;

    public static void init(UI_Battle ui_Battle,List<Vector3> path){
		Battle_Manager.ui_Battle = ui_Battle;
		Battle_Manager.path = path;
        enemies = ui_Battle.enemies;
		if (!ui_Battle.isDefenceMode && ui_Battle.tower_coor_list != null)
        {
            for (int j = 0; j < ui_Battle.tower_coor_list[0].Count; j++)
            {
                create_Tower(ui_Battle.tower_coor_list[0][j], new Tower_Info("tower1", "enemy1", 1, 0.8f, 2, "", "bullet", 10, 0));
            }
            for (int j = 0; j < ui_Battle.tower_coor_list[1].Count; j++)
            {
                create_Tower(ui_Battle.tower_coor_list[1][j], new Tower_Info("tower2", "enemy2", 2, 0.8f, 2, "", "bullet", 10, 0));
            }
            for (int j = 0; j < ui_Battle.tower_coor_list[2].Count; j++)
            {
                create_Tower(ui_Battle.tower_coor_list[2][j], new Tower_Info("tower3", "enemy3", 3, 0.8f, 3, "", "bullet", 10, 0));
            }
            for (int j = 0; j < ui_Battle.tower_coor_list[3].Count; j++)
            {
                create_Tower(ui_Battle.tower_coor_list[3][j], new Tower_Info("tower4", "enemy4", 3, 0.8f, 5, "", "bullet", 10, 0));
            }


        }

        cur_Wave = 0;
		base_Life = BASE_LIFE;
		time = 0;
		money = 100;

        for (int i = 0; i < enemy_List.Count; i++)
        {
            GameObject.Destroy(enemy_List[i].gameObject);
        }
        enemy_List.Clear();
        stop = false;

		ui_Battle.money.text = "Money: " + money;
        
	}
    

	public static void enemy_Attack(){
		if (Battle_Manager.base_Life <= 1) {
			Battle_Manager.base_Life = 0;
			stop = true;
			if (!Battle_Manager.ui_Battle.isDefenceMode) {
				float percentage = (float)Battle_Manager.enemy_Left / Battle_Manager.total_Enemy;
				if (percentage >= 2.0 / 3)
					scores [cur_Level - 1] = 3;
				else if (percentage >= 1.0 / 3 && scores[cur_Level-1]<2)
					scores [cur_Level - 1] = 2;
				else if(scores[cur_Level-1]<1)
					scores [cur_Level - 1] = 1;
			}
			if (ui_Battle.isDefenceMode)
				UI_Manager.Enter<UI_Result> ().init (false);
			else
				UI_Manager.Enter<UI_Result> ().init (true);
			ui_Battle.tower_Content.SetActive (false);
		} else
			Battle_Manager.base_Life--;
		ui_Battle.life.text = "Health left: " + base_Life;
	}
	public static void create_Tower(Vector3 pos,Tower_Info info){
		Tower tower = Tower_Manager.create (info.model).AddComponent<Tower>();
		tower.transform.localPosition = pos;
		tower.init (info);
		tower_List.Add (tower);
	}
	
	public static void clear() {
        for (int i = 0; i < tower_List.Count; i++)
        {
            GameObject.Destroy(tower_List[i].gameObject);
        }
		tower_List.Clear ();

        for (int i = 0; i < enemy_List.Count; i++)
        {
            GameObject.Destroy(enemy_List[i].gameObject);
        }
        enemy_List.Clear();

        for (int i = 0; i < Map_Manager.path_Box.Count; i++)
        {
            GameObject.Destroy(Map_Manager.path_Box[i]);
        }
        Map_Manager.path_Box.Clear();

        
		
		for (int i = 0; i < Bullet_Manager.list.Count; i++)
        { 
			GameObject.Destroy (Bullet_Manager.list [i]);
        }
        Bullet_Manager.list.Clear ();

		GameObject.Destroy (Map_Manager.farm);
	}

	public static bool next_Wave=true;
	public static void mUpdate(){
		if (stop==false) {
			if (enemies.Count > 0) {
				time += Time.deltaTime;
				if (time >= 1 && ui_Battle.isDefenceMode || time >= 0 && !ui_Battle.isDefenceMode) { // decrese time
					time--;
					if (wave_Time > 0) { // count down from 5 to 0
						wave_Time--;
						ui_Battle.wave_Decrease (wave_Time);
					} else if (next_Wave==true) { // start next wave and start count down from 5, wava_time = 0
						next_Wave = false;
						cur_Wave++;
                        beforeCountDown = false;
                        wave_Time = 5;
						ui_Battle.wave_Decrease (wave_Time);
						if(ui_Battle.isDefenceMode)
							ui_Battle.wave.text = "Wave number: " + cur_Wave;
					} else if (ui_Battle.isDefenceMode)
                    { // next wave is false, in current wave, creating enemy, wava_time = 0
                        Enemy enemy = Enemy_Manager.create(enemies[0], path);
                        ui_Battle.bind(enemy);
                        enemy_List.Add(enemy);
                        if (enemies[0].number == 1) // enemy creation finished
                        {
                            enemies.RemoveAt(0);
                            next_Wave = true;

							money += 10;
                        }
                        else // enemy creating
                            enemies[0].number--;
                    } else if (!ui_Battle.isDefenceMode)
                    {
                        int type = ui_Battle.enemy_ciked;
                        if (type != -1)
                        {
                            enemy_wave_create(type);
                        }
                    }
				}
			}
			for (int i = enemy_List.Count - 1; i >= 0; i--) {
				if (enemy_List [i] == null)
					enemy_List.RemoveAt (i);
				else {
					enemy_List [i].mUpdate ();
					if (enemy_List [i].health <= 0)
						enemy_List.RemoveAt (i);
				}
			}

			if (base_Life > 0 && enemies.Count == 0 && enemy_List.Count == 0) {
				stop = true;
                if (ui_Battle.isDefenceMode)
                {
                    if (base_Life == BASE_LIFE)
                        scores[cur_Level - 1] = 3;
                    else if (base_Life >= 0.5 * BASE_LIFE && scores[cur_Level - 1] < 3)
                        scores[cur_Level - 1] = 2;
                    else
                        scores[cur_Level - 1] = 1;
                }
				ui_Battle.tower_Content.SetActive (false);
				if (ui_Battle.isDefenceMode)
					UI_Manager.Enter<UI_Result> ().init (true);
				else
					UI_Manager.Enter<UI_Result> ().init (false);
			}

			ui_Battle.money.text = "Money: " + money;
		}
	}

	public static bool wrong_Pos(Vector3 pos) {
		for (int i = 0; i < Map_Manager.path_Box.Count; i++) {
			if (Vector3.Distance (pos, Map_Manager.path_Box [i].transform.localPosition) < 0.1f)
				return true;
		}
		return false;
	}

	public static bool ACE_Pos(Vector3 pos){
		for (int i = 0; i < Map_Manager.path_Box.Count; i++) {
			if (Vector3.Distance (pos, Map_Manager.path_Box [i].transform.localPosition) < 0.5f)
				return true;
		}
		return false;
	}

    public static int upgrade_Tower(Vector3 pos)
    {
        for (int i = 0; i < tower_List.Count; i++)
        {
            if (pos == tower_List[i].transform.localPosition)
                return i;
        }
        return -1;
    }

    public static void enemy_wave_create(int type)
    {
        Debug.Log("wave!!");
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].model.Equals("enemy"+(type+1)) ||
                enemies[i].model.Equals("tower" + (type + 1)))
            {   
                Enemy enemy = Enemy_Manager.create(enemies[i], path);
                ui_Battle.bind(enemy);
                enemy_List.Add(enemy);
                if (enemies[i].number == 1) // enemy creation finished
                {
                    enemies.RemoveAt(i);
                    
                    next_Wave = true;
                    ui_Battle.enemy_ciked = -1; // reset 
                    beforeCountDown = true;
                }
                else // enemy creating
                    enemies[i].number--;
                break;
            }
        }
        
    }
}
