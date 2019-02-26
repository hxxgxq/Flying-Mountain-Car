using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Ai
{
    public class Getprop : MonoBehaviour
    {
        public string[] proplist = new string[2];
        bool isChange = false;
        bool getProp = false;
        string propName = "";

        void Start()
        {
            proplist[0] = "";
            proplist[1] = "";
        }

        /// 碰撞检测
        /// </summary>
        /// <param name="prop"></param>
        void OnTriggerEnter(Collider prop)
        {
            if (prop.gameObject.tag == "tool")
            {
                getProp = true;
            }
            if (prop.gameObject.tag == "Nitrogen")
            {
                UseProp("Nitrogen");
                Destroy(prop.gameObject);
            }
            if (getProp)
            {
                Destroy(prop.gameObject);
                Debug.Log("获得道具");
                getProp = false;
                propName = RandomProp();

                //闪电风暴和氮气直接用
                if (propName == "ThunderStorm" || propName == "Nitrogen")
                    UseProp(propName);
                else if (propName != proplist[0] && propName != proplist[1])
                {
                    if (proplist[0] == "")
                    {
                        proplist[0] = propName;
                    }
                    else if (proplist[1] == "")
                    {
                        proplist[1] = propName;
                    }
                    else
                    {
                        proplist[0] = proplist[1];
                        proplist[1] = propName;
                    }
                }
                Debug.Log(propName);
            }
        }
        /// 随机获得一个道具
        /// </summary>
        /// <returns 道具名称></returns>
        string RandomProp()
        {
            string propName = "";
            switch (Random.Range(1, 5))
            {
                case 1:
                    propName = "Shield";
                    break;
                case 2:
                    propName = "ThunderStorm";
                    break;
                case 3:
                    propName = "Wing";
                    break;
                case 4:
                    propName = "Barry";
                    break;
                case 5:
                    propName = "Nitrogen";
                    break;
            }
            return propName;
        }
        /// <summary>
        /// 使用道具
        /// </summary>
        /// <param name="propname_1"></param>
        public void UseProp(string propname_1)
        {
            if (propname_1 == "Shield")
            {

            }
            if (propname_1 == "Wing")
            {
                GetComponent<Rigidbody>().mass = GetComponent<CarAIControl2>().m_mass / 3;
            }
            if (propname_1 == "Barry")
            {
                gameObject.GetComponent<Properties>().BarrierInstantiate();
            }
            if (propname_1 == "ThunderStorm")
            {
                gameObject.GetComponent<Properties>().ThunderStorm();
            }
            if(propname_1=="Nitrogen")
            {
                GetComponent<CarAIControl2>().danqi += 0.5f;
            }
        }
    }
}
