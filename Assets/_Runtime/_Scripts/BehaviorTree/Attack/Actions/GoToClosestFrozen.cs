using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class GoToClosestFrozen : DecisionNode
{
    // Start is called before the first frame update

    float goalRange = 100f;//must be likely changed later
    bool going;
    public GoToClosestFrozen(float range)
    {

        goalRange = range;
    }
    public GoToClosestFrozen()
    {
        goalRange = 100f;
    }

    public override NodeState Evaluate()
    {
        Node guy = (Node)GetData("ClosestFrozen");



        if (GetData("Goal") != null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (guy != null)
        {


            Pathfinder p = referenceTree.GetComponent<Pathfinder>();




            if ((bool)GetData("Spotted") == false)
            {
                Debug.Log("");

                p.target = guy;
                p.start = p.mostRecentNode;
                p.findPathGlobal();


            }


            if (Vector3.Distance(guy.transform.position, referenceTree.transform.position) > 1)
            {
                state = NodeState.RUNNING;
                return state;
            }
            if (Vector3.Distance(guy.transform.position, referenceTree.transform.position) <= 1)
            {
                root.ClearData("ClosestFrozen");
                state = NodeState.SUCCESS;
                return state;
            }
        }

        state = NodeState.FAILURE;
        return state;
    }
}
