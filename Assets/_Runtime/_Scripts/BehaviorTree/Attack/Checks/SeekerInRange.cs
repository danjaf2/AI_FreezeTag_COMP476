using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerInRange : DecisionNode
{
    // Start is called before the first frame update
    float range = 5f;

    public SeekerInRange(float desiredRange)
    {
        range = desiredRange;
    }
    public SeekerInRange()
    {
        range = 5;


    }

    public override NodeState Evaluate()
    {
        foreach (AIAgent seeker in AIAgent.seekerList)
        {
            if (!seeker.isFrozen)
            {
                if(Vector3.Distance(referenceTree.transform.position, seeker.transform.position)<range)
                {
                    referenceTree.FreezeBomb();
                    break;
                }
            }
        }
        state = NodeState.FAILURE;
        return state;
    }

}