using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UI_Battle : UI_Base {
	public const int BTN = 150;
	private const int BTN_TEXT_SIZE = 40;
	private const int TITLE_SIZE = 70;
	private const int CAMERA_FIELD_VIEW = 70;
	private const int SHIELD_MONEY = 30,ACCELERATE_MONEY = 30,PORTAL_MONEY=30,BOMB_MONEY=30,BLOCK_MONEY=3;
	private const float SHIELD_EFFECT = 1.5f,ACCELERATE_EFFECT=1.5f;
	private const int SHIELD_TIME = 3,ACCELERATE_TIME=3,PORTAL_TIME=1,BOMB_TIME=3,BLOCK_TIME=3;
	private const int BOMB_EFFECT=-2,BOMB_RANGE=5;
	private const float BOMB_PERIOD = 0.5f,PORTAL_PERIOD=10f,BLOCK_PERIOD=5f;
	private const int PORTAL_DISTANCE = 5;

	private Vector3 pre_Touch_Pos;
	private float pre_Range;
	private bool drag = false;
	private bool click_Btn=false;
	public List<Tower_Info> towers=new List<Tower_Info>();
    public List<Enemy_Info> enemies = new List<Enemy_Info>();
    public List<GameObject> buttons=new List<GameObject>();
	public GameObject tower_Content,base_Tower,health_Info;
	public Text life,wave,money,wave_Time;
    public bool isDefenceMode = true;
    public GameObject enemy_btn_list;
    public int[] enemy_count_list = new int[4];
    public GameObject [] enemy_btn_obj_list = new GameObject[4];
    public int enemy_ciked = -1;
    public List<List<Vector3>> tower_coor_list = null;
    public GameObject upgrade_btn;
	public GameObject btn_Bomb;
	public bool use_Bomb = false;
	public int bomb_Buffer_Time = 0;
	public Text Buffer_Time;
	public GameObject bomb;
	public GameObject btn_Block;
	public bool use_Block = false;
	public int block_Buffer_Time = 0;
	public Text block_Time;
	public List<GameObject> blocks=new List<GameObject>();

	public GameObject item_Btn_List;
	public GameObject btn_Shield,btn_Accelerate,btn_Portal;
	public bool use_Portal=false;
	public Text shield_Buffer_Time, accelerate_Buffer_Time, portal_Buffer_Time;
	public int shield_Time=0, accelerate_Time=0, portal_Time=0;
	public GameObject Portal1,Portal2;
	public List<GameObject> portals1 = new List<GameObject> ();
	public List<GameObject> portals2 = new List<GameObject> ();
//	public bool portal_Finished=false;
    public int star = 0;
    public int old_tower_index = -1;
	public float time = 0;

    public void init(string map,List<Tower_Info> towers, List<Enemy_Info> enemies, bool isDefence, int star){
		this.towers = towers;
        this.enemies = enemies;
        this.isDefenceMode = isDefence;
        click_Btn = true;
        this.star = star;
        Camera.main.transform.localPosition = new Vector3 (0, 10, 0);
        //Camera.main.transform.localPosition = new Vector3 (0, 9, -6);
		Camera.main.fieldOfView = CAMERA_FIELD_VIEW;
        if (!isDefenceMode)
            tower_coor_list = Map_Manager.get_tower(map);
        Battle_Manager.init(this, Map_Manager.get_Path(map));
		if (!isDefenceMode) {
			enemy_btn_list.SetActive (true);
			item_Btn_List.SetActive (true);
			Debug.Log ("is attack mode!!");

			Battle_Manager.total_Enemy = 0;
			for (int i = 0; i < enemies.Count; i++) {   
				Battle_Manager.total_Enemy += enemies [i].number;
				switch (enemies [i].model) {
				case "tower1":
					enemy_count_list [0]++;
					break;
				case "tower2":
					enemy_count_list [1]++;
					break;
				case "tower3":
					enemy_count_list [2]++;
					break;
				case "tower4":
					enemy_count_list [3]++;
					break;
				}
			}
			Battle_Manager.enemy_Left = Battle_Manager.total_Enemy;
			if (enemy_count_list [0] == 0) {
				enemy_btn_obj_list [0].SetActive (false);
				for (int i = 1; i < 4; i++)
					enemy_btn_obj_list [i].transform.localPosition = 
                        new Vector3 (enemy_btn_obj_list [i].transform.localPosition.x, 
						enemy_btn_obj_list [i].transform.localPosition.y + BTN, 
						enemy_btn_obj_list [i].transform.localPosition.z);
			}
			if (enemy_count_list [1] == 0) {
				enemy_btn_obj_list [1].SetActive (false);
				for (int i = 2; i < 4; i++)
					enemy_btn_obj_list [i].transform.localPosition = 
                        new Vector3 (enemy_btn_obj_list [i].transform.localPosition.x, 
						enemy_btn_obj_list [i].transform.localPosition.y + BTN, 
						enemy_btn_obj_list [i].transform.localPosition.z);
			}
			if (enemy_count_list [2] == 0) {
				enemy_btn_obj_list [2].SetActive (false);
				for (int i = 3; i < 4; i++)
					enemy_btn_obj_list [i].transform.localPosition = 
                        new Vector3 (enemy_btn_obj_list [i].transform.localPosition.x, 
						enemy_btn_obj_list [i].transform.localPosition.y + BTN, 
						enemy_btn_obj_list [i].transform.localPosition.z);
			}
			if (enemy_count_list [3] == 0) {
				enemy_btn_obj_list [3].SetActive (false);
			}

			for (int i = 0; i < 4; i++) {
				if (enemy_btn_obj_list [i].activeSelf) {
					enemy_btn_obj_list [i].GetComponent<Button> ().interactable = false;
				}
			}
                
		} else {
			Debug.Log ("is defence mode!!");
			btn_Bomb.SetActive (true);
			btn_Block.SetActive (true);
		}
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

            case "Btn_Upgrade":
                Tower_Info new_tower_info = Main.getNewTowerInfo(Battle_Manager.tower_List[old_tower_index].name);
                if (new_tower_info.money > Battle_Manager.money) return;
                Vector3 pos_up = upgrade_btn.transform.localPosition;

                RectTransform rect_up = this.GetComponent<RectTransform>();
                pos_up.x /= rect_up.rect.width;
                pos_up.y /= rect_up.rect.height;
                pos_up.x += 0.5f;
                pos_up.y += 0.5f;
                Ray ray_up = Camera.main.ViewportPointToRay(pos_up);
                RaycastHit[] hits_up = Physics.RaycastAll(ray_up);
                for (int i = 0; i < hits_up.Length; i++)
                {
                    if (hits_up[i].collider.name == "map_show")
                    {
                        pos_up = hits_up[i].point;
                        pos_up.x = Mathf.RoundToInt(pos_up.x);
                        pos_up.z = Mathf.RoundToInt(pos_up.z);
                        GameObject t = Battle_Manager.tower_List[old_tower_index].gameObject;
                        Battle_Manager.tower_List.RemoveAt(old_tower_index);
                        GameObject.Destroy(t);
                        Battle_Manager.create_Tower(pos_up, new_tower_info);
                        Battle_Manager.money -= new_tower_info.money;
                        old_tower_index = -1;
                        click_Btn = true;
                        upgrade_btn.SetActive(false);

                    }
                }
                
                break;

			case "Btn_Bomb":
				use_Bomb = true;
				click_Btn = true;
				break;

			case "Btn_Block":
				use_Block = true;
				click_Btn = true;
				break;

			case "Portal":
				use_Portal = true;
				click_Btn = true;
				Debug.Log ("portal clicked");
				break;

			case "Shield":
			if (Battle_Manager.money < SHIELD_MONEY || shield_Time != 0)
					return;
				for (int j = 0; j < Battle_Manager.enemy_List.Count; j++) {
					Battle_Manager.enemy_List [j].health *= SHIELD_EFFECT;
					Battle_Manager.enemy_List [j].max_Health *= SHIELD_EFFECT;
				}
				Battle_Manager.money -= SHIELD_MONEY;
				shield_Time = SHIELD_TIME;
				shield_Buffer_Time.gameObject.SetActive (true);
				shield_Buffer_Time.text = shield_Time.ToString ();
				btn_Shield.GetComponent<Button> ().interactable = false;
				break;

			case "Accelerate":
				if (Battle_Manager.money < ACCELERATE_MONEY || accelerate_Time != 0)
					return;
				for (int j = 0; j < Battle_Manager.enemy_List.Count; j++)
					Battle_Manager.enemy_List [j].speed *= ACCELERATE_EFFECT;
				Battle_Manager.money -= ACCELERATE_MONEY;
				accelerate_Time = ACCELERATE_TIME;
				accelerate_Buffer_Time.gameObject.SetActive (true);
				accelerate_Buffer_Time.text = accelerate_Time.ToString ();
				btn_Accelerate.GetComponent<Button> ().interactable = false;
				break;

            case "Btn_Tower":
			    if (towers [buttons.IndexOf (obj)].money > Battle_Manager.money)
					return;
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
						Battle_Manager.money -= towers [buttons.IndexOf (obj)].money;
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
            case "Pause":
                if (Time.timeScale != 0)
                    Time.timeScale = 0;
                else Time.timeScale = 1;
                break;
        }
		
	}
	public override void node_Asset(string name,GameObject obj){
		
		if (name.Equals ("Tower_Info")) {
			obj.SetActive (false);
			tower_Content = obj;
		}
        if(name.Equals("Pause"))
        {
            GameObject btn_pause = obj;
            RectTransform rectTransform = btn_pause.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(BTN * 1.0f, BTN * 1.0f);
            rectTransform.localPosition = new Vector3(Screen.width / 2f - rectTransform.sizeDelta.x / 2f, 0, 0);
        }
        if (name.Equals("Btn_Upgrade"))
        {
            obj.SetActive(false);
            upgrade_btn = obj;
        }
		if (name.Equals ("Btn_Bomb")) {
			obj.SetActive (false);
			btn_Bomb = obj;
            RectTransform rectTransform = btn_Bomb.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(BTN * 1.0f, BTN * 1.0f);
            rectTransform.localPosition = new Vector3(Screen.width/2f - rectTransform.sizeDelta.x / 2f, -Screen.height / 2f + rectTransform.sizeDelta.y / 2f, 0);
        }
		if (name.Equals ("Btn_Block")) {
			obj.SetActive (false);
			btn_Block = obj;
			RectTransform rectTransform = btn_Block.GetComponent<RectTransform> ();
			rectTransform.sizeDelta = new Vector2 (BTN * 1.0f, BTN * 1.0f);
			rectTransform.localPosition = new Vector3 (Screen.width / 2f - rectTransform.sizeDelta.x * 3 / 2f, -Screen.height / 2f + rectTransform.sizeDelta.y * 3 / 2f, 0);
		}
        if (name.Equals ("Buffer_Time")) {
			obj.SetActive (false);
			Buffer_Time = obj.GetComponent<Text>();
		}
		if (name.Equals ("block_Time")) {
			obj.SetActive (false);
			block_Time = obj.GetComponent<Text> ();
		}
		if (name.Equals ("Health")) {
			obj.SetActive (false);
			health_Info = obj;
		}

        if (name.Equals("status"))
        {
            Image status = obj.GetComponent<Image>();
            RectTransform rectTransform = status.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(Screen.width * 1.0f, Screen.height / 7f);
            rectTransform.localPosition = new Vector3(0, Screen.height / 2f-rectTransform.sizeDelta.y/2f, 0);
        }
        if (name.Equals("Life"))
        {
            life = obj.GetComponent<Text>();
            Text tx = life.GetComponentInChildren<Text>();
            tx.fontSize = TITLE_SIZE;
            RectTransform rectTransform = life.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(Screen.width/3f, Screen.height / 6f);
            rectTransform.localPosition = new Vector3(-Screen.width / 4f, 0, 0);
        }
            
        if (name.Equals ("Wave"))
        {
            wave = obj.GetComponent<Text>();
            Text tx = wave.GetComponentInChildren<Text>();
            tx.fontSize = TITLE_SIZE;
            RectTransform rectTransform = wave.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(Screen.width/3f, Screen.height / 6f);
            rectTransform.localPosition = new Vector3(Screen.width / 4f, 0, 0);
        }
			
		if (name.Equals ("Money"))
        {
            money = obj.GetComponent<Text>();
            Text tx = money.GetComponentInChildren<Text>();
            tx.fontSize = TITLE_SIZE;
            RectTransform rectTransform = money.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(Screen.width/3f, Screen.height / 6f);
            rectTransform.localPosition = new Vector3(0, 0, 0);
            
        }
			
		if (name.Equals ("Wave_Time"))
			wave_Time = obj.GetComponent<Text> ();
		if (name.Equals ("Btn_Tower")) {
			obj.SetActive (false);
			base_Tower = obj;
		}
        if (name.Equals ("Enemy_List"))
        {
            Image list = obj.GetComponent<Image>();
            RectTransform rectTransform = list.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(BTN*1.0f, BTN * 1.0f);
            rectTransform.localPosition = new Vector3(BTN/2f-Screen.width/2f, BTN*1.0f, 0);
            obj.SetActive(false);
            enemy_btn_list = obj;
        }
        if (name.Equals("enemy1"))
        {
            enemy_btn_obj_list[0] = obj;
            enemy_btn_obj_list[0].GetComponent<Button>().interactable = false;
        }
        if (name.Equals("enemy2"))
        {
            enemy_btn_obj_list[1] = obj;
            enemy_btn_obj_list[1].GetComponent<Button>().interactable = false;
        }
        if (name.Equals("enemy3"))
        {
            enemy_btn_obj_list[2] = obj;
            enemy_btn_obj_list[2].GetComponent<Button>().interactable = false;
        }
        if (name.Equals("enemy4"))
        {
            enemy_btn_obj_list[3] = obj;
            enemy_btn_obj_list[3].GetComponent<Button>().interactable = false;
        }
		if (name.Equals ("Item_List")) {
			Image list = obj.GetComponent<Image> ();
			RectTransform rectTransform = list.GetComponent<RectTransform> ();
			rectTransform.sizeDelta = new Vector2 (BTN * 1.0f, BTN * 1.0f);
			rectTransform.localPosition = new Vector3 (-BTN * 2.5f + Screen.width / 2f, -Screen.height / 2f + rectTransform.sizeDelta.y / 2f, 0);
			obj.SetActive (false);
			item_Btn_List = obj;
		}
		if (name.Equals ("Shield"))
			btn_Shield = obj;
		if (name.Equals ("Accelerate"))
			btn_Accelerate = obj;
		if (name.Equals ("Portal"))
			btn_Portal = obj;
		if (name.Equals ("Shield_Buffer_Time")) {
			obj.SetActive (false);
			shield_Buffer_Time = obj.GetComponent<Text> ();
		}
		if (name.Equals ("Accelerate_Buffer_Time")) {
			obj.SetActive (false);
			accelerate_Buffer_Time = obj.GetComponent<Text> ();
		}
		if (name.Equals ("Portal_Buffer_Time")) {
			obj.SetActive (false);
			portal_Buffer_Time = obj.GetComponent<Text> ();
		}
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.touchCount >= 2) {
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

        */
		time += Time.deltaTime;
		if (time > 1) {
			time--;
			if (bomb_Buffer_Time > 0) {
				bomb_Buffer_Time--;
				Buffer_Time.text = bomb_Buffer_Time.ToString();
				if (bomb_Buffer_Time == 0)
                {
                    Buffer_Time.gameObject.SetActive(false);
                    btn_Bomb.GetComponent<Button>().interactable = true;
                }		
			}
			if (block_Buffer_Time > 0) {
				block_Buffer_Time--;
				block_Time.text = block_Buffer_Time.ToString ();
				if (block_Buffer_Time == 0) {
					block_Time.gameObject.SetActive (false);
					btn_Block.GetComponent<Button> ().interactable = true;
				}
			}
			if (shield_Time > 0) {
				shield_Time--;
				shield_Buffer_Time.text = shield_Time.ToString ();
				if (shield_Time == 0) {
					shield_Buffer_Time.gameObject.SetActive (false);
					btn_Shield.GetComponent<Button> ().interactable = true;
				}
			}
			if (accelerate_Time > 0) {
				accelerate_Time--;
				accelerate_Buffer_Time.text = accelerate_Time.ToString ();
				if (accelerate_Time == 0) {
					accelerate_Buffer_Time.gameObject.SetActive (false);
					btn_Accelerate.GetComponent<Button> ().interactable = true;
				}
			}
			if (portal_Time > 0) {
				portal_Time--;
				portal_Buffer_Time.text = portal_Time.ToString ();
				if (portal_Time == 0) {
					portal_Buffer_Time.gameObject.SetActive (false);
					btn_Portal.GetComponent<Button> ().interactable = true;
//					portal_Finished = false;
				}
			}
		}
        if (!this.isDefenceMode) // attack mode, set enemy btn enable or disable
        {
            if (Battle_Manager.wave_Time <= 0 && enemy_ciked == -1 && !Battle_Manager.beforeCountDown)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (enemy_count_list[i] > 0)
                    {
                        enemy_btn_obj_list[i].GetComponent<Button>().interactable = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    enemy_btn_obj_list[i].GetComponent<Button>().interactable = false;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                Text tmp_text = enemy_btn_obj_list[i].transform.FindChild("Text").GetComponents<Text>()[0];
                tmp_text.text = "Attacker " + (1 + i) + "\n"+ "left: " + enemy_count_list[i];
            }
        }


        Battle_Manager.mUpdate();

        if (isDefenceMode) // defence mode
        {
            
            if (click_Btn == false &&
            Battle_Manager.stop == false &&
            ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Canceled) ||
            Input.GetMouseButtonUp(0)))
            {
                if (drag)
                    drag = false;
                else
                {
                    if (tower_Content.activeSelf)
                    {
                       // if (EventSystem.current.IsPointerOverGameObject() == false) // tower_btn has been clicked
                      tower_Content.SetActive(false);
                    } else if (upgrade_btn.activeSelf)
                    {
                        //if (EventSystem.current.IsPointerOverGameObject() == false) // tower_btn has been clicked
                       upgrade_btn.SetActive(false);
                       old_tower_index = -1; 
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
                                
								if (use_Bomb) {
									if (Battle_Manager.bomb_Pos (pos)) {
										use_Bomb = false;
										if (Battle_Manager.money < BOMB_MONEY || bomb_Buffer_Time != 0)
											return;
										bomb = Resources.Load<GameObject> ("Model/bomb");
										bomb = GameObject.Instantiate (bomb);
										bomb.transform.localPosition = pos;
										for (int j = 0; j < Battle_Manager.enemy_List.Count; j++) {
											if (Vector3.Distance (pos, Battle_Manager.enemy_List [j].transform.localPosition) <= BOMB_RANGE) {
												Battle_Manager.enemy_List [j].change_Health (BOMB_EFFECT);
											}
										}
										Destroy (bomb, BOMB_PERIOD);
										Battle_Manager.money -= BOMB_MONEY;
										bomb_Buffer_Time = BOMB_TIME;
										Buffer_Time.gameObject.SetActive (true);
										Buffer_Time.text = bomb_Buffer_Time.ToString ();
										btn_Bomb.GetComponent<Button> ().interactable = false;
									}
								} else if (use_Block) {
									if (Battle_Manager.bomb_Pos (pos)) {
										use_Block = false;
										if (Battle_Manager.money < BLOCK_MONEY || block_Buffer_Time != 0)
											return;
										GameObject block = Resources.Load<GameObject> ("Model/block");
										block = GameObject.Instantiate (block);
										block.transform.localPosition = pos;
										for (int k = 0; k < Battle_Manager.path.Count; k++) {
											if (Vector3.Distance (Battle_Manager.path [k], pos) < 0.1f) {
												if((k>0&&Battle_Manager.path[k-1].x==Battle_Manager.path[k].x)||(k<Battle_Manager.path.Count-1&&Battle_Manager.path[k+1].x==Battle_Manager.path[k].x)){
													var rotationVector = block.transform.rotation.eulerAngles;
													rotationVector.y = 90;
													block.transform.rotation = Quaternion.Euler (rotationVector);
												}
											}
										}
										int j = 0;
										for (; j < blocks.Count; j++) {
											if (blocks [j] == null) {
												blocks [j] = block;
												break;
											}
										}
										if (j == blocks.Count)
											blocks.Add (block);
										Destroy (blocks [j], BLOCK_PERIOD);
										Battle_Manager.money -= BLOCK_MONEY;
										block_Buffer_Time = BLOCK_TIME;
										block_Time.gameObject.SetActive (true);
										block_Time.text = block_Buffer_Time.ToString ();
										btn_Block.GetComponent<Button> ().interactable = false;
									}
								} else if ((old_tower_index = Battle_Manager.upgrade_Tower(pos)) != -1)
                                {
                                    string name = Battle_Manager.tower_List[old_tower_index].name;
                                    if (star == 1 || name.Equals("tower1_3") ||
                                        name.Equals("tower2_3") ||
                                        name.Equals("tower3_3") ||
                                        name.Equals("tower4_3")) return;
                                    else if (star == 2 && (name.Equals("tower1_2") ||
                                        name.Equals("tower2_2") ||
                                        name.Equals("tower3_2") ||
                                        name.Equals("tower4_2"))) return;
                                    upgrade_btn.SetActive(true);
                                    upgrade_btn.transform.localPosition = btn_pos;
                                    return;

                                }
                                else if (Battle_Manager.wrong_Pos(pos) == false &&
                                    Vector3.Distance(btn_pos, btn_Bomb.transform.localPosition) > btn_Bomb.GetComponent<RectTransform>().sizeDelta.x/1.0f &&
									Vector3.Distance(btn_pos, btn_Block.transform.localPosition) > btn_Block.GetComponent<RectTransform>().sizeDelta.x/1.0f)
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
			if (click_Btn == false &&
			    Battle_Manager.stop == false &&
			    ((Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Canceled) ||
			    Input.GetMouseButtonUp (0))) {
				Vector3 pos = Input.mousePosition;
				pos.x -= Screen.width / 2;
				pos.y -= Screen.height / 2;
				//					pos.x *= 598f / Screen.height;
				//					pos.y *= 598f / Screen.height;
//				Vector3 btn_pos = pos;

				RectTransform rect = this.GetComponent<RectTransform>();
				pos.x /= rect.rect.width;
				pos.y /= rect.rect.height;
				pos.x += 0.5f;
				pos.y += 0.5f;
				Ray ray = Camera.main.ViewportPointToRay(pos);
				RaycastHit[] hits = Physics.RaycastAll(ray);
				for (int i = 0; i < hits.Length; i++) {
					if (hits [i].collider.name == "map_show") {
						pos = hits [i].point;
						pos.x = Mathf.RoundToInt (pos.x);
						pos.z = Mathf.RoundToInt (pos.z);

						if (use_Portal) {
							if (Battle_Manager.bomb_Pos (pos)) {
								use_Portal = false;
								if (Battle_Manager.money < PORTAL_MONEY || portal_Time != 0)
									return;
								
								Portal1 = Resources.Load<GameObject> ("Model/Portal");
								Portal1 = GameObject.Instantiate (Portal1);
								Portal1.transform.localPosition = pos;
								int end_Index = 0;
								for (int j = 0; j < Battle_Manager.path.Count; j++) {
									if (Vector3.Distance (Battle_Manager.path [j], pos) < 0.1f) {
										if((j>0&&Battle_Manager.path[j-1].x==Battle_Manager.path[j].x)||(j<Battle_Manager.path.Count-1&&Battle_Manager.path[j+1].x==Battle_Manager.path[j].x)){
											var rotationVector = Portal1.transform.rotation.eulerAngles;
											rotationVector.y = 90;
											Portal1.transform.rotation = Quaternion.Euler (rotationVector);
										}
										end_Index = Mathf.Min (Battle_Manager.path.Count - 2, j + PORTAL_DISTANCE);
										break;
									}
								}
								Portal2 = Resources.Load<GameObject> ("Model/Portal");
								Portal2 = GameObject.Instantiate (Portal2);
								Portal2.transform.localPosition = Battle_Manager.path [end_Index];
								if((end_Index>0&&Battle_Manager.path[end_Index-1].x==Battle_Manager.path[end_Index].x)||(end_Index<Battle_Manager.path.Count-1&&Battle_Manager.path[end_Index+1].x==Battle_Manager.path[end_Index].x)){
									var rotationVector = Portal1.transform.rotation.eulerAngles;
									rotationVector.y = 90;
									Portal2.transform.rotation = Quaternion.Euler (rotationVector);
								}
//								portal_Finished = true;
								Battle_Manager.money -= PORTAL_MONEY;

								int k=0;
								for (; k < portals1.Count; k++) {
									if (portals1 [k] == null) {
										portals1 [k] = Portal1;
										portals2 [k] = Portal2;
										break;
									}
								}
								if (k == portals1.Count) {
									portals1.Add (Portal1);
									portals2.Add (Portal2);
								}

								Destroy (portals1[k], PORTAL_PERIOD);
								Destroy (portals2[k], PORTAL_PERIOD);

								portal_Time = PORTAL_TIME;
								portal_Buffer_Time.gameObject.SetActive (true);
								portal_Buffer_Time.text = portal_Time.ToString ();
								btn_Portal.GetComponent<Button> ().interactable = false;
							}
						}
					}
				}
			}
			click_Btn = false;
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
