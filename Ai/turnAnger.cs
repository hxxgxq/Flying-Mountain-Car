using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Ai
{
    public class turnAnger : MonoBehaviour
    {
        //记录转角大小，
        [SerializeField]
        public float anger = 70;
        [SerializeField]
        public float direction = -1;//左转为负，右转为正

    }
}