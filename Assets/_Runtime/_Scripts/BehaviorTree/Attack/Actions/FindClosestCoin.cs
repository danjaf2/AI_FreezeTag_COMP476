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
        if(coins.Length > 0 )
        {
            Spin closestCoin = coins[0];
            foreach(var coin in coins)
            {
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
        
        



        state = NodeState.FAILURE;
        return state;

    }
}
