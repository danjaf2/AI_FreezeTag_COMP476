using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AI
{
    public class AIAgent : MonoBehaviour
    {
        public static List<AIAgent> aliveList = new List<AIAgent>();
        public static List<AIAgent> seekerList = new List<AIAgent>();
        public float maxSpeed;
        public float maxDegreesDelta;
        public bool lockY = true;
        public bool isFrozen = false;
        public bool canGroup = false;
        public bool isPlayer = false;
        public bool isGroupLeader = false;
        public bool chasingCoin = false;
        public bool move = true;
        public bool debug;
        public GameObject wings;
        public GameObject wings2;

        public GameObject freezeBomb;
        public int ammoBomb=2;

        public static GameObject seekersWin = null;

        public bool isSeeker = false;
        public AIAgent AIAgentTarget;

        public AIAgent AIAgentRescue;

        public GameObject SeekerPrefab;

        public enum EBehaviorType { Kinematic, Steering }
        public EBehaviorType behaviorType;

        private Animator animator;

        [SerializeField] public Node trackedTarget;
        [SerializeField] public Vector3 targetPosition;
        public Vector3 TargetPosition
        {
            get => trackedTarget != null ? trackedTarget.transform.position : targetPosition;
            set { }
        }
        public Vector3 TargetForward
        {
            get => trackedTarget != null ? trackedTarget.transform.forward : Vector3.forward;
            set { }
        }
        public Vector3 TargetVelocity
        {
            get
            {
                Vector3 v = Vector3.zero;
                if (trackedTarget != null)
                {
                    AIAgent targetAgent = trackedTarget.GetComponent<AIAgent>();
                    if (targetAgent != null)
                        v = targetAgent.Velocity;
                }

                return v;
            }
            set { }
        }

        public Vector3 Velocity { get; set; }

        //public void TrackTarget(Transform targetTransform)
        //{
            //trackedTarget = targetTransform;
        //}

        public void UnTrackTarget()
        {
            trackedTarget = null;
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void Start()
        {
            if(seekersWin == null)
            {
                seekersWin = GameObject.Find("SeekersWin");
                seekersWin.SetActive(false);
            }
            
            
            if (this.gameObject.name == "Player")
            {
                if (isSeeker)
                {
                    seekerList.Add(this);
                }
                else
                {
                    aliveList.Add(this);
                }
                
            }
            if (move)
            {
                if (!isSeeker)
                {
                    aliveList.Add(this);
                }
                else
                {
                    seekerList.Add(this);
                    //print(seekerList.Count);
                }
            }

        }
        private float timeCount = 0.0f;

        private void Update()
        {
            if (isPlayer)
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                Vector3 movement = Camera.main.transform.TransformDirection(new Vector3(horizontal, 0, vertical));
                movement.y = 0;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    FreezeBomb();
                }

                if (movement.magnitude != 0)
                {
                    Quaternion _lookRotation = Quaternion.LookRotation(movement, Vector3.up);
                    Vector3 newDir = Vector3.RotateTowards(transform.forward, movement, 5f * Time.deltaTime, 0.0F);
                    transform.rotation = Quaternion.LookRotation(newDir, Vector3.up);
                    animator.SetBool("running", true);
                }
                else
                {
                    animator.SetBool("running", false);
                }
            }
            else
            {
                RaycastHit hit;
                LayerMask layerMask = LayerMask.GetMask("UI", "Walls");
                if(trackedTarget!=null) {
                if (Physics.Raycast(transform.position, trackedTarget.transform.position - transform.position, out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.transform.gameObject.layer == 6)
                    {
                        //this.GetComponent<Pathfinder>().findNextRoom();
                    }
                }
                }
            }

            if (move == true)
            {
                if (!isPlayer)
                {

               
                if (trackedTarget == null)
                {
                    trackedTarget = this.GetComponent<Pathfinder>().mostRecentNode;
                }
                foreach (AIAgent agent in aliveList)
                {
                    if (agent.AIAgentRescue == this && !isFrozen)
                    {
                        agent.AIAgentRescue = null;
                    }
                }
                /*
                if (isSeeker)
                {

                    foreach (AIAgent runner in aliveList)
                    {
                        if (!chasingCoin)
                        {
                            if (Vector3.Distance(this.transform.position, runner.transform.position) < 10)
                            {
                                if (Vector3.Distance(AIAgentTarget.transform.position, this.transform.position) > Vector3.Distance(runner.transform.position, this.transform.position))
                                {
                                    AIAgentTarget = runner;
                                }
                            }
                        }
                    }
                    if (AIAgentTarget.isFrozen)
                    {
                        //this.findSeekerNextTarget();
                    }
                }
                else
                {
                    if (AIAgentRescue == null)
                    {
                        if (this.GetComponent<Pathfinder>().mostRecentNode.nodeRoom.frozenAgents.Count > 0)
                        {
                            this.GetComponent<AIAgent>().AIAgentRescue = this.GetComponent<Pathfinder>().mostRecentNode.nodeRoom.frozenAgents[0];
                            this.GetComponent<Pathfinder>().target = this.GetComponent<Pathfinder>().mostRecentNode.nodeRoom.frozenAgents[0].GetComponent<Pathfinder>().mostRecentNode;
                            this.GetComponent<Pathfinder>().findPathGlobal();
                        }
                    }
                }
                */

                if (debug)
                    Debug.DrawRay(transform.position, Velocity, Color.red);

                Vector3 finalVelocity = new Vector3();
                Quaternion finalRotation = new Quaternion();
                if (behaviorType == EBehaviorType.Kinematic)
                {
                    if (!isFrozen)
                    {
                        // TODO: average all kinematic behaviors attached to this object to obtain the final kinematic output and then apply it
                        GetKinematicAvg(out finalVelocity, out finalRotation);
                        Velocity = finalVelocity * maxSpeed;
                        Velocity = new Vector3(Velocity.x, 0, Velocity.z);
                        this.transform.position += Velocity * Time.deltaTime;
                        //finalRotation.x = 0;
                        //finalRotation.z = 0;
                        this.transform.rotation = finalRotation;
                        timeCount = timeCount + Time.deltaTime;
                    }
                }
                else
                {
                    // TODO: combine all steering behaviors attached to this object to obtain the final steering output and then apply it
                    GetSteeringSum(out finalVelocity, out finalRotation);
                    Vector3 acc = finalVelocity / 1;
                    Velocity += acc * Time.deltaTime;
                    Velocity = Vector3.ClampMagnitude(Velocity, maxSpeed);
                    transform.position += Velocity * Time.deltaTime;
                    this.transform.rotation *= finalRotation;
                    //print(finalRotation);

                }

                animator.SetBool("walking", Velocity.magnitude > 0.1);
                animator.SetBool("running", Velocity.magnitude > maxSpeed / 2);
                if (!isSeeker)
                    animator.SetBool("frozen", isFrozen);
            }
            }
        }

        private void GetKinematicAvg(out Vector3 kinematicAvg, out Quaternion rotation)
        {
            kinematicAvg = Vector3.zero;
            Quaternion eulerAvg = GetComponent<LookWhereYouAreGoing>().GetKinematic(this).angular;
            AIMovement[] movements = GetComponents<AIMovement>();
            int count = 0;

            foreach (AIMovement movement in movements)
            {
                kinematicAvg += movement.GetKinematic(this).linear;
                ++count;
            }

            if (count > 0)
            {
                kinematicAvg = Vector3.ClampMagnitude(kinematicAvg, maxSpeed);
                rotation = eulerAvg;
            }
            else
            {
                kinematicAvg = Velocity;
                rotation = transform.rotation;
            }
        }

        private void GetSteeringSum(out Vector3 steeringForceSum, out Quaternion rotation)
        {
            steeringForceSum = Vector3.zero;
            rotation = Quaternion.identity;
            AIMovement[] movements = GetComponents<AIMovement>();
            foreach (AIMovement movement in movements)
            {
                steeringForceSum += movement.GetSteering(this).linear;
                rotation *= movement.GetSteering(this).angular;
                //print(movement.GetSteering(this).angular);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (this.gameObject.layer == 12)
            {
                return;
            }
            if (other.gameObject.layer == 7 || other.gameObject.layer == 11)
            {

                if (other.gameObject.GetComponent<AIAgent>().isSeeker&& !other.gameObject.GetComponent<AIAgent>().isFrozen)
                {
                    if(isPlayer)
                    {
                        EndGame();
                    }
                    if (isSeeker == false && isFrozen == false && aliveList.Contains(this))
                    {
                        print("contact");
                        Velocity = Vector3.zero;
                        aliveList.Remove(this);
                        //other.gameObject.GetComponent<AIAgent>().findSeekerNextTarget();
                        this.GetComponent<Pathfinder>().mostRecentNode.nodeRoom.frozenAgents.Add(this);
                        animator.SetBool("frozen", true);
                        foreach (AIAgent agent in aliveList)
                        {
                            if (Vector3.Distance(agent.transform.position, this.transform.position) < 50)
                            {
                                if (agent.AIAgentRescue == null)
                                {
                                    foreach (AIAgent seeker in seekerList)
                                    {
                                        LayerMask layerMask = LayerMask.GetMask("Enemy", "Walls");
                                        RaycastHit hit;
                                        if (Physics.Raycast(agent.transform.position, seeker.transform.position - agent.transform.position, out hit, Mathf.Infinity, layerMask))
                                        {
                                            if (hit.transform.gameObject.layer == 11)
                                            {
                                                //NAHHHH
                                            }
                                            else
                                            {
                                                if (!agent.isPlayer)
                                                {
                                                    //agent.GetComponent<Flee>().canRecallPath = true;
                                                    //agent.AIAgentRescue = this;
                                                    //agent.GetComponent<Pathfinder>().target = this.GetComponent<Pathfinder>().mostRecentNode;
                                                    //agent.GetComponent<Pathfinder>().findPathGlobal();
                                                }

                                            }

                                        }
                                    }

                                }

                            }

                        }
                        if (isFrozen == false)
                        {
                            StartCoroutine(transformation());
                        }


                    }
                }
                if (!other.gameObject.GetComponent<AIAgent>().isSeeker)
                {
                    if (isSeeker == false && isFrozen == true)
                    {
                        if (other.GetComponent<AIAgent>().isPlayer)
                        {
                            print("Touch");
                            isFrozen = false;
                            aliveList.Add(this);
                            other.GetComponent<AIAgent>().canGroup = true;
                            other.GetComponent<AIAgent>().isFrozen = false;
                            this.GetComponent<Pathfinder>().mostRecentNode.nodeRoom.frozenAgents.Remove(this);
                            animator.SetBool("frozen", false);
                        }
                        else if (!other.GetComponent<Allignement>().neighbors.Contains(this.GetComponent<Collider>()))
                        {


                            print("Saved");
                            foreach (AIAgent agent in aliveList)
                            {
                                agent.AIAgentRescue = null;
                               //agent.GetComponent<Pathfinder>().findNextRoom();
                            }
                            isFrozen = false;
                            //this.GetComponent<Pathfinder>().findNextRoom();
                            other.GetComponent<AIAgent>().isFrozen = false;
                            aliveList.Add(this);
                            this.GetComponent<Pathfinder>().mostRecentNode.nodeRoom.frozenAgents.Remove(this);
                            
                            other.GetComponent<AIAgent>().AIAgentRescue = null;
                            foreach (AIAgent agent in aliveList)
                            {
                                if (agent.AIAgentRescue == this)
                                {
                                    agent.AIAgentRescue = null;
                                }
                            }
                            animator.SetBool("frozen", false);

                            if (other.GetComponent<Pathfinder>().mostRecentNode.nodeRoom.frozenAgents.Count > 0)
                            {
                                //other.GetComponent<AIAgent>().AIAgentRescue = other.GetComponent<Pathfinder>().mostRecentNode.nodeRoom.frozenAgents[0];
                                //other.GetComponent<Pathfinder>().target = other.GetComponent<Pathfinder>().mostRecentNode.nodeRoom.frozenAgents[0].GetComponent<Pathfinder>().mostRecentNode;
                                //other.GetComponent<Pathfinder>().findPathGlobal();
                            }



                           
                        }
                    }
                }
            }
        }
        IEnumerator transformation()
        {
            isFrozen = true;
            yield return new WaitForSeconds(25f);
            if (isFrozen == true)
            {
                this.GetComponent<Pathfinder>().mostRecentNode.nodeRoom.frozenAgents.Remove(this);
                GameObject s = Instantiate(SeekerPrefab, this.transform.position, this.transform.rotation);
                s.GetComponent<Pathfinder>().mostRecentNode = this.GetComponent<Pathfinder>().mostRecentNode;
                Destroy(this.gameObject);
            }

        }

        
        
        public void findSeekerNextTarget()
        {
            if (aliveList.Count > 0)
            {
                System.Random random = new System.Random();
                var randomIndex = random.Next(0, AIAgent.aliveList.Count);
                print(AIAgent.aliveList.Count);
                if (!AIAgent.aliveList[randomIndex].isFrozen)
                {
                    //AIAgentTarget = AIAgent.aliveList[randomIndex];
                    //this.GetComponent<Pathfinder>().target = AIAgent.aliveList[randomIndex].GetComponent<Pathfinder>().mostRecentNode;
                    //this.GetComponent<Pathfinder>().findPathGlobal();
                }
                else
                {
                    findSeekerNextTarget();
                }


            }
            else
            {
                EndGame();
            }

        }
        

        public void EndGame()
        {
            aliveList.Clear();
            seekerList.Clear();
            seekersWin.SetActive(true);
            GameObject.Find("Score").GetComponent<TMP_Text>().text = "Score: " + Mathf.Round(GameObject.Find("MainMenu").GetComponent<MainMenu>().score).ToString(); 
            Time.timeScale = 0f;
            Spin.ResetValues();
            print("Seekers win");
        }

        public void FreezeBomb()
        {
            if (!isFrozen)
            {
                if (ammoBomb > 0)
                {
                    Debug.Log("FreezeEM!");
                    GameObject obj = Instantiate(freezeBomb, transform.position, Quaternion.identity);
                    Destroy(obj, 1.5f);
                    LayerMask layerMask = LayerMask.GetMask("Enemy");
                    Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 5f, layerMask);
                    foreach (var hitCollider in hitColliders)
                    {
                        if (!hitCollider.GetComponent<AIAgent>().isFrozen)
                        {
                            hitCollider.GetComponent<AIAgent>().StartCoroutine(hitCollider.GetComponent<AIAgent>().Freeze());
                        }
                    }
                    ammoBomb--;
                }
            }
            
        }

        IEnumerator Freeze()
        {
            isFrozen = true;
            yield return new WaitForSeconds(5f);
            isFrozen= false;

        }


    }

}