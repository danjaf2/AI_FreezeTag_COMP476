using UnityEngine;

namespace AI
{
    public class WallCollisionAvoidance : AIMovement
    {
        public LayerMask layerMask;
        public override SteeringOutput GetKinematic(AIAgent agent)
        {
            var output = base.GetKinematic(agent);
            Vector3 desiredVelocity = Vector3.zero;
            RaycastHit hit;
            
                if (Physics.Raycast(transform.position, agent.Velocity, out hit, 0.5f, layerMask))
                {
                    Vector3 point = hit.point + hit.normal;
                    desiredVelocity = (point - transform.position);
                }

                output.linear = hit.normal * agent.maxSpeed * weight;

            
            
            

            return output;
        }

        public override SteeringOutput GetSteering(AIAgent agent)
        {
            var output = base.GetSteering(agent);

            // TODO: calculate linear component
            output.linear = GetKinematic(agent).linear - agent.Velocity;

            return output;
        }
    }
}