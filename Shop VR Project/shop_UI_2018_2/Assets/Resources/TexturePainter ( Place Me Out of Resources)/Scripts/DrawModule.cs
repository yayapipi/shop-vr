﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawModule : MonoBehaviour
{
    public Renderer canvasBase;
    public Camera canvasCamera;
    public TexturePainter texturePainter;
    public Material material_color;
    public Texture texture_color;
    public Material baseMaterial;
    private Material originalMaterial;

    void Start ()
    {

    }

    public void Enable()
    {
        gameObject.SetActive(true);
        SetPosition();

        //Initial Color Matetial Object
        Texture painterRT_new = Instantiate(texture_color, transform.position, transform.rotation);
        Material baseMaterial_new = Instantiate(baseMaterial, transform.position, transform.rotation);

        Material objMaterial = MainController.Instance().obj_point.GetComponent<Renderer>().material;
        Texture objTexture = objMaterial.GetTexture("_MainTex");
        originalMaterial = Instantiate(objMaterial, transform.position, transform.rotation);

        //Change base material texture to obj texture
        baseMaterial_new.SetTexture("_MainTex", objTexture);

        //Change obj material texture to painterRT
        objMaterial.SetTexture("_MainTex", painterRT_new);

        //Change DrawModule Texture Target
        canvasBase.material = baseMaterial_new;
        canvasCamera.targetTexture = (RenderTexture)painterRT_new;
        texturePainter.canvasTexture = (RenderTexture)painterRT_new;
        texturePainter.baseMaterial = baseMaterial_new;
    }

    public void SetPosition()
    {
        transform.position = new Vector3(MainController.Instance().cameraEye.position.x, 0, MainController.Instance().cameraEye.position.z);
        transform.rotation = Quaternion.Euler(new Vector3(0, MainController.Instance().cameraEye.eulerAngles.y, 0));
    }

    public void SaveAndDisable()
    {
        texturePainter.Save();
        Invoke("DisableModule", 0.15f);
    }

    public void CancelAndDisable()
    {
        texturePainter.ClearBrushes();
        MainController.Instance().obj_point.GetComponent<Renderer>().material = originalMaterial;
        Invoke("DisableModule", 0.15f);
    }

    private void DisableModule()
    {
        gameObject.SetActive(false);
    }
}
