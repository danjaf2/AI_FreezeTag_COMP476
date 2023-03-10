using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClosestFrozen : DecisionNode
{
    // Start is called before the first frame update

    float range = 300f;//must be likely changed later
    LayerMask mask;
    public FindClosestFrozen(float range, LayerMask mask)
    {
        this.range = range;
        this.mask = mask;
    }
    public FindClosestFrozen()
    {
        range = 300f;
    }

    public override NodeState Evaluate()
    {
        
        Room[] rooms = GameObject.FindObjectsOfType<Room>();
        AIAgent closestFrozen = null;
        foreach (Room room in rooms)
        {
            if(closestFrozen== null)
            {
                if (room.frozenAgents.Count > 0)
                {
                    closestFrozen = room.frozenAgents[0];
                }
            }
            foreach(AIAgent frozen in room.frozenAgents)
            {
                if (Vector3.Distance(frozen.transform.position, referenceTree.transform.position) < Vector3.Distance(closestFrozen.transform.position, referenceTree.transform.position))
                {
                    closestFrozen = frozen;
                }
            }
        }

        if (closestFrozen != null)
        {
            root.SetData("ClosestFrozen", closestFrozen.GetComponent<Pathfinder>().mostRecentNode);
            referenceTree.GetComponent<AIAgent>().AIAgentRescue = closestFrozen;
            state = NodeState.SUCCESS;
            return state;
        }
        
        state = NodeState.FAILURE;
        return state;

    }
}
