# NeatHook
Wrapper for Global Key Hooks based on an example from Microsoft.

Registering CTRL+M Example

    NeatHook.RegisterKey(new KeyHookOptions
    {
        Handle = Handle,
        Id = 1,
        Handler = null,
        Key = Keys.M,
        Modifiers = KeyModifiers.Control
    });


Registering B Key Example with a handler

    NeatHook.RegisterKey(new KeyHookOptions
    {
        Handle = Handle,
        Id = 1,
        Handler = (s,e) => MessageBox.Show("B Pressed"),
        Key = Keys.B,
        Modifiers = KeyModifiers.None
    });
