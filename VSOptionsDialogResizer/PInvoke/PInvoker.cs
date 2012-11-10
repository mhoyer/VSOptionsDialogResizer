using System;
using System.Collections.Generic;
using System.Text;

namespace VSOptionsDialogResizer.PInvoke
{
    public class PInvoker : IPInvoker
    {
        public IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd)
        {
            return StaticPInvoke.GetWindow(hWnd, uCmd);
        }

        public uint GetWindowLong(IntPtr hWnd, GetWindowLong nIndex)
        {
            return StaticPInvoke.GetWindowLong(hWnd, nIndex);
        }

        public uint SetWindowLong(IntPtr hWnd, GetWindowLong nIndex, uint dwNewLong)
        {
            return StaticPInvoke.SetWindowLong(hWnd, nIndex, dwNewLong);
        }

        public void ResizeWindow(IntPtr hWnd, uint width, uint height)
        {
            Rect rect;
            StaticPInvoke.GetWindowRect(hWnd, out rect);
            StaticPInvoke.MoveWindow(hWnd, rect.X1, rect.Y1, width, height, true);
        }

        public Rect GetClientRect(IntPtr hWnd)
        {
            Rect clientRect;
            StaticPInvoke.GetClientRect(hWnd, out clientRect);

            return clientRect;
        }

        public IEnumerable<IntPtr> FindAllChildrenByClassName(IntPtr hWnd, string className)
        {
            return null;
        }

        public IEnumerable<IntPtr> FindWindows(string caption)
        {
            var result = new List<IntPtr>();
            EnumWindows(w =>
                {
                    if (GetWindowText(w) == caption) result.Add(w);
                });

            return result;
        }

        void EnumWindows(Action<IntPtr> block)
        {
            StaticPInvoke.EnumWindows((w, p) =>
            {
                block.Invoke(w);
                return true;
            }, IntPtr.Zero);
        }

        string GetWindowText(IntPtr hWnd, int nMaxCount = 1024)
        {
            var caption = new StringBuilder(nMaxCount);
            StaticPInvoke.GetWindowText(hWnd, caption, caption.Capacity);

            return caption.ToString();
        }
    }
}