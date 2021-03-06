﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    //Script handles all weapon equipping, unequiping
public class WeaponManager : MonoBehaviour {

    public float range = 2;
    public GameObject w_initial = null;

    private Vector3 w_position;
    private GameObject w_current;
    private Rigidbody w_rb;
    private Camera fpsCam;

    private W_Makarov w_script;

	// Use this for initialization
	private void Awake () {
        fpsCam = GetComponentInChildren<Camera>();
        if (w_initial!=null)
        {
            Equip(w_initial);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
            //draws line from player to center of screen, 'hit' refers to the gameObject the line ends at
        RaycastHit hit;
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));

            //player picks up weapon when "e" is pressed
        if (Input.GetKeyDown("e"))
        {
            // hit.collider.gameObject.GetComponent<>
            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, range))
            {
                if (hit.collider.gameObject.tag.Equals("Weapon"))
                {
                    GameObject w_new = hit.collider.gameObject;
                        //player already has weapon
                    if (w_current != null)
                    {
                        Unequip();
                        Equip(w_new);
                    }
                        //player has no weapon
                    else
                    {
                        Equip(w_new);
                    }
                }
                else
                {
                    print("not a weapon");
                }
            }
        }
 /*       else if (Input.GetKeyDown("r"))
        {
            w_script.Reload();
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            w_script.Fire();
        }*/

    }
        //Equips weapon w_new
    void Equip (GameObject w_new)
    {
        print("take"); //debug
        w_rb = w_new.GetComponent<Rigidbody>();
        w_rb.isKinematic = true;
        w_rb.useGravity = false;

            //disable collision
        Collider[] c_off = w_new.GetComponents<Collider>();
        foreach (Collider c in c_off)
        {
            c.enabled = false;
        }

        w_new.GetComponent<LineRenderer>().enabled = true;

            //enables W_Makarov script on weapon
        w_script = w_new.GetComponentInChildren<W_Makarov>();

        w_script.enabled = true;
        w_script.equipped = true;

        w_current = w_new;
        w_current.transform.parent = fpsCam.transform;
            
            //reorients weapon on screen
        w_current.transform.position = fpsCam.transform.position;
        w_current.transform.Translate(w_script.cam_pos, fpsCam.transform);
        w_current.transform.localEulerAngles = new Vector3(transform.rotation.y, fpsCam.transform.rotation.x - 180, 0);
    }

        //Unequips current weapon, unequipped weapons have their scripts dsiabled and are treated like a physics object until re-equipped
    void Unequip()
    {
        print("drop");

        if (w_script.reloading)
        {
            w_current.transform.Translate(0, 0, 2);
            w_script.cur_clip = 0;
        }

            //unparent
        w_current.transform.parent = null;

            //enable collision
        Collider[] c_on = w_current.GetComponents<Collider>();
        foreach(Collider c in c_on)
        {
            c.enabled = true;
        }

            //enable physics
        w_rb.isKinematic = false;
        w_rb.useGravity = true;
        w_current.GetComponent<LineRenderer>().enabled = false;
        w_script.equipped = false;
        w_script.enabled = false;

        w_rb.AddForce(fpsCam.transform.forward);
    }

    public void SwapEquip(GameObject enemy)
    {

    }

    public void SwapUnequip()
    {

    }
}
