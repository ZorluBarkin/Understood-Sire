using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBackScript : MonoBehaviour
{

    public int health;

    public bool attack;
    private bool retreat;
    private bool run;
    public bool engage; // this is used for commanding

    private const float range = 5f;
    private int soldierCount = 17;

    private Vector3 rayPosition;

    private float fireTime;

    [SerializeField] private int unitRank = 0;

    private int damage;
    
    private float timeToFire = 16;

    public float morale; // has to be public

    [SerializeField] private Sprite rearward;
    private Sprite normalSprite;

    [SerializeField] private GameObject smoke; // currently assign in editor

    Vector3 smokePos; 

    [SerializeField] private AudioSource musketFire;

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

        attack = true;
        retreat = false;
        run = false;
        engage = true;

        fireTime = 15 - (unitRank * 1.5f); // At max level Firing time is 7.5 seconds

        // to rotate relative to the camera
        /*float rotateY = transform.position.x;
        GetComponentInChildren<Transform>().localEulerAngles = new Vector3(0f, rotateY, 0f);
        //eulerAngles = new Vector3(0f, rotateY, 0f);*/

        RotateChild();

        normalSprite = GetComponentInChildren<SpriteRenderer>().sprite;

        initialPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        damage = 0;

        if (health <= 0)
        {
            Destroy(transform.gameObject);
            return; // to end reading the code
        }

        // Used for calculating the morale shock
        if(health < oldHealth)
        {
            healthDifference = oldHealth - health;
        }

        // reduce the number of soldiers in the unit
        if (!(soldierCount == health))
        {
            DestroyChild();
        }

        // Unit Morale Shock Calculation
        morale -= (healthDifference * (1 / (unitRank+1) * 6.5f));

        MoraleBehaviour();

        // reload timer
        timeToFire += Time.deltaTime * morale / 100;

        if (engage && attack && timeToFire >= fireTime)
        {
            rayPosition = transform.position;
            rayPosition.y += 0.5f;

            smokePos = transform.position;
            smokePos.y += 1f;

            Shoot();
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

        float numberOfHits = Random.Range(1, soldierCount);

        float damage = numberOfHits * (unitRank * Random.Range(1f, 20f) / 100) + (Random.Range(0f, 1f) * 10) + 10 * 1 / distance;

        int casualtiesInflicted = Mathf.RoundToInt(damage/1.5f);

        return casualtiesInflicted;

    }

    private void RotateChild()
    {
        float rotateY = transform.position.x;
        GameObject childObject;

        for(int i = 0; i < transform.childCount; i++)
        {
            childObject = transform.GetChild(i).gameObject;
            childObject.transform.eulerAngles = new Vector3(30f, 0f, -rotateY);
        }
        
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

            if(run == false) 
            { 
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = rearward;
                }
            }

            Run();
            return;
        }
        else if (morale <= 35)
        {

            if(retreat == false)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = rearward;
                }

            }
               
            RunToRegroup();
        }
        else if(morale > 35)
        {
            if (retreat == true)
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
        transform.position -= new Vector3(0f, 0f, 0.01f);
        morale = 14;
        run = true;
    }

    private void RunToRegroup()
    {
        timeToFire = 0;
        transform.position -= new Vector3(0f, 0f, 0.01f);
        retreat = true;
    }

    private void Regroup()
    {
        if (transform.position.z >= initialPosition.z)
        {
            attack = true;
            return;
        }

        transform.position += new Vector3(0f, 0f, 0.005f);
    }

    private void Shoot()
    {

        RaycastHit hit;

        if (Physics.Raycast(rayPosition, transform.forward, out hit, range))
        {

            if (hit.collider.CompareTag("Enemy"))
            {

                Instantiate(smoke, smokePos, transform.rotation); // smoke spawn

                musketFire.PlayOneShot(musketFire.clip, 0.25f);

                damage = Volley(hit.distance);

                // Do Damage
                hit.transform.gameObject.GetComponent<LineFrontScript>().health -= damage;

                // Under Fire Stress
                hit.transform.gameObject.GetComponent<LineFrontScript>().morale -= soldierCount;

                timeToFire = 0;

            }else if (hit.collider.CompareTag("Friendly"))
            {
                //attack = false;
            }

        }
        else
        {
            transform.position += new Vector3(0f, 0f, 0.005f);
        }

    }

}
