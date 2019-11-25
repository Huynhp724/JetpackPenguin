using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
    public LayerMask ground;
    public bool pickedUp = false;
    [SerializeField] Transform bottomOfBlock;
    [SerializeField] Transform topOfBlock;
    [SerializeField] GameObject ButtonPrompt;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 14 && checkIfThrown())
        {
            collision.gameObject.GetComponent<EnemyHealth>().LoseHp();
        }
    }

    private bool checkIfThrown()
    {
        if (Physics.CheckSphere(gameObject.transform.position - new Vector3(0, GetComponent<Collider>().bounds.extents.y / 2 + 0.2f, 0), GetComponent<Collider>().bounds.extents.y / 2, ground))
        {
            return false;
        }
        return true;
    }

    public float getHalfSize()
    {
        print(topOfBlock.position.y + " : " + bottomOfBlock.position.y);
        return topOfBlock.position.y - bottomOfBlock.position.y;
    }

    public void activatePrompt()
    {
        ButtonPrompt.SetActive(true);
    }

    public void deactivatePrompt()
    {
        ButtonPrompt.SetActive(false);
    }
}
