using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour
{
    public List<Transform> Tails;
    [Range(0,3)]
    public float BonesDistance;
    public GameObject BonePrefab;
    [Range(0, 4)]
    public float Speed;
    private Transform _transform;
    private bool isDownLeft;
    private bool isDownRight;
    public float XSize = 4.3f;
    public float ZSize = 4.3f;

    public GameObject FoodSpeed;
    public GameObject FoodUnspeed;
    public GameObject FoodDel;

    public Vector3 posSpeed;
    public Vector3 posUnspeed;
    public Vector3 posDel;

    public GameObject curSpeed;
    public GameObject curUnspeed;
    public GameObject curDel;

    public Text Schet;
    private void Start()
    {
        RandomPos();
        curSpeed = GameObject.Instantiate(FoodSpeed, posSpeed, Quaternion.identity);
        curUnspeed = GameObject.Instantiate(FoodUnspeed, posUnspeed, Quaternion.identity);
        curDel = GameObject.Instantiate(FoodDel, posDel, Quaternion.identity);

        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    private void Update()
    {
        MoveSnake(_transform.position + _transform.forward * Speed);

        if (isDownLeft) _transform.Rotate(0, -1, 0);
        if (isDownRight) _transform.Rotate(0, 1, 0);

        Schet.text = "Ñ÷¸ò:" + (Tails.Count-4).ToString();
    }

    private void RandomPos()
    {
        posSpeed = new Vector3(Random.Range(XSize * -1, XSize), 0.0812f, Random.Range(ZSize * -1, ZSize));
        posUnspeed = new Vector3(Random.Range(XSize * -1, XSize), 0.0812f, Random.Range(ZSize * -1, ZSize));
        posDel = new Vector3(Random.Range(XSize * -1, XSize), 0.0812f, Random.Range(ZSize * -1, ZSize));
    }

    public void LeftDown()
    {
        this.isDownLeft = true;
    }
    public void LeftUp()
    {
        this.isDownLeft = false;
    }

    public void RightDown()
    {
        this.isDownRight = true;
    }
    public void RightUp()
    {
        this.isDownRight = false;
    }

    private void MoveSnake(Vector3 newPosition)
    {
        float sqrDistance = BonesDistance * BonesDistance;
        Vector3 previorusPosition = _transform.position;

        foreach(var bone in Tails)
        {
            if ((bone.position - previorusPosition).sqrMagnitude > sqrDistance)
            {
                var temp = bone.position;
                bone.position = previorusPosition;
                previorusPosition = temp;
            }
            else
            {
                break;
            }
        }


        _transform.position = newPosition;
    }
    private void OnCollisionEnter(Collision collision)
    {
       if(collision.gameObject.tag == "Speed")
        {
            Destroy(collision.gameObject);

            Speed += 0.002f;

            var bone = Instantiate(BonePrefab);
            Tails.Add(bone.transform);

            RandomPos();
            curSpeed = GameObject.Instantiate(FoodSpeed, posSpeed, Quaternion.identity);
            curDel = GameObject.Instantiate(FoodDel, posDel, Quaternion.identity);
        }

        if (collision.gameObject.tag == "Unspeed")
        {
            Destroy(collision.gameObject);

            if (Speed > 0.001f) Speed -= 0.002f;
            

            var bone = Instantiate(BonePrefab);
            Tails.Add(bone.transform);

            RandomPos();
            curUnspeed = GameObject.Instantiate(FoodUnspeed, posUnspeed, Quaternion.identity);
            curDel = GameObject.Instantiate(FoodDel, posDel, Quaternion.identity);
        }

        if (collision.gameObject.tag == "Del")
        {
            Destroy(collision.gameObject);

            if (Tails.Count > 4)
            {
                Destroy(Tails[Tails.Count - 1].gameObject);
                Tails.Remove(Tails[Tails.Count - 1]);
            }

        }
    }
}
