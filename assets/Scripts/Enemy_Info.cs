﻿using UnityEngine;
using System.Collections;

public class Enemy_Info {
	public string model;
	public int health;
	public float speed;
	public int number;
	public int money;
	public Enemy_Info(string model,int health,float speed,int number,int money){
		this.model = model;
		this.health = health;
		this.speed = speed;
		this.number = number;
		this.money = money;
	}
}
