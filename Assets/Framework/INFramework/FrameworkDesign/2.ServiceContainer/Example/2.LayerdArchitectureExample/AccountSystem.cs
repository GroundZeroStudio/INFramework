/****************************************************
    文件：AccountSystem.cs
    作者：Olivia
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

namespace INFrameworkDesign.ServiceLocator.LayerdArchitectureExample
{
    public interface IAccountSystem : ISystem
    {
        bool IsLogin { get; }

        void Login(string rUsername, string rPassword, Action<bool> rOnLogin);

        void Logout(Action rOnLogout);
    }

    public class AccountSystem : IAccountSystem
    {
        public bool IsLogin { get; private set; }

        public void Login(string rUsername, string rPassword, Action<bool> rOnLogin)
        {
            PlayerPrefs.SetString("username", rUsername);
            PlayerPrefs.SetString("password", rPassword);
            IsLogin = true;
            rOnLogin(true);
        }

        public void Logout(Action rOnLogout)
        {
            PlayerPrefs.SetString("username", string.Empty);
            PlayerPrefs.SetString("passward", string.Empty);
            IsLogin = false;
        }
    }

}

