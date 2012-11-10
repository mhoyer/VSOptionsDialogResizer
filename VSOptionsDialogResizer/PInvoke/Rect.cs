using System.Runtime.InteropServices;

namespace VSOptionsDialogResizer.PInvoke
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int X1;
        public int Y1;
        public int X2;
        public int Y2;

        public int Width { get { return X2 - X1; } }
        public int Height { get { return Y2 - Y1; } }
    }
}