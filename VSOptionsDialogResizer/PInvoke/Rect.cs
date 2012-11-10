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

        public uint Width { get { return (uint) (X2 - X1); } }
        public uint Height { get { return (uint) (Y2 - Y1); } }
    }
}