using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Node>connectedNodes= new List<Node>();
    public List<float> costs = new List<float>();
    public Room nodeRoom;
    void Start()
    {
        nodeRoom = GetComponentInParent<Room>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (AIAgent agent in AIAgent.seekerList)
        {
            if (Vector3.Distance(this.transform.position, agent.transform.position) < 1.5)
            {
                
                if (agent.GetComponent<Pathfinder>().betweenTwoNodes)
                {
                    if (agent.trackedTarget == this)
                    {
                        OnTriggerStay(agent.GetComponent<Collider>());
                    }
                    agent.GetComponent<Pathfinder>().mostRecentNode = this;
                }
                
            }
        }
        
        foreach (AIAgent agent in AIAgent.aliveList)
        {
            if (Vector3.Distance(this.transform.position, agent.transform.position) < 1.5)
            {
                if (agent.GetComponent<Pathfinder>().betweenTwoNodes)
                {
                    if (agent.trackedTarget == this)
                    {
                        OnTriggerStay(agent.GetComponent<Collider>());
                    }
                    agent.GetComponent<Pathfinder>().mostRecentNode = this;
                }
                
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        
        foreach (Node target in connectedNodes)
        if (target != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7|| other.gameObject.layer == 11)
        {
            if (other.GetComponent<AIAgent>().isPlayer == false) { 
                if(other.GetComponent<AIAgent>().trackedTarget == null)
                {
                    //other.GetComponent<Pathfinder>().findNextRoom();
                }
                if (other.GetComponent<AIAgent>().trackedTarget != null)
                {
                    if (other.GetComponent<AIAgent>().trackedTarget.gameObject == this.transform.gameObject)
                    {

                        other.GetComponent<Pathfinder>().mostRecentNode = this;
                        other.GetComponent<Pathfinder>().NextTarget();
                        other.GetComponent<Pathfinder>().betweenTwoNodes = false;
                        if (other.GetComponent<Pathfinder>().goalRoom == this.nodeRoom)
                        {
                            //other.GetComponent<Pathfinder>().findNextRoom();
                        }


                    }
                }
                
            
            }
            other.GetComponent<Pathfinder>().mostRecentNode = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Pathfinder>().mostRecentNode == this)
        {
            other.GetComponent<Pathfinder>().betweenTwoNodes = true;
        }
    }
    }
