using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class GoToClosestCoin : DecisionNode
{
    // Start is called before the first frame update

    float goalRange = 100f;//must be likely changed later
    bool going;
    public GoToClosestCoin(float range)
    {

        goalRange = range;
    }
    public GoToClosestCoin()
    {
        goalRange = 100f;
    }

    public override NodeState Evaluate()
    {
        Node coin = (Node)GetData("Coin");
        
        

        if(GetData("Goal")!=null)
        {
            state = NodeState.FAILURE;
            return state;
        }
        
        if (coin != null)
        {
            
            Node target = coin;
            Pathfinder p = referenceTree.GetComponent<Pathfinder>();



            
            if ((bool)GetData("Spotted") == false)
                {
                //Debug.Log("");
                
                    p.target = target;
                    p.start = p.mostRecentNode;
                    p.findPathGlobal();
                
                
                }
            

            if (Vector3.Distance(target.transform.position, referenceTree.transform.position) > 1)
            {
                state = NodeState.RUNNING;
                return state;
            }
            if (Vector3.Distance(target.transform.position, referenceTree.transform.position) <= 1)
            { 
                root.ClearData("Coin");
                state = NodeState.SUCCESS;
                return state;
            }
        }
        
        state = NodeState.FAILURE;
        return state;
    }
}
