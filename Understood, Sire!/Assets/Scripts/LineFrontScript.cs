using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFrontScript : MonoBehaviour
{

    public int health;
    public bool attack;
    private bool retreat;
    private bool run;

    private const float range = 5f;
    private int soldierCount = 17;

    private Vector3 rayPosition;

    private float fireTime;

    [SerializeField] private int unitRank = 0;

    private int damage;

    private float timeToFire = 16;

    public float morale;

    [SerializeField] private Sprite rearward;
    private Sprite normalSprite;

    private Vector3 initialPosition; // for the return position of the retreat

    // to calculate moral Shock
    private int oldHealth;
    private int healthDifference;

    // Start is called before the first frame update
    void Start()
    {
        health = 17; // currenly every unit start at full health
        morale = 100;
        oldHealth = health;

        attack = false;
        run = false;
        retreat = false;

        rayPosition = transform.position;
        rayPosition.y += 0.5f;

        fireTime = 15 - (unitRank * 1.5f); // At max level Firing time is 7.5 seconds

        // to rotate relative to the camera
        float rotateY = transform.position.x;
        GetComponentInChildren<Transform>().eulerAngles = new Vector3(0f, rotateY, 0f);

        normalSprite = GetComponentInChildren<SpriteRenderer>().sprite;

        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        damage = 0;

        if (health <= 0)
        {
            Destroy(this.transform.gameObject);
            return; // to end reading the code
        }

        // Used for calculating the morale shock
        if (health < oldHealth)
        {
            healthDifference = oldHealth - health;
        }

        // reduce the number of soldiers in the unit
        if (!(soldierCount == health))
        {
            DestroyChild();
        }

        // Unit Morale Shock Calculation
        morale -= (healthDifference * (1 / (unitRank + 1) * 6.5f));

        MoraleBehaviour();

        // reload timer
        timeToFire += Time.deltaTime * morale / 100;

        if (attack && timeToFire >= fireTime)
        {

            RaycastHit hit;

            if (Physics.Raycast(rayPosition, -transform.forward, out hit, range))
            {

                if (hit.collider.CompareTag("Friendly"))
                {
                    damage = Volley(hit.distance);

                    // Do Damage
                    hit.transform.gameObject.GetComponent<LineBackScript>().health -= damage;

                    // Under Fire Stress
                    hit.transform.gameObject.GetComponent<LineBackScript>().morale -= soldierCount;
                    
                    timeToFire = 0;
                }
                
            }

        }

        if (timeToFire >= 15.1f)
        {
            timeToFire = 15.1f;
        }

        healthDifference = 0;

        oldHealth = health;

        morale += Time.deltaTime*2 + (unitRank * 2f);
    }

    private int Volley(float distance)
    {

        float numberOfHits = Random.Range(1, 18);

        float damage = numberOfHits * (unitRank * Random.Range(1f, 20f) / 100) + (Random.Range(0f, 1f) * 10) + 10 * 1 / distance;
        damage -= oldHealth - health; // reduce the number of guns
        damage -= 2;
        int casualtiesInflicted = Mathf.RoundToInt(damage);

        return casualtiesInflicted;

    }

    private void DestroyChild()
    {
        GameObject childObject;
        childObject = transform.GetChild(Random.Range(0, transform.childCount)).gameObject;
        Destroy(childObject);
        soldierCount--;
    }

    private void MoraleBehaviour()
    {
        if (morale >= 100)
        {
            morale = 100;
        }
        else if (morale < 15 || health <= 4)
        {
            timeToFire = 0;

            if (run == false)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = rearward;
                }
            }

            Run();
            return;
        }
        else if (morale <= 30)
        {

            if (retreat == false)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = rearward;
                }

            }

            RunToRegroup();
        }
        else if (morale > 30)
        {
            if(retreat == true)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = normalSprite;
                }

                retreat = false;
            }
            
            Regroup();
           
        }
    }

    private void Run()
    {
        transform.position += new Vector3(0f, 0f, 0.01f);
        morale = 14;
        run = true;
    }

    private void RunToRegroup()
    {
        timeToFire = 0;
        transform.position += new Vector3(0f, 0f, 0.01f);
        retreat = true;
    }

    private void Regroup()
    {
        if(transform.position.z <= initialPosition.z)
        {
            return;
        }

        transform.position -= new Vector3(0f, 0f, 0.005f);
    }
}
