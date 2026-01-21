using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HActLib;
using Microsoft.Win32;
using System.Globalization;

namespace CMNEdit
{
    public class NumberBox : TextBox
    {
        public enum NumberMode
        {
            Text,
            Byte,
            Short,
            Ushort,
            Int,
            UInt,
            Float,
            Long,
            ULong,
        }

        private NumberMode mode;
        public Action<string> action;

        private bool m_hex = false;

        public NumberBox(NumberMode mode, Action<string> act, bool hex = false)
        {
            this.mode = mode;
            action = act;
            m_hex = hex;
        }

        private string currentText;


        public static bool Validate(string text, NumberMode mode, bool hex)
        {
            if (text.Length <= 0)
                return false;

            switch(mode)
            {
                case NumberMode.Float:
                    float result;
                    return float.TryParse(text, NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out result);
                case NumberMode.Int:
                    int i;
                    return int.TryParse(text, hex ? NumberStyles.HexNumber : NumberStyles.Number, null, out i);
                case NumberMode.UInt:
                    uint ui;
                    return uint.TryParse(text, hex ? NumberStyles.HexNumber : NumberStyles.Number, null, out ui);
                case NumberMode.Long:
                    long l;
                    return long.TryParse(text, hex ? NumberStyles.HexNumber : NumberStyles.Number, null, out l);
                case NumberMode.ULong:
                    ulong ul;
                    return ulong.TryParse(text, hex ? NumberStyles.HexNumber : NumberStyles.Number, null, out ul);

                case NumberMode.Short:
                    short s;
                    return short.TryParse(text, out s);

                case NumberMode.Ushort:
                    ushort us;
                    return ushort.TryParse(text, out us);

                case NumberMode.Byte:
                    byte b;
                    return byte.TryParse(text, out b);
            }

            return true;
        }

        protected void Validate()
        {
            bool valid = Validate(Text, mode, m_hex);

            if (valid)
                action?.Invoke(Text);
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
