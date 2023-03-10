using System.Collections.Generic;
using AI;
using BehaviorTree;

public class Runner : Tree
{
    protected override DecisionNode SetupTree()
    {
        DecisionNode run = new GoToSaferRoom();
        DecisionNode root = new Selector(new List<DecisionNode>
        {
            
             new Sequence(new List<DecisionNode>
               {
                 new SeekerIsVisible(),
                 run
               }),
            new Sequence(new List<DecisionNode>
            {
               new SeekerInfluenceHigh(),
                   run
            }),

            new Sequence(new List<DecisionNode>
            {
               new NoSeekerIsVisible(),
               new FindClosestFrozen(),
               new GoToClosestFrozen()
            }),
            new Sequence(new List<DecisionNode>
            {
               new NoSeekerIsVisible(),
               new FindClosestCoin(),
               new GoToClosestCoin()
            }), run
            
        }) ;
        
        setRoot(root.children, root);
        setTreeRef(root.children, this.GetComponent<AIAgent>());
        return root;
    }
}
