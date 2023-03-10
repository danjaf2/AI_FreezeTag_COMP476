using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class GoToSaferRoom : DecisionNode
{
    // Start is called before the first frame update

    float goalRange = 100f;//must be likely changed later
    Room goal= null;
    Node g;
    public GoToSaferRoom(float range)
    {

        goalRange = range;
    }
    public GoToSaferRoom()
    {
        goalRange = 100f;
    }

    public override NodeState Evaluate()
    {
        Pathfinder p = referenceTree.GetComponent<Pathfinder>();
        if(p.mostRecentNode == p.target)
        {
            root.ClearData("Goal");
        }
        if ((Room)GetData("Goal") != null)
        {
            if(p.mostRecentNode.nodeRoom==p.goalRoom)
            {

                Debug.Log("Yo");
                state = NodeState.SUCCESS;
                root.ClearData("Goal");
                p.target = p.mostRecentNode;
                p.findPathGlobal();
                goal = null;
                return state;
            }
            else
            {
                state = NodeState.FAILURE;
                return state;
            }
            
        }
        
        if (goal == null)
        {
            g = p.findNextRoom();
            goal = g.nodeRoom;
            root.SetData("Goal", goal);
        }
            
        
            p.goalRoom = g.nodeRoom;
            p.target = g;
       
        p.open = new PriorityQueue<float, Path>();
        p.closed.Clear();
        p.currentIndex = 0;
        p.findPath();



        if (p.mostRecentNode.nodeRoom != p.goalRoom)
        {
            state = NodeState.RUNNING;
            return state;
        }
        else
        {

            Debug.Log("Yo");
            state = NodeState.SUCCESS;
            root.ClearData("Goal");
            p.target = p.mostRecentNode;
            p.findPathGlobal();
            goal= null;
            return state;
        }

        
    }
}
