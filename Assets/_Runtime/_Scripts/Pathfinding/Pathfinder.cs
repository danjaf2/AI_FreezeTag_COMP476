using AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    // Start is called before the first frame update
    public Node start;
    public Node target;
    public Node nullNode;
    bool found = false;
    public Path solution = new Path();
    public int currentIndex=0;

    public PriorityQueue<float, Path> open = new PriorityQueue<float, Path>();
    public List<Node> closed = new List<Node>();
    

    private AIAgent agent;

    public Node mostRecentNode=null;

    public Room mostRecentRoom = null;

    public Room goalRoom;

    public Room startRoom;

    public bool betweenTwoNodes;


    void Start()
    {
        if (this.GetComponent<AIAgent>().move)
        {

       
        //this.GetComponent<AIAgent>().findSeekerNextTarget();
        goalRoom = mostRecentNode.nodeRoom;
        agent= GetComponent<AIAgent>();
        if (start == null)
        {
            start = mostRecentNode;
            if (agent.isSeeker)
            {
                //InvokeRepeating("findNextRoom", 0f, 0.25f);
            }
            else
            {
                //findNextRoom();
            }
            
        }
        }
    }
    float timer = 0;
    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<AIAgent>().move)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {


            }

            if (mostRecentRoom != mostRecentNode.nodeRoom)
            {
                if (agent.isSeeker)
                {

                    mostRecentNode.nodeRoom.dangerLevel += 20;
                    mostRecentRoom = mostRecentNode.nodeRoom;
                    foreach (Room r in mostRecentNode.nodeRoom.connectedRooms)
                    {
                        r.dangerLevel += 10;
                    }
                    StartCoroutine(roomDangerLevelChange(mostRecentNode.nodeRoom));

                }
            }

            if (currentIndex > 50)
            {
                currentIndex = 0;
                //findPathGlobal();
            }

        }
        if(this.GetComponent<AIAgent>().isPlayer&&this.GetComponent<AIAgent>().isSeeker) {
            if (mostRecentRoom != mostRecentNode.nodeRoom)
            {
                if (this.GetComponent<AIAgent>().isSeeker)
                {

                    mostRecentNode.nodeRoom.dangerLevel += 20;
                    mostRecentRoom = mostRecentNode.nodeRoom;
                    foreach (Room r in mostRecentNode.nodeRoom.connectedRooms)
                    {
                        r.dangerLevel += 10;
                    }
                    StartCoroutine(roomDangerLevelChange(mostRecentNode.nodeRoom));

                }
            }
        }




    }

    IEnumerator roomDangerLevelChange(Room room)
    {
        yield return new WaitForSeconds(5f);

        if (this.GetComponent<AIAgent>().isSeeker)
        {
            room.dangerLevel -= 20;
            mostRecentRoom = null;
            foreach (Room r in room.connectedRooms)
            {
                r.dangerLevel -= 10;
            }
        }

    }

    public bool findPath()
    {
        if(agent==null)
        {
            agent = this.GetComponent<AIAgent>();
        }
        if (agent.isSeeker)
        {
            return false;
        }
        startRoom = mostRecentNode.nodeRoom;
        start = agent.trackedTarget;
        if(start==target||target==null||start==null)
        {
            return false;
        }
        List<Node> p = new List<Node>();
        open = new PriorityQueue<float, Path>();
        found = false;
        closed.Clear();
        p.Add(start);
        Path s = new Path(start, p);
        open.Enqueue(0, s);
        while (!found)
        {

            Path sol = open.Dequeue().data;
            while (closed.Contains(sol.currentNode))
            {
                sol = open.Dequeue().data;
            }
            Node visiting = sol.currentNode;


            if (sol.currentNode.gameObject == target.gameObject)
            {
                solution = sol;
                found = true;
                //Debug.Log("solution found");
                break;
            }
            for (int i = 0; i < visiting.connectedNodes.Count; i++)
            {
                
                
                    float hValue = HeuristicEuclideDistance(visiting.connectedNodes[i], target);
                    sol.cost = sol.cost + visiting.costs[i];
                    float priority = hValue + (sol.cost);
                    if (visiting.connectedNodes[i].nodeRoom == startRoom || visiting.connectedNodes[i].nodeRoom == goalRoom)
                {
                    if (closed.Contains(visiting.connectedNodes[i]))
                    {
                        int index = closed.FindIndex(a => a == visiting.connectedNodes[i]);
                        //print(index);
                        continue;
                    }

                    List<Node> l = new List<Node>(sol.prevNode);
                    l.Add(visiting.connectedNodes[i]);
                    Path newPath = new Path(visiting.connectedNodes[i], l);
                    open.Enqueue(priority, newPath);
                    if (!closed.Contains(visiting))
                    {
                        closed.Add(visiting);
                    }
                }
                else
                {
                    continue;
                }     
            }
            if (open.Count == 0)
            {
                solution = sol;
                found = false;
                findPathGlobal();
                open = new PriorityQueue<float, Path>();
                closed.Clear();
                currentIndex = 0;
                Debug.Log("no solution going global");
                return false;
            }
        }
        if (found)
        {
            currentIndex = 0;
            
                currentIndex = 0;

                agent.TargetPosition = agent.trackedTarget.transform.position;
                agent.trackedTarget = agent.trackedTarget;

            if (solution.prevNode.Count > 1)
            {
                LayerMask layerMask = LayerMask.GetMask("UI", "Walls");
                RaycastHit hit;
                //print(solution.prevNode[0].gameObject.name);
                if (Physics.Raycast(transform.position, solution.prevNode[1].transform.position - (this.transform.position), out hit, Mathf.Infinity, layerMask))
                {

                    if (hit.transform.gameObject.layer == 5)
                    {
                        if (hit.transform.gameObject == solution.prevNode[1].gameObject)
                        {
                            //print(hit.transform.gameObject.name);
                            agent.TargetPosition = solution.prevNode[1].transform.position;
                            agent.trackedTarget = solution.prevNode[1];

                            solution.prevNode.Remove(solution.prevNode[0]);
                            //print(agent.trackedTarget.name);
                        }
                    }
                }
            }


            }
            

       
        return true;
    }


    public bool findPathGlobal()
    {
        if(agent.trackedTarget== null)
        {
            startRoom = mostRecentNode.nodeRoom;
            start = mostRecentNode;
        }
        else
        {
            startRoom = agent.trackedTarget.nodeRoom;
            start = agent.trackedTarget;
        }
        
        
        if(start == target)
        {
            if(start==target||target==null||start==null)
        {
            return false;
        }
            if(agent.isSeeker) {
                if (agent.chasingCoin)
                {
                    //target = agent.AIAgentTarget.GetComponent<Pathfinder>().mostRecentNode;
                }
            }
        }
        List<Node> p = new List<Node>();
        open = new PriorityQueue<float, Path>();
        found = false;
        closed.Clear();
        p.Add(start);
        Path s = new Path(start, p);
        open.Enqueue(0, s);
        while (!found)
        {
            Path sol = open.Dequeue().data;
            while (closed.Contains(sol.currentNode))
            {
                sol = open.Dequeue().data;
            }
            Node visiting = sol.currentNode;

            if(target!=null) {
                if (sol.currentNode.gameObject == target.gameObject)
                {
                    solution = sol;
                    found = true;
                    //Debug.Log("solution found");
                    break;
                }
            }
            
            for (int i = 0; i < visiting.connectedNodes.Count; i++)
            {


                float hValue = HeuristicEuclideDistance(visiting.connectedNodes[i], target);
                sol.cost = sol.cost + visiting.costs[i];
                float priority = hValue + (sol.cost) * sol.prevNode.Count;
                
                    if (closed.Contains(visiting.connectedNodes[i]))
                    {
                        int index = closed.FindIndex(a => a == visiting.connectedNodes[i]);
                        //print(index);
                        
                            continue;
                       
                    }

                    List<Node> l = new List<Node>(sol.prevNode);
                    l.Add(visiting.connectedNodes[i]);
                    Path newPath = new Path(visiting.connectedNodes[i], l);
                    open.Enqueue(priority, newPath);
                    if (!closed.Contains(visiting))
                    {
                        closed.Add(visiting);
                    }
            }
            if (open.Count == 0)
            {
                solution = sol;
                found = false;
                Debug.Log("no solution");
                return false;
            }
        }
        if (found)
        {
            currentIndex = 0;
            
                currentIndex = 0;

            if (agent.trackedTarget == null)
            {
                agent.trackedTarget = mostRecentNode;
                agent.TargetPosition = mostRecentNode.transform.position;
            }
            else
            {
                agent.TargetPosition = agent.trackedTarget.transform.position;
                agent.trackedTarget = agent.trackedTarget;
            }
            
                
                
                    LayerMask layerMask = LayerMask.GetMask("UI", "Walls");
                    RaycastHit hit;
                    //print(solution.prevNode[0].gameObject.name);
                    if(solution.prevNode.Count > 1)
                    {

                    if (Physics.Raycast(transform.position, solution.prevNode[1].transform.position - this.transform.position, out hit, Mathf.Infinity, layerMask))
                    {

                        if (hit.transform.gameObject.layer == 5)
                        {
                            if (hit.transform.gameObject == solution.prevNode[1].gameObject)
                            {
                                //print(hit.transform.gameObject.name);
                                agent.TargetPosition = solution.prevNode[1].transform.position;
                                agent.trackedTarget = solution.prevNode[1];
                            solution.prevNode.Remove(solution.prevNode[0]);
                            }
                        }
                    }
                    goalRoom = solution.prevNode[solution.prevNode.Count - 1].nodeRoom;
                   }


        }

        return true;
    }


    public bool findPathGlobalFlank(Path otherSolution)
    {
        if(otherSolution == null)
        {
            return false;
        }
        if (agent.trackedTarget == null)
        {
            startRoom = mostRecentNode.nodeRoom;
            start = mostRecentNode;
        }
        else
        {
            startRoom = agent.trackedTarget.nodeRoom;
            start = agent.trackedTarget;
        }


        if (start == target)
        {
            if (start == target || target == null || start == null)
            {
                return false;
            }
            if (agent.isSeeker)
            {
                if (agent.chasingCoin)
                {
                    //target = agent.AIAgentTarget.GetComponent<Pathfinder>().mostRecentNode;
                }
            }
        }
        List<Node> p = new List<Node>();
        open = new PriorityQueue<float, Path>();
        found = false;
        closed.Clear();
        for(int i = 0; i<otherSolution.prevNode.Count-1;i++)
        {
            closed.Add(otherSolution.prevNode[i]);
        }
        p.Add(start);
        Path s = new Path(start, p);
        open.Enqueue(0, s);
        while (!found)
        {
            Path sol = open.Dequeue().data;
            while (closed.Contains(sol.currentNode))
            {
                if(open.Count> 0)
                {
                sol = open.Dequeue().data;
                }
                else
                {
                    solution = sol;
                    found = false;
                    Debug.Log("no solution");
                    findPathGlobal();
                    return false;
                }
            }
            Node visiting = sol.currentNode;

            if (target != null)
            {
                if (sol.currentNode.gameObject == target.gameObject)
                {
                    solution = sol;
                    found = true;
                    //Debug.Log("solution found");
                    break;
                }
            }

            for (int i = 0; i < visiting.connectedNodes.Count; i++)
            {


                float hValue = HeuristicEuclideDistance(visiting.connectedNodes[i], target);
                sol.cost = sol.cost + visiting.costs[i];
                float priority = hValue + (sol.cost) * sol.prevNode.Count;

                if (closed.Contains(visiting.connectedNodes[i]))
                {
                    int index = closed.FindIndex(a => a == visiting.connectedNodes[i]);
                    //print(index);

                    continue;

                }

                List<Node> l = new List<Node>(sol.prevNode);
                l.Add(visiting.connectedNodes[i]);
                Path newPath = new Path(visiting.connectedNodes[i], l);
                open.Enqueue(priority, newPath);
                if (!closed.Contains(visiting))
                {
                    closed.Add(visiting);
                }
            }
            if (open.Count == 0)
            {
                solution = sol;
                found = false;
                Debug.Log("no solution");
                findPathGlobal();
                return false;
            }
        }
        if (found)
        {
            currentIndex = 0;

            currentIndex = 0;

            if (agent.trackedTarget == null)
            {
                agent.trackedTarget = mostRecentNode;
                agent.TargetPosition = mostRecentNode.transform.position;
            }
            else
            {
                agent.TargetPosition = agent.trackedTarget.transform.position;
                agent.trackedTarget = agent.trackedTarget;
            }



            LayerMask layerMask = LayerMask.GetMask("UI", "Walls");
            RaycastHit hit;
            //print(solution.prevNode[0].gameObject.name);
            if (solution.prevNode.Count > 1)
            {

                if (Physics.Raycast(transform.position, solution.prevNode[1].transform.position - this.transform.position, out hit, Mathf.Infinity, layerMask))
                {

                    if (hit.transform.gameObject.layer == 5)
                    {
                        if (hit.transform.gameObject == solution.prevNode[1].gameObject)
                        {
                            //print(hit.transform.gameObject.name);
                            agent.TargetPosition = solution.prevNode[1].transform.position;
                            agent.trackedTarget = solution.prevNode[1];
                            solution.prevNode.Remove(solution.prevNode[0]);
                        }
                    }
                }
                goalRoom = solution.prevNode[solution.prevNode.Count - 1].nodeRoom;
            }


        }

        return true;
    }


    public Node findNextRoom()
    {
        Node n=null;
        Room r=null;
        if(agent == null)
        {
            agent = this.GetComponent<AIAgent>();
        }
        if (agent.isSeeker)
        {
            if (agent.AIAgentTarget == null)
            {
                //agent.findSeekerNextTarget();
            
            }
                //target = agent.AIAgentTarget.GetComponent<Pathfinder>().mostRecentNode;
                open = new PriorityQueue<float, Path>();
                closed.Clear();
                //findPathGlobal();
                //NextTarget();
        }
        else
        {
            System.Random random = new System.Random();
            startRoom = mostRecentNode.nodeRoom;
            float min = startRoom.connectedRooms[0].dangerLevel;
            List<Room> smallestDangerRooms = new List<Room>();
            foreach (Room room in startRoom.connectedRooms)
            {
                if(room.dangerLevel <= min)
                {
                    if (room.dangerLevel == min)
                    {
                        smallestDangerRooms.Add(room);
                    }
                    else
                    {
                        smallestDangerRooms.Clear();
                        min = room.dangerLevel;
                        smallestDangerRooms.Add(room);
                    }
                }
            }
            var randomIndex = random.Next(0, smallestDangerRooms.Count);
            r = smallestDangerRooms[randomIndex];
            
                Node[] nodeInRoom = r.GetComponentsInChildren<Node>();
                var randomNode = random.Next(0, nodeInRoom.Length);
                n = nodeInRoom[randomNode];
            
            
        }
        return n;
        
    }
    

    public float HeuristicEuclideDistance(Node n1, Node n2)
    {
        float value = Vector3.Magnitude(n1.transform.position - n2.transform.position);

        return value;
    }
    private void OnDrawGizmos()
    {
       
        for (int i = 0; i < solution.prevNode.Count; i++)
        {
            if (i != 0)
            {
                Gizmos.DrawLine(solution.prevNode[i].transform.position, solution.prevNode[i - 1].transform.position);

            }
        }
    }

    public bool NextTarget()
    {
        
        
        currentIndex++;
        if(found)
        {
            if (currentIndex < solution.prevNode.Count)
            {
                agent.TargetPosition = solution.prevNode[currentIndex].transform.position;
                agent.trackedTarget = solution.prevNode[currentIndex];
                
            }
            else
            {
                agent.TargetPosition = solution.prevNode[solution.prevNode.Count - 1].transform.position;
                agent.trackedTarget = solution.prevNode[solution.prevNode.Count - 1];
            }

        }
        
       else
        {
            return false;
        }
        return true;
    }
}
