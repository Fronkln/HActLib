using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HActLib;
using Microsoft.Win32;
using System.Globalization;

namespace Pager
{
    public class NumberBox : TextBox
    {
        public enum NumberMode
        {
            Text,
            Byte,
            Ushort,
            Int,
            UInt,
            Float,
            Long,
        }

        private NumberMode mode;
        public Action<string> action;

        public NumberBox(NumberMode mode, Action<string> act)
        {
            this.mode = mode;
            action = act;
        }

        private string currentText;


        protected void Validate()
        {
            bool valid = false;

            if (mode == NumberMode.Text)
            {
                if (action != null)
                    action.Invoke(Text);
            }
            else
            {

                if (this.Text.Length > 0)
                {
                    if (mode == NumberMode.Float)
                    {
                        float result;
                        valid = float.TryParse(this.Text, NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);

                        if (valid)
                            this.Select(this.Text.Length, 0);
                    }
                    else
                    if (mode == NumberMode.Int)
                    {
                        int result;
                        valid = int.TryParse(this.Text, out result);

                        if (valid)
                            this.Select(this.Text.Length, 0);
                    }
                    else
                    if (mode == NumberMode.Long)
                    {
                        long result;
                        valid = long.TryParse(this.Text, out result);

                        if (valid)
                            this.Select(this.Text.Length, 0);
                    }
                    else
                    if (mode == NumberMode.Ushort)
                    {
                        ushort result;
                        valid = ushort.TryParse(this.Text, out result);

                        if (valid)
                            this.Select(this.Text.Length, 0);
                    }
                    else
                    if (mode == NumberMode.UInt)
                    {
                        uint result;
                        valid = uint.TryParse(this.Text, out result);

                        if (valid)
                            this.Select(this.Text.Length, 0);
                    }
                    else
                    if (mode == NumberMode.Byte)
                    {
                        byte result;
                        valid = byte.TryParse(this.Text, out result);

                        if (valid)
                            this.Select(this.Text.Length, 0);
                    }

                    if (!valid)
                    {
                        this.Text = currentText;
                        this.Select(this.Text.Length, 0);
                    }
                    else
                    {
                        currentText = Text;

                        if (action != null)
                            action.Invoke(Text);

                    }
                }
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Validate();

            base.OnLostFocus(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            bool valid = false;


            if (e.KeyCode == Keys.Enter)
            {
                Validate();
            }
        }
    }
}
