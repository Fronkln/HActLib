using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMNEdit.Windows
{
    public partial class FlagEditor : Form
    {
        private Action<uint> m_complete;

        public FlagEditor()
        {
            InitializeComponent();
        }

        public void Init(string[] list, uint val, Action<uint> onApply)
        {
            checkedListBox1.Items.Clear();

            if (list == null)
            {
                list = new string[32];

                for (int i = 0; i < list.Length; i++)
                {
                    list[i] = "Flag " + i;
                }

            }

            checkedListBox1.Items.AddRange(list);

            m_complete = onApply;

            for (int i = 0; i < list.Length && i < 32; i++)
            {
                if (((ulong)val & (ulong)((ulong)1 << i)) > 0)
                    checkedListBox1.SetItemChecked(i, true);
            }

        }

        public void Apply()
        {
            uint val = 0;

            for (int i = 0; i < 32 && i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                    val |= (uint)((uint)1 << i);
            }

            m_complete?.Invoke(val);
        }

        private void FlagEditor64_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Apply();
        }
    }
}
