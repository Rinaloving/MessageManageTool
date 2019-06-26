﻿using BdcRsaDll;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModifyMessageTool
{
    class HeadHandler
    {


        private static HeadHandler mInstance = null;

        private static readonly object lockAssistant = new object();

        public static HeadHandler Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (lockAssistant)
                    {
                        if (mInstance == null)
                        {
                            mInstance = new HeadHandler();
                        }
                    }
                }
                return mInstance;
            }
        }

        private HeadHandler()
        { }
        /// <summary>
        /// 生成加密签名报文
        /// </summary>
        /// <param name="BizFileFullPath">未加密签名的上报报文完整路径</param>
        public void CreateMsgWithSignature(string BizFileFullPath)
        {
            StreamReader sr = new StreamReader(BizFileFullPath, System.Text.Encoding.UTF8);
            string privateKey = @"MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBANP1Ow58aqG/7d
                                PF5T+M3CyQ6Vc8XfkZ9+bigaNTILZqOmUSJFn7nvHnBhe2qHUHe6WdJBjU8s78/Dy
                                LSODW7BCg7SLMaTfUKsiWgWkoBy97kl8bHRcErVx9+wakW+PMv+C9Fkce+oY4fUE7
                                JJPwRw6sOe5PdjcW1hsZ14OUfoAZAgMBAAECgYEAt0IFEI5Dx5vg7cPhZOPODX4xM
                                WqROWnZa7eVLHgYBX+tA2v/IAmssCv1mZUk6yJQJq3J4upjENGs6E/o7/UC3f3juJ
                                CIAj0xyJ49x8FoKfHob+VZkkZEBIt7mPyvo1Pl9vW6dE50SQGrEkIIVOZdlKeWKzM
                                KhsyFZqKGNM5cAgECQQD6EeHoHnakfQicM7LHwho2QEtJO+CjKl3EhmDbE3qKGn/W/x
                                VpaDB/EqTfSR45r48009s1bmFjxIslIjNT10lBAkEA2Pv6uPZenhC/cvQNzUZIhHdUJ
                                ti086N8AAedMkW9sr1Bp3O3hspKvvNYtzwEfGvQWa42qVPLrIjmAmlxNn9o2QJAWFJ2
                                kpAn4ULUBq9vxoP01BJzRMNkPNzaz22Sye2gSyS+4EWp31fQQSFpn/9oMIGkN8lX1BB
                                PT3h8mLnynPsdwQJBALkanNTVN/pYBzqlgHCxmIOI2L0a+aMuwEW2OR/95spoMW4Mh
                                W/zerhTGEeYZ6tMvj0DJZZl6caSMsWz9eSr5GECQDq4SKdjEu21caizzRHqyTIlo36
                                75w+iPPrWxx9sKbqfN3OhEPgA7DKdH7s19Smw3kszdu0hVVAR0RpznGAp0Y4=";
            string temp = sr.ReadToEnd();
            sr.Close();
            string result = BdcRsa.GetNewMsgWithSignature(temp, privateKey);
            System.IO.File.WriteAllText(BizFileFullPath, result, Encoding.UTF8);


        }


    }
}
