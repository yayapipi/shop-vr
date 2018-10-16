using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {
        [Header("Related Objects")]
        public Transform itemContent;
        public GameObject ShopItemPanelPrefab;
        public GameObject cartMainPrefab;
        public GameObject messagePrefab;
        public ButtonChecker itemScrollUp;
        public ButtonChecker itemScrollDown;
        public Transform itemInformationSpawnPoint;
        public Transform cartSpawnPoint;
        public Transform messageSpawnPoint;

        private ScrollRect itemScrollRect;
        private GameObject mask;
        private Transform subUI;

        [Header("Variables")]
        [SerializeField] private string viewKind;
        [SerializeField] private int viewType;
        [SerializeField] private bool isLoadToEnd;
        [SerializeField] private bool isLoadingItems;
        [Range(100, 1000)] public int scrollSpeed;
        [Range(6, 30)] public int itemsShowOnce;

        private static int counter;
        public static bool isOpenCart;
        private static ShopController _instance = null;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public static ShopController Instance()
        {
            if (_instance == null)
            {
                throw new Exception("UnityMainThreadDispatcher could not find the ShopController object.");
            }
            return _instance;
        }

        void Start()
        {
            isOpenCart = false;
            counter = 0;
            isLoadToEnd = false;
            isLoadingItems = false;
            viewKind = "default";
            viewType = 0;
            itemScrollRect = itemContent.parent.GetComponentInParent<ScrollRect>();
            mask = transform.Find("mask").gameObject;
            subUI = transform.parent.Find("sub_UI");

            //show items
            GetShopItems(viewKind, viewType);
        }

        void Update()
        {
        //load items
            if (itemScrollRect.verticalNormalizedPosition <= 0.01 && !isLoadToEnd && !isLoadingItems)
                GetShopItems(viewKind, viewType);

            if (itemScrollUp.buttonPressed)
            {
                itemContent.localPosition += Vector3.down * scrollSpeed * Time.deltaTime;
            }

            if (itemScrollDown.buttonPressed)
            {
                itemContent.localPosition += Vector3.up * scrollSpeed * Time.deltaTime;
            }
            
            
        }

        public void SetViewKind(string kind)
        {
            if (string.Compare(viewKind, kind) != 0)
            {
                viewKind = kind;
                counter = 0;
                isLoadToEnd = false;
                isLoadingItems = false;

                foreach (Transform child in itemContent)
                    Destroy(child.gameObject);

                GetShopItems(viewKind, viewType);
            }
        }

        public void SetViewType(int type)
        {
            if (viewType != type)
            {
                viewType = type;
                counter = 0;
                isLoadToEnd = false;
                isLoadingItems = false;

                foreach (Transform child in itemContent)
                    Destroy(child.gameObject);

                GetShopItems(viewKind, viewType);
            }
        }

        public void OpenCart()
        {
            if (!isOpenCart)
            {
                isOpenCart = true;
                Instantiate(cartMainPrefab, cartSpawnPoint.position, cartSpawnPoint.rotation, subUI);
                
            }
        }

        public static bool GetIsOpenCart()
        {
            return isOpenCart;
        }

        public Transform GetSubUI()
        {
            return subUI;
        }


        /* Thread sqlapi */
        public static void Buy(int itemID, int amount, Action callbackDelegate)
        {
            Debug.Log("Buy amount = " + amount + " itemID = " + itemID);

            //using thread to buy item
            callbackDelegate += Instance().ShowMessage;
            callbackDelegate += MainController.UpdateUserData;
            ShopThread tws = new ShopThread(itemID, amount, callbackDelegate);
            Thread t = new Thread(new ThreadStart(tws.Buy));
            t.Start();
        }

        public static void Cart(int itemID, int amount, Action callbackDelegate)
        {
            Debug.Log("Cart: amount = " + amount + " itemID = " + itemID);

            //using thread to cart item
            callbackDelegate += Instance().ShowMessage;
            ShopThread tws = new ShopThread(itemID, amount, callbackDelegate);
            Thread t = new Thread(new ThreadStart(tws.Cart));
            t.Start();
        }

        public static void Checkout(Action callbackDelegate)
        {
            Debug.Log("Checkout: ");

            //using thread to checkout item
            callbackDelegate += Instance().ShowMessage;
            callbackDelegate += MainController.UpdateUserData;
            CheckoutThread tws = new CheckoutThread(callbackDelegate);
            Thread t = new Thread(new ThreadStart(tws.Checkout));
            t.Start();
        }

        public static void DeleteFromCart(int itemID)
        {
            Debug.Log("delete2");
            //Debug.Log("Delete item from cart: itemID = " + itemID);

            //using thread to delete from cart
            DeleteFromCartThread tws = new DeleteFromCartThread(itemID);
            Thread t = new Thread(new ThreadStart(tws.DeleteFromCart));
            t.Start();
        }

        private void ShowMessage()
        {
            GameObject newObj = Instantiate(messagePrefab, messageSpawnPoint.position, messageSpawnPoint.rotation);
            newObj.GetComponent<MessageController>().Set(EventManager.GetMessage1(), EventManager.GetMessage2());
        }

        private void GetShopItems(string kind, int type)
        {
            isLoadingItems = true;

            //using thread to get shop items
            GetShopItemsThread tws;
            switch (kind)
            {
                case "default":
                    tws = new GetShopItemsThread(1, type, "id", "asc", counter * itemsShowOnce, itemsShowOnce, GetShopItemsFinished());
                    break;
                case "new":
                    tws = new GetShopItemsThread(1, type, "shop_items.created_at", "desc", counter * itemsShowOnce, itemsShowOnce, GetShopItemsFinished());
                    break;
                case "hot":
                    tws = new GetShopItemsThread(1, type, "click_times", "desc", counter * itemsShowOnce, itemsShowOnce, GetShopItemsFinished());
                    break;
                case "recommend":
                    tws = new GetShopItemsThread(1, type, "id", "asc", counter * itemsShowOnce, itemsShowOnce, GetShopItemsFinished());
                    break;
                default:
                    tws = new GetShopItemsThread(1, type, "id", "asc", counter * itemsShowOnce, itemsShowOnce, GetShopItemsFinished());
                    break;
            }
            Thread t = new Thread(new ThreadStart(tws.GetShopItems));
            t.Start();
        }

        private IEnumerator GetShopItemsFinished()
        {
            shopitems[] shopItemsData = EventManager.GetShopItemsData();
            GameObject newObj;

            foreach (shopitems item in shopItemsData)
            {
                newObj = Instantiate(ShopItemPanelPrefab, itemContent);
                newObj.transform.localPosition = Vector3.zero;
                newObj.GetComponent<ShopItemController>().Set(item);  //display data on UI
                yield return null;
            }

            if (shopItemsData.Length == 0)
            {
                isLoadToEnd = true;
            }

            counter++;

            isLoadingItems = false;
        }

        public static void GetShopItemPics(int itemID, IEnumerator callbackEnumerator)
        {
            //using thread to get item pics
            GetShopItemPicsThread tws = new GetShopItemPicsThread(itemID, callbackEnumerator);
            Thread t = new Thread(new ThreadStart(tws.GetShopItemPics));
            t.Start();
        }

        public static void GetShopCartItems(IEnumerator callbackEnumerator)
        {
            //using thread to get item pics
            GetShopCartItemsThread tws = new GetShopCartItemsThread(callbackEnumerator);
            Thread t = new Thread(new ThreadStart(tws.GetShopCartItems));
            t.Start();
        }

        public static void Grab(int itemID/*, Action callbackDelegate*/)
        {
            Debug.Log(" itemID = " + itemID);

        //using thread to buy item
        //callbackDelegate += Instance().ShowMessage;
        //callbackDelegate += MainController.UpdateUserData;
            GrabThread tws = new GrabThread(itemID);
            Thread t = new Thread(new ThreadStart(tws.Grab));
            t.Start();
        }

        public static void PutBack(int itemID/*, Action callbackDelegate*/)
        {
            Debug.Log(" itemID = " + itemID);

            PutBackThread tws = new PutBackThread(itemID);
            Thread t = new Thread(new ThreadStart(tws.PutBack));
            t.Start();
        }

    public void Disable()
        {
            mask.SetActive(true);
        /*
            GetComponent<VRTK.VRTK_UICanvas>().enabled = false;
            foreach (Transform child in itemContent)
                if (child.GetComponent<VRTK.VRTK_UICanvas>() != null)
                    child.GetComponent<VRTK.VRTK_UICanvas>().enabled = false;
        */
        }

        public void Enable()
        {
            mask.SetActive(false);
            /*
            GetComponent<VRTK.VRTK_UICanvas>().enabled = true;
            foreach (Transform child in itemContent)
                if (child.GetComponent<VRTK.VRTK_UICanvas>() != null)
                    child.GetComponent<VRTK.VRTK_UICanvas>().enabled = true;
             */
        }

        public static void CloseCart()
        {
            isOpenCart = false;
            ShopController.Instance().Enable();
        }

        public void Close()
        {
            MainController.Instance().CloseShop();
            Destroy(transform.parent.gameObject);
        }

        void OnDestroy()
        {
            _instance = null;
        }
    /*
        public void VRTK_Enable(bool value)
        {
            if (GameObject.Find("shop_UI"))
                GameObject.Find("shop_UI").GetComponent<VRTK.VRTK_UICanvas>().enabled = value;
            if (GameObject.Find("inventory_UI"))
                GameObject.Find("inventory_UI").GetComponent<VRTK.VRTK_UICanvas>().enabled = value;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("shop_item_panel"))
            {
                obj.GetComponent<VRTK.VRTK_UICanvas>().enabled = value;
            }
        }*/
    }