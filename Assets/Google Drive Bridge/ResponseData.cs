using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoogleDriveBridge
{
    [Serializable]
    public struct ResponseData
    {
        public bool result;
        public string msg;

        //login
        public string sid;

        //get all tables
        public string[] tables;

        //get all columns of a table
        public string[] columnName;

        public string json;
    }
}
