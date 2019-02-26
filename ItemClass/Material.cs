using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material 
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int BuyPrice { get; set; }
    public int SellPrice { get; set; }
    public int Capacity { get; set; }
    public string Type { get; set; }
    public string Quality { set; get; }
    public string Sprite { get; set; }
    public double SpeedIncrease { get; set; }
    public double AccelerationIncrease { get; set; }
    public double DashSpeedIncrease { get; set; }
    public Material()
    {
        this.ID = -1;
    }
    public Material(int id, string name, string des, int buyPrice, int sellPrice, 
        int capacity, string type, string quality,string sprite,double speedIncrease,
        double accelerationIncrease,double dashSpeedIncrease)
    {
        this.ID = id;
        this.Name = name;
        this.Description = des;
        this.BuyPrice = buyPrice;
        this.SellPrice = sellPrice;
        this.Capacity = capacity;
        this.Type = type;
        this.Quality = quality;
        this.Sprite = sprite;
        this.SpeedIncrease = speedIncrease;
        this.AccelerationIncrease = accelerationIncrease;
        this.DashSpeedIncrease = dashSpeedIncrease;
    }

}
