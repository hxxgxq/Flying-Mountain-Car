using UnityEngine;  
using System.Collections;  
using UnityEngine.UI;  
using UnityEngine.EventSystems;  
using UnityEngine.Events;
public class InputListener : MonoBehaviour
{
    private GameObject currentObject;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null)
        {
            currentObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            //Debug.Log(currentObject.name);
            if (currentObject.name.Length >= 4 && currentObject.name.Substring(0, 4) == "Car_")
            {
                string name = currentObject.name;
                string id = System.Text.RegularExpressions.Regex.Replace(name, @"[^0-9]+", "");
                PlayerPrefs.SetString("ModifyCarID", id);
            }
            if (currentObject.name.Length == 16 && currentObject.name.Substring(0, 14) == "Modify_button_")
            {
                string name = currentObject.name;
                string id = System.Text.RegularExpressions.Regex.Replace(name, @"[^0-9]+", "");
                PlayerPrefs.SetString("ModifyMaterialID", id);
                //Debug.Log(PlayerPrefs.GetString("ModifyMaterialID"));
            }

        }
    }
    public GameObject GetCurrentObject(GameObject gameobject)
    {
        return gameobject;
    }
}