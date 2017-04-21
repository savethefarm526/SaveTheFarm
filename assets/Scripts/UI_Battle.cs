using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UI_Battle : UI_Base {
	public const int BTN = 150;
	private const int BTN_TEXT_SIZE = 10;
	private const int TITLE_SIZE = 50;
	private const int CAMERA_FIELD_VIEW = 70;
	private const int SHIELD_MONEY = 5,ACCELERATE_MONEY = 5,PORTAL_MONEY=5,BOMB_MONEY=10,BLOCK_MONEY=5;
	private const float SHIELD_EFFECT = 1.5f,ACCELERATE_EFFECT=1.5f;
	private const float SHIELD_TIME = 8,ACCELERATE_TIME=5,PORTAL_TIME=10,BOMB_TIME=3,BLOCK_TIME=3;
	private const int BOMB_EFFECT=-3,BOMB_RANGE=3;
	private const float BOMB_PERIOD = 1f,PORTAL_PERIOD=15f,BLOCK_PERIOD=8f;
	private const int PORTAL_DISTANCE = 5;
    private const float CAB_FULL_SIZE = 1f;
    private const float PUM_FULL_SIZE = 1f;
    public int def_wave_count = 6;
    public int def_total_wave_count = 6;
    private const int CAB_MONEY = 5;
    private const int PUM_MONEY = 5;
    private const float CAB_TIME = 10;
    private const float PUM_TIME = 10;

    private GameObject btn_cab, btn_pum;
    private Vector3 pre_Touch_Pos;
	private float pre_Range;
	private bool drag = false;
	private bool click_Btn=false;
	public List<Tower_Info> towers=new List<Tower_Info>();
    public List<Enemy_Info> enemies = new List<Enemy_Info>();
    public List<Enemy_Info> attackers = new List<Enemy_Info>();
    public List<GameObject> buttons=new List<GameObject>();
	public GameObject tower_Content,base_Tower,health_Info;
	public Text life,wave,money,wave_Time, defwave;
    
    public GameObject attacker_btn_list;
    public int[] attacker_count_list = new int[4];
    public GameObject [] attacker_btn_obj_list = new GameObject[4];
    public int attacker_ciked = -1;
    public List<List<Vector3>> enemy_tower_coor_list = null;
    public GameObject upgrade_btn;
	public GameObject btn_Bomb;
	public bool use_Bomb = false;
	public GameObject bomb;
	public GameObject btn_Block;
	public bool use_Block = false;
	public List<GameObject> blocks=new List<GameObject>();

    public bool use_cab = false;
    public bool use_pum = false;
    
	
	public GameObject btn_Shield,btn_Accelerate,btn_Portal, btn_Help;
	public bool use_Portal=false;
	public GameObject Portal1,Portal2;
	public List<GameObject> portals1 = new List<GameObject> ();
	public List<GameObject> portals2 = new List<GameObject> ();
    public int star = 0;
    public int old_tower_index = -1;

	public float block_CoolDown = BLOCK_TIME;
	public float block_Current_CoolDown = BLOCK_TIME;
	public Image block_Image_Cooldown;
	public float bomb_CoolDown = BOMB_TIME;
	public float bomb_Current_CoolDown = BOMB_TIME;
	public Image bomb_Image_Cooldown;
	public float shield_CoolDown = SHIELD_TIME;
	public float shield_Current_CoolDown = SHIELD_TIME;
	public Image shield_Image_Cooldown;
	public float accelerate_CoolDown = ACCELERATE_TIME;
	public float accelerate_Current_CoolDown = ACCELERATE_TIME;
	public Image accelerate_Image_Cooldown;
	public float portal_CoolDown = PORTAL_TIME;
	public float portal_Current_CoolDown = PORTAL_TIME;
	public Image portal_Image_Cooldown;


    public float cab_Current_CoolDown = CAB_TIME;
    public float cab_CoolDown = CAB_TIME;
    public Image cab_Image_Cooldown;

    public float pum_Current_CoolDown = PUM_TIME;
    public float pum_CoolDown = PUM_TIME;
    public Image pum_Image_Cooldown;

    public bool isDefenceMode;
    public bool defenseWin = false;
    public bool attackWin = false;
    public bool start_attack_mode_s, start_attack_mode_e, attack_mode_ready, start_attack;
    public GameObject att_item_list, def_item_list;
    public GameObject btn_Sell;

    public Button mBtn_Help;

	public static bool fadeIn=true,fadeOut=false;
	public static int choice;
	public bool spread_Anim = false;

	public int level;
	public int total_Waves;

    public bool bonus_added = false;

	public GameObject gift;
	public Image bonus_money;
	public Image bonus_life;
	public float time;


    public GameObject sell_mask;


    public Enemy target;

    public bool use_cab_block = false;
    public bool use_pum_bomb = false;
    public int cab_block_time = 0;
    private const int CAB_FULL_BLOCK_TIME = 25;
    public int pum_bomb_range = 0;
    private const int PUM_FULL_BOMB_RANGE = 10;
    public int pum_bomb_power = 0;
    private const int PUM_FULL_BOMB_POWER = 20;

    public int pum_bomb_money = 0;
    private const int PUM_FULL_MONEY = 80;
    public int cab_block_money = 0;
    private const int CAB_FULL_MONEY = 80;
    




    /*
    public static string context_map;
    public static List<Tower_Info> context_towers;
    public static List<Enemy_Info> context_enemies;
    public static List<Enemy_Info> context_attackers;
    public static int context_star;*/

    public void init(string map,List<Tower_Info> towers, List<Enemy_Info> enemies, List<Enemy_Info> attackers, int star){
		time = 0;

		/*context_map = map;
        context_towers = new List<Tower_Info>(towers);
        context_enemies = new List<Enemy_Info>(enemies);
        context_attackers = new List<Enemy_Info>(attackers);
        context_star = star;*/
        isDefenceMode = true;
       // Debug.Log(enemies.Count);

		level = int.Parse (map);
		total_Waves = enemies.Count;

        this.towers = towers;
        this.enemies = enemies;
        this.attackers = attackers;
        click_Btn = true;
        start_attack_mode_s = false;
        start_attack_mode_e = false;
        attack_mode_ready = false;
        start_attack = false;
        this.star = star;
        Camera.main.transform.localPosition = new Vector3 (1f, 8f, -4f);
        Camera.main.transform.localRotation = Quaternion.Euler(70, 0, 0);
		Camera.main.fieldOfView = CAMERA_FIELD_VIEW;
        
        enemy_tower_coor_list = Map_Manager.get_enemy_tower(map); // set attack mode enemy tower
        Map_Manager.get_grows_coors(map);
		Battle_Manager.init(this, Map_Manager.get_Path(map), Map_Manager.get_Path_Alternate(map), Map_Manager.get_Path2(map));

		
        life.text = "" + Battle_Manager.base_Life;
        
		for (int i = 0; i < towers.Count; i++) {
			GameObject obj = GameObject.Instantiate (base_Tower);
			obj.SetActive (true);
			obj.GetComponentInChildren<Button> ().onClick.AddListener (() => {
				button_Click ("Btn_Tower", obj);
			});
			obj.GetComponentInChildren<Text> ().text = ""+towers[i].money;
			obj.GetComponentInChildren<Button>().image.sprite = Resources.Load<Sprite> ("Texture/" + towers [i].name);
			obj.transform.SetParent (tower_Content.transform);
			buttons.Add (obj);
		}
	}

	public void life_Effect(int mode){
		StartCoroutine (real_Life_Effect (mode));
	}
	public IEnumerator real_Life_Effect(int mode){
		if (mode == 0) {
			life.color = Color.green;
			yield return new WaitForSeconds (0.2f);
			life.color = Color.white;
		} else if (mode == 1) {
			life.color = Color.red;
			yield return new WaitForSeconds (0.2f);
			life.color = Color.white;
		}
	}

	public void money_Effect(int mode){
		StartCoroutine (real_Money_Effect (mode));
	}
	public IEnumerator real_Money_Effect(int mode){
		if (mode == 0) {
			money.color = Color.green;
			yield return new WaitForSeconds (0.2f);
			money.color = Color.white;
		} else if (mode == 1) {
			money.color = Color.red;
			yield return new WaitForSeconds (0.2f);
			money.color = Color.white;
		} else if (mode == 2) {
            Audio_Manager.PlaySound("no_money");
            money.color = Color.red;
			money.fontSize += 20;
			yield return new WaitForSeconds (0.2f);
			money.color = Color.white;
			money.fontSize -= 20;
		}
	}

	public override void button_Click(string name,GameObject obj) {
        Audio_Manager.PlaySound("btn");
        switch (name)
        {
            
		case "Btn_Upgrade":
				Tower_Info new_tower_info = Main.getNewTowerInfo (Battle_Manager.tower_List [old_tower_index].name);
				if (new_tower_info.money > Battle_Manager.money) {
					money_Effect (2);
					return;
				}

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
                        Tower tmp = Battle_Manager.create_Tower(pos_up, new_tower_info);
						bind_Tower (tmp);
                        Battle_Manager.money -= new_tower_info.money;
						money_Effect (1);
                        old_tower_index = -1;
                        click_Btn = true;
                        upgrade_btn.SetActive(false);

                    }
                }
                Audio_Manager.PlaySound("upgrade");
                break;

			case "Btn_Bomb":
				if (bomb_Current_CoolDown >= bomb_CoolDown) {
					use_Bomb = true;
					click_Btn = true;
				}
				break;

			case "Btn_Block":
				if (block_Current_CoolDown >= block_CoolDown) {
					use_Block = true;
					click_Btn = true;
				}
				break;

			case "Portal":
				if (portal_Current_CoolDown >= portal_CoolDown) {
					use_Portal = true;
					click_Btn = true;
				}
				break;

            case "btn_cab":
                if (cab_Current_CoolDown >= cab_CoolDown)
                {
                    use_cab = true;
                    click_Btn = true;
                }
                break;
            case "btn_pum":
                if (pum_Current_CoolDown >= pum_CoolDown)
                {
                    use_pum = true;
                    click_Btn = true;
                }
                break;

            case "Shield":
				if (shield_Current_CoolDown >= shield_CoolDown) {
					if (Battle_Manager.money < SHIELD_MONEY) {
                        
						money_Effect (2);
						return;
					}
                    
					shield_Current_CoolDown = 0;
					for (int j = 0; j < Battle_Manager.attacker_List.Count; j++) {
						Battle_Manager.attacker_List [j].health *= SHIELD_EFFECT;
						Battle_Manager.attacker_List [j].max_Health *= SHIELD_EFFECT;
						StartCoroutine (Battle_Manager.attacker_List [j].show_Status ("power_up"));
					}
					Battle_Manager.money -= SHIELD_MONEY;
					money_Effect (1);
                    Audio_Manager.PlaySound("item_shield");
                }
				break;

			case "Accelerate":
				if(accelerate_Current_CoolDown>=accelerate_CoolDown){
					if (Battle_Manager.money < ACCELERATE_MONEY) {
						money_Effect (2);
						return;
					}
                    
                    accelerate_Current_CoolDown = 0;
					for (int j = 0; j < Battle_Manager.attacker_List.Count; j++) {
						Battle_Manager.attacker_List [j].speed *= ACCELERATE_EFFECT;
						StartCoroutine (Battle_Manager.attacker_List [j].show_Status ("speed_up"));
					}
					Battle_Manager.money -= ACCELERATE_MONEY;
					money_Effect (1);
                    Audio_Manager.PlaySound("item_speed");
                }
				break;

			case "Btn_Tower":
				if (towers [buttons.IndexOf (obj)].money > Battle_Manager.money) {
					money_Effect (2);
					return;
				}
                if (def_wave_count <= 0)
                {
                    Audio_Manager.PlaySound("no_money"); // audio save as no_moeny
                    return;
                }
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
                        Tower tower = Battle_Manager.create_Tower(pos, towers[buttons.IndexOf(obj)]);
						bind_Tower (tower);
						Battle_Manager.money -= towers [buttons.IndexOf (obj)].money;
						money_Effect (1);
                        click_Btn = true;
                        tower_Content.SetActive(false);
                    }
                }
                def_wave_count--;
                Audio_Manager.PlaySound("create_defender");
                defwave.text = def_wave_count + "/" + def_total_wave_count;


                break;
            case "attacker1":
                attacker_ciked = 0;
                attacker_count_list[attacker_ciked]--;
                break;
            case "attacker2":
                attacker_ciked = 1;
                attacker_count_list[attacker_ciked]--;
                break;
            case "attacker3":
                attacker_ciked = 2;
                attacker_count_list[attacker_ciked]--;
                break;
            case "attacker4":
                attacker_ciked = 3;
                attacker_count_list[attacker_ciked]--;
                break;
            case "Pause":
                if (Time.timeScale != 0)
                    Time.timeScale = 0;
                else Time.timeScale = 1;
                break;

			case "Btn_Help":
				//			UI_Manager.Exit (this);
				fadeOut = true;
				choice = 1;
				break;
        }
		
	}
	public override void node_Asset(string name,GameObject obj){

		if (name.Equals ("Gift")) {
			gift = obj.transform.FindChild ("3dGift").gameObject;
			bonus_money = obj.transform.FindChild ("bonus_money").GetComponent<Image> ();
			bonus_life = obj.transform.FindChild ("bonus_life").GetComponent<Image> ();
			bonus_money.gameObject.SetActive (false);
			bonus_life.gameObject.SetActive (false);
			gift.gameObject.SetActive (false);
		}
		if (name.Equals ("Tower_Info")) {
			obj.SetActive (false);
			tower_Content = obj;
		}
        if(name.Equals("Pause"))
        {
            GameObject btn_pause = obj;
            RectTransform rectTransform = btn_pause.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(BTN * 1.0f, BTN * 1.0f);
            rectTransform.localPosition = new Vector3(- Screen.width / 2f + BTN *1.5f, -Screen.height/2f+BTN/2f, 0);
        }
        if (name.Equals("Btn_Upgrade"))
        {
            obj.SetActive(false);
            upgrade_btn = obj;
            
        }
		if (name.Equals ("Btn_Bomb")) {
			btn_Bomb = obj;
			Text tx = obj.GetComponentInChildren<Text> ();
			tx.text = ""+BOMB_MONEY;
			if (UI_Select.lv <= 2)
				btn_Bomb.SetActive (false);
			else
				btn_Bomb.SetActive (true);
        }
		if (name.Equals ("Btn_Block")) {
			btn_Block = obj;
			Text tx = obj.GetComponentInChildren<Text> ();
			tx.text = ""+BLOCK_MONEY;
			if (UI_Select.lv <= 1)
				btn_Block.SetActive (false);
			else
				btn_Block.SetActive (true);
		}

        if (name.Equals("btn_cab"))
        {
            btn_cab = obj;
            Text tx = obj.GetComponentInChildren<Text>();
            tx.text = "" + CAB_MONEY;
        }

        if (name.Equals("btn_pum"))
        {
            btn_pum = obj;
            Text tx = obj.GetComponentInChildren<Text>();
            tx.text = "" + PUM_MONEY;
        }

		if (name.Equals ("block_CoolDown")) {
			block_Image_Cooldown = obj.GetComponent<Image>();
		}
		if (name.Equals ("bomb_CoolDown")) {
			bomb_Image_Cooldown = obj.GetComponent<Image> ();
		}
		if (name.Equals ("shield_CoolDown")) {
			shield_Image_Cooldown = obj.GetComponent<Image> ();
		}
		if (name.Equals ("accelerate_CoolDown")) {
			accelerate_Image_Cooldown = obj.GetComponent<Image> ();
		}
		if (name.Equals ("portal_CoolDown")) {
			portal_Image_Cooldown = obj.GetComponent<Image> ();
		}
        if (name.Equals("pum_CoolDown"))
        {
            pum_Image_Cooldown = obj.GetComponent<Image>();
        }
        if (name.Equals("cab_CoolDown"))
        {
            cab_Image_Cooldown = obj.GetComponent<Image>();
        }
        if (name.Equals ("Health")) {
			obj.SetActive (false);
			health_Info = obj;
		}

        if (name.Equals("status"))
        {
            Image status = obj.GetComponent<Image>();
            RectTransform rectTransform = status.GetComponent<RectTransform>();
        }
        if (name.Equals("Life"))
        {
			life = obj.transform.FindChild("Life_Text").GetComponent<Text>();
            Text tx = life.GetComponentInChildren<Text>();
            tx.fontSize = TITLE_SIZE;
        }
            
        if (name.Equals ("Wave"))
        {
			wave = obj.transform.FindChild("Wave_Text").GetComponent<Text>();
            Text tx = wave.GetComponentInChildren<Text>();
            tx.fontSize = TITLE_SIZE;
        }

        
        if (name.Equals("DefWave"))
        {
            defwave = obj.transform.FindChild("Wave_Text").GetComponent<Text>();
            Text tx = wave.GetComponentInChildren<Text>();
            tx.fontSize = TITLE_SIZE;
            defwave.text = def_wave_count + "/" + def_total_wave_count;
        }

        if (name.Equals ("Money"))
        {
			money = obj.transform.FindChild("Money_Text").GetComponent<Text>();
            Text tx = money.GetComponentInChildren<Text>();
            tx.fontSize = TITLE_SIZE;
            
        }
			
		if (name.Equals ("Wave_Time"))
			wave_Time = obj.GetComponent<Text> ();
		if (name.Equals ("Btn_Tower")) {
			obj.SetActive (false);
			base_Tower = obj;
		}

        if (name.Equals ("Attacker_List"))
        {
            Image list = obj.GetComponent<Image>();
            RectTransform rectTransform = list.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(BTN*1.0f, BTN * 1.0f);
            rectTransform.localPosition = new Vector3(-BTN/2f-Screen.width/2f, BTN*2f, 0);
            attacker_btn_list = obj;
        }
        if (name.Equals("attacker1"))
        {
            attacker_btn_obj_list[0] = obj;
            attacker_btn_obj_list[0].GetComponent<Button>().interactable = false;
        }
        if (name.Equals("attacker2"))
        {
            attacker_btn_obj_list[1] = obj;
            attacker_btn_obj_list[1].GetComponent<Button>().interactable = false;
        }
        if (name.Equals("attacker3"))
        {
            attacker_btn_obj_list[2] = obj;
            attacker_btn_obj_list[2].GetComponent<Button>().interactable = false;
        }
        if (name.Equals("attacker4"))
        {
            attacker_btn_obj_list[3] = obj;
            attacker_btn_obj_list[3].GetComponent<Button>().interactable = false;
        }
		if (name.Equals ("Att_Item_List")) {
            att_item_list = obj;
            att_item_list.transform.localPosition = new Vector3 (-BTN * 1f + Screen.width / 2f, -Screen.height / 2f - BTN / 2f, 0);
			
            
        }
        if (name.Equals("Def_Item_List"))
        {
            def_item_list = obj;
            def_item_list.transform.localPosition = new Vector3(-BTN * 0.5f + Screen.width / 2f, -Screen.height / 2f + BTN / 2f, 0);
            
        }
		if (name.Equals ("Shield")) {
			btn_Shield = obj;
			Text tx = obj.GetComponentInChildren<Text> ();
			tx.text = ""+SHIELD_MONEY;
		}
		if (name.Equals ("Accelerate")) {
			btn_Accelerate = obj;
			Text tx = obj.GetComponentInChildren<Text> ();
			tx.text = ""+ACCELERATE_MONEY;
			if (UI_Select.lv <= 1)
				btn_Accelerate.SetActive (false);
			else
				btn_Accelerate.SetActive (true);
		}
		if (name.Equals ("Portal")) {
			btn_Portal = obj;
			Text tx = obj.GetComponentInChildren<Text> ();
			tx.text = ""+PORTAL_MONEY;
			if (UI_Select.lv <= 2)
				btn_Portal.SetActive (false);
			else
				btn_Portal.SetActive (true);
		}

		if (name.Equals ("Btn_Function")) {
			RectTransform rectTransform = obj.GetComponent<RectTransform> ();
			rectTransform.localPosition = new Vector3 (-Screen.width/2f+ BTN * 1f/2, -Screen.height/2f+ BTN * 1f/2, 0);
		}
        if (name.Equals("Btn_Help"))
        {
            btn_Help = obj;
        }
        if (name.Equals("Btn_Sell"))
        {
            btn_Sell = obj;

        }
        if (name.Equals("sell_mask"))
        {
            sell_mask = obj;
        }
    }
	// Use this for initialization
	void Start () {
//		GameObject.Find ("background_black").GetComponent<CanvasGroup> ().alpha = 0;
	}

    private const float GROW_YOUNG_SIZE = 0.4f;
    
	// Update is called once per frame
	void Update () {

		time += Time.deltaTime;
		if (time >= 2 && time < 10 && gift.gameObject.activeSelf) {
			gift.gameObject.SetActive (false);
		}
		if (time >= 10) {
			time = 0;
			gift.gameObject.SetActive (true);
			gift.transform.position = new Vector3 (Random.value * 15 - 7, 0.3f, Random.value * 9 - 4);
		}

        for (int i = 0; i < Battle_Manager.grows_list.Count; i++)
        {
            //Debug.Log(Battle_Manager.grows_list[i].name);
            float x = Battle_Manager.grows_list[i].transform.localScale.x;
            float y = Battle_Manager.grows_list[i].transform.localScale.y;
            float z = Battle_Manager.grows_list[i].transform.localScale.z;
            if (Battle_Manager.grows_status[i] == 0)
            {
                if (y < GROW_YOUNG_SIZE)
                {
                    Battle_Manager.grows_list[i].transform.localScale = new Vector3(x, y + 0.0005f, z);
                } else
                {
                    Battle_Manager.grows_status[i] = 1;
                    if (Battle_Manager.grows_name_list[i].StartsWith("pum"))
                    {
                        GameObject pum = Resources.Load<GameObject>("Model/cab&pum/pum");
                        pum = GameObject.Instantiate(pum);
                        pum.transform.localScale = new Vector3(0.25f, 0.3f, 0.25f);
                        pum.transform.localPosition = new Vector3(Battle_Manager.grows_list[i].transform.localPosition.x, Battle_Manager.grows_list[i].transform.localPosition.y, Battle_Manager.grows_list[i].transform.localPosition.z);
                        GameObject.Destroy(Battle_Manager.grows_list[i]);
                        Battle_Manager.grows_list[i] = pum;
                        pum.transform.SetParent(Battle_Manager.parent_obj.transform);
                    } else
                    {
                        Battle_Manager.grows_list[i].transform.localScale = new Vector3(0.2f, 0.4f, 0.2f);
                    }
                }
            }
            else if (Battle_Manager.grows_list[i].name.StartsWith("cab") && y < CAB_FULL_SIZE)
            {
                Battle_Manager.grows_list[i].transform.localScale = new Vector3(x + 0.0002f, y + 0.0001f, z + 0.0002f);
            }
            else if (Battle_Manager.grows_list[i].name.StartsWith("pum") && y < PUM_FULL_SIZE)
            {
                Battle_Manager.grows_list[i].transform.localScale = new Vector3(x + 0.0002f, y + 0.0002f, z + 0.0002f);
            }
        }
        

		if (fadeIn) {
			if (this.GetComponent<CanvasGroup> ().alpha < 1) {
				this.GetComponent<CanvasGroup> ().alpha += 0.05f;
				GameObject.Find ("background_black").GetComponent<CanvasGroup> ().alpha -= 0.05f;
			} else {
				fadeIn = false;
			}
		}
		if (fadeOut) {
			if (GameObject.Find ("background_black").GetComponent<CanvasGroup> ().alpha < 1) {
				GameObject.Find ("background_black").GetComponent<CanvasGroup> ().alpha += 0.05f;
				this.GetComponent<CanvasGroup> ().alpha -= 0.05f;
			} else {
				fadeOut = false;
				if (choice == 1) {
//					this.GetComponent<CanvasGroup> ().alpha = 0;
					GameObject battle = this.gameObject;
					battle.SetActive (false);
					GameObject map = GameObject.Find ("map_show");
					Vector3 tmp = map.transform.localPosition;
					tmp.y = 0.5f;
					map.transform.localPosition = tmp;
					map.GetComponent<Renderer> ().material.color = Color.black;

					UI_Manager.Enter<UI_Func> ().init (battle);
					UI_Func.fadeIn = true;
				} else if (choice == 2) {
					UI_Result.fadeOut = false;
					UI_Manager.Exit_All ();
					UI_Manager.Enter<UI_Select> ().init();
					Battle_Manager.clear ();
					UI_Select.fadeIn = true;
				}
			}
		}

		if (spread_Anim) {
			if (tower_Content.GetComponent<GridLayoutGroup> ().cellSize.x != 150) {
				tower_Content.GetComponent<GridLayoutGroup> ().cellSize += new Vector2 (10, 10);
			} else {
				spread_Anim = false;
			}
		}

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
        
		if (UI_Func.pause)
			return;

		if (block_Current_CoolDown < block_CoolDown) {
			block_Current_CoolDown += Time.deltaTime;
			block_Image_Cooldown.fillAmount = 1 - block_Current_CoolDown / block_CoolDown;
		}
		if (bomb_Current_CoolDown < bomb_CoolDown) {
			bomb_Current_CoolDown += Time.deltaTime;
			bomb_Image_Cooldown.fillAmount = 1 - bomb_Current_CoolDown / bomb_CoolDown;
		}
		if (shield_Current_CoolDown < shield_CoolDown) {
			shield_Current_CoolDown += Time.deltaTime;
			shield_Image_Cooldown.fillAmount = 1 - shield_Current_CoolDown / shield_CoolDown;
		}
		if (accelerate_Current_CoolDown < accelerate_CoolDown) {
			accelerate_Current_CoolDown += Time.deltaTime;
			accelerate_Image_Cooldown.fillAmount = 1 - accelerate_Current_CoolDown / accelerate_CoolDown;
		}
		if (portal_Current_CoolDown < portal_CoolDown) {
			portal_Current_CoolDown += Time.deltaTime;
			portal_Image_Cooldown.fillAmount = 1 - portal_Current_CoolDown / portal_CoolDown;
		}

        if (pum_Current_CoolDown < pum_CoolDown)
        {
            pum_Current_CoolDown += Time.deltaTime;
            pum_Image_Cooldown.fillAmount = 1 - pum_Current_CoolDown / pum_CoolDown;
        }

        if (cab_Current_CoolDown < cab_CoolDown)
        {
            cab_Current_CoolDown += Time.deltaTime;
            cab_Image_Cooldown.fillAmount = 1 - cab_Current_CoolDown / cab_CoolDown;
        }


        // start change to attack stage
        if (start_attack_mode_s && !start_attack_mode_e)
        {
            if (!bonus_added)
            {
                // add bonus attacker
                if (Battle_Manager.scores[Battle_Manager.cur_Level * 2 - 2] == 1)
                {
                    attackers.Add(new Enemy_Info("tower1", 2, 0.8f, 10, 5, 1.8f));
                }
                else if (Battle_Manager.scores[Battle_Manager.cur_Level * 2 - 2] == 2)
                {
                    attackers.Add(new Enemy_Info("tower2", 5, 0.6f, 10, 6, 1.8f));
                }
                else if (Battle_Manager.scores[Battle_Manager.cur_Level * 2 - 2] == 3)
                {
                    attackers.Add(new Enemy_Info("tower3", 10, 0.5f, 5, 7, 1.8f));
                }

                // inital attacker list
                Battle_Manager.total_Attacker = 0;
                for (int i = 0; i < attackers.Count; i++)
                {
                    Battle_Manager.total_Attacker += attackers[i].number;
                    switch (attackers[i].model)
                    {
                        case "tower1":
                            attacker_count_list[0]++;
                            break;
                        case "tower2":
                            attacker_count_list[1]++;
                            break;
                        case "tower3":
                            attacker_count_list[2]++;
                            break;
                        case "tower4":
                            attacker_count_list[3]++;
                            break;
                    }
                }
                Battle_Manager.attacker_Left = Battle_Manager.total_Attacker;
                if (attacker_count_list[0] == 0)
                {
                    attacker_btn_obj_list[0].SetActive(false);
                }
                if (attacker_count_list[1] == 0)
                {
                    attacker_btn_obj_list[1].SetActive(false);
                }
                if (attacker_count_list[2] == 0)
                {
                    attacker_btn_obj_list[2].SetActive(false);
                }
                if (attacker_count_list[3] == 0)
                {
                    attacker_btn_obj_list[3].SetActive(false);
                }

                for (int i = 0; i < 4; i++)
                {
                    if (attacker_btn_obj_list[i].activeSelf)
                    {
                        attacker_btn_obj_list[i].GetComponent<Button>().interactable = false;
                    }
                }

                bonus_added = true;
            }
            
            // Debug.Log("start change cam pos!" + Camera.main.transform.localPosition);
            Battle_Manager.beforeCountDown = true;
            Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x + 0.6f, Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z);

            if (Camera.main.transform.localPosition.x >= 25f) {
             //   Debug.Log("cam in right pos!");
                start_attack_mode_e = true;
            }

			for (int i = 0; i < Battle_Manager.tower_List.Count; i++) {
				if(Battle_Manager.tower_List[i]!=null)
					Battle_Manager.tower_List [i].changeAnimator ();
			}
        }

        // start to ready buttons in attack stage
        if (start_attack_mode_e && !attack_mode_ready)
        {
            







            //Debug.Log("start change btn pos!");
            bool att_list_right = Mathf.Abs(attacker_btn_list.transform.localPosition.x - (BTN / 2f - Screen.width / 2f)) < 1f;
            bool item_list_right = Mathf.Abs(att_item_list.transform.localPosition.y + Screen.height / 2f - BTN / 1.6f) < 1f;
            if (!item_list_right)
                att_item_list.transform.localPosition = new Vector3(att_item_list.transform.localPosition.x, att_item_list.transform.localPosition.y + 2f, att_item_list.transform.localPosition.z);
            if (!att_list_right)
                attacker_btn_list.transform.localPosition = new Vector3(attacker_btn_list.transform.localPosition.x + 2f, attacker_btn_list.transform.localPosition.y, attacker_btn_list.transform.localPosition.z);
            if (item_list_right && att_list_right)
            {
               // Debug.Log("btn in right pos!");
                attack_mode_ready = true;
            }
        }


        // inital attack mode
        if (attack_mode_ready && !start_attack)
        {
            Battle_Manager.base_Life = 5;
            //Battle_Manager.money = 100;
            wave.text = "";
			wave.GetComponentInParent<Image> ().gameObject.SetActive (false);
            defwave.text = "";
            defwave.GetComponentInParent<Image>().gameObject.SetActive(false);
            life.text = "" + Battle_Manager.base_Life;
            Battle_Manager.next_Wave = true;
            start_attack = true;

        }


        // start attack mode
        if (start_attack)
        {

        }
        








        if (!isDefenceMode) // attack mode, set enemy btn enable or disable
        {
            if (Battle_Manager.wave_Time <= 0 && attacker_ciked == -1 && !Battle_Manager.beforeCountDown)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (attacker_count_list[i] > 0)
                    {
                        attacker_btn_obj_list[i].GetComponent<Button>().interactable = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    attacker_btn_obj_list[i].GetComponent<Button>().interactable = false;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                Text tmp_text = attacker_btn_obj_list[i].transform.FindChild("Text").GetComponents<Text>()[0];
                tmp_text.text = "left: " + attacker_count_list[i];
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
                        Vector3 ori_pos = pos;
                        pos.x -= Screen.width / 2;
                        pos.y -= Screen.height / 2;
                        //					pos.x *= 598f / Screen.height;
                        //					pos.y *= 598f / Screen.height;
                        Vector3 btn_pos = pos;
                        //Debug.Log(btn_pos);
                       
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
                                if (use_cab)
                                {

                                    if (Battle_Manager.grows_Pos(pos) && !Battle_Manager.cur_grows_pos_list.Contains(pos))
                                    {
                                        use_cab = !use_cab;
                                        if (Battle_Manager.money < CAB_MONEY)
                                        {
                                            money_Effect(2);
                                            return;
                                        }
                                        cab_Current_CoolDown = 0;
                                        GameObject cab = Resources.Load<GameObject>("Model/cab&pum/cab");
                                        cab = GameObject.Instantiate(cab);
                                        cab.transform.localPosition = pos;
                                        cab.transform.localScale = new Vector3(0.1f, 0.2f, 0.1f);
                                        cab.transform.SetParent(Battle_Manager.parent_obj.transform);
                                        Battle_Manager.grows_list.Add(cab);
                                        Battle_Manager.cur_grows_pos_list.Add(pos);
                                        Battle_Manager.grows_status.Add(0);
                                        Battle_Manager.grows_name_list.Add("cab");
                                    }
                                }
                                else if (use_pum)
                                {
                                    if (Battle_Manager.grows_Pos(pos) && !Battle_Manager.cur_grows_pos_list.Contains(new Vector3((int)pos.x, (int)pos.y, (int)pos.z)))
                                    {
                                        use_pum = !use_pum;
                                        if (Battle_Manager.money < PUM_MONEY)
                                        {
                                            money_Effect(2);
                                            return;
                                        }
                                        pum_Current_CoolDown = 0;
                                        GameObject pum = Resources.Load<GameObject>("Model/cab&pum/cab");
                                        pum = GameObject.Instantiate(pum);
                                        pum.transform.localPosition = pos;
                                        pum.transform.localScale = new Vector3(0.1f, 0.2f, 0.1f);
                                        pum.transform.SetParent(Battle_Manager.parent_obj.transform);
                                        Battle_Manager.grows_list.Add(pum);
                                        Battle_Manager.cur_grows_pos_list.Add(pos);
                                        Battle_Manager.grows_status.Add(0);
                                        Battle_Manager.grows_name_list.Add("pum");
                                    }
                                }
                                else if (use_cab_block)
                                {
                                    if (Battle_Manager.bomb_Pos(pos))
                                    {
                                        use_cab_block = false;
                                        GameObject block = Resources.Load<GameObject>("Model/cab&pum/cab");
                                        block = GameObject.Instantiate(block);
                                        block.transform.localPosition = pos;
                                        block.transform.SetParent(Battle_Manager.parent_obj.transform);
                                        int j = 0;
                                        for (; j < blocks.Count; j++)
                                        {
                                            if (blocks[j] == null)
                                            {
                                                blocks[j] = block;
                                                break;
                                            }
                                        }
                                        if (j == blocks.Count)
                                            blocks.Add(block);
                                        Destroy(blocks[j], cab_block_time);
                                        sell_mask.SetActive(true);
                                    }
                                    else if (Vector3.Distance(pos, btn_Sell.transform.localPosition) < BTN * 1f)
                                    {
                                        use_cab_block = false;
                                        bonus_money.gameObject.SetActive(true);
                                        Text tx = bonus_money.gameObject.transform.FindChild("Text").GetComponent<Text>();
                                        tx.text = "" + cab_block_money;
                                        StartCoroutine(disappear_bonus(bonus_money));
                                        Battle_Manager.money += cab_block_money;
                                        money_Effect(0);
                                        sell_mask.SetActive(true);
                                    }

                                }
                                else if (use_pum_bomb)
                                {
                                    if (Battle_Manager.bomb_Pos(pos))
                                    {
                                        use_pum_bomb = false;
                                        bomb = Resources.Load<GameObject>("Model/cab&pum/pum");
                                        bomb = GameObject.Instantiate(bomb);
                                        bomb.transform.localPosition = pos;
                                        bomb.transform.SetParent(Battle_Manager.parent_obj.transform);
                                        for (int j = 0; j < Battle_Manager.enemy_List.Count; j++)
                                        {
                                            if (Vector3.Distance(pos, Battle_Manager.enemy_List[j].transform.localPosition) <= pum_bomb_range)
                                            {
                                                //Debug.Log(pum_bomb_power);
                                                Battle_Manager.enemy_List[j].change_Health(-pum_bomb_power, "bomb");
                                            }
                                        }
                                        Destroy(bomb, BOMB_PERIOD);
                                        sell_mask.SetActive(true);
                                    }
                                    else if (Vector3.Distance(pos, btn_Sell.transform.localPosition) < BTN * 1f)
                                    {
                                        use_pum_bomb = false;
                                        bonus_money.gameObject.SetActive(true);
                                        Text tx = bonus_money.gameObject.transform.FindChild("Text").GetComponent<Text>();
                                        tx.text = "" + pum_bomb_money;
                                        StartCoroutine(disappear_bonus(bonus_money));
                                        Battle_Manager.money += pum_bomb_money;
                                        money_Effect(0);
                                        sell_mask.SetActive(true);
                                    }


                                }

                                else if (use_Bomb) {
									// Debug.Log("defense, cliked, bomb");
									if (Battle_Manager.bomb_Pos (pos)) {
										Audio_Manager.PlaySound ("put_sun");
										use_Bomb = false;
										if (Battle_Manager.money < BOMB_MONEY) {
											money_Effect (2);
											return;
										}
										bomb_Current_CoolDown = 0;
										bomb = Resources.Load<GameObject> ("Model/bomb");
										bomb = GameObject.Instantiate (bomb);
										bomb.transform.localPosition = pos;
                                        bomb.transform.SetParent(Battle_Manager.parent_obj.transform);
                                        for (int j = 0; j < Battle_Manager.enemy_List.Count; j++) {
											if (Vector3.Distance (pos, Battle_Manager.enemy_List [j].transform.localPosition) <= BOMB_RANGE) {
												Battle_Manager.enemy_List [j].change_Health (BOMB_EFFECT, "bomb");
											}
										}
										Destroy (bomb, BOMB_PERIOD);
										Battle_Manager.money -= BOMB_MONEY;
										money_Effect (1);
									}
								} else if (use_Block) {
//                                    Debug.Log("defense, cliked, block");
									if (Battle_Manager.bomb_Pos (pos)) {
										Audio_Manager.PlaySound ("put_block");
										use_Block = false;
										if (Battle_Manager.money < BLOCK_MONEY) {
											money_Effect (2);
											return;
										}
										block_Current_CoolDown = 0;
										GameObject block = Resources.Load<GameObject> ("Model/block");
										block = GameObject.Instantiate (block);
                                        block.transform.SetParent(Battle_Manager.parent_obj.transform);
                                        block.transform.localPosition = pos;
										for (int k = 0; k < Battle_Manager.path.Count; k++) {
											if (Vector3.Distance (Battle_Manager.path [k], pos) < 0.1f) {
												if ((k > 0 && Battle_Manager.path [k - 1].x == Battle_Manager.path [k].x) || (k < Battle_Manager.path.Count - 1 && Battle_Manager.path [k + 1].x == Battle_Manager.path [k].x)) {
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
										money_Effect (1);
									}
								} else if (gift.gameObject.activeSelf && Vector3.Distance (pos, gift.transform.position) < 1f) {
									float rand = Random.value * 10;
									if (rand <= 9) {
										bonus_money.gameObject.SetActive (true);
                                        Text tx = bonus_money.gameObject.GetComponentInChildren<Text>();
                                        tx.text = "" + 10;
										StartCoroutine (disappear_bonus(bonus_money));
										Battle_Manager.money += 10;
										money_Effect (0);
									} else {
										bonus_life.gameObject.SetActive (true);
                                        Text tx = bonus_life.gameObject.GetComponentInChildren<Text>();
                                        tx.text = "" + 1;
                                        StartCoroutine (disappear_bonus(bonus_life));
										Battle_Manager.base_Life++;
										life_Effect (0);
										life.text = "" + Battle_Manager.base_Life;
									}
									gift.gameObject.SetActive (false);
								} else if((target = get_Enemy_Target(pos))!=null && !target.item_Health.pointing_Down.gameObject.activeSelf){
									for (int j = 0; j < Battle_Manager.enemy_List.Count; j++) {
										if (Battle_Manager.enemy_List[j].item_Health.pointing_Down.gameObject.activeSelf) {
											Battle_Manager.enemy_List [j].item_Health.pointing_Down.gameObject.SetActive (false);
										}
									}
									target.item_Health.pointing_Down.gameObject.SetActive (true);
								}

                                else if (Battle_Manager.cur_grows_pos_list.Contains(pos))
                                {
                                    Debug.Log("clicked on grows");
                                    for (int j = 0; j < Battle_Manager.grows_list.Count; j++)
                                    {
                                        if (Battle_Manager.grows_list[j].transform.localPosition.Equals(pos))
                                        {
                                            Debug.Log("list contains the pos");
                                            if (Battle_Manager.grows_status[j] == 1)
                                            {
                                                Debug.Log("grows is muture");
                                                float current_size = Battle_Manager.grows_list[j].transform.localScale.y;
                                                if (Battle_Manager.grows_name_list[j].Equals("cab"))
                                                {
                                                    use_cab_block = true;
                                                    cab_block_time = (int)(CAB_FULL_BLOCK_TIME * current_size / CAB_FULL_SIZE);
                                                    cab_block_money = (int)(CAB_FULL_MONEY * current_size / CAB_FULL_SIZE);
                                                }
                                                else
                                                {
                                                    use_pum_bomb = true;
                                                    pum_bomb_range = (int)(PUM_FULL_BOMB_RANGE * current_size / CAB_FULL_SIZE);
                                                    pum_bomb_power = (int)(PUM_FULL_BOMB_POWER * current_size / CAB_FULL_SIZE);
                                                    pum_bomb_money = (int)(PUM_FULL_MONEY * current_size / CAB_FULL_SIZE);
                                                }
                                                sell_mask.SetActive(false);

                                                GameObject.Destroy(Battle_Manager.grows_list[j]);
                                                Battle_Manager.grows_list.RemoveAt(j);
                                                Battle_Manager.grows_name_list.RemoveAt(j);
                                                Battle_Manager.grows_status.RemoveAt(j);
                                                Battle_Manager.cur_grows_pos_list.Remove(pos);
                                                
                                            }

                                            break;
                                        }
                                    }
                                } 






                                else if ((old_tower_index = Battle_Manager.upgrade_Tower (pos)) != -1) {
									

									string name = Battle_Manager.tower_List [old_tower_index].name;

									if (name.Equals ("tower1") && Battle_Manager.locked_towers [4] ||
									                               name.Equals ("tower2") && Battle_Manager.locked_towers [5] ||
									                               name.Equals ("tower3") && Battle_Manager.locked_towers [6] ||
									                               name.Equals ("tower4") && Battle_Manager.locked_towers [7] ||
									                               name.Equals ("tower1_2") && Battle_Manager.locked_towers [8] ||
									                               name.Equals ("tower2_2") && Battle_Manager.locked_towers [9] ||
									                               name.Equals ("tower3_2") && Battle_Manager.locked_towers [10] ||
									                               name.Equals ("tower4_2") && Battle_Manager.locked_towers [11]) {
										upgrade_btn.SetActive (true);
										upgrade_btn.transform.localPosition = btn_pos;
									}
									return;

								} else if (Battle_Manager.wrong_Pos(pos) == false &&
                                    Vector3.Distance(ori_pos, btn_Bomb.transform.position) > BTN * 0.5f &&
									Vector3.Distance(ori_pos, btn_Block.transform.position) > BTN * 0.5f &&
                                    Vector3.Distance(ori_pos, btn_pum.transform.position) > BTN * 0.5f &&
                                    Vector3.Distance(ori_pos, btn_cab.transform.position) > BTN * 0.5f &&
                                    Vector3.Distance(ori_pos, btn_Help.transform.position) > BTN * 0.5f &&
                                    Vector3.Distance(ori_pos, btn_Sell.transform.position) > BTN * 0.5f)
                                {
                                    //  Debug.Log("defense, cliked, create tower list");
                                   // Debug.Log(btn_pos+", "+ btn_pum.transform.localPosition + ", "+BTN+": "+ Vector3.Distance(btn_pos, btn_pum.transform.localPosition));
                                    tower_Content.SetActive(true);
                                    tower_Content.transform.localPosition = btn_pos;
									tower_Content.GetComponent<GridLayoutGroup>().cellSize=new Vector2(0,0);
									spread_Anim = true;
                                    return;
                                } else {
                                   // Debug.Log("defense, others");
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
                           // Debug.Log("in portal handler");
							if (Battle_Manager.portal_Pos(pos)) {
                                Audio_Manager.PlaySound("put_gate");
								use_Portal = false;
								if (Battle_Manager.money < PORTAL_MONEY)
                                {
									money_Effect (2);
                                    return;
                                }
								portal_Current_CoolDown = 0;
								Portal1 = Resources.Load<GameObject> ("Model/Portal");
								Portal1 = GameObject.Instantiate (Portal1);
								Portal1.transform.localPosition = pos;
								int end_Index = 0;
								for (int j = 0; j < Battle_Manager.path2.Count; j++) {
									if (Vector3.Distance (Battle_Manager.path2 [j], pos) < 0.1f) {
										if((j>0&&Battle_Manager.path2[j-1].x==Battle_Manager.path2[j].x)||(j<Battle_Manager.path2.Count-1&&Battle_Manager.path2[j+1].x==Battle_Manager.path2[j].x)){
											var rotationVector = Portal1.transform.rotation.eulerAngles;
											rotationVector.y = 90;
											Portal1.transform.rotation = Quaternion.Euler (rotationVector);
										}
										end_Index = Mathf.Min (Battle_Manager.path2.Count - 2, j + PORTAL_DISTANCE);
										break;
									}
								}
								Portal2 = Resources.Load<GameObject> ("Model/Portal");
								Portal2 = GameObject.Instantiate (Portal2);
								Portal2.transform.localPosition = Battle_Manager.path2 [end_Index];
								if((end_Index>0&&Battle_Manager.path2[end_Index-1].x==Battle_Manager.path2[end_Index].x)||(end_Index<Battle_Manager.path2.Count-1&&Battle_Manager.path2[end_Index+1].x==Battle_Manager.path2[end_Index].x)){
									var rotationVector = Portal1.transform.rotation.eulerAngles;
									rotationVector.y = 90;
									Portal2.transform.rotation = Quaternion.Euler (rotationVector);
								}
								Battle_Manager.money -= PORTAL_MONEY;
								money_Effect (1);

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
            if (time > 1) Audio_Manager.PlaySound("countdown1");
            else if (time == 1) Audio_Manager.PlaySound("countdown2");
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
	public void bind_Tower(Tower tower){
		GameObject obj = Instantiate (health_Info);
		obj.transform.SetParent (this.transform);
		obj.SetActive (true);
		Health health = obj.AddComponent<Health> ();
		health.init (tower);
	}
	public IEnumerator disappear_bonus(Image img){
		yield return new WaitForSeconds (1);
		img.gameObject.SetActive (false);
	}
	public Enemy get_Enemy_Target(Vector3 pos){
		for (int j = 0; j < Battle_Manager.enemy_List.Count; j++) {
			if (Vector3.Distance (Battle_Manager.enemy_List [j].transform.localPosition, pos) < 1f)
				return Battle_Manager.enemy_List [j];
		}
		return null;
	}
}
