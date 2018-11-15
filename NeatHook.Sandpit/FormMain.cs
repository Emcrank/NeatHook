using System;
using System.Diagnostics;
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
            //Explicit handler
            NeatHook.RegisterKey(new KeyHookOptions
            {
                Handle = Handle,
                Id = 1,
                Handler = () => MessageBox.Show("B Pressed"),
                Key = Keys.K,
                Modifiers = KeyModifiers.Control
            });
            
            //No explicit handler
            NeatHook.RegisterKey(new KeyHookOptions
            {
                Handle = Handle,
                Id = 2,
                Handler = null,
                Key = Keys.M,
                Modifiers = KeyModifiers.Control
            });

            NeatHook.OnHookedKeyPress += NeatHookOnHookedKeyPress;
        }

        private void NeatHookOnHookedKeyPress(KeyPressEventArgs obj)
        {
            Debug.WriteLine($"{obj.Modifiers}+{obj.Key}");
        }
    }
}