using AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Allignement : AIMovement
{
    public LayerMask layerMask;
    public float avoidanceRadius = 7f;
    public List<Collider> neighbors = new List<Collider>();
    public AIAgent player;

    public override SteeringOutput GetKinematic(AIAgent agent)
    {
        var output = base.GetKinematic(agent);
        player = null;
        if (agent.canGroup) {
        neighbors = GetNeighborContext().ToList();
        output.linear += Alignment(neighbors);
        }
        return output;
    }

    public override SteeringOutput GetSteering(AIAgent agent)
    {
        var output = base.GetSteering(agent);

        // TODO: calculate linear component
        output.linear = GetKinematic(agent).linear - agent.Velocity;

        return output;
    }

    public List<Collider> GetNeighborContext()
    {
        List<Collider> n = Physics.OverlapSphere(transform.position, avoidanceRadius, layerMask).ToList();

        n.RemoveAll(item => item.gameObject.GetComponent<AIAgent>().isSeeker || item.gameObject.GetComponent<AIAgent>().isFrozen);

        return n;
    }

    Vector3 Alignment(List<Collider> neighbors)
    {
        Vector3 alignedDirection = Vector3.zero;
        if(!this.GetComponent<AIAgent>().isFrozen) { 
        foreach (Collider c in neighbors)
        {
            if (!c.transform.GetComponent<AIAgent>().isSeeker && !c.transform.GetComponent<AIAgent>().isFrozen)
            {
                if (c.transform.gameObject != this.transform.gameObject)
                {
                        if(c.GetComponent<AIAgent>().isPlayer&&c.GetComponent<AIAgent>().canGroup)
                        {
                            player = c.GetComponent<AIAgent>();
                            alignedDirection += player.transform.forward * 10;
                        }
                        else
                        {
                            AIAgent neighbor = c.GetComponent<AIAgent>();
                            this.GetComponent<AIAgent>().trackedTarget = neighbor.trackedTarget;
                            this.GetComponent<AIAgent>().TargetPosition = neighbor.targetPosition;
                            alignedDirection += neighbor.Velocity;
                        }
                    
                }
                else
                {
                    continue;
                }
            }
            }
        }

        if (neighbors.Count - 1 > 0)
        {
            alignedDirection = alignedDirection / neighbors.Count;
        }
        
        return alignedDirection.normalized * weight;
        
        

    }

   

}

