using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace AI
{
    public class LookWhereYouAreGoing : AIMovement
    {
        public float RotationSpeed =1;
        public override SteeringOutput GetKinematic(AIAgent agent)
        {

            if (agent != null)
            {

                if (agent.trackedTarget != null)
                {
                    var output = base.GetKinematic(agent);
                    if (agent.trackedTarget != null)
                    {
                        Vector3 _direction = (agent.trackedTarget.transform.position - transform.position).normalized;



                        if (agent != null)
                        {


                            // TODO: calculate linear component
                            if (agent.isSeeker)
                            {
                                if (agent.AIAgentTarget != null)
                                {

                                    Vector3 desiredVelocityFace = agent.AIAgentTarget.transform.position - this.transform.position;
                                    //print(angle);
                                    RaycastHit hit;
                                    // Does the ray intersect any objects excluding the player layer
                                    LayerMask layerMask = LayerMask.GetMask("Player", "Walls");

                                    if (Physics.Raycast(transform.position, desiredVelocityFace, out hit, 20f, layerMask))
                                    {
                                        if (hit.transform.gameObject.layer == 7)
                                        {
                                            if (Vector3.Magnitude(agent.AIAgentTarget.transform.position - this.transform.position) < 10f)
                                            {
                                                _direction = desiredVelocityFace;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                            _direction.y = 0;
                            Quaternion _lookRotation = Quaternion.LookRotation(_direction, Vector3.up);
                            Vector3 newDir = Vector3.RotateTowards(transform.forward, _direction, RotationSpeed * Time.deltaTime, 0.0F);
                            if (Vector3.Magnitude(agent.trackedTarget.transform.position - transform.position) > 1f)
                            {
                                output.angular = Quaternion.LookRotation(newDir, Vector3.up);
                            }
                            else
                            {
                                output.angular = transform.rotation;
                            }
                            return output;
                        }
                    }
                }
                return new SteeringOutput();
            }
        

        public static float ClampAngle(float _Angle)
        {
            float ReturnAngle = _Angle;

            if (_Angle < 0f)
                ReturnAngle = (_Angle + (360f * ((_Angle / 360f) + 1)));

            else if (_Angle > 360f)
                ReturnAngle = (_Angle - (360f * (_Angle / 360f)));

            else if (ReturnAngle == 360) //Never use 360, only go from 0 to 359
                ReturnAngle = 0;

            return ReturnAngle;
        }

        public override SteeringOutput GetSteering(AIAgent agent)
        {
            var output = base.GetSteering(agent);

            // TODO: calculate angular component
            if (agent.lockY)
            {
                // get the rotation around the y-axis
                Vector3 from = Vector3.ProjectOnPlane(agent.transform.forward, Vector3.up);
                Vector3 to = GetKinematic(agent).angular * Vector3.forward;
                float angleY = Vector3.SignedAngle(from, to, Vector3.up);
                output.angular = Quaternion.AngleAxis(angleY, Vector3.up);
            }
            else
                output.angular = Quaternion.FromToRotation(agent.transform.forward, GetKinematic(agent).angular * Vector3.forward);

            return output;
        }
    }
}