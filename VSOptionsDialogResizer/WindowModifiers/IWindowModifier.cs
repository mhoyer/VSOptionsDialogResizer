using System;

namespace VSOptionsDialogResizer.WindowModifiers
{
    public interface IWindowModifier
    {
        void Modify(IntPtr window, int width, int height);
    }
}