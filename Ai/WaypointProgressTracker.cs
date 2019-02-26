using System;
using UnityEngine;
using Random = UnityEngine.Random;

//namespace UnityStandardAssets.Utility
//namespace UnityStandardAssets.Vehicles.Car
namespace Scripts.Ai
{
    public class WaypointProgressTracker : MonoBehaviour
    {
        // This script can be used with any object that is supposed to follow a
        // route marked out by waypoints.

        // This script manages the amount to look ahead along the route,
        // and keeps track of progress and laps.

        //[SerializeField] 
        private WaypointCircuit circuit; // A reference to the waypoint-based route we should follow

        [SerializeField] private float lookAheadForTargetOffset = 5;
        // The offset ahead along the route that the we will aim for

        [SerializeField] private float lookAheadForTargetFactor = .1f;
        // A multiplier adding distance ahead along the route to aim for, based on current speed

        [SerializeField] private float lookAheadForSpeedOffset = 10;
        // The offset ahead only the route for speed adjustments (applied as the rotation of the waypoint target transform)

        [SerializeField] private float lookAheadForSpeedFactor = .2f;
        // A multiplier adding distance ahead along the route for speed adjustments

        [SerializeField] private ProgressStyle progressStyle = ProgressStyle.SmoothAlongRoute;
        // whether to update the position smoothly along the route (good for curved paths) or just when we reach each waypoint.

        [SerializeField] private float pointToPointThreshold = 4;
        // proximity to waypoint which must be reached to switch target to next waypoint : only used in PointToPoint mode.
        [SerializeField]
        private float timeforcorrection = 4;           //车辆进度没有增加的最大许可时间，超过该时间，车辆位置重置
        [SerializeField]
        private bool stopAtThefinal=false;              //车辆到达路径终点时是否停下来

        public enum ProgressStyle
        {
            SmoothAlongRoute,
            PointToPoint,
        }

        // these are public, readable by other objects - i.e. for an AI to know where to head!
        public WaypointCircuit.RoutePoint targetPoint { get; private set; }
        public WaypointCircuit.RoutePoint speedPoint { get; private set; }
        public WaypointCircuit.RoutePoint progressPoint { get; private set; }

        //public Transform target;
        private Transform target;
        public Transform getTarget() { return target; }

        private float progressDistance=0; // The progress round the route, used in smooth mode.
        private int progressNum; // the current waypoint number, used in point-to-point mode.
        private Vector3 lastPosition; // Used to calculate current speed (since we may not have a rigidbody component)
        private float speed; // current speed of this object (calculated from delta since last frame)
        private float stoptime=0;//当前车辆进度未增加的持续时间   
        private String circleTag;
        private float changeDelay = 0;//记录换道延迟，当且仅当换道之后一段时间后，才可以再次换道

        // setup script properties
        private void Start()
        {
            circuit = GameObject.FindGameObjectWithTag("road").GetComponent<WaypointCircuit>();
            circleTag = "road";

            // we use a transform to represent the point to aim for, and the point which
            // is considered for upcoming changes-of-speed. This allows this component
            // to communicate this information to the AI without requiring further dependencies.

            // You can manually create a transform and assign it to this component *and* the AI,
            // then this component will update it, and the AI can read it.
            if (target == null)
            {
                //target = new GameObject(name + " Waypoint Target").transform;
                target=new GameObject().transform;
                //Debug.Log("target");
            }
            Reset();
        }


        // reset the object to sensible values
        public void Reset()
        {
            progressDistance = 0;
            progressNum = 0;
            if (progressStyle == ProgressStyle.PointToPoint)
            {
                target.position = circuit.Waypoints[progressNum].position;
                target.rotation = circuit.Waypoints[progressNum].rotation;
            }
        }


        private void Update()
        {

            changeDelay += Time.deltaTime;//记录时间，用于防止频繁换道
            //如果到达目标就停止，那就不会在更新目标了???
            if (progressStyle == ProgressStyle.SmoothAlongRoute)
            {
                // determine the position we should currently be aiming for
                // (this is different to the current progress position, it is a a certain amount ahead along the route)
                // we use lerp as a simple way of smoothing out the speed over time.
                if (Time.deltaTime > 0)
                {
                    speed = Mathf.Lerp(speed, (lastPosition - transform.position).magnitude/Time.deltaTime,
                                       Time.deltaTime);
                }
                target.position =
                    circuit.GetRoutePoint(progressDistance + lookAheadForTargetOffset + lookAheadForTargetFactor*speed)
                           .position;
                target.rotation =
                    Quaternion.LookRotation(
                        circuit.GetRoutePoint(progressDistance + lookAheadForSpeedOffset + lookAheadForSpeedFactor*speed)
                               .direction);
                // get our current progress along the route
                progressPoint = circuit.GetRoutePoint(progressDistance);
                Vector3 progressDelta = progressPoint.position - transform.position;
                if (Vector3.Dot(progressDelta, progressPoint.direction) < 0&&!GetComponent<CarAIController>().iffly())//此处控制进度增加，根据是角度，需要对转弯处理，飞行图中不进行进度增加
                {

                    progressDistance += progressDelta.magnitude*0.5f;
                    stoptime = 0;//车辆进度增加，重置停留时间
                    //Debug.Log("update progress");
                    //Debug.Log(progressDistance);
                }
                stoptime += Time.deltaTime;
                lastPosition = transform.position;//记录上一次的位置(不管有没有重置都会记录)                
            }
            else
            {
                // point to point mode. Just increase the waypoint if we're close enough:

                Vector3 targetDelta = target.position - transform.position;
                if (targetDelta.magnitude < pointToPointThreshold)
                {
                    progressNum = (progressNum + 1)%circuit.Waypoints.Length;
                }


                target.position = circuit.Waypoints[progressNum].position;
                target.rotation = circuit.Waypoints[progressNum].rotation;

                // get our current progress along the route
                progressPoint = circuit.GetRoutePoint(progressDistance);
                Vector3 progressDelta = progressPoint.position - transform.position;
                if (Vector3.Dot(progressDelta, progressPoint.direction) < 0)
                {
                    progressDistance += progressDelta.magnitude;
                    stoptime = 0;
                }
                lastPosition = transform.position;
                stoptime += Time.deltaTime;
            }
            //位置重置
            correction();
        }

       
        private void OnDrawGizmos()
        {
            //Debug.Log("hua");
            if (Application.isPlaying)
            {
                Gizmos.color = Color.green;
                //Gizmos.DrawLine(transform.position, target.position);
                //Gizmos.DrawWireSphere(circuit.GetRoutePosition(progressDistance), 1);
                //Gizmos.color = Color.yellow;
                //Gizmos.DrawLine(target.position, target.position + target.forward);
            }
        }


        //该函数跟踪当前进度，若是在行驶期间车辆有持续3秒进程没有增加，则将车辆重置到进度处。此方法需要汽车路线在合适的位置上
        private void correction()
        {
           if(stoptime>=timeforcorrection&&GetComponent<CarAIControl2>().m_Driving==true)//车辆停滞时间超过3秒
           {  
               Collider[] col=Physics.OverlapSphere(progressPoint.position, 3);
               bool nocar=true;
              /* if (col != null)
               {
                   foreach (var item in col)
                   {
                       //此处的tag的处理策略？
                       if (item.transform.root.tag == "Player" || item.transform.root.tag == "AI")
                       {
                           Debug.Log(" time is ok ,cannot reborn");
                           nocar = false;
                           break;
                       }
                   }
               }*/
               if (nocar == true)
               {
                   Debug.Log("reborn");                   
                   //当此处没有车辆时，将汽车回放到此处，汽车处于驾驶状态时才会重生
                   transform.position = target.position;
                   transform.rotation = target.rotation;
                   //GetComponent<Rigidbody>().velocity = Vector3.one;//是否与运动机制矛盾
                   GetComponent<CarAIController>().Move(0, 0, -1, 0);
                   //Debug.Log(transform.position);
                   lastPosition = transform.position;
                   stoptime = 0;
                   GetComponent<CarAIControl2>().closeforce();
               }
           }
           if (GetComponent<CarAIControl2>().m_Driving == false)
           {
               stoptime = 0;
           }
        }
        //遇到择路点，有几率切换道路
        public void OnTriggerStay(Collider col)
        {
            float chang = GetComponent<CarAIControl2>().changRoadfactor;//获取换道几率
            //产生随机数（0-1），如果小于切换几率，那么切换道路
            //注意在停留期间会不断的切换
            if (col.tag.Equals("selectRoadPoint")&&changeDelay>3)//换道后十秒内不可以再次换道
            {
                changeDelay = 0;
                if (Random.value <= chang)
                {
                    if (circleTag=="road")
                    {
                        Debug.Log("change to close");
                        circuit = GameObject.FindGameObjectWithTag("closeroad").GetComponent<WaypointCircuit>();
                        circleTag = "closeroad";
                    }
                    else
                    {
                        Debug.Log("change to normal");
                        circuit = GameObject.FindGameObjectWithTag("road").GetComponent<WaypointCircuit>();
                        circleTag = "road";
                    }
                    Reset();
                }
            }
        }
    }
}
