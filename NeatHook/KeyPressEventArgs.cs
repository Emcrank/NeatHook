using System.Windows.Forms;

namespace NeatHook
{
    public class KeyPressEventArgs
    {
        public Keys Key { get; set; }

        public KeyModifiers Modifiers { get; set; }
    }
}