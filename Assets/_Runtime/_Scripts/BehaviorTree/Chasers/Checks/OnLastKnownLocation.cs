using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLastKnownLocation : DecisionNode
{
    
    float range = 100f;

    public OnLastKnownLocation(float desiredRange)
    {
        range = desiredRange;
    }
    public OnLastKnownLocation()
    {
        range = 100;


    }

    public override NodeState Evaluate()
    {
        Pathfinder p = referenceTree.GetComponent<Pathfinder>();
        Node lastSeen = (Node)GetData("LastSeenNode");
        if (lastSeen != null)
        {
            if(p.mostRecentNode == lastSeen)
            {
                state = NodeState.SUCCESS;
                return state;
            }
        }

        state = NodeState.FAILURE; 
        return state;
    }

    
}
