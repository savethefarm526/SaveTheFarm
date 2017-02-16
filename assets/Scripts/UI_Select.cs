using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Select : UI_Base {

    private bool isDefence = true;
    public void init(bool isDefence)
    {
        this.isDefence = isDefence;
    }


	public override void button_Click(string name,GameObject obj){
        
		if (name.Equals ("Btn_Back")) {
			UI_Manager.Exit (this);
			UI_Manager.Enter<UI_Start> ();
		} else {
			UI_Manager.Exit (this);
			init_Battle (name.Split (' ') [1]);
		}
	}
	public void init_Battle(string level){

        
        List<Tower_Info> towers = new List<Tower_Info>();

        towers.Add(new Tower_Info("tower1", "tower1", 1, 0.8f, 2, "", "bullet", 10));
        if (int.Parse(level) > 1)
            towers.Add(new Tower_Info("tower2", "tower2", 2, 0.8f, 2, "", "bullet", 10));
        if (int.Parse(level) > 2)
            towers.Add(new Tower_Info("tower3", "tower3", 3, 0.8f, 3, "", "bullet", 10));
        if (int.Parse(level) > 3)
            towers.Add(new Tower_Info("tower4", "tower4", 3, 0.8f, 5, "", "bullet", 10));


        List<Enemy_Info> enemies = new List<Enemy_Info>();
        
        switch(level)
        {
            case "1":
                enemies.Add(new Enemy_Info("enemy1", 2, 5, 10));
                enemies.Add(new Enemy_Info("enemy2", 5, 2, 10));
                enemies.Add(new Enemy_Info("enemy1", 2, 5, 10));
                enemies.Add(new Enemy_Info("enemy2", 5, 2, 10));
                enemies.Add(new Enemy_Info("enemy1", 2, 5, 10));
                enemies.Add(new Enemy_Info("enemy2", 5, 2, 10));
                enemies.Add(new Enemy_Info("enemy1", 2, 5, 10));
                enemies.Add(new Enemy_Info("enemy3", 10, 1, 5));
                enemies.Add(new Enemy_Info("enemy4", 20, 1, 3));
                enemies.Add(new Enemy_Info("enemy4", 20, 1, 3));
                enemies.Add(new Enemy_Info("enemy4", 20, 1, 3));
                enemies.Add(new Enemy_Info("enemy2", 5, 2, 10));
                enemies.Add(new Enemy_Info("enemy1", 2, 5, 10));
                enemies.Add(new Enemy_Info("enemy2", 5, 2, 10));
                enemies.Add(new Enemy_Info("enemy3", 10, 1, 5));
                enemies.Add(new Enemy_Info("enemy4", 20, 1, 3));
                break;
            case "2":
                enemies.Add(new Enemy_Info("enemy3", 10, 1, 5));
                enemies.Add(new Enemy_Info("enemy4", 20, 1, 3));
                break;
        }

       

        
        UI_Manager.Enter<UI_Battle>().init(level, towers, enemies, isDefence);
        
		
	}
}
