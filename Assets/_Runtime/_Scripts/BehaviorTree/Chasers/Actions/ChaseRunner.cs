using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseRunner : DecisionNode
{
    // Start is called before the first frame update
    float rangeOfCommunication = 10f;
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
        if (GetData("Flank") == null)
        {
            p.findPathGlobal();
        }
        else
        {
            Path path = (Path)GetData("Flank");
            //Debug.Log(path.currentNode.name);
            p.findPathGlobalFlank(path);
        }
        if (GetData("Runner") != null)
        {

            foreach (AIAgent agent in AIAgent.seekerList)
            {
                if (Vector3.Distance(agent.transform.position, referenceTree.transform.position) < rangeOfCommunication)
                {
                    //if(agent.GetComponent<Chaser>()._root.GetData("LastSeenNode")==null&& agent.GetComponent<Chaser>()._root.GetData("CoinLastSeen") == null)
                    {
                        if (!agent.isPlayer)
                        {
                        if (agent.GetComponent<Chaser>()._root.GetData("Flank") == null)
                        {
                            agent.GetComponent<Pathfinder>().target = target;
                            agent.GetComponent<Pathfinder>().findPathGlobalFlank(p.solution);
                            agent.GetComponent<Chaser>()._root.SetData("Flank", referenceTree.GetComponent<Pathfinder>().solution);
                            agent.GetComponent<Chaser>()._root.SetData("LastSeenNode", referenceTree.GetComponent<Pathfinder>().target);
                            agent.GetComponent<Chaser>()._root.ClearData("Wander");
                        }

                        }
                    }
                }
            }
        }


        state = NodeState.SUCCESS;
        return state;
    }
}
