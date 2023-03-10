using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // Start is called before the first frame update
    public string roomName;
    public List<Room> connectedRooms;
    public float dangerLevel = 0;
    public float baseDanger = 0;
    public List<AIAgent> frozenAgents = new List<AIAgent>();

    private void Awake()
    {
        roomName = this.gameObject.name;
        dangerLevel = baseDanger;
    }
    private void Update()
    {
        if (frozenAgents.Count > 0)
        {
            //dangerLevel = 0;
        }

    }

    
    /*
    public void dangerLevelCalc()
    {
        dangerLevel = baseDanger;
        if (agents.Length <= 0)
        {
            agents = (AIAgent[])GameObject.FindObjectsOfType(typeof(AIAgent));
        }
        
        foreach (AIAgent agent in agents)
        {
            if (agent != null)
            {
                if (agent.isSeeker)
                {
                    if(agent.GetComponent<Pathfinder>().mostRecentNode.nodeRoom == this)
                    {
                        dangerLevel = dangerLevel + 10;
                        foreach(Room room in connectedRooms) {
                            room.dangerLevel = room.dangerLevel + 10;
                            print(room.dangerLevel);
                        }
                    }
                }
            }
        }
    }
    */

}
