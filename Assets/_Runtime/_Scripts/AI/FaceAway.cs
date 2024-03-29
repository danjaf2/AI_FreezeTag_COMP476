﻿using UnityEngine;

namespace AI
{
    public class FaceAway : AIMovement
    {
        public override SteeringOutput GetKinematic(AIAgent agent)
        {
            
            var output = base.GetKinematic(agent);

            // TODO: calculate angular component
            Vector3 direction = this.transform.position - agent.transform.position;

            if (direction.normalized == transform.forward || Mathf.Approximately(direction.magnitude, 0f))
            {
                output.angular = transform.rotation;
            }
            else
            {
                output.angular = Quaternion.LookRotation(direction);
            }
            return output;
        }

        public override SteeringOutput GetSteering(AIAgent agent)
        {
            var output = base.GetSteering(agent);

            // TODO: calculate angular component
            output.angular = Quaternion.FromToRotation(agent.transform.forward, GetKinematic(agent).angular * Vector3.forward);

            return output;
        }
    }
}
