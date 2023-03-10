using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerInfluenceHigh : DecisionNode
{
    // Start is called before the first frame update
    public SeekerInfluenceHigh(float attackAngle, float range)
    {

    }
    public SeekerInfluenceHigh()
    {
       
    }

    public override NodeState Evaluate()
    {
        if (referenceTree.GetComponent<Pathfinder>().mostRecentNode.nodeRoom.dangerLevel >= 10)
        {
            Debug.Log("I DONT LIKE IT");
            if ((bool)GetData("Spotted") == false)
            {
                referenceTree.StartCoroutine(danger());
            }
            state = NodeState.SUCCESS;
            return state;
        }
        //Evaluation of state and apply transformation changes
        state = NodeState.FAILURE;
        return state;
    }

    IEnumerator danger()
    {
        Debug.Log("????????????");
        root.SetData("Spotted", true);
        yield return new WaitForSeconds(2.5f);
        root.SetData("Spotted", false);
        


    }
}
