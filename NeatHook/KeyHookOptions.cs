using System;
using System.Windows.Forms;

namespace NeatHook
{
    public sealed class KeyHookOptions
    {
        public IntPtr Handle { get; set; }
        public Action<object, EventArgs> Handler { get; set; }
        public int Id { get; set; }
        public Keys Key { get; set; }
        public KeyModifiers Modifiers { get; set; }
    }
}