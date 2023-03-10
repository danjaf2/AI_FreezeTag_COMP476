using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetInFOV : DecisionNode
{
    // Start is called before the first frame update
    float viewAngle = 160f;
    public CheckTargetInFOV()
    {
        //Constructor for setting up base values in Example Character Tree
    }

    public override NodeState Evaluate()
    {
        foreach(var target in AIAgent.aliveList)
        {

        if (target != null)
        {
            Vector3 toTarget = target.transform.position - referenceTree.transform.position;
            if (Vector3.Angle(referenceTree.transform.forward, toTarget) <= viewAngle)
            {
                    LayerMask layerMask = LayerMask.GetMask("Players", "Walls");
                    if (Physics.Raycast(referenceTree.transform.position, toTarget, out RaycastHit hit, layerMask))
                {
                    if(hit.transform.GetComponent<AIAgent>() == target)
                    {
                        root.SetData("Runner", hit.transform.GetComponent<AIAgent>());
                        referenceTree.GetComponent<AIAgent>().AIAgentTarget = hit.transform.GetComponent<AIAgent>();
                        root.SetData("LastSeenNode", hit.transform.GetComponent<Pathfinder>().mostRecentNode);
                            root.ClearData("Wander");
                            state = NodeState.SUCCESS;
                        return state;
                    }
                }
            }
        }

        }
        //Evaluation of state and apply transformation changes
        root.ClearData("Runner");
        referenceTree.GetComponent<AIAgent>().AIAgentTarget = null;
        state = NodeState.FAILURE;
        return state;
    }
}
