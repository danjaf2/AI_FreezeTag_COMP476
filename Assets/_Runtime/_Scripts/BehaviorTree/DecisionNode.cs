using AI;
using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree
{
    public enum NodeState
    {
        NONE,
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class DecisionNode
    {
        protected NodeState state;

        public DecisionNode parent;
        public DecisionNode root;
        public AIAgent referenceTree;
        public List<DecisionNode> children = new List<DecisionNode>();
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public DecisionNode()
        {
            parent = null;
        }
        public DecisionNode(List<DecisionNode> children)
        {
            foreach (DecisionNode child in children)
                _Attach(child);
        }

        private void _Attach(DecisionNode node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            DecisionNode node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            DecisionNode node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }
            return false;
        }


    }
}
