using UnityEngine;

namespace AI
{
    public class Seek : AIMovement
    {
        public override SteeringOutput GetKinematic(AIAgent agent)
        {
           
            if (agent != null)
            {
                var output = base.GetKinematic(agent);

                // TODO: calculate linear component
                if (agent.isSeeker) {
                if (agent.AIAgentTarget != null)
                {

                    Vector3 desiredVelocity = agent.AIAgentTarget.transform.position - this.transform.position;
                        //print(angle);
                        RaycastHit hit;
                        // Does the ray intersect any objects excluding the player layer
                        LayerMask layerMask = LayerMask.GetMask("Player", "Walls");

                        if (Physics.Raycast(transform.position, desiredVelocity, out hit, Mathf.Infinity, layerMask))
                        {
                            if(hit.transform.gameObject.layer == 7)
                            {
                                if (Vector3.Magnitude(agent.AIAgentTarget.transform.position - this.transform.position) < 10f)
                                {
                                    desiredVelocity = desiredVelocity.normalized * agent.maxSpeed/0.90f;
                                    output.linear = desiredVelocity * weight;
                                }
                                else
                                {
                                    output.linear = Vector3.zero;
                                }
                            }
                        }
                            


                    if (debug) Debug.DrawRay(transform.position, output.linear, Color.cyan);

                    return output;
                }
                }
                else
                {
                    if (agent.AIAgentRescue != null)
                    {

                        Vector3 desiredVelocity = agent.AIAgentRescue.transform.position - this.transform.position;
                        //print(angle);
                        RaycastHit hit;
                        // Does the ray intersect any objects excluding the player layer
                        LayerMask layerMask = LayerMask.GetMask("Player", "Walls");
                        if (Physics.Raycast(transform.position, desiredVelocity, out hit, 20f, layerMask))
                        {
                            if (hit.transform.gameObject.layer == 7)
                            {
                                if (Vector3.Magnitude(agent.AIAgentRescue.transform.position - this.transform.position) < 3f)
                                {
                                    desiredVelocity = desiredVelocity.normalized * agent.maxSpeed;
                                    output.linear = desiredVelocity * weight;
                                }
                                else
                                {
                                    output.linear = Vector3.zero;
                                }
                            }
                        }

                        if (debug) Debug.DrawRay(transform.position, output.linear, Color.cyan);

                        return output;
                    }
                }
            }
            var output2 = base.GetKinematic(agent);
            return output2;
        }

        public override SteeringOutput GetSteering(AIAgent agent)
        {
            var output = base.GetSteering(agent);

            // TODO: calculate linear component

            output.linear = GetKinematic(agent).linear - agent.Velocity;
            if (debug) Debug.DrawRay(transform.position + agent.Velocity, output.linear, Color.green);

            return output;
        }
    }
}
