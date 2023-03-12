using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClosestCoin : DecisionNode
{
    // Start is called before the first frame update

    float range = 300f;//must be likely changed later
    LayerMask mask;
    public FindClosestCoin(float range, LayerMask mask)
    {
        this.range = range;
        this.mask = mask;
    }
    public FindClosestCoin()
    {
        range = 300f;
    }

    public override NodeState Evaluate()
    {
        
        Spin[] coins = GameObject.FindObjectsOfType<Spin>();
        if (GetData("Coin") == null) {
        if(coins.Length > 0 )
        {
            Spin closestCoin = null;
            foreach(var coin in coins)
            {
                bool match = false;
                foreach(AIAgent agent in AIAgent.aliveList)
                {
                    
                    if (agent.GetComponent<Pathfinder>().target != null) {
                    if (agent.GetComponent<Pathfinder>().target == coin.GetComponent<Pathfinder>().mostRecentNode)
                    {
                        match = true;
                        break;
                    }
                 }
                    
                }
                if(match)
                {
                    match = false;
                    continue;
                }
                if (closestCoin == null)
                {
                    closestCoin = coin;
                }
                if(Vector3.Distance(coin.transform.position, referenceTree.transform.position)< Vector3.Distance(closestCoin.transform.position, referenceTree.transform.position))
                {
                    closestCoin = coin;
                }
            }
            if(closestCoin != null)
            {
                root.SetData("Coin", closestCoin.GetComponent<Pathfinder>().mostRecentNode);
                state = NodeState.SUCCESS;
                return state;
            }
            
        }
        }
        if (GetData("Coin") != null)
        {
            state = NodeState.SUCCESS;
            return state;
        }




        state = NodeState.FAILURE;
        return state;

    }
}
