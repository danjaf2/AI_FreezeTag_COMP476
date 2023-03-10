using AI;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public bool spawned=false;
    List<Node> components = new List<Node>();
    public GameObject prefab;
    public  TMP_Text coinText;
    void Start()
    {
        coinText = GameObject.Find("CoinSpawned").GetComponent<TMP_Text>();
        Node[] init = GameObject.FindObjectsOfType<Node>();
        foreach (Node node in init)
        {
            if(node.transform.gameObject.name != "NullNode")
            {
                components.Add(node);
            }
        }
    }

   public float timer;
    // Update is called once per frame
    void Update()
    {
        if (!spawned)
        {
            timer += Time.deltaTime;
            if(timer > 3)
            {
                timer = 0;
                int number = Random.Range(1, 6);
                print("Rolled" + number.ToString());
                if (number == 2)
                {
                    spawned = true;
                    int index = Random.Range(0, components.Count);
                    Instantiate(prefab, components[index].transform.position, components[index].transform.rotation);
                    coinText.text = "COIN HAS SPAWNED";
                }
            }
            
        }
        
    }
}
