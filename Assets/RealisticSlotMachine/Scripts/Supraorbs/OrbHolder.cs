using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;



namespace Mkey
{
    public class OrbHolder : MonoBehaviour
    {
        public static OrbHolder Instance;

        [SerializeField]
        private bool debug = true;

        [SerializeField]
        private bool isLogined = false;

        public string playerID;
        public string playerName;

        public static Action<bool, string> LoginEvent; // (logined, result)
        public static Action LogoutEvent;

        #region regular
        private void Awake()
        {
            if (Instance) Destroy(gameObject);
            else Instance = this;
            Initialize();
        }

        private void Start()
        {
            // add listeners for login event
            LoginEvent += (logined, result) => { };
            // if (LastSessionLogined) FBlogin();// as options
        }
        #endregion regular

        #region init
        public void Initialize()
        {
            playerName = "Supraorbs";
        }
        #endregion init

        #region login
        public void OrbLogin(string orbUser)
        {
            Debug.Log("OrbLogin starts seting details");

            isLogined = true;
            playerName = orbUser;
            LoginEvent?.Invoke(IsLogined, "Supraorbs");
        }

        public void OrbLogOut()
        {
            if (debug) Debug.Log("IsLogined: " + IsLogined);
            isLogined = false;

            LogoutEvent?.Invoke();
        }

        public void OrbLogOut(Action logOutCallBack)
        {
            if (debug) Debug.Log("IsLogined: " + IsLogined);
            LogoutEvent?.Invoke();
            logOutCallBack?.Invoke();

        }

        public bool IsLogined
        {
            get { return isLogined; }
        }
        
        #endregion login
    }
}
