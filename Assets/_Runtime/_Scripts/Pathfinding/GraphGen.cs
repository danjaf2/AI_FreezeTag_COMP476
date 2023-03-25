using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GraphGen : MonoBehaviour
{
    public LayerMask mask;
    // Start is called before the first frame update
   void Awake()
    {
        Node[] nodes = GameObject.FindObjectsOfType<Node>();
        foreach (Node node in nodes)
        {
            node.connectedNodes.Clear();
            node.costs.Clear();
            node.GetComponent<BoxCollider>().enabled = false;
            foreach (Node target in nodes)
            {

                if (node.gameObject != target.gameObject)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(node.transform.position, target.transform.position - node.transform.position, out hit, 7f, mask))
                    {
                        if (hit.transform.gameObject.TryGetComponent<Node>(out Node hitNode))
                        {
                            if (hitNode.gameObject == target.gameObject)
                            {
                                node.connectedNodes.Add(target);
                                //node.costs.Add(Vector3.Magnitude(target.transform.position - node.transform.position));
                                node.costs.Add(1);
                            }
                            
                        }


                    }
                }

            }
            node.GetComponent<BoxCollider>().enabled = true;

        }
    }
}
