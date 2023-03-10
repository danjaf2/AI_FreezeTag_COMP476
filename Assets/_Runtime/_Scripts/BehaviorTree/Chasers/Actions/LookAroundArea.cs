using AI;
using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundArea : DecisionNode
{
    // Start is called before the first frame update
    bool lookingAround = false;
    public LookAroundArea()
    {
        //Constructor for setting up base values in Example Character Tree
    }

    public override NodeState Evaluate()
    {
        Node lastSeen = (Node)GetData("LastSeenNode");
        if (!lookingAround &&GetData("LastSeenNode")==null)
        {
            state = NodeState.FAILURE;
            return state;
        }
        if (!lookingAround)
        {
            referenceTree.StartCoroutine(Rotate(4f, lastSeen));
            state = NodeState.RUNNING;
            return state;
        }


        state = NodeState.SUCCESS;
        return state;
    }

    IEnumerator Rotate(float duration, Node lastSeen)
    {
        float startRotation = referenceTree.transform.eulerAngles.y;
        float endRotation = startRotation + 360.0f;
        lookingAround = true;
        float t = 0.0f;
        while (t < duration&& (Node)GetData("LastSeenNode") == lastSeen)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            referenceTree.transform.eulerAngles = new Vector3(referenceTree.transform.eulerAngles.x, yRotation, referenceTree.transform.eulerAngles.z);
            yield return null;
        }
        if ((Node)GetData("LastSeenNode")==lastSeen)
        {
            root.ClearData("LastSeenNode");
            root.ClearData("Wander");
            lookingAround = false;
        }
        else
        {
            root.ClearData("Wander");
            lookingAround = false;
        }
       
    }
}
