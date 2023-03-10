using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerIsVisible : DecisionNode
{
    // Start is called before the first frame update
    float range = 100f;

    public SeekerIsVisible(float desiredRange)
    {
        range = desiredRange;
    }
    public SeekerIsVisible()
    {
        range = 100;

        
    }

    public override NodeState Evaluate()
    {
        
        if (GetData("Spotted") == null)
        {
            root.SetData("Spotted", false);
        }
        foreach (AIAgent seeker in AIAgent.seekerList)
        {
            
            
                
                    LayerMask layerMask = LayerMask.GetMask("Enemy", "Walls");
                    RaycastHit hit;
                    if (Physics.Raycast(referenceTree.transform.position, seeker.transform.position - referenceTree.transform.position, out hit, Mathf.Infinity, layerMask))
                    {
                        if (hit.transform.gameObject.layer == 11)
                        {
                            if (hit.transform.GetComponent<AIAgent>().isSeeker)
                            {
                            Debug.Log("HE SEES ME");
                        if ((bool)GetData("Spotted") == false)
                        {
                            referenceTree.StartCoroutine(danger());
                        }
                        state = NodeState.SUCCESS;
                            return state;
                        }
                        }
                    }

                
                
        }
        state = NodeState.FAILURE;
        return state;
    }

    IEnumerator danger()
    {
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        root.SetData("Spotted", true);
        yield return new WaitForSeconds(2.5f);
        root.SetData("Spotted", false);
        


    }
}
