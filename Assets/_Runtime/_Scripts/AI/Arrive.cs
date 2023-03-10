using UnityEngine;

namespace AI
{
    public class Arrive : AIMovement
    {
        public float slowRadius;
        public float stopRadius;

        private void DrawDebug(AIAgent agent)
        {
            if (debug)
            {
                DebugUtil.DrawCircle(agent.TargetPosition, transform.up, Color.yellow, stopRadius);
                DebugUtil.DrawCircle(agent.TargetPosition, transform.up, Color.magenta, slowRadius);
            }
        }

        public override SteeringOutput GetKinematic(AIAgent agent)
        {
            
            if (agent!= null) { 

            var output = base.GetKinematic(agent);
                if(this.GetComponent<Pathfinder>().target != null) {
            // TODO: calculate linear component
            Vector3 desiredVelocity = agent.TargetPosition - this.transform.position;
            float angle = Vector3.Angle(desiredVelocity.normalized, transform.forward);
            float requiredAngle = Mathf.Lerp(10, 50, agent.maxSpeed / Vector3.Magnitude(agent.Velocity));
            //print(angle);
            if (angle < requiredAngle || Vector3.Magnitude(agent.TargetPosition - this.transform.position) <= 1.5f)
            {
                desiredVelocity = desiredVelocity.normalized * agent.maxSpeed;
            }
            else
            {
                output.linear = Vector3.zero;
                return output;
            }
            float distance = Vector3.Magnitude(transform.position - this.GetComponent<Pathfinder>().target.transform.position);

            if (agent.isSeeker==false)
            {
                if (distance <= stopRadius)
                {
                    desiredVelocity *= 0;
                }
                else if (distance < slowRadius)
                {
                    desiredVelocity *= (distance / slowRadius);
                }

                output.linear = desiredVelocity*weight;
            }
            else
            {
                output.linear = desiredVelocity*weight;
            }
            return output;
                }
            }
            return new SteeringOutput();
        }

        public override SteeringOutput GetSteering(AIAgent agent)
        {
            DrawDebug(agent);

            var output = base.GetSteering(agent);

            // TODO: calculate linear component
            output.linear = GetKinematic(agent).linear - agent.Velocity;

            return output;
        }
    }
}
