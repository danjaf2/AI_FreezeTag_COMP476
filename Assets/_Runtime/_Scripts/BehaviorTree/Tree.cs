using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {

        private DecisionNode _root = null;

        protected void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if (_root != null)
                _root.Evaluate();
        }

        protected abstract DecisionNode SetupTree();

        public void setRoot(List<DecisionNode> children, DecisionNode root)
        {
            foreach (DecisionNode node in children)
            {
                node.root = root;
                if (node.children.Count > 0)
                {
                    setRoot(node.children, root);
                }
            }
        }

        public void setTreeRef(List<DecisionNode> children, AIAgent agent)
        {
            foreach (DecisionNode node in children)
            {
                node.referenceTree = agent;
                if (node.children.Count > 0)
                {
                    setTreeRef(node.children, agent);
                }
            }
        }

    }

}
