using AI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] 
    private Vector3 rotationPerSecond;
    public LayerMask mask;
    CoinSpawner spawner;
    public static GameObject ScoreKeeper;

    public static GameObject RunnerCoin;
    public static GameObject SeekerCoin;

    public static int coinCountRunner;
    public static int coinCountSeeker;

    void Update() {
        transform.Rotate(rotationPerSecond * Time.deltaTime);
        foreach (AIAgent seeker in AIAgent.seekerList)
        {
            //seeker.AIAgentTarget = this.GetComponent<AIAgent>();
            //seeker.GetComponent<Pathfinder>().target = this.GetComponent<Pathfinder>().mostRecentNode;
            //seeker.chasingCoin = true;
            //seeker.wings.SetActive(false);
            //seeker.wings2.SetActive(false);
        }
    }

    private void Start()
    {
        ScoreKeeper = GameObject.Find("MainMenu");
        RunnerCoin = GameObject.Find("RunnerCoin");
        SeekerCoin = GameObject.Find("SeekerCoin");
        spawner = GameObject.FindObjectOfType<CoinSpawner>();
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 2f, mask);
        foreach (var hitCollider in hitColliders)
        {
            this.GetComponent<Pathfinder>().mostRecentNode = hitCollider.GetComponent<Node>();
        }
        //foreach (AIAgent seeker in AIAgent.seekerList)
        //{
            //seeker.AIAgentTarget = this.GetComponent<AIAgent>();
            //seeker.GetComponent<Pathfinder>().target = this.GetComponent<Pathfinder>().mostRecentNode;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 11)
        {
            other.GetComponent<AIAgent>().maxSpeed += 0.1f;
            spawner.spawned = false;
            coinCountSeeker++;
            SeekerCoin.GetComponent<TMP_Text>().text = "Seekers: " + coinCountSeeker.ToString();
            //spawner.coinText.text = "No coin has spawned";
            foreach (AIAgent seeker in AIAgent.seekerList)
            {
                //seeker.chasingCoin = false;
                //seeker.findSeekerNextTarget();
                //seeker.wings.SetActive(true);
                //seeker.wings2.SetActive(true);
            }
            Destroy(this.gameObject);
        }
        if (other.gameObject.layer == 7)
        {
                ScoreKeeper.GetComponent<MainMenu>().score += 10;
                spawner.spawned = false;
                coinCountRunner++;
                RunnerCoin.GetComponent<TMP_Text>().text = "Runners: " + coinCountRunner.ToString();
                //spawner.coinText.text = "No coin has spawned";
                foreach (AIAgent seeker in AIAgent.seekerList)
                {
                    //seeker.chasingCoin = false;
                    //seeker.findSeekerNextTarget();
                    //seeker.wings.SetActive(true);
                    //seeker.wings2.SetActive(true);
                }
                Destroy(this.gameObject);
            
            
        }
        }

    public static void ResetValues()
    {
        coinCountRunner = 0;
        coinCountSeeker = 0;
    }
}
