using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
public class ShowCarUI : MonoBehaviour
{
    public Text piecenum;
    public Text carname;
    Car car;
    private void Start()
    {

        showCarUI();
    }
    public void showCarUI()
    {
        string name = gameObject.name;
        string id = System.Text.RegularExpressions.Regex.Replace(name, @"[^0-9]+", "");
        car = CarManager.Instance.GetCarById((int.Parse(id)));
        carname.text = car.Name;
        IsCarCollected();
        if (car.IsCollected)
        {
            Image image = gameObject.GetComponent<Image>();
            Sprite sp = Resources.Load("Textrues/Cars/car_" + id, typeof(Sprite)) as Sprite;
            image.sprite = sp;
            image.color = new Vector4(1, 1, 1, 1.0f);
            piecenum.text = null;
            gameObject.GetComponent<Button>().enabled = true;
            
        }
        else
        {
            Image image = gameObject.GetComponent<Image>();
            Sprite sp = Resources.Load("Textrues/Cars/car_" + id, typeof(Sprite)) as Sprite;
            image.sprite = sp;
            image.color = new Vector4(1, 1, 1, 0.5f);
            gameObject.GetComponent<Button>().enabled = false;
            if (car.Quality == "normal")
            {
               
                piecenum.text = car.CarPiece.ToString() + "/5";
            }
            else if (car.Quality == "rare")
            {

                piecenum.text = car.CarPiece.ToString() + "/10";
            }
            else if (car.Quality == "epic")
            {

                piecenum.text = car.CarPiece.ToString() + "/15";
            }
            else if (car.Quality == "legendary")
            {

                piecenum.text = car.CarPiece.ToString() + "/20";
            }
        }

    }
    public void IsCarCollected()
    {
        if (car.Quality == "normal")
        {
            if (car.CarPiece >= 5)
                car.IsCollected = true;
            else
                car.IsCollected = false;
        }
        else if (car.Quality == "rare")
        {
            if (car.CarPiece >= 10)
                car.IsCollected = true;
            else
                car.IsCollected = false;
        }
        else if (car.Quality == "epic")
        {
            if (car.CarPiece >= 15)
                car.IsCollected = true;
            else
                car.IsCollected = false;
        }
        else if (car.Quality == "legendary")
        {
            if (car.CarPiece >= 20)
                car.IsCollected = true;
            else
                car.IsCollected = false;
        }
    }
}
  