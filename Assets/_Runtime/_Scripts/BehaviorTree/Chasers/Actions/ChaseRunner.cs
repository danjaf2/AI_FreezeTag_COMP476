using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseRunner : DecisionNode
{
    // Start is called before the first frame update
    float viewAngle = 30f;
    public ChaseRunner()
    {
        //Constructor for setting up base values in Example Character Tree
    }

    public override NodeState Evaluate()
    {
        Node target = (Node)GetData("LastSeenNode");
        Pathfinder p = referenceTree.GetComponent<Pathfinder>();

                Debug.Log("");
                p.target = target;
                p.findPathGlobal();
          

        state = NodeState.SUCCESS;
        return state;
    }
}
