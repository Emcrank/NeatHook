using System;
using System.Windows.Forms;

namespace NeatHook.Sandpit.WinForms
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            Load += HandleLoad;
        }

        private void HandleLoad(object sender, EventArgs e)
        {
            NeatHook.RegisterKey(new KeyHookOptions
            {
                Handle = Handle,
                Id = 1,
                Handler = null,
                Key = Keys.K,
                Modifiers = KeyModifiers.Control
            });
            NeatHook.RegisterKey(new KeyHookOptions
            {
                Handle = Handle,
                Id = 1,
                Handler = null,
                Key = Keys.M,
                Modifiers = KeyModifiers.Control
            });

            NeatHook.OnHookedKeyPress += NeatHookOnHookedKeyPress;
        }

        private void NeatHookOnHookedKeyPress(KeyPressEventArgs obj)
        {
            MessageBox.Show($"{obj.Modifiers}+{obj.Key}");
        }
    }
}