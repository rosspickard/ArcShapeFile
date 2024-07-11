using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ArcShapeFile
{
    /* ****************************************************************************
     *  This is my own interpretation of a Well Known Text reader for spatial
     *  projection info as implemented in the ESRI prj file.  I've tried to 
     *  emulate the XML reader structure in that each base class (or Node) will
     *  have a name, a parent, attributes and possibly children (or Nodes).
     *  The tricky part is iterating through the nodes to find the one you want
     *  I've provided a couple of methods to make it easier for you:
     *      GetNodesByName - returns all nodes regardless of depth that match your
     *                       criteria.
     *      GetAttribValuebyName - Returns the value of the xth attribute that
     *                       matches your criteria
     *      GetAttribName - Returns the name of the attibute gievn a tag name and/or
     *                       parent tag name
     *  Anyway  ... it seems to work
     * *****************************************************************************
     */
    /// <summary>
    /// This is my own interpretation of a Well Known Text reader for spatial
    /// projection info as implemented in the ESRI prj file.  I've tried to
    /// emulate the XML reader structure in that each base class (or Node) will
    /// have a name, a parent, attributes and possibly children (or Nodes).
    /// <para>The tricky part is iterating through the nodes to find the one you want</para>
    /// <para>I've provided a couple of methods to make it easier for you:</para>
    /// <list type="bullet">
    /// <item>
    /// <description>GetNodesByName - Returns all nodes regardless of depth that match your criteria
    /// </description>
    /// </item>  
    /// <item>
    /// <description>GetAttribValuebyName - Returns the value of the xth attribute that matches your criteria
    /// </description>
    /// </item>  
    /// <item>
    /// <description>GetAttribName - Returns the name of the attibute given a tag name and/or parent tag name</description>
    /// </item>  
    /// </list>
    /// <para>Anyway  ... it seems to work</para>
    /// </summary>
    public class WKTReader
    {
        private ArrayList LevelList;

        private WKTNodes mvarNodes;

        /// <summary>
        /// The collection of individual nodes from the WKT input
        /// </summary>
        public WKTNodes Nodes
        {
            get { return mvarNodes; }
        }

        /// <summary>
        /// Opens the WKT file and loads all the data into the reader's nodes
        /// </summary>
        /// <param name="filename">The name of the WKT file to load</param>
        public void Read(string filename)
        {
            if (!System.IO.File.Exists(filename))
            { throw new Exception("WKT File does not exist"); }
            LevelList = new ArrayList();
            mvarNodes = new WKTNodes();

            try
            {

                string inData = System.IO.File.ReadAllText(filename);
                // Remove unwanted characters
                inData = inData.Replace("\t", "");
                inData = inData.Replace("\n", "");
                inData = inData.Replace("\r", "");
                inData = inData.Replace(", ", ",");


                int majLevel = 0;

                while (inData != "")
                {
                    int brkPos = inData.IndexOf("[");
                    if (brkPos == -1)
                        return;

                    string valName = (inData.Substring(0, brkPos)).Trim();
                    if (valName.Contains(","))
                        valName = valName.Substring(valName.IndexOf(",") + 1);
                    string innerText = LoadParameterFromWKT(valName, inData);
                    string param = null;
                    inData = (inData.Substring(brkPos + 2 + innerText.Length)).Trim();

                    if (innerText.Contains("["))
                    {
                        brkPos = innerText.IndexOf("[");
                        brkPos = innerText.LastIndexOf(",", brkPos);
                        param = (innerText.Substring(0, brkPos)).Trim();
                    }
                    else
                    { param = innerText; }

                    majLevel++;
                    mvarNodes.Add("Top", valName, innerText, param, "");

                }

                FindAllLevels();
            }
            catch { }
        }

        private void FindAllLevels()
        {
            bool HasChildren = true;
            LevelList.Clear();
            for (int i = 0; i < mvarNodes.Count; i++)
            {
                LevelList.Add(i.ToString() + ".");
            }

            int startSearch = 0;
            int noAdded = 0;

            while (HasChildren)
            {
                HasChildren = false;
                for (int i = startSearch; i < LevelList.Count; i++)
                {
                    startSearch += noAdded;
                    noAdded = 0;
                    string thisLevel = LevelList[i].ToString();
                    thisLevel = thisLevel.Substring(0, thisLevel.Length - 1);
                    string[] testLevels = thisLevel.Split('.');
                    WKTNode tNode = mvarNodes[Convert.ToInt32(testLevels[0])];
                    for (int j = 1; j < testLevels.Length; j++)
                    { tNode = tNode.Children[Convert.ToInt32(testLevels[j])]; }
                    if (tNode.hasChildren)
                    {
                        // Does this already exist in the list?
                        for (int nNode = 0; nNode < tNode.Children.Count; nNode++)
                        {
                            string newLevel = LevelList[i].ToString() + nNode.ToString() + ".";
                            if (!LevelList.Contains(newLevel))
                            {
                                HasChildren = true;
                                LevelList.Add(newLevel);
                                noAdded++;
                            }
                        }
                    }

                }

            }

            for (int i = 0; i < LevelList.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine(LevelList[i].ToString());
            }
        }

        private string LoadParameterFromWKT(string Header, string WKTString)
        {
            int paramStart = WKTString.IndexOf(Header);
            if (paramStart < 0)
            { return null; }
            paramStart += Header.Length + 1;
            string tString = WKTString.Substring(paramStart);
            int paramEnd = tString.IndexOf("]");
            int NoOfClosedBrks = 1;
            int NoOfOpenBrks = 0;

            while (NoOfClosedBrks != NoOfOpenBrks)
            {
                NoOfOpenBrks = 1;
                int brkPos = 0;
                do
                {
                    // how many "[" are there between the start and the first closed brackets

                    brkPos = tString.IndexOf("[", brkPos + 1);
                    if (brkPos > -1 & brkPos < paramEnd)
                        NoOfOpenBrks++;
                }
                while (brkPos > -1 & brkPos < paramEnd);
                if (NoOfOpenBrks != NoOfClosedBrks)
                {
                    paramEnd = tString.IndexOf("]", paramEnd + 1);
                    NoOfClosedBrks++;
                    NoOfOpenBrks = 0;
                };
            }

            return tString.Substring(0, paramEnd);
        }

        /// <summary>
        /// Returns all the nodes in the WKT string that match
        /// </summary>
        /// <param name="ParentName">The name of the Parent of the Tag (e.g. DATUM is the parent of SHEROID)</param>
        /// <param name="TagName">"The name of the Tag (e.g. DATUM)</param>
        /// <param name="AttribName">The name of the Attribute to search for (e.g. Latitude_Of_Origin)</param>
        public WKTNodes GetNodesbyName(string ParentName, string TagName, string AttribName)
        {
            WKTNodes getNodes = new WKTNodes();
            // Iterate through all the nodes and their children to find the match
            foreach (string testLevel in LevelList)
            {
                string[] splitLevels = testLevel.Split('.');
                WKTNode retNode = mvarNodes[Convert.ToInt32(splitLevels[0])];
                for (int i = 1; i < splitLevels.Length - 1; i++)
                {
                    retNode = retNode.Children[Convert.ToInt32(splitLevels[i])];
                }
                if (retNode.ParentName.ToUpper() == ParentName.ToUpper() & retNode.TagName.ToUpper() == TagName.ToUpper() & retNode.AttributeName.ToUpper() == AttribName.ToUpper())
                { getNodes.Add(retNode); }
            }
            return getNodes;
        }

        /// <summary>
        /// Returns all the nodes in the WKT string that match
        /// </summary>
        /// <param name="TagName">"The name of the Tag (e.g. DATUM)</param>
        /// <param name="AttribName">The name of the Attribute to search for (e.g. Latitude_Of_Origin)</param>
        public WKTNodes GetNodesbyName(string TagName, string AttribName)
        {
            WKTNodes getNodes = new WKTNodes();
            // Iterate through all the nodes and their children to find the match
            foreach (string testLevel in LevelList)
            {
                string[] splitLevels = testLevel.Split('.');
                WKTNode retNode = mvarNodes[Convert.ToInt32(splitLevels[0])];
                for (int i = 1; i < splitLevels.Length - 1; i++)
                {
                    retNode = retNode.Children[Convert.ToInt32(splitLevels[i])];
                }
                if (retNode.TagName.ToUpper() == TagName.ToUpper() & retNode.AttributeName.ToUpper() == AttribName.ToUpper())
                { getNodes.Add(retNode); }
            }
            return getNodes;
        }

        /// <summary>
        /// Returns all the nodes in the WKT string that match
        /// </summary>
        /// <param name="TagName">"The name of the Tag (e.g. DATUM)</param>
        public WKTNodes GetNodesbyName(string TagName)
        {
            WKTNodes getNodes = new WKTNodes();
            // Iterate through all the nodes and their children to find the match
            foreach (string testLevel in LevelList)
            {
                string[] splitLevels = testLevel.Split('.');
                WKTNode retNode = mvarNodes[Convert.ToInt32(splitLevels[0])];
                for (int i = 1; i < splitLevels.Length - 1; i++)
                {
                    retNode = retNode.Children[Convert.ToInt32(splitLevels[i])];
                }
                if (retNode.TagName.ToUpper() == TagName.ToUpper())
                { getNodes.Add(retNode); }
            }
            return getNodes;
        }

        /// <summary>
        /// Gets an attribute value for a particular Tag
        /// </summary>
        /// <param name="ParentName">The name of the Parent of the Tag (e.g. DATUM is the parent of SHEROID)</param>
        /// <param name="TagName">"The name of the Tag (e.g. DATUM)</param>
        /// <param name="AttribName">The name of the Attribute to search for (e.g. Latitude_Of_Origin)</param>
        /// <param name="AttribNumber">The item number of the Attribute array (defaults to 0)</param>
        public string GetAttribValuebyName(string ParentName, string TagName, string AttribName, int AttribNumber)
        {
            string RetString = null;
            // Iterate through all the nodes and their children to find the match
            foreach (string testLevel in LevelList)
            {
                string[] splitLevels = testLevel.Split('.');
                WKTNode retNode = mvarNodes[Convert.ToInt32(splitLevels[0])];
                for (int i = 1; i < splitLevels.Length - 1; i++)
                {
                    retNode = retNode.Children[Convert.ToInt32(splitLevels[i])];
                }
                if (!String.IsNullOrEmpty(retNode.AttributeName))
                {
                    if (retNode.ParentName.ToUpper() == ParentName.ToUpper() & retNode.TagName.ToUpper() == TagName.ToUpper() & retNode.AttributeName.ToUpper() == AttribName.ToUpper())
                    {
                        RetString = (string)retNode.Attributes[AttribNumber];
                        break;
                    }
                }
            }
            return RetString;
        }

        /// <summary>
        /// Gets an attribute value for a particular Tag
        /// </summary>
        /// <param name="TagName">"The name of the Tag (e.g. DATUM)</param>
        /// <param name="AttribName">The name of the Attribute to search for (e.g. Latitude_Of_Origin)</param>
        /// <param name="AttribNumber">The item number of the Attribute array (defaults to 0)</param>
        public string GetAttribValuebyName(string TagName, string AttribName, int AttribNumber)
        {
            string RetString = null;
            // Iterate through all the nodes and their children to find the match
            foreach (string testLevel in LevelList)
            {
                string[] splitLevels = testLevel.Split('.');
                WKTNode retNode = mvarNodes[Convert.ToInt32(splitLevels[0])];
                for (int i = 1; i < splitLevels.Length - 1; i++)
                {
                    retNode = retNode.Children[Convert.ToInt32(splitLevels[i])];
                }

                if (!String.IsNullOrEmpty(retNode.AttributeName))
                {
                    if (retNode.TagName.ToUpper() == TagName.ToUpper() & retNode.AttributeName.ToUpper() == AttribName.ToUpper())
                    {
                        RetString = (string)retNode.Attributes[AttribNumber];
                        break;
                    }
                }
            }
            return RetString;
        }

        /// <summary>
        /// Gets an attribute value for a particular Tag
        /// </summary>
        /// <param name="ParentName">The name of the Parent of the Tag (e.g. DATUM is the parent of SHEROID)</param>
        /// <param name="TagName">"The name of the Tag (e.g. DATUM)</param>
        /// <param name="AttribName">The name of the Attribute to search for (e.g. Latitude_Of_Origin)</param>
        public string GetAttribValuebyName(string ParentName, string TagName, string AttribName)
        {
            string RetString = null;
            // Iterate through all the nodes and their children to find the match
            foreach (string testLevel in LevelList)
            {
                string[] splitLevels = testLevel.Split('.');
                WKTNode retNode = mvarNodes[Convert.ToInt32(splitLevels[0])];
                for (int i = 1; i < splitLevels.Length - 1; i++)
                {
                    retNode = retNode.Children[Convert.ToInt32(splitLevels[i])];
                }
                if (!String.IsNullOrEmpty(retNode.AttributeName))
                {
                    if (retNode.ParentName.ToUpper() == ParentName.ToUpper() & retNode.TagName.ToUpper() == TagName.ToUpper() & retNode.AttributeName.ToUpper() == AttribName.ToUpper())
                    {
                        RetString = (string)retNode.Attributes[0];
                        break;
                    }
                }
            }
            return RetString;
        }

        /// <summary>
        /// Gets an attribute value for a particular Tag
        /// </summary>
        /// <param name="TagName">"The name of the Tag (e.g. DATUM)</param>
        /// <param name="AttribName">The name of the Attribute to search for (e.g. Latitude_Of_Origin)</param>
        public string GetAttribValuebyName(string TagName, string AttribName)
        {
            string RetString = null;
            // Iterate through all the nodes and their children to find the match
            foreach (string testLevel in LevelList)
            {
                string[] splitLevels = testLevel.Split('.');
                WKTNode retNode = mvarNodes[Convert.ToInt32(splitLevels[0])];
                for (int i = 1; i < splitLevels.Length - 1; i++)
                {
                    retNode = retNode.Children[Convert.ToInt32(splitLevels[i])];
                }
                if (!String.IsNullOrEmpty(retNode.AttributeName))
                {
                    if (retNode.TagName.ToUpper() == TagName.ToUpper() & retNode.AttributeName.ToUpper() == AttribName.ToUpper())
                    {
                        RetString = (string)retNode.Attributes[0];
                        break;
                    }
                }
            }
            return RetString;
        }

        /// <summary>
        /// Gets an attribute name for a particular Tag
        /// </summary>
        /// <param name="ParentName">The name of the Parent of the Tag (e.g. DATUM is the parent of SHEROID)</param>
        /// <param name="TagName">"The name of the Tag (e.g. DATUM)</param>
        public string GetAttribName(string ParentName, string TagName)
        {
            string RetString = null;
            // Iterate through all the nodes and their children to find the match
            foreach (string testLevel in LevelList)
            {
                string[] splitLevels = testLevel.Split('.');
                WKTNode retNode = mvarNodes[Convert.ToInt32(splitLevels[0])];
                for (int i = 1; i < splitLevels.Length - 1; i++)
                {
                    retNode = retNode.Children[Convert.ToInt32(splitLevels[i])];
                }

                if (retNode.ParentName.ToUpper() == ParentName.ToUpper() & retNode.TagName.ToUpper() == TagName.ToUpper() )
                {
                    RetString = (string)retNode.AttributeName;
                    break;
                }
            }
            return RetString;
        }

        /// <summary>
        /// Gets an attribute name for a particular Tag
        /// </summary>
        /// <param name="TagName">"The name of the Tag (e.g. DATUM)</param>
        public string GetAttribName(string TagName)
        {
            string RetString = null;
            // Iterate through all the nodes and their children to find the match
            foreach (string testLevel in LevelList)
            {
                string[] splitLevels = testLevel.Split('.');
                WKTNode retNode = mvarNodes[Convert.ToInt32(splitLevels[0])];
                for (int i = 1; i < splitLevels.Length - 1; i++)
                {
                    retNode = retNode.Children[Convert.ToInt32(splitLevels[i])];
                }
                if (retNode.TagName.ToUpper() == TagName.ToUpper())
                {
                    RetString = (string)retNode.AttributeName;
                    break;
                }
            }
            return RetString;
        }

        public void LoadWKT(string wkt)
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// A collection of WKT nodes
    /// </summary>
    public class WKTNodes : System.Collections.CollectionBase
    {
        private int Counter = 0;
        private string sLevel = "";

        internal void Add(string Parent, string Name, string InnerText, string Parameters, string Level)
        {
            //create a new object
            string[] paramList;
            WKTNode objNewMember = default(WKTNode);
            objNewMember = new WKTNode();
            WKTNodes Kids = new WKTNodes();

            sLevel = Level;
            Counter++;

            //set the properties passed into the method
            objNewMember.TagName = Name.Replace("\"", "");
            objNewMember.InnerText = InnerText;
            objNewMember.ParentName = Parent;
            objNewMember.Children = Kids;
            //if (Parent == "Top")
            //{ objNewMember.Level = Level; }
            //else
            objNewMember.Level = Level + "." + Counter.ToString();


            if (Parameters != null)
            {
                paramList = Parameters.Split(',');
                if(paramList[0].StartsWith("\""))
                {objNewMember.AttributeName = paramList[0].Replace("\"", "");}
                else
                {objNewMember.Attributes.Add(paramList[0].Trim());}
                for (int i = 1; i < paramList.Length; i++)
                {
                    // TO DO: Because the paramList is an ArrayList there is no reason why everything has to be 
                    // Stored as a string.  
                    objNewMember.Attributes.Add(paramList[i].Replace("\"", "").Trim());
                }
            }

            List.Add(objNewMember);

            while (InnerText != null)
            {
                int brkPos = InnerText.IndexOf("[");
                string valName = null;
                string subText = null;
                string param = null;
                if (brkPos > -1)
                {
                    valName = (InnerText.Substring(0, brkPos)).Trim();
                    if (valName.Contains(","))
                        valName = (valName.Substring(valName.LastIndexOf(",") + 1)).Trim();
                    subText = LoadParameterFromWKT(valName, InnerText);
                    InnerText = (InnerText.Substring(brkPos + 2 + subText.Length)).Trim();
                    if (InnerText.StartsWith(","))
                        InnerText = InnerText.Substring(1);
                    InnerText = InnerText.Trim();

                    if (subText.Contains("["))
                    {
                        brkPos = subText.IndexOf("[");
                        brkPos = subText.LastIndexOf(",", brkPos);
                        param = subText.Substring(0, brkPos).Trim();
                    }
                    else
                    {
                        param = subText;
                        subText = null;
                    }
                }
                else
                {
                    param = InnerText;
                    InnerText = null;
                }
                if (InnerText == "")
                    InnerText = null;

                objNewMember = (WKTNode)List[List.Count - 1];
                objNewMember.hasChildren = true;
                objNewMember.Children.Add(objNewMember.TagName, valName, subText, param, objNewMember.Level);
            }

        }
        internal void Add(WKTNode addNode)
        {
            List.Add(addNode);
        }

        private string LoadParameterFromWKT(string Header, string WKTString)
        {
            if (Header == null)
            { return null; }
            int paramStart = WKTString.IndexOf(Header);
            if (paramStart < 0)
            { return null; }
            paramStart += Header.Length + 1;
            string tString = WKTString.Substring(paramStart);
            int paramEnd = tString.IndexOf("]");
            int NoOfClosedBrks = 1;
            int NoOfOpenBrks = 0;

            while (NoOfClosedBrks != NoOfOpenBrks)
            {
                NoOfOpenBrks = 1;
                int brkPos = 0;
                do
                {
                    // how many "[" are there between the start and the first closed brackets

                    brkPos = tString.IndexOf("[", brkPos + 1);
                    if (brkPos > -1 & brkPos < paramEnd)
                        NoOfOpenBrks++;
                }
                while (brkPos > -1 & brkPos < paramEnd);
                if (NoOfOpenBrks != NoOfClosedBrks)
                {
                    paramEnd = tString.IndexOf("]", paramEnd + 1);
                    NoOfClosedBrks++;
                    NoOfOpenBrks = 0;
                };
            }

            return tString.Substring(0, paramEnd);
        }

        /// <summary>
        /// Well Know Text node by Index
        /// </summary>
        /// <param name="Index"></param>
        public WKTNode this[int Index]
        {
            get
            {
                return (WKTNode)List[Index];
            }
        }
        /// <summary>
        /// Well Know Text node by Name
        /// </summary>
        /// <param name="Name"></param>
        public WKTNode this[string Name]
        {
            get
            {
                int retIndex = -1;
                for (int Index = 0; Index < List.Count; Index++)
                {
                    WKTNode testField = (WKTNode)List[Index];
                    if (testField.TagName == Name.ToUpper())
                    {
                        retIndex = Index;
                        break;
                    }
                }
                if (retIndex > -1)
                { return (WKTNode)List[retIndex]; }
                else
                { return null; }

            }
        }

    }

    /// <summary>
    /// A Node read from a WKT file
    /// </summary>
    public class WKTNode
    {
        private string mvarInnerText;
        private bool mvarhasInner;
        private string mvarParent;
        private string mvarName;
        private string mvarAttribName;
        private string mvarLevel;
        private WKTNodes mvarChildren;

        ArrayList mvarParams = new ArrayList();

        /// <summary>
        /// The name of the Attribute
        /// </summary>
        public string AttributeName
        {
            get { return mvarAttribName; }
            set { mvarAttribName = value; }
        }
        /// <summary>
        /// The Text Tag
        /// </summary>
        public string TagName
        {
            get { return mvarName; }
            set { mvarName = value; }
        }
        /// <summary>
        /// Then Name of the Parnet
        /// </summary>
        public string ParentName
        {
            get { return mvarParent; }
            set { mvarParent = value; }
        }
        internal string Level
        {
            get { return mvarLevel; }
            set { mvarLevel = value; }
        }
        /// <summary>
        /// The Node Inner Text
        /// </summary>
        public string InnerText
        {
            get { return mvarInnerText; }
            set { mvarInnerText = value; }
        }
        /// <summary>
        /// Do any children nodes exist?
        /// </summary>
        public bool hasChildren
        {
            get { return mvarhasInner; }
            set { mvarhasInner = value; }
        }
        /// <summary>
        /// The attribute of the node
        /// </summary>
        public ArrayList Attributes
        {
            get { return mvarParams; }
            set { mvarParams = value; }
        }
        /// <summary>
        /// The children of the node
        /// </summary>
        public WKTNodes Children
        {
            get { return mvarChildren; }
            set { mvarChildren = value; }
        }

    }
}
