using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Battle_Manager {
	public const int BASE_LIFE=1;
	public const int MONEY_CURRENT_WAVE = 10;
	public const int INIT_WAVE_TIME = 5;
	public const int BASE_MONEY = 100;

    public static bool next_Wave = true;
	public const float ENEMY_PERIOD = 10;
	public static float next_Enemy = ENEMY_PERIOD;

    public static GameObject parent_obj = GameObject.Find("map_parent_node");

    public static UI_Battle ui_Battle;
	public static List<Vector3> path, path2;
	public static List<Enemy_Info> enemies, attackers;
	public static List<Enemy> enemy_List=new List<Enemy>();
    public static List<Enemy> attacker_List = new List<Enemy>();
    public static List<Tower> tower_List=new List<Tower>();
	public static int cur_Wave, wave_Time, base_Life;
	private static float time = 0;
	public static bool stop;

	public static int[] scores = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
	public static int cur_Level;

	public static int total_Attacker;
	public static int attacker_Left;

	public static int money;

    public static bool beforeCountDown = true;

	public static bool[] locked_towers = new bool[] {true, false, false, false, false, false, false, false, false, false, false, false};

    public static void init(UI_Battle ui_Battle,List<Vector3> path, List<Vector3> path2){
		Battle_Manager.ui_Battle = ui_Battle;
		Battle_Manager.path = path;
        Battle_Manager.path2 = path2;
        enemies = ui_Battle.enemies;
        attackers = ui_Battle.attackers;
        next_Enemy = ENEMY_PERIOD;
		next_Wave = true;


        if (ui_Battle.enemy_tower_coor_list != null)
        {
            for (int j = 0; j < ui_Battle.enemy_tower_coor_list[0].Count; j++)
			{
                create_Tower(ui_Battle.enemy_tower_coor_list[0][j], new Tower_Info("tower1", "zombie1", 1, 2f, 2, "", "", 0, 0));
            }
            for (int j = 0; j < ui_Battle.enemy_tower_coor_list[1].Count; j++)
            {
                create_Tower(ui_Battle.enemy_tower_coor_list[1][j], new Tower_Info("tower2", "zombie2", 2, 2f, 2, "", "", 0, 0));
            }
            for (int j = 0; j < ui_Battle.enemy_tower_coor_list[2].Count; j++)
            {
                create_Tower(ui_Battle.enemy_tower_coor_list[2][j], new Tower_Info("tower3", "zombie3", 3, 2f, 3, "", "", 0, 0));
            }
            for (int j = 0; j < ui_Battle.enemy_tower_coor_list[3].Count; j++)
            {
                create_Tower(ui_Battle.enemy_tower_coor_list[3][j], new Tower_Info("tower4", "zombie4", 3, 2f, 5, "", "", 0, 0));
            }


        }

        cur_Wave = 0;
		base_Life = BASE_LIFE;
		time = 0;
		money = BASE_MONEY;

        for (int i = 0; i < enemy_List.Count; i++)
        {
            GameObject.Destroy(enemy_List[i].gameObject);
        }
        enemy_List.Clear();

        for (int i = 0; i < attacker_List.Count; i++)
        {
            GameObject.Destroy(attacker_List[i].gameObject);
        }
        attacker_List.Clear();

        stop = false;

		ui_Battle.money.text = "" + money;
        
	}

	public static void enemy_Attack(){
		if (Battle_Manager.base_Life <= 1) {
			Battle_Manager.base_Life = 0;
			stop = true;
			if (!ui_Battle.isDefenceMode) {

				float percentage = (float)attacker_Left / total_Attacker;
               // Debug.Log(attacker_Left + ", " + total_Attacker);
				if (percentage >= 2.0 / 3)
					scores [cur_Level*2 - 1] = 3;
				else if (percentage >= 1.0 / 3 && scores[cur_Level * 2 - 1]<2)
					scores [cur_Level * 2 - 1] = 2;
				else if(scores[cur_Level * 2 - 1]<1)
					scores [cur_Level * 2 - 1] = 1;
			}
			if (ui_Battle.isDefenceMode)
            {
                
                UI_Manager.Enter<UI_Result>().init(true, false);
            }
				
			else
				UI_Manager.Enter<UI_Result> ().init (false, true);
			ui_Battle.tower_Content.SetActive (false);
		} else
			Battle_Manager.base_Life--;
		ui_Battle.life.text = "" + base_Life;
	}


	public static void create_Tower(Vector3 pos,Tower_Info info){
		Tower tower = Tower_Manager.create (info.model).AddComponent<Tower>();
		tower.transform.localPosition = pos;
        tower.transform.SetParent(parent_obj.transform);
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
            if (enemy_List[i] != null) GameObject.Destroy(enemy_List[i].gameObject);
        }
        enemy_List.Clear();

        for (int i = 0; i < attacker_List.Count; i++)
        {
            if (attacker_List[i] != null) GameObject.Destroy(attacker_List[i].gameObject);
        }
        attacker_List.Clear();

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




	
	public static void mUpdate(){
		if (stop==false) {
			if (ui_Battle.isDefenceMode && enemies.Count > 0 || !ui_Battle.isDefenceMode && attackers.Count > 0) {
              //  Debug.Log("in battle_Manager mUpdate");
				time += Time.deltaTime;
				if (time >= 1 && ui_Battle.isDefenceMode || time >= 0 && !ui_Battle.isDefenceMode) { // decrese time
					time--;next_Enemy--;
					if (wave_Time > 0) { // count down from 5 to 0
                                         // Debug.Log("coount down");
                        
                        if (enemies.Count > 0)
							next_Enemy = wave_Time + 1f / enemies [0].speed;
						else
							next_Enemy = wave_Time + 1f;
						wave_Time--;
                        
                        ui_Battle.wave_Decrease (wave_Time);
					} else if (next_Wave==true) { // start next wave and start count down from 5, wava_time = 0
//                        Debug.Log("start next wave");
						next_Wave = false;
						cur_Wave++;
                        beforeCountDown = false;
						wave_Time = INIT_WAVE_TIME;
						ui_Battle.wave_Decrease (wave_Time);
						if(ui_Battle.isDefenceMode)
							ui_Battle.wave.text = cur_Wave + "/" + ui_Battle.total_Waves;
					} else if (ui_Battle.isDefenceMode && next_Enemy < 0)
                    { // next wave is false, in current wave, creating enemy, wava_time = 0
						next_Enemy=0.1f * ENEMY_PERIOD/enemies[0].speed;
                        Enemy enemy = Enemy_Manager.create(enemies[0], path);
                        ui_Battle.bind(enemy);
                        enemy.transform.SetParent(parent_obj.transform);
                        enemy_List.Add(enemy);
                        if (enemies[0].number == 1) // enemy creation finished
                        {
                            enemies.RemoveAt(0);
                            next_Wave = true;

							money += MONEY_CURRENT_WAVE;
							ui_Battle.money_Effect (0);
                        }
                        else // enemy creating
                            enemies[0].number--;
                    } else if (!ui_Battle.isDefenceMode && next_Enemy < 0)
                    {
                        int type = ui_Battle.attacker_ciked;
                        if (type != -1)
                        {
                            attacker_wave_create(type, next_Enemy);
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
            for (int i = attacker_List.Count - 1; i >= 0; i--)
            {
                if (attacker_List[i] == null)
                    attacker_List.RemoveAt(i);
                else
                {
                    attacker_List[i].mUpdate();
                    if (attacker_List[i].health <= 0)
                        attacker_List.RemoveAt(i);
                }
            }

            if (ui_Battle.isDefenceMode && base_Life > 0 && enemies.Count == 0 && enemy_List.Count == 0)
            {
                stop = true;
                if (base_Life == BASE_LIFE)
                    scores[cur_Level * 2 - 2] = 3;
				else if (base_Life >= 0.5 * BASE_LIFE && scores[cur_Level * 2 - 2] < 3)
					scores[cur_Level * 2 - 2] = 2;
                else
					scores[cur_Level * 2 - 2] = 1;
                ui_Battle.tower_Content.SetActive(false);

                ui_Battle.defenseWin = true;
                UI_Manager.Enter<UI_Result>().init(true, true);
                ui_Battle.isDefenceMode = false;
                next_Wave = false;
                stop = false;
            }
            else if (!ui_Battle.isDefenceMode && base_Life > 0 && attackers.Count == 0 && attacker_List.Count == 0)
            {
				stop = true;
                UI_Manager.Enter<UI_Result>().init(false, false);
            }
            ui_Battle.money.text = "" + money;
		}
	}

	public static bool wrong_Pos(Vector3 pos) {
		for (int i = 0; i < Map_Manager.path_Box.Count; i++) {
			if (Vector3.Distance (pos, Map_Manager.path_Box [i].transform.localPosition) < 0.1f)
				return true;
		}
		
		return false;
	}

	public static bool bomb_Pos(Vector3 pos){
		for (int i = 0; i < Map_Manager.path_Box.Count; i++) {
			if (Vector3.Distance (pos, Map_Manager.path_Box [i].transform.localPosition) < 0.5f)
				return true;
		}
		return false;
	}

    public static bool portal_Pos(Vector3 pos)
    {
        for (int i = 0; i < Map_Manager.path_Box2.Count; i++)
        {
            if (Vector3.Distance(pos, Map_Manager.path_Box2[i].transform.localPosition) < 0.5f)
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

    public static void attacker_wave_create(int type, float next_attacker)
    {
        //Debug.Log("wave!!");
        for (int i = 0; i < attackers.Count; i++)
        {
            if (attackers[i].model.Equals("tower" + (type + 1)))
            {
                next_Enemy = 0.1f * ENEMY_PERIOD / attackers[i].speed;
                Enemy enemy = Enemy_Manager.create(attackers[i], path2);
                ui_Battle.bind(enemy);
                enemy.transform.SetParent(parent_obj.transform);
                attacker_List.Add(enemy);
                if (attackers[i].number == 1) // enemy creation finished
                {
                    attackers.RemoveAt(i);
                    
                    next_Wave = true;
                    ui_Battle.attacker_ciked = -1; // reset 
                    beforeCountDown = true; // disinteract attacker list button before count down
                }
                else // enemy creating
                    attackers[i].number--;
                break;
            }
        }
        
    }
}
