using System;

namespace VSOptionsDialogResizer
{
    public interface IWindowModifier
    {
        void Modify(IntPtr window);
    }
}