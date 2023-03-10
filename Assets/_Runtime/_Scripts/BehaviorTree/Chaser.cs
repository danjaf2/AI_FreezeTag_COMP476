using System.Collections.Generic;
using AI;
using BehaviorTree;

public class Chaser : Tree
{
    protected override DecisionNode SetupTree()
    {
        //DecisionNode run = new GoToSaferRoom();
        DecisionNode root = new Selector(new List<DecisionNode>
        {

             new Sequence(new List<DecisionNode>
               {
                 new CheckTargetInFOV(),
                 new ChaseRunner()
               }),
             new Sequence(new List<DecisionNode>
               {
                 new OnLastKnownLocation(),
                 new LookAroundArea()
               }),

             new WanderRandomly()


        }) ;
        
        setRoot(root.children, root);
        setTreeRef(root.children, this.GetComponent<AIAgent>());
        return root;
    }
}
