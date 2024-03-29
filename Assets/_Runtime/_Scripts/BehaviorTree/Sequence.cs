using System.Collections.Generic;

namespace BehaviorTree
{
    public class Sequence : DecisionNode
    {
        public Sequence() : base() { }
        public Sequence(List<DecisionNode> children) : base(children) { }

        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (DecisionNode node in children)
            {
                switch (node.Evaluate())
                {
                   case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;

                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    default:
                        continue;

                }
            }

            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }

    }

}
