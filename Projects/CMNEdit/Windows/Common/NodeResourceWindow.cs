using System.Linq;
using CMNEdit.Windows;
using HActLib;

namespace CMNEdit
{
 
    //not to be confused with resources tab
    internal static class NodeResourceWindow
    {
        public static void Draw(Form1 form, Node node)
        {

            TreeViewItemResource res = Form1.Instance.resTree.Nodes.Cast<TreeViewItemResource>().ToArray().FirstOrDefault(x =>  x.Resource.NodeGUID == node.Guid);

            if (res == null)
                return;

            form.CreateHeader("Resource");
            form.CreateInput("Name", res.Resource.Name, delegate (string val) { res.Text = val; res.Resource.Name = val; });
        }
    }
}
