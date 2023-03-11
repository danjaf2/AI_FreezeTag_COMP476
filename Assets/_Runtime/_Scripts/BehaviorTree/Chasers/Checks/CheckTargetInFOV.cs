using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetInFOV : DecisionNode
{
    // Start is called before the first frame update
    float viewAngle = 90f;
    bool found =false;
    public CheckTargetInFOV()
    {
        //Constructor for setting up base values in Example Character Tree
    }

    public override NodeState Evaluate()
    {
        
        if ((Node)GetData("CoinLastSeen") == null&& GetData("Runner")==null)
        {
            Spin[] coins = GameObject.FindObjectsOfType<Spin>();
            foreach (Spin coin in coins)
            {
                Vector3 toTarget = coin.transform.position - referenceTree.transform.position;

                LayerMask layerMask = LayerMask.GetMask("Coin", "Walls");
                //Debug.Log(layerMask.value.ToString());
                if (Physics.Raycast(referenceTree.transform.position, toTarget, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    //Debug.Log(hit.transform.name);
                    if (hit.transform.GetComponent<AIAgent>() == coin.GetComponent<AIAgent>())
                    {
                        if (Vector3.Angle(referenceTree.transform.forward, hit.transform.position - referenceTree.transform.position) <= viewAngle)
                        {

                            root.SetData("CoinLastSeen", GameObject.Find(hit.transform.GetComponent<Pathfinder>().mostRecentNode.name).GetComponent<Node>());
                        }
                    }
                }
            }
        }
        found = false;
        foreach (var target in AIAgent.aliveList)
        {
            
            if (target != null)
            {
                Vector3 toTarget = target.transform.position - referenceTree.transform.position;
                
                    LayerMask layerMask = LayerMask.GetMask("Player", "Walls");
                //Debug.Log(layerMask.value.ToString());
                    if (Physics.Raycast(referenceTree.transform.position, toTarget, out RaycastHit hit,Mathf.Infinity,layerMask))
                    {
                    //Debug.Log(hit.transform.name);
                    if (hit.transform.GetComponent<AIAgent>() == target)
                        {
                        if (!hit.transform.GetComponent<AIAgent>().isFrozen) { 
                        if (Vector3.Angle(referenceTree.transform.forward, hit.transform.position - referenceTree.transform.position) <= viewAngle)
                        {
                            root.SetData("Runner", hit.transform.GetComponent<AIAgent>());
                            referenceTree.GetComponent<AIAgent>().AIAgentTarget = hit.transform.GetComponent<AIAgent>();
                            root.SetData("LastSeenNode", hit.transform.GetComponent<Pathfinder>().mostRecentNode);
                            root.ClearData("Wander");
                            root.ClearData("Flank");
                            found= true;
                            state = NodeState.SUCCESS;
                            return state;
                        }
                        }
                    }
                }
            }

        }
        
        if (GetData("Flank") != null)
        {
           Node n =(Node)GetData("LastSeenNode");
            if (referenceTree.GetComponent<AIAgent>().AIAgentTarget != null)
            {
                if (Vector3.Distance(referenceTree.GetComponent<AIAgent>().AIAgentTarget.transform.position, referenceTree.transform.position) <= 4)
                {
                    root.ClearData("Flank");
                    state = NodeState.FAILURE;
                    return state;
                }
            }
            
            if (Vector3.Distance(n.transform.position, referenceTree.transform.position)<=5)
            {
                root.ClearData("Flank");
                state = NodeState.FAILURE;
                return state;
            }
            else
            {
                state = NodeState.SUCCESS;
                return state;
            }
            
            
            
        }
        
        //Evaluation of state and apply transformation changes
        root.ClearData("Runner");
        referenceTree.GetComponent<AIAgent>().AIAgentTarget = null;
        state = NodeState.FAILURE;
        return state;
    }
   
}
