using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map_Manager {
	public static bool defense = true;
	public static List<GameObject> path_Box = new List<GameObject>();
	public static GameObject farm;
    //public static List<GameObject> tower_Box = new List<GameObject>();

    // get path
	public static List<Vector3> get_Path(string name){
		for (int i = 0; i < path_Box.Count; i++)
			GameObject.Destroy (path_Box [i]);
		GameObject.Destroy (farm);
		string text = Resources.Load<TextAsset> ("Map/" + name).text;
		string[] path_tower = text.Split ('#');
		string[] path = path_tower[0].Trim('\n').Split('\n');
		List<Vector3> path_Points = new List<Vector3> ();
		List<Vector3> path_Total_Points = new List<Vector3> ();
		for (int i = 0; i < path.Length; i++) {
			string[] point = path [i].Split (',');
			path_Points.Add (new Vector3 (int.Parse (point [0]), 0, int.Parse (point [1])));
		}
		if (path_Points.Count > 0) {
			GameObject box = Resources.Load<GameObject> ("Model/map_box");
			box = GameObject.Instantiate (box);
			Vector3 pos = path_Points [0];
			box.transform.localPosition = pos;
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
					path_Box.Add (box);
					path_Total_Points.Add (pos);
				}
			}
			farm = Resources.Load<GameObject> ("Model/Farm");
			farm = GameObject.Instantiate (farm);
			pos = path_Points [path_Points.Count - 1];
			if (path_Points [path_Points.Count - 2].x == path_Points [path_Points.Count - 1].x) {
				if (path_Points [path_Points.Count - 2].y < path_Points [path_Points.Count - 1].y)
					pos.y += 0.5f;
				else
					pos.y -= 0.5f;
			} else {
				if (path_Points [path_Points.Count - 2].x < path_Points [path_Points.Count - 1].x)
					pos.x += 0.5f;
				else
					pos.x -= 0.5f;
			}
			farm.transform.localPosition = pos;
		}
//		return path_Points;
		return path_Total_Points;
	}

	//get tower 
    public static List<List<Vector3>> get_tower(string name)
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
}
