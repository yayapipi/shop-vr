using System;
using System.Collections;
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
    private static DrawModule _instance = null;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public static DrawModule Instance()
    {
        if (_instance == null)
        {
            throw new Exception(" could not find the DrawModule object.");
        }
        return _instance;
    }

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

        Material objMaterial = MainController.Instance("DrawModule").obj_point.GetComponent<Renderer>().material;
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
        float posy = MainController.Instance("DrawModule").cameraEye.position.y - 1.24f;
        transform.position = new Vector3(MainController.Instance("DrawModule").cameraEye.position.x, posy, MainController.Instance("DrawModule").cameraEye.position.z);
        transform.rotation = Quaternion.Euler(new Vector3(0, MainController.Instance("DrawModule").cameraEye.eulerAngles.y, 0));
    }

    public void SaveAndDisable()
    {
        texturePainter.Save();
        Invoke("DisableModule", 0.15f);
    }

    public void CancelAndDisable()
    {
        texturePainter.ClearBrushes();
        MainController.Instance("DrawModule").obj_point.GetComponent<Renderer>().material = originalMaterial;
        Invoke("DisableModule", 0.15f);
    }

    private void DisableModule()
    {
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        _instance = null;
    }
}
