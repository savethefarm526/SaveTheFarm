using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
    // Use this for initialization
    void Start () {
        Time.timeScale = 1;
        Audio_Manager.PlayMusic("music_nobattle");
		Camera.main.transform.localPosition = new Vector3(1f, 7f, -4f);
		Camera.main.transform.localRotation = Quaternion.Euler(60, 0, 0);
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
                return new Tower_Info("tower1_2", "tower1_2", 3, 1.6f, 2, "", "bullet1", 1, 18, 10);
            case "tower2":
                return new Tower_Info("tower2_2", "tower2_2", 4, 1.6f, 2, "", "bullet2", 1, 23, 10);
            case "tower3":
                return new Tower_Info("tower3_2", "tower3_2", 4, 1.6f, 3, "", "bullet3", 5, 23, 10);
            case "tower4":
                return new Tower_Info("tower4_2", "tower4_2", 5, 1.6f, 5, "", "bullet4", 1, 28, 10);
            case "tower1_2":
                return new Tower_Info("tower1_3", "tower1_3", 4, 1.4f, 2, "", "bullet1", 1, 20, 10);
            case "tower2_2":
                return new Tower_Info("tower2_3", "tower2_3", 5, 1.4f, 2, "", "bullet2", 1, 28, 10);
            case "tower3_2":
                return new Tower_Info("tower3_3", "tower3_3", 5, 1.4f, 3, "", "bullet3", 5, 28, 10);
            case "tower4_2":
                return new Tower_Info("tower4_3", "tower4_3", 6, 1.4f, 5, "", "bullet4", 1, 40, 10);
        }
        return null;
    }
    // Update is called once per frame
    void Update () {
	
	}
}
