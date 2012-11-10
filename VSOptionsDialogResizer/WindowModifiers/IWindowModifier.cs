using System;

namespace VSOptionsDialogResizer.WindowModifiers
{
    public interface IWindowModifier
    {
        void Modify(IntPtr window, uint width, uint height);
    }
}