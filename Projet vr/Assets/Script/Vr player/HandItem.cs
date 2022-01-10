using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
using static ItemProperties;

public class HandItem : MonoBehaviour
{
    public Transform controlleur;
    public LayerMask mask;
    public XRNode inputSource;

    public Item selecter;

    public MapGenerateur gene;

    private GameObject instance;
    bool grabInput;

    private Vector3 destroyBlock;
    private Vector3 PoseBloc;
    private bool found = false;

    bool instancier = false;

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.gripButton, out grabInput);

        //PlaceBloc();

        if (selecter == null)
            return;

        if (selecter.itemType == ItemType.TOOL)
        {
            if (!instancier)
            {
                //pour avoir le transform attache pas trouver un autre moyen
                instance = Instantiate<GameObject>(selecter.model, controlleur.GetChild(0).GetChild(0).GetChild(0).GetChild(0).position, controlleur.transform.rotation, controlleur.GetChild(0).GetChild(0).GetChild(0));
                instancier = true;
            }
            
            if (instancier)
            {
                GameObject.Destroy(instance.gameObject);
                instancier = false;
            }
            CasseBloc();
/*
            if(grabInput && found)
            {
                gene.EditWorld(destroyBlock.x, destroyBlock.y, destroyBlock.z, false, ItemName.STONE_BLOC);
            }
*/
        }
        else if (selecter.itemType == ItemType.BLOC)
        {
            if (!instancier)
            {
                if(instance != null)
                {
                    GameObject.Destroy(instance.gameObject);
                }
                //pour avoir le transform attache pas trouver un autre moyen
                instance = Instantiate<GameObject>(selecter.model, controlleur.GetChild(0).GetChild(0).GetChild(0).GetChild(0).position, controlleur.transform.rotation, controlleur.GetChild(0).GetChild(0).GetChild(0));
                instancier = true;
            }
            PBloc();
/*
            if (grabInput && found)
            {
                gene.EditWorld(PoseBloc.x, PoseBloc.y, PoseBloc.z, true, ItemName.STONE_BLOC);
            }
*/
        }
    }

    public void PlaceBloc()
    {
        float step = 0.1f;
        Vector3 lastPos = new Vector3();

        while (step < 8f)
        {
            Vector3 pos = controlleur.transform.position + (controlleur.transform.forward * step);
            if (gene.CheckBloc(pos.x, pos.y, pos.z))
            {
                destroyBlock = new Vector3(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));
                PoseBloc = lastPos;
                found = true;

                return;
            }
            lastPos = new Vector3(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));

            step += 0.1f;
        }
        found = false;
    }
    public void PBloc()
    {
        if (grabInput)
        {
            
            RaycastHit hit;
            if (Physics.Raycast(controlleur.position, controlleur.forward, out hit, Mathf.Infinity, mask))
            {
                Chunk c = hit.collider.gameObject.GetComponent<Chunk>();
                
                
                Vector3 position = hit.point;

                int x = Mathf.FloorToInt(position.x) - c.x * c.xSize;
                int y = Mathf.FloorToInt(position.y);
                int z = Mathf.FloorToInt(position.z) - c.z * c.zSize;
            
                c.data[x, y, z].terre = true;
                c.refresh();
            }
            
        }
    }

    public void CasseBloc()
    {
        if (grabInput)
        {
            RaycastHit hit;
            if (Physics.Raycast(controlleur.position, controlleur.forward, out hit, Mathf.Infinity, mask))
            {
                Chunk c = hit.collider.gameObject.GetComponent<Chunk>();
                Vector3 position = hit.point;

                int x = Mathf.FloorToInt(position.x) - c.x * c.xSize;
                int y = Mathf.FloorToInt(position.y);
                int z = Mathf.FloorToInt(position.z) - c.z * c.zSize;

                c.data[x, y, z].terre = false;
                c.refresh();
            }
        }
    }
}
