using System;
using System.Collections.Generic;
using System.Linq;
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

        public Rect GetWindowRect(IntPtr hWnd)
        {
            Rect rect;
            StaticPInvoke.GetWindowRect(hWnd, out rect);

            return rect;
        }

        public void ResizeWindow(IntPtr hWnd, uint width, uint height)
        {
            Rect rect = GetWindowRect(hWnd);
            MoveWindow(hWnd, rect.X1, rect.Y1, width, height, true);
        }

        public void MoveWindow(IntPtr hWnd, int x, int y, uint width, uint height, bool repaint)
        {
            StaticPInvoke.MoveWindow(hWnd, x, y, width, height, true);
        }

        public Rect GetClientRect(IntPtr hWnd)
        {
            Rect clientRect;
            StaticPInvoke.GetClientRect(hWnd, out clientRect);

            return clientRect;
        }

        public string GetClassName(IntPtr wnd)
        {
            var className = new StringBuilder(100);
            StaticPInvoke.GetClassName(wnd, className, className.Capacity);

            return className.ToString();
        }

        public IEnumerable<IntPtr> FindAllChildrenByClassName(IntPtr hWnd, string className)
        {
            var result = new List<IntPtr>();
            EnumChildren(hWnd, result.Add);

            return result.Where(c => GetClassName(c) == className);
        }

        public IEnumerable<IntPtr> FindAllWindowsByCaption(string caption)
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

        void EnumChildren(IntPtr parent, Action<IntPtr> block)
        {
            StaticPInvoke.EnumChildWindows(parent, (w, p) =>
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