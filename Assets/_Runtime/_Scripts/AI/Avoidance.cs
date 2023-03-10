using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoidance : AIMovement
{
    public LayerMask layerMask;
    public float avoidanceRadius = 7f;
    public float avoidanceWeight;
    public float cohesionWeight;
    public override SteeringOutput GetKinematic(AIAgent agent)
    {
        var output = base.GetKinematic(agent);

        Collider[] neighbors = GetNeighborContext();
        output.linear += Avoid(neighbors);
        return output;
    }

    public override SteeringOutput GetSteering(AIAgent agent)
    {
        var output = base.GetSteering(agent);

        // TODO: calculate linear component
        output.linear = GetKinematic(agent).linear - agent.Velocity;

        return output;
    }

    public Collider[] GetNeighborContext()
    {
        Collider[] neighbors = Physics.OverlapSphere(transform.position, avoidanceRadius, layerMask);

        if (debug)
            DebugUtil.DrawWireSphere(transform.position, Color.Lerp(Color.white, Color.red, neighbors.Length), avoidanceRadius);

        return neighbors;
    }

    Vector3 Alignment(Collider[] neighbors)
    {
        // alignedDirection is equal to the average direction of neighbors 
        // TODO
        Vector3 alignedDirection = Vector3.zero;
        foreach (Collider c in neighbors)
        {
            if (c.transform.gameObject != this.transform.gameObject)
            {
                AIAgent neighbor = c.GetComponent<AIAgent>();
                alignedDirection += neighbor.Velocity;
            }
            else
            {
                continue;
            }

        }

        if (neighbors.Length - 1 > 0)
        {
            alignedDirection = alignedDirection / neighbors.Length;
        }

        return alignedDirection.normalized*weight;


    }

    Vector3 Avoid(Collider[] neighbors)
    {
        // movement is equal to the average of the sum of all vectors going from neighbor to flock agent within the avoidance radius
        // TODO
        Vector3 avoidanceDirection = Vector3.zero;
        float squaredRadius = avoidanceRadius * avoidanceRadius;
        foreach (Collider c in neighbors)
        {
            if (!c.transform.GetComponent<AIAgent>().isSeeker && !c.transform.GetComponent<AIAgent>().isFrozen)
            {
                if (c.transform.gameObject != this.transform.gameObject)
                {
                    if (Vector3.SqrMagnitude(c.transform.position - transform.position) <= squaredRadius)
                    {
                        Vector3 nta = transform.position - c.transform.position;
                        if (nta == Vector3.zero)
                        {
                            nta = Random.insideUnitSphere * 0.1f;
                        }
                        avoidanceDirection += nta;
                    }
                }
                else
                {
                    continue;
                }
            }
        }

        if (neighbors.Length - 1 > 0)
        {
            avoidanceDirection = avoidanceDirection / neighbors.Length;
        }

        return avoidanceDirection.normalized * avoidanceWeight;
    }

}

