using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map_Manager {
	public static bool defense = true;
	public static List<GameObject> path_Box = new List<GameObject>();
    //public static List<GameObject> tower_Box = new List<GameObject>();

    // get path
	public static List<Vector3> get_Path(string name){
		for (int i = 0; i < path_Box.Count; i++)
			GameObject.Destroy (path_Box [i]);
		string text = Resources.Load<TextAsset> ("Map/" + name).text;
		string[] path_tower = text.Split ('#');
		string[] path = path_tower[0].Trim('\n').Split('\n');
		List<Vector3> path_Points = new List<Vector3> ();
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
			for (int i = 1; i < path_Points.Count; i++) {
				while (pos.x != path_Points [i].x) {
					if (pos.x > path_Points [i].x)
						pos.x--;
					else
						pos.x++;
					box = GameObject.Instantiate (box);
					box.transform.localPosition = pos;
					path_Box.Add (box);
				}
				while (pos.z != path_Points [i].z) {
					if (pos.z > path_Points [i].z)
						pos.z--;
					else
						pos.z++;
					box = GameObject.Instantiate (box);
					box.transform.localPosition = pos;
					path_Box.Add (box);
				}
			}
		}
		return path_Points;
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
			if (path_all.Length > 0) {
				string[] path = path_all.Split('\n');
				List<Vector3> tower_Points = new List<Vector3>();
				for (int i = 0; i < path.Length; i++)
				{
					string[] point = path[i].Split(',');

					//Debug.Log(path[i]);
					tower_Points.Add(new Vector3(int.Parse(point[0]), 0, int.Parse(point[1])));
				}
				/*if (tower_Points.Count > 0)
            {
                for (int i = 0; i < tower_Points.Count; i++)
                {
                    GameObject tower = Resources.Load<GameObject>("Model/tower" + tower_num);
                    tower = GameObject.Instantiate(tower);
                    Vector3 pos = tower_Points[i];
                    tower.transform.localPosition = pos;
                    tower_Box.Add(tower);
                }
            }*/
				tower_list.Add(tower_Points);
				tower_num++;
			} else
				return null;

            
        }
        return tower_list;
    }
}
