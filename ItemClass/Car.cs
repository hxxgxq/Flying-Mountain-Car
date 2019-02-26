using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int BuyPrice { get; set; }
    public bool IsCollected { get; set; }
    public string Type { get; set; }
    public string Quality { set; get; }
    public double Speed { get; set; }
    public double Acceleration { get; set; }
    public double DashSpeed { get; set; }
    public int CarPiece { get; set; }
    public int Material_01_Quality { get; set; }
    public int Material_02_Quality { get; set; }
    public int Material_03_Quality { get; set; }
    public int Material_04_Quality { get; set; }
    public bool Material_01_Finished { get; set; }
    public bool Material_02_Finished { get; set; }
    public bool Material_03_Finished { get; set; }
    public bool Material_04_Finished { get; set; }
    public Car()
    {
        this.ID = -1;
    }

    public Car(int id, string name, string des, int buyPrice, bool iscollected, string type, string quality, 
        double speed, double acceleration,double dashspeed, int carpiece,
        int material_01_Quality, int material_02_Quality, int material_03_Quality, int material_04_Quality, 
        bool material_01_Finished, bool material_02_Finished, bool material_03_Finished, bool material_04_Finished)
    {
        this.ID = id;
        this.Name = name;
        this.Description = des;
        this.BuyPrice = buyPrice;
        this.IsCollected = iscollected;
        this.Type = type;
        this.Quality = quality;
        this.Speed = speed;
        this.Acceleration = acceleration;
        this.DashSpeed = dashspeed;
        this.CarPiece = carpiece;
        this.Material_01_Quality = material_01_Quality;
        this.Material_02_Quality = material_02_Quality;
        this.Material_03_Quality = material_03_Quality;
        this.Material_04_Quality = material_04_Quality;
        this.Material_01_Finished = material_01_Finished;
        this.Material_02_Finished = material_02_Finished;
        this.Material_03_Finished = material_03_Finished;
        this.Material_04_Finished = material_04_Finished;
    }
}
