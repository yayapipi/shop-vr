using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class ExampleUseof_MeshCut : MonoBehaviour {

	public Material capMaterial;
    public GameObject a1;
    private MainController mainController;

	// Use this for initialization
	void Start () 
    {
        mainController = MainController.Instance("ExampleUseif_MeshCut");
        this.transform.parent = MainController.currentPointerCamera.transform;
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localRotation = Quaternion.identity;
	}

    void OnEnable()
    {
        MainController.UIPointerEvent += ChangeEventCamera;
        MainController.RTriggerClickDown += ControllerCut;
    }

    void OnDisable()
    {
        MainController.UIPointerEvent -= ChangeEventCamera;
        MainController.RTriggerClickDown -= ControllerCut;
    }

    private void ChangeEventCamera(Camera eventCamera)
    {
        if (eventCamera != null)
        {
            this.transform.parent = eventCamera.transform;
            this.transform.localPosition = new Vector3(0, 0, 0);
            this.transform.localRotation = Quaternion.identity;
        }
    }

	void Update()
    {
        
        if (mainController.UIPointerState == 3 && Input.GetMouseButtonDown(0))
        {
            cut();

        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Mesh mesh  = GetCacheItem("C:/Users/user/MeshRender/byte1");
            GameObject a = Instantiate(a1);
            a.GetComponent<MeshFilter>().mesh = mesh;
        }
    }

    private void ControllerCut()
    {
        if (mainController.UIPointerState == 1)
        {
            cut();
        }
    }

    private void cut()
    {
        if (mainController.enablePointerCutMesh)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                GameObject victim = hit.collider.gameObject;
                bool rigid_weight = false;

                if (victim.tag == "Model")
                {
                    GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.right, capMaterial);

                    //  if (pieces[0].GetComponent<Rigidbody>())
                    //      pieces[0].GetComponent<Rigidbody>().isKinematic = true;


                    if (pieces[0].GetComponent<MeshCollider>())
                    {
                        if (pieces[0].GetComponent<Rigidbody>())
                        {
                            rigid_weight = pieces[0].GetComponent<Rigidbody>().isKinematic;
                            pieces[0].GetComponent<Rigidbody>().isKinematic = true;
                        }
                        Destroy(pieces[0].GetComponent<MeshCollider>());
                    }

                    pieces[0].AddComponent<MeshCollider>().convex = true;
                    if (pieces[0].GetComponent<Rigidbody>())
                    {
                        pieces[0].GetComponent<Rigidbody>().isKinematic = rigid_weight;
                    }

                    if (pieces[0].GetComponent<BoxCollider>())
                        Destroy(pieces[0].GetComponent<BoxCollider>());
                    if (pieces[0].GetComponent<SphereCollider>())
                        Destroy(pieces[0].GetComponent<SphereCollider>());
                    if (pieces[0].GetComponent<CapsuleCollider>())
                        Destroy(pieces[0].GetComponent<CapsuleCollider>());

                    //CacheItem("C:/Users/lab/Desktop/VR SHOP/MeshRender/byte1", pieces[0].GetComponent<MeshFilter>().mesh);


                    if (!pieces[1].GetComponent<Rigidbody>())
                        pieces[1].AddComponent<Rigidbody>();

                    Destroy(pieces[1], 1);
                }
            }
        }
    }

    public static void CacheItem(string url, Mesh mesh)
    {
        //string path = Path.Combine(Application.persistentDataPath, url);
        byte[] bytes = MeshSerializer.WriteMesh(mesh, true);
        Debug.Log(bytes[0]);
        //string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
        Debug.Log(Application.dataPath);
        //System.IO.Path.Combine(Path , fileName)
            
        File.WriteAllBytes(url, bytes);
    }

    public static Mesh GetCacheItem(string url)
    {
        string path = Path.Combine(Application.persistentDataPath, url);
        if (File.Exists(path) == true)
        {
            byte[] bytes = File.ReadAllBytes(path);
            return MeshSerializer.ReadMesh(bytes);
        }
        return null;
    }

    void OnDrawGizmosSelected() {

		Gizmos.color = Color.green;

		Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + -transform.up * 0.5f, transform.position + -transform.up * 0.5f + transform.forward * 5.0f);

		Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
		Gizmos.DrawLine(transform.position,  transform.position + -transform.up * 0.5f);

	}

}
