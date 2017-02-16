﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UI_Battle : UI_Base {
	private Vector3 pre_Touch_Pos;
	private float pre_Range;
	private bool drag = false;
	private bool click_Btn=false;
	public List<Tower_Info> towers=new List<Tower_Info>();
    public List<Enemy_Info> enemies = new List<Enemy_Info>();
    public List<GameObject> buttons=new List<GameObject>();
	public GameObject tower_Content,base_Tower,health_Info;
	public Text life,wave,wave_Time;
    public bool isDefenceMode = true;
    public GameObject enemy_btn_list;
    public int[] enemy_count_list = new int[4];
    public GameObject [] enemy_btn_obj_list = new GameObject[4];
    public int enemy_ciked = -1;
    public List<List<Vector3>> tower_coor_list = new List<List<Vector3>>();


    public void init(string map,List<Tower_Info> towers, List<Enemy_Info> enemies, bool isDefence){
		this.towers = towers;
        this.enemies = enemies;
        this.isDefenceMode = isDefence;
        Camera.main.transform.localPosition = new Vector3 (0, 10, 0);
		Camera.main.fieldOfView = 70;
        if (!isDefenceMode)
            tower_coor_list = Map_Manager.get_tower(map);
        Battle_Manager.init(this, Map_Manager.get_Path(map));
        if (!isDefenceMode)
        {
            enemy_btn_list.SetActive(true);
            Debug.Log("is attack mode!!");
            for (int i = 0; i < enemies.Count; i++)
            {   
                switch(enemies[i].model)
                {
                    case "enemy1":
                        enemy_count_list[0]++;
                        break;
                    case "enemy2":
                        enemy_count_list[1]++;
                        break;
                    case "enemy3":
                        enemy_count_list[2]++;
                        break;
                    case "enemy4":
                        enemy_count_list[3]++;
                        break;
                }
            }
            if (enemy_count_list[0] == 0)
            {
                enemy_btn_obj_list[0].SetActive(false);
                for (int i = 1; i < 4; i++)
                    enemy_btn_obj_list[i].transform.localPosition = 
                        new Vector3(enemy_btn_obj_list[i].transform.localPosition.x, 
                        enemy_btn_obj_list[i].transform.localPosition.y + 60, 
                        enemy_btn_obj_list[i].transform.localPosition.z);
             }
            if (enemy_count_list[1] == 0)
            {
                enemy_btn_obj_list[1].SetActive(false);
                for (int i = 2; i < 4; i++)
                    enemy_btn_obj_list[i].transform.localPosition = 
                        new Vector3(enemy_btn_obj_list[i].transform.localPosition.x, 
                        enemy_btn_obj_list[i].transform.localPosition.y + 60, 
                        enemy_btn_obj_list[i].transform.localPosition.z);
            }
            if (enemy_count_list[2] == 0)
            {
                enemy_btn_obj_list[2].SetActive(false);
                for (int i = 3; i < 4; i++)
                    enemy_btn_obj_list[i].transform.localPosition = 
                        new Vector3(enemy_btn_obj_list[i].transform.localPosition.x, 
                        enemy_btn_obj_list[i].transform.localPosition.y + 60, 
                        enemy_btn_obj_list[i].transform.localPosition.z);
            }
            if (enemy_count_list[3] == 0)
            {
                enemy_btn_obj_list[3].SetActive(false);
            }

            for (int i = 0; i < 4; i++)
            {
                if (enemy_btn_obj_list[i].activeSelf)
                {
                    enemy_btn_obj_list[i].GetComponentInChildren<Button>().enabled = false;
                    
                }
            }
                
        }
        else 
            Debug.Log("is defence mode!!");
        life.text = "Life left: " + Battle_Manager.base_Life;
        
		for (int i = 0; i < towers.Count; i++) {
			GameObject obj = GameObject.Instantiate (base_Tower);
			obj.SetActive (true);
			obj.GetComponentInChildren<Button> ().onClick.AddListener (() => {
				button_Click ("Btn_Tower", obj);
			});
			obj.GetComponentInChildren<Text> ().text = towers [i].name;
			obj.transform.SetParent (tower_Content.transform);
			buttons.Add (obj);
		}
	}
	public override void button_Click(string name,GameObject obj){

        switch(name)
        {
            case "Btn_Tower":
                Vector3 pos = tower_Content.transform.localPosition;
                RectTransform rect = this.GetComponent<RectTransform>();
                pos.x /= rect.rect.width;
                pos.y /= rect.rect.height;
                pos.x += 0.5f;
                pos.y += 0.5f;
                Ray ray = Camera.main.ViewportPointToRay(pos);
                RaycastHit[] hits = Physics.RaycastAll(ray);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.name == "map_show")
                    {
                        pos = hits[i].point;
                        pos.x = Mathf.RoundToInt(pos.x);
                        pos.z = Mathf.RoundToInt(pos.z);
                        Battle_Manager.create_Tower(pos, towers[buttons.IndexOf(obj)]);
                        click_Btn = true;
                        tower_Content.SetActive(false);
                    }
                }
                break;
            case "enemy1":
                enemy_ciked = 0;
                enemy_count_list[enemy_ciked]--;
                break;
            case "enemy2":
                enemy_ciked = 1;
                enemy_count_list[enemy_ciked]--;
                Debug.Log("enemy2 down@@");
                break;
            case "enemy3":
                enemy_ciked = 2;
                enemy_count_list[enemy_ciked]--;
                break;
            case "enemy4":
                enemy_ciked = 3;
                enemy_count_list[enemy_ciked]--;
                break;
        }
		
	}
	public override void node_Asset(string name,GameObject obj){
		if (name.Equals ("Life"))
			life = obj.GetComponent<Text> ();
		if (name.Equals ("Tower_Info")) {
			obj.SetActive (false);
			tower_Content = obj;
		}
		if (name.Equals ("Health")) {
			obj.SetActive (false);
			health_Info = obj;
		}
		if (name.Equals ("Wave"))
			wave = obj.GetComponent<Text> ();
		if (name.Equals ("Wave_Time"))
			wave_Time = obj.GetComponent<Text> ();
		if (name.Equals ("Btn_Tower")) {
			obj.SetActive (false);
			base_Tower = obj;
		}
        if (name.Equals ("Enemy_List"))
        {
            obj.SetActive(false);
            enemy_btn_list = obj;
        }
        if (name.Equals("enemy1"))
        {
            enemy_btn_obj_list[0] = obj;
        }
        if (name.Equals("enemy2"))
        {
            enemy_btn_obj_list[1] = obj;
        }
        if (name.Equals("enemy3"))
        {
            enemy_btn_obj_list[2] = obj;
        }
        if (name.Equals("enemy4"))
        {
            enemy_btn_obj_list[3] = obj;
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount >= 2) {
			if (pre_Touch_Pos != Vector3.zero)
				pre_Touch_Pos = Vector3.zero;
			Touch touch1 = Input.GetTouch (0);
			Touch touch2 = Input.GetTouch (1);
			if (pre_Range >= 0)
				Camera.main.fieldOfView *= Vector2.Distance (touch1.position, touch2.position) / pre_Range;
			else
				pre_Range = Vector2.Distance (touch1.position, touch2.position);
		} else if (Input.touchCount > 0||Input.GetMouseButton(0)) {
			if (pre_Range >= 0)
				pre_Range = -1;
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.name == "map_show") {
					if (pre_Touch_Pos == Vector3.zero)
						pre_Touch_Pos = hit.point;
					else if (pre_Touch_Pos != hit.point) {
						drag = true;
						Camera.main.transform.localPosition += pre_Touch_Pos - hit.point;
						ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						if (Physics.Raycast (ray, out hit)) {
							if (hit.collider.name == "map_show")
								pre_Touch_Pos = hit.point;
							else
								pre_Touch_Pos = Vector3.zero;
						} else
							pre_Touch_Pos = Vector3.zero;
					}
				}
			}
		} else {
			pre_Range = -1;
			pre_Touch_Pos = Vector3.zero;
			if (Input.GetKey (KeyCode.W)) {
				Vector3 pos = Camera.main.transform.localPosition;
				pos.z += 0.3f;
				Camera.main.transform.localPosition = pos;
			}
			if (Input.GetKey (KeyCode.A)) {
				Vector3 pos = Camera.main.transform.localPosition;
				pos.x -= 0.3f;
				Camera.main.transform.localPosition = pos;
			}
			if (Input.GetKey (KeyCode.S)) {
				Vector3 pos = Camera.main.transform.localPosition;
				pos.z -= 0.3f;
				Camera.main.transform.localPosition = pos;
			}
			if (Input.GetKey (KeyCode.D)) {
				Vector3 pos = Camera.main.transform.localPosition;
				pos.x += 0.3f;
				Camera.main.transform.localPosition = pos;
			}
			if (Input.GetAxis ("Mouse ScrollWheel") < 0)
				Camera.main.fieldOfView *= 1.1f;
			if (Input.GetAxis ("Mouse ScrollWheel") > 0)
				Camera.main.fieldOfView *= 0.9f;
		}








		Battle_Manager.mUpdate ();
        

        if (!this.isDefenceMode) // attack mode, set enemy btn enable or disable
        {
            if (Battle_Manager.wave_Time <= 0 && enemy_ciked == -1)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (enemy_count_list[i] > 0)
                    {
                        enemy_btn_obj_list[i].GetComponentInChildren<Button>().enabled = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    enemy_btn_obj_list[i].GetComponentInChildren<Button>().enabled = false;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                Text tmp_text = enemy_btn_obj_list[i].transform.FindChild("Text").GetComponents<Text>()[0];
                tmp_text.text = "Enemy" + (1 + i) + ", left: " + enemy_count_list[i];
            }
        }
        


        
        if (isDefenceMode) // defence mode
        {
            if (click_Btn == false &&
            Battle_Manager.stop == false &&
            ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Canceled) || Input.GetMouseButtonUp(0)))
            {
                if (drag)
                    drag = false;
                else
                {
                    if (tower_Content.activeSelf)
                    {
                        if (EventSystem.current.IsPointerOverGameObject() == true) // tower_btn has been clicked
                        {
                            tower_Content.SetActive(false);
                        }

                    }
                    else
                    { // change tower_content from false to true to show  all available tower_btn
                        Vector3 pos = Input.mousePosition;
                        pos.x -= Screen.width / 2;
                        pos.y -= Screen.height / 2;
                        //					pos.x *= 598f / Screen.height;
                        //					pos.y *= 598f / Screen.height;
                        Vector3 btn_pos = pos;

                        RectTransform rect = this.GetComponent<RectTransform>();
                        pos.x /= rect.rect.width;
                        pos.y /= rect.rect.height;
                        pos.x += 0.5f;
                        pos.y += 0.5f;
                        Ray ray = Camera.main.ViewportPointToRay(pos);
                        RaycastHit[] hits = Physics.RaycastAll(ray);
                        for (int i = 0; i < hits.Length; i++)
                        {
                            if (hits[i].collider.name == "map_show")
                            {
                                pos = hits[i].point;
                                pos.x = Mathf.RoundToInt(pos.x);
                                pos.z = Mathf.RoundToInt(pos.z);
                                if (Battle_Manager.wrong_Pos(pos) == false)
                                {
                                    tower_Content.SetActive(true);
                                    tower_Content.transform.localPosition = btn_pos;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            click_Btn = false;
        }
        else // attack mode, no need to show tower btn, this is for add item code
        {

        }
	}

	public void wave_Decrease(int time){
		if (time > 0) {
			wave_Time.gameObject.SetActive (true);
			wave_Time.text = time.ToString ();
		} else
			wave_Time.gameObject.SetActive (false);
	}
	public void bind(Enemy enemy){
		GameObject obj = Instantiate (health_Info);
		obj.transform.SetParent (this.transform);
		obj.SetActive (true);
		Health health = obj.AddComponent<Health> ();
		health.init (enemy);
	}
}
