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
    public partial class FlagEditor64 : Form
    {
        private Action<ulong> m_complete;

        public FlagEditor64()
        {
            InitializeComponent();
        }

        public void Init(string[] list, ulong val, Action<ulong> onApply)
        {
            checkedListBox1.Items.Clear();

            checkedListBox1.Items.AddRange(list);

            m_complete = onApply;

            for(int i = 0; i < list.Length && i < 64; i++)
            {
                if(((ulong)val & (ulong)((ulong)1 << i)) > 0)
                    checkedListBox1.SetItemChecked(i, true);
            }
        }

        public void Apply()
        {
            ulong val = 0;

            for(int i = 0; i < 64 && i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                    val |= (ulong)((ulong)1 << i);
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
