using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NeatHook
{
    public sealed class NeatHook : IDisposable
    {
        public IDictionary<int, KeyHook> KeyHooks { get; } = new Dictionary<int, KeyHook>();
        private static readonly NeatHook instance = new NeatHook();
        private bool disposing;

        private bool registeredDispose;

        public static event Action<KeyPressEventArgs> OnHookedKeyPress;

        public static void FinalizeClass(object sender, EventArgs e)
        {
            instance.Dispose();
            Application.ApplicationExit -= FinalizeClass;
        }

        public static void RegisterKey(KeyHookOptions options)
        {
            if(instance.KeyHooks.ContainsKey(options.Id))
                throw new InvalidOperationException("A key hook has already been defined with an Id of " + options.Id);

            var hook = new KeyHook(options.Handle, options.Id, options.Modifiers, options.Key);
            hook.HotKeyPressed += (s, e) => KeyPressed(hook);

            if (options.Handler != null)
                hook.HotKeyPressed += (s, e) => options.Handler(s, e);

            instance.KeyHooks[options.Id] = hook;

            if (instance.registeredDispose)
                return;

            instance.registeredDispose = true;
            RegisterDispose();
        }

        public void Dispose()
        {
            if (disposing)
                return;
            disposing = true;

            foreach (var key in KeyHooks)
                key.Value.Dispose();
        }

        private static void KeyPressed(KeyHook keyHook)
        {
            OnHookedKeyPress?.Invoke(new KeyPressEventArgs { Key = keyHook.Key, Modifiers = keyHook.Modifiers });
        }

        private static void RegisterDispose()
        {
            Application.ApplicationExit += FinalizeClass;
        }
    }
}