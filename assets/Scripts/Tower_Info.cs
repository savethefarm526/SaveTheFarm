using UnityEngine;
using System.Collections;

public class Tower_Info {
	public string name;
	public string model;
	public int power;
	public float period;
	public float range;
	public string animation;
	public string bullet;
	public float bullet_spd;
	public int money;
	public float health;
	public Tower_Info(string name,string model,int power,float period,float range,string animation,string bullet,float bullet_spd,int money, float health){
		this.name = name;
		this.model = model;
		this.power = power;
		this.period = period;
		this.range = range;
		this.animation = animation;
		this.bullet = bullet;
		this.bullet_spd = bullet_spd;
		this.money = money;
		this.health = health;
	}
}
