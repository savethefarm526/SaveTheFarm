using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
    // Use this for initialization
    void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        UI_Manager.Enter<UI_Start> ();

	}
	public static Tower_Info getNewTowerInfo(string old_name)
    {

        /*
        towers.Add(new Tower_Info("tower1_2", "tower1_2", 2, 0.8f, 2, "", "bullet", 10, 18));
        towers.Add(new Tower_Info("tower2_2", "tower2_2", 3, 0.9f, 2, "", "bullet", 10, 23));
        towers.Add(new Tower_Info("tower3_2", "tower3_2", 2, 0.5f, 3, "", "bullet", 10, 23));
        towers.Add(new Tower_Info("tower4_2", "tower4_2", 3, 0.8f, 5, "", "bullet", 10, 28));

        towers.Add(new Tower_Info("tower1_3", "tower1_3", 3, 0.8f, 2, "", "bullet", 10, 20));
        towers.Add(new Tower_Info("tower2_3", "tower2_3", 4, 0.9f, 2, "", "bullet", 10, 28));
        towers.Add(new Tower_Info("tower3_3", "tower3_3", 3, 0.5f, 3, "", "bullet", 10, 28));
        towers.Add(new Tower_Info("tower4_3", "tower4_3", 4, 0.5f, 5, "", "bullet", 10, 40));
                 */
        switch (old_name)
        {
            case "tower1":
                return new Tower_Info("tower1_2", "tower1_2", 2, 0.8f, 2, "", "bullet", 10, 18);
            case "tower2":
                return new Tower_Info("tower2_2", "tower2_2", 3, 0.9f, 2, "", "bullet", 10, 23);
            case "tower3":
                return new Tower_Info("tower3_2", "tower3_2", 2, 0.5f, 3, "", "bullet", 10, 23);
            case "tower4":
                return new Tower_Info("tower4_2", "tower4_2", 3, 0.8f, 5, "", "bullet", 10, 28);
            case "tower1_2":
                return new Tower_Info("tower1_3", "tower1_3", 3, 0.8f, 2, "", "bullet", 10, 20);
            case "tower2_2":
                return new Tower_Info("tower2_3", "tower2_3", 4, 0.9f, 2, "", "bullet", 10, 28);
            case "tower3_2":
                return new Tower_Info("tower3_3", "tower3_3", 3, 0.5f, 3, "", "bullet", 10, 28);
            case "tower4_2":
                return new Tower_Info("tower4_3", "tower4_3", 4, 0.5f, 5, "", "bullet", 10, 40);
        }
        return null;
    }
    // Update is called once per frame
    void Update () {
	
	}
}
