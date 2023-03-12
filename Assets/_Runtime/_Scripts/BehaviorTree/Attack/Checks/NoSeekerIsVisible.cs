using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class NoSeekerIsVisible : DecisionNode
{
    // Start is called before the first frame update
    float range = 100f;

    public NoSeekerIsVisible(float desiredRange)
    {
        range = desiredRange;
    }
    public NoSeekerIsVisible()
    {
        range = 100;

    }

    public override NodeState Evaluate()
    {
        

        if (GetData("Spotted") != null)
        {
            if ((bool)GetData("Spotted") == true)
            {
                Debug.Log("Im spotted");
                state = NodeState.FAILURE;
                return state;
            }
        }


        if (referenceTree.GetComponent<Pathfinder>().mostRecentNode.nodeRoom.dangerLevel >= 10)
        {
            Debug.Log("I DONT LIKE IT again");
            root.ClearData("Coin");
            state = NodeState.FAILURE;
            return state;
        }
        //Evaluation of state and apply transformation changes
        foreach (AIAgent seeker in AIAgent.seekerList)
        {
            

            
            LayerMask layerMask = LayerMask.GetMask("Enemy", "Walls");
            RaycastHit hit;
            if (Physics.Raycast(referenceTree.transform.position, seeker.transform.position - referenceTree.transform.position, out hit, Mathf.Infinity, layerMask))
            {
                //Debug.Log(hit.transform.gameObject.name);
                if (hit.transform.gameObject.layer == 11)
                {
                    if (hit.transform.GetComponent<AIAgent>().isSeeker)
                    {
                        Debug.Log("HE SEES ME again");
                        root.ClearData("Coin");
                        state = NodeState.FAILURE;
                        return state;
                    }
                }
            }



        }
        state = NodeState.SUCCESS;
        return state;
    }


}