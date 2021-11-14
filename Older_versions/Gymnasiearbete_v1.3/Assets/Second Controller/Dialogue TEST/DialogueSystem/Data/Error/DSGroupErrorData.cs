using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Data.Error
{
    using Elements;
    public class DSGroupErrorData
    {
        public DSErrorData ErrorData { get; set; }
        public List<DSGroup> Group { get; set; }

        public DSGroupErrorData()
        {
            ErrorData = new DSErrorData();
            Group = new List<DSGroup>();
        }

    }
}
