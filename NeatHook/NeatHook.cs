using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NeatHook
{
    public sealed class NeatHook : IDisposable
    {
        public IReadOnlyDictionary<int, KeyHook> KeyHooks => (IReadOnlyDictionary<int, KeyHook>)keyHooks;

        private static readonly NeatHook instance = new NeatHook();
        private readonly IDictionary<int, KeyHook> keyHooks = new Dictionary<int, KeyHook>();
        private bool disposing;
        private bool registeredDispose;

        /// <summary>
        /// This event is fired when any hooked key which does not have an explicit handler set is pressed.
        /// </summary>
        public static event EventHandler<KeyPressEventArgs> OnHookedKeyPress;

        public static void FinalizeClass(object sender, EventArgs e)
        {
            instance.Dispose();
            Application.ApplicationExit -= FinalizeClass;
        }

        /// <summary>
        /// Registers a Key Hook
        /// </summary>
        /// <param name="options">
        /// Options parameter containing the information required to register a key hook.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown in the attempt to register a key with an id that is already regiseted.
        /// </exception>
        public static void RegisterKey(KeyHookOptions options)
        {
            if (instance.KeyHooks.ContainsKey(options.Id))
                throw new InvalidOperationException("A key hook has already been defined with an Id of " + options.Id);

            var hook = new KeyHook(options.Handle, options.Id, options.Modifiers, options.Key);
            hook.HotKeyPressed = k => HookedKeyPress(instance, hook);

            if (options.Handler != null)
                hook.HotKeyPressed = options.Handler;

            instance.keyHooks[options.Id] = hook;

            if (instance.registeredDispose)
                return;

            instance.registeredDispose = true;
            RegisterDispose();
        }

        /// <summary>
        /// Unregisters a Key Hook.
        /// </summary>
        /// <param name="id">Id of the KeyHook to be unregistered.</param>
        public static void UnregisterKey(int id)
        {
            if (!instance.KeyHooks.ContainsKey(id))
                return;

            var keyHook = instance.KeyHooks[id];
            keyHook.Dispose();
            instance.keyHooks.Remove(id);
        }

        /// <summary>
        /// Unregisters a Key Hook.
        /// </summary>
        /// <param name="keyHook">KeyHook to be unregistered.</param>
        public static void UnregisterKey(KeyHook keyHook)
        {
            UnregisterKey(keyHook.Id);
        }

        /// <summary>
        /// Unregisteres all Key Hooks and releases resources.
        /// </summary>
        public void Dispose()
        {
            if (disposing)
                return;
            disposing = true;

            foreach (var key in KeyHooks)
                key.Value.Dispose();
        }

        private static void HookedKeyPress(NeatHook neatHook, KeyHook keyHook)
        {
            OnHookedKeyPress?.Invoke(neatHook, new KeyPressEventArgs { Key = keyHook.Key, Modifiers = keyHook.Modifiers });
        }

        private static void RegisterDispose()
        {
            Application.ApplicationExit += FinalizeClass;
        }
    }
}