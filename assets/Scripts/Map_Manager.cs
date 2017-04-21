using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map_Manager {
	public static bool defense = true;
	public static List<GameObject> path_Box = new List<GameObject>();
	public static List<GameObject> path_Box_Alternate = new List<GameObject> ();
	public static GameObject farm;
    public static GameObject enemyBase;
    public static List<GameObject> path_Box2 = new List<GameObject>();
    public static List<GameObject> build_pos_box = new List<GameObject>();
    //public static List<GameObject> tower_Box = new List<GameObject>();
    public static GameObject parent_obj = GameObject.Find("map_parent_node");
    public static List<Vector2> grows_pos_coors = new List<Vector2>();
    // get grows pos coordinate

    public static void get_grows_coors(string name)
    {
        string text = Resources.Load<TextAsset>("Map/" + name).text;
        string[] path_tower = text.Split('#');
        string[] path = path_tower[6].Trim('\n').Trim().Split('\n');
        for (int i = 0;  i < path.Length;i++)
        {
            string[] coors = path[i].Split(',');
            grows_pos_coors.Add(new Vector2(int.Parse(coors[0]), int.Parse(coors[1])));
        }
    }

    // get path
    public static List<Vector3> get_Path(string name){
		for (int i = 0; i < path_Box.Count; i++)
			GameObject.Destroy (path_Box [i]);
		GameObject.Destroy (farm);
		string text = Resources.Load<TextAsset> ("Map/" + name).text;
		string[] path_tower = text.Split ('#');
		string[] path = path_tower[0].Trim('\n').Split('\n');
        //Debug.Log(path);
        List<Vector3> path_Points = new List<Vector3> ();
		List<Vector3> path_Total_Points = new List<Vector3> ();
		for (int i = 0; i < path.Length; i++) {
			string[] point = path [i].Split (',');
			path_Points.Add (new Vector3 (int.Parse (point [0]), 0, int.Parse (point [1])));
		}
		if (path_Points.Count > 0) {
			GameObject box = Resources.Load<GameObject> ("Model/env/path");
			box = GameObject.Instantiate (box);
			Vector3 pos = path_Points [0];
			box.transform.localPosition = pos;
            box.transform.SetParent(parent_obj.transform);
            path_Box.Add (box);
			path_Total_Points.Add (pos);
			for (int i = 1; i < path_Points.Count; i++) {
				while (pos.x != path_Points [i].x) {
					if (pos.x > path_Points [i].x)
						pos.x--;
					else
						pos.x++;
					box = GameObject.Instantiate (box);
					box.transform.localPosition = pos;
                    box.transform.SetParent(parent_obj.transform);
                    path_Box.Add (box);
					path_Total_Points.Add (pos);
				}
				while (pos.z != path_Points [i].z) {
					if (pos.z > path_Points [i].z)
						pos.z--;
					else
						pos.z++;
					box = GameObject.Instantiate (box);
					box.transform.localPosition = pos;
                    box.transform.SetParent(parent_obj.transform);
                    path_Box.Add (box);
					path_Total_Points.Add (pos);
				}
			}
			farm = Resources.Load<GameObject> ("Model/env/Building_Cottage_03");
			farm = GameObject.Instantiate (farm);
			pos = path_Points [path_Points.Count - 1];
			if (path_Points [path_Points.Count - 2].x == path_Points [path_Points.Count - 1].x) {
				if (path_Points [path_Points.Count - 2].y < path_Points [path_Points.Count - 1].y)
                {
                    
                    farm.transform.localRotation = Quaternion.Euler(0, 270, 0);
                    pos.z -= 3f;
                }
				else
                {
                    
                    farm.transform.localRotation = Quaternion.Euler(0, 90, 0);
                    pos.z += 3f;
                }
			} else {
				if (path_Points [path_Points.Count - 2].x < path_Points [path_Points.Count - 1].x)
                {
                    
                    farm.transform.localRotation = Quaternion.Euler(0, 180, 0);
                    pos.x += 3f;
                }
				else
					pos.x -= 3f;
			}
			farm.transform.localPosition = pos;
            farm.transform.SetParent(parent_obj.transform);
		}
//		return path_Points;
		return path_Total_Points;
	}

	public static List<Vector3> get_Path_Alternate(string name){
		for (int i = 0; i < path_Box_Alternate.Count; i++)
			GameObject.Destroy (path_Box_Alternate [i]);
		string text = Resources.Load<TextAsset> ("Map/" + name).text;
		string[] path_tower = text.Split ('#');
		string[] path = path_tower[7].Trim('\n').Trim().Split('\n');
		//Debug.Log(path);
		List<Vector3> path_Points = new List<Vector3> ();
		List<Vector3> path_Total_Points = new List<Vector3> ();
		for (int i = 0; i < path.Length; i++) {
			string[] point = path [i].Split (',');
			path_Points.Add (new Vector3 (int.Parse (point [0]), 0, int.Parse (point [1])));
		}
		if (path_Points.Count > 0) {
			GameObject box = Resources.Load<GameObject> ("Model/env/path");
			box = GameObject.Instantiate (box);
			Vector3 pos = path_Points [0];
			box.transform.localPosition = pos;
			box.transform.SetParent(parent_obj.transform);
			path_Box_Alternate.Add (box);
			path_Total_Points.Add (pos);
			for (int i = 1; i < path_Points.Count; i++) {
				while (pos.x != path_Points [i].x) {
					if (pos.x > path_Points [i].x)
						pos.x--;
					else
						pos.x++;
					box = GameObject.Instantiate (box);
					box.transform.localPosition = pos;
					box.transform.SetParent(parent_obj.transform);
					path_Box_Alternate.Add (box);
					path_Total_Points.Add (pos);
				}
				while (pos.z != path_Points [i].z) {
					if (pos.z > path_Points [i].z)
						pos.z--;
					else
						pos.z++;
					box = GameObject.Instantiate (box);
					box.transform.localPosition = pos;
					box.transform.SetParent(parent_obj.transform);
					path_Box_Alternate.Add (box);
					path_Total_Points.Add (pos);
				}
			}
//			pos = path_Points [path_Points.Count - 1];
		}
		//		return path_Points;
		return path_Total_Points;
	}

    // get path of attack
    public static List<Vector3> get_Path2(string name) 
    {
        for (int i = 0; i < path_Box2.Count; i++)
            GameObject.Destroy(path_Box2[i]);
		path_Box2.Clear ();
        GameObject.Destroy(enemyBase);
        string text = Resources.Load<TextAsset>("Map/" + name).text;
        string[] path_tower = text.Split('#');
        //Debug.Log(path_tower[5]);
		string[] path = path_tower[5].Trim('\n').Trim().Split('\n');
        
        List<Vector3> path_Points = new List<Vector3>();
        List<Vector3> path_Total_Points = new List<Vector3>();
        for (int i = 0; i < path.Length; i++)
        {
            string[] point = path[i].Split(',');
            path_Points.Add(new Vector3(int.Parse(point[0]), 0, int.Parse(point[1])));
        }
        if (path_Points.Count > 0)
        {
            GameObject box = Resources.Load<GameObject>("Model/env/path");
            box = GameObject.Instantiate(box);
            Vector3 pos = path_Points[0];
            box.transform.localPosition = pos;
            box.transform.SetParent(parent_obj.transform);
            path_Box2.Add(box);
            path_Total_Points.Add(pos);
            for (int i = 1; i < path_Points.Count; i++)
            {
                while (pos.x != path_Points[i].x)
                {
                    if (pos.x > path_Points[i].x)
                        pos.x--;
                    else
                        pos.x++;
                    box = GameObject.Instantiate(box);
                    box.transform.localPosition = pos;
                    box.transform.SetParent(parent_obj.transform);
                    path_Box2.Add(box);
                    path_Total_Points.Add(pos);
                }
                while (pos.z != path_Points[i].z)
                {
                    if (pos.z > path_Points[i].z)
                        pos.z--;
                    else
                        pos.z++;
                    box = GameObject.Instantiate(box);
                    box.transform.localPosition = pos;
                    box.transform.SetParent(parent_obj.transform);
                    path_Box2.Add(box);
                    path_Total_Points.Add(pos);
                }
            }
            enemyBase = Resources.Load<GameObject>("Model/env/SF_Bld_Undead_Castle_Keep_01");
            enemyBase = GameObject.Instantiate(enemyBase);
            pos = path_Points[path_Points.Count - 1];
            if (path_Points[path_Points.Count - 2].x == path_Points[path_Points.Count - 1].x)
            {
                if (path_Points[path_Points.Count - 2].y < path_Points[path_Points.Count - 1].y)
                    pos.z -= 3f;
                else
                    pos.z += 3f;
            }
            else
            {
                if (path_Points[path_Points.Count - 2].x < path_Points[path_Points.Count - 1].x)
                    pos.x += 3f;
                else
                    pos.x -= 3f;
            }
            enemyBase.transform.localPosition = pos;
            enemyBase.transform.SetParent(parent_obj.transform);
        }
        //		return path_Points;
        return path_Total_Points;
    }

	//get tower 
    public static List<List<Vector3>> get_enemy_tower(string name)
    {
        //for (int i = 0; i < tower_Box.Count; i++) GameObject.Destroy(tower_Box[i]);

        string text = Resources.Load<TextAsset>("Map/" + name).text;
        string[] path_tower = text.Split('#');

        List<List<Vector3>> tower_list = new List<List<Vector3>>();
        int tower_num = 1;
        while (tower_num <= 4)
        {
            //Debug.Log("##"+path_tower[tower_num].Trim('\n') + "##");
             

			string path_all = path_tower [tower_num].Trim ().Trim ('\n');
            List<Vector3> tower_Points = new List<Vector3>();
            if (path_all.Length > 0) {
                string[] path = path_all.Split('\n');
                for (int i = 0; i < path.Length; i++)
                {
                    string[] point = path[i].Split(',');
                    tower_Points.Add(new Vector3(int.Parse(point[0]), 0, int.Parse(point[1])));
                }
            }
            
            tower_list.Add(tower_Points);
			tower_num++;
        }
        return tower_list;
    }

    // get build_pos
    /*public static List<Vector3> get_build_pos(string name)
    {
        for (int i = 0; i < build_pos_box.Count; i++)
            GameObject.Destroy(build_pos_box[i]);
        build_pos_box.Clear();
        List<Vector3> list = new List<Vector3>();
        string text = Resources.Load<TextAsset>("Map/" + name).text;
        string[] all = text.Split('#');
        string[] poss = all[6].Trim('\n').Trim().Split('\n');
        for (int i = 0; i < poss.Length; i++)
        {
            string[] xz = poss[i].Split(',');
            Vector3 v3 = new Vector3(int.Parse(xz[0]),0, int.Parse(xz[1]));
            list.Add(v3);
            GameObject box = Resources.Load<GameObject>("Model/build_pos");
            box = GameObject.Instantiate(box);
            Vector3 pos = v3;
            box.transform.localPosition = pos;
            box.transform.SetParent(parent_obj.transform);
            build_pos_box.Add(box);
        }
        return list;
    } */
}
