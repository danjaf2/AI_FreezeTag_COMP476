using System.Collections;
using UnityEngine;

namespace AI
{
   
    public class Flee : AIMovement
    {
        public Room ogRoom =null;
        public bool canRecallPath = true;
        public override SteeringOutput GetKinematic(AIAgent agent)
        {

            if (agent != null)
            {
                var output = base.GetKinematic(agent);

                // TODO: calculate linear component
                Vector3 desiredVelocity = Vector3.zero;
                Vector3 finalVelocity= Vector3.zero;
                int count = 0;
                foreach (AIAgent seeker in AIAgent.seekerList)
                {
                desiredVelocity = this.transform.position - seeker.transform.position;
                //print(angle);
                if (Vector3.Magnitude(seeker.transform.position - this.transform.position) < 3f)
                {
                    finalVelocity = desiredVelocity.normalized * agent.maxSpeed;
                    output.linear += desiredVelocity * weight;
                        count++;
                }
                else
                {
                    output.linear += Vector3.zero;
                }
                /*
                    if (Vector3.Magnitude(seeker.transform.position - this.transform.position) < 10f && canRecallPath)
                    {
                        LayerMask layerMask = LayerMask.GetMask("Enemy", "Walls");
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, seeker.transform.position - this.transform.position, out hit, 20.0f, layerMask))
                        {
                            if (hit.transform.gameObject.layer == 11)
                            {
                                if (hit.transform.GetComponent<AIAgent>().isSeeker)
                                {
                                    // if (agent.AIAgentRescue == null)
                                    //{
                                    print("Enemy");
                                        //ogRoom = agent.GetComponent<Pathfinder>().mostRecentNode.nodeRoom;
                                        //print("Recal");
                                        //StartCoroutine(timer());
                                        //this.GetComponent<Pathfinder>().findNextRoom();
                                        //output.linear += Vector3.zero;
                                    //}
                                    //else
                                    //{
                                        if(Vector3.Distance(this.transform.position, agent.AIAgentRescue.transform.position) > Vector3.Distance(hit.transform.position, agent.AIAgentRescue.transform.position))
                                        {
                                            ogRoom = agent.GetComponent<Pathfinder>().mostRecentNode.nodeRoom;
                                            print("Recal");
                                            StartCoroutine(timer());
                                            this.GetComponent<Pathfinder>().findNextRoom();
                                            output.linear += Vector3.zero;
                                        }
                                        
                                    //}
                                    

                                }
                            }
                        }
                            
                    }
                    */
                }
                if (count!=0)
                {
                    output.linear = output.linear / count;
                }
                
                if (debug) Debug.DrawRay(transform.position, output.linear, Color.cyan);
                return output;
                }
            



            return new SteeringOutput();
        }

        public override SteeringOutput GetSteering(AIAgent agent)
        {
            var output = base.GetSteering(agent);

            // TODO: calculate linear component
            output.linear = GetKinematic(agent).linear - agent.Velocity;

            return output;
        }
        public IEnumerator timer() {
            canRecallPath = false;
            while (this.GetComponent<Pathfinder>().mostRecentNode.nodeRoom == ogRoom)
            {
                yield return null;
            }
            canRecallPath = true;
        }
    }
    
}
