using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WanderRandomly : DecisionNode
{
    // Start is called before the first frame update
    float goalRange = 100f;//must be likely changed later
    bool going;
    List<Node> components = new List<Node>();
    public WanderRandomly(float range)
    {

        goalRange = range;
    }
    public WanderRandomly()
    {
        goalRange = 100f;
        Node[] init = GameObject.FindObjectsOfType<Node>();
        foreach (Node node in init)
        {
            if (node.transform.gameObject.name != "NullNode")
            {
                components.Add(node);
            }
        }
    }

    public override NodeState Evaluate()
    {
        Node node = (Node)GetData("Wander");
        Node coin = (Node)GetData("CoinLastSeen");
        
        if (coin != null)
        {
            if (Vector3.Distance(coin.transform.position, referenceTree.transform.position) <= 1)
            {
                root.ClearData("CoinLastSeen");
                root.ClearData("Wander");
                Debug.Log("Cleared Coin");
                state = NodeState.FAILURE;
                return state;
            }
            if (GetData("LastSeenNode") == null && GetData("Runner") == null)
            {
                //Debug.Log(components.Count);
                Pathfinder p = referenceTree.GetComponent<Pathfinder>();
                p.target = coin;
                p.findPathGlobal();

            }
        }

        if (GetData("Wander") != null)
        {
            if (Vector3.Distance(node.transform.position, referenceTree.transform.position) <= 1)
            {
                root.ClearData("Wander");
                state = NodeState.FAILURE;
                return state;
            }
            state = NodeState.FAILURE;
            return state;
        }

        if (node == null&&GetData("LastSeenNode")==null&&GetData("Runner")==null)
        {
            //Debug.Log(components.Count);
            int index = Random.Range(0, components.Count);
            Node target = components[index];
            Pathfinder p = referenceTree.GetComponent<Pathfinder>();
            root.SetData("Wander", target);
                p.target = target;
                p.findPathGlobal();
            
        }

        state = NodeState.FAILURE;
        return state;
    }
}
